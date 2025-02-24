using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Dto;
using PharmaWeb.Models;
using PharmaWeb.Persistencia;
using PharmaWeb.Repositories;

namespace PharmaWeb.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Order> _repositoryOrder;
        private readonly IRepository<Medicine> _repositoryMedicine;
        private PharmaWebContext _context { get; set; }

        public OrderController(IRepository<Order> repositoryOrder, IRepository<Medicine> repositoryMedicine, PharmaWebContext pharmaWebContext)
        {
            _repositoryOrder = repositoryOrder;
            _repositoryMedicine = repositoryMedicine;
            _context = pharmaWebContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAll()
        {
            try
            {
                var orders = await _repositoryOrder.GetAllAsync(q => q.Include(o => o.OrdersMedicines));
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching orders: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(int id)
        {
            try
            {
                var order = await _repositoryOrder.GetByIdAsync(id, q => q.Include(o => o.OrdersMedicines));
                return order != null ? Ok(order) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching order: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto orderDto)
        {
            try
            {
                var order = new Order
                {
                    OrderDate = orderDto.OrderDate,
                    OrderTotal = orderDto.OrderTotal,
                    ClientId = orderDto.ClientId,
                    OrdersMedicines = new List<OrderMedicine>()
                };

                // adiciona a ordem no banco para gerar o ID
                await _repositoryOrder.AddAsync(order);

                // lista de medicamentos a serem adicionados
                var addedMedicines = new List<OrderMedicineDto>();

                // verificar e adicionar os Medicine à composição
                foreach (var item in orderDto.OrdersMedicines)
                {
                    var medicine = await _repositoryMedicine.GetByIdAsync(item.MedicineId);
                    if (medicine == null)
                        return BadRequest($"MedicineId {item.MedicineId} not found.");

                    // adicionar relação na tabela de composição
                    var orderMedicine = new OrderMedicine
                    {
                        OrderId = order.OrderId, // Agora temos certeza de que esse ID existe
                        MedicineId = medicine.MedicineId,
                        Quantity = item.Quantity
                    };

                    _context.OrdersMedicines.Add(orderMedicine);

                    // adiciona os medicamentos para a lista de adição
                    addedMedicines.Add(new OrderMedicineDto
                    {
                        MedicineId = medicine.MedicineId,
                        Quantity = item.Quantity
                    });
                }

                // Atualiza o estoque, passando os medicamentos adicionados
                var stockUpdated = await UpdateStockQuantityForCreate(order.OrderId, addedMedicines);
                if (!stockUpdated)
                {
                    return BadRequest("Error updating the stock. Please check if there is enough stock available.");
                }

                order.OrderTotal = await CalculateOrderTotal(order.OrderId);
                order.OrderDate = DateTime.Now;

                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating order: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderUpdateDto orderDto)
        {
            try
            {
                if (id != orderDto.OrderId)
                    return BadRequest("ID inconsistente.");

                var existingOrder = await _context.Orders
                    .Include(o => o.OrdersMedicines)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (existingOrder == null)
                    return NotFound();

                // verifica medicamentos removidos e quantidades reduzidas
                var removedMedicines = existingOrder.OrdersMedicines
                    .Where(om => !orderDto.OrdersMedicines.Any(newOm => newOm.MedicineId == om.MedicineId))
                    .ToList();

                var updatedMedicines = orderDto.OrdersMedicines
                    .Where(newOm => existingOrder.OrdersMedicines
                        .Any(om => om.MedicineId == newOm.MedicineId && om.Quantity != newOm.Quantity))
                    .ToList();

                // atualiza apenas os campos necessários não precisando atualizar os dados do Client
                existingOrder.OrderDate = orderDto.OrderDate;
                existingOrder.ClientId = orderDto.ClientId;

                //var existingMedicines = existingOrder.OrdersMedicines.ToList();

                // remove os medicamentos que não estão no novo DTO
                existingOrder.OrdersMedicines.RemoveAll(om =>
                    !orderDto.OrdersMedicines.Any(newOm => newOm.MedicineId == om.MedicineId));

                // adiciona ou atualiza os medicamentos do novo DTO
                foreach (var newOm in orderDto.OrdersMedicines)
                {
                    var existingOm = existingOrder.OrdersMedicines
                        .FirstOrDefault(om => om.MedicineId == newOm.MedicineId);

                    if (existingOm != null)
                    {
                        // atualiza a quantidade do medicamento existente
                        existingOm.Quantity = newOm.Quantity;
                    }
                    else
                    {
                        // adiciona um novo medicamento
                        existingOrder.OrdersMedicines.Add(new OrderMedicine
                        {
                            OrderId = existingOrder.OrderId,
                            MedicineId = newOm.MedicineId,
                            Quantity = newOm.Quantity
                        });
                    }
                }

                // atualiza o estoque, considerando os medicamentos removidos e as quantidades reduzidas
                var stockUpdated = await UpdateStockQuantity(existingOrder.OrderId, removedMedicines, updatedMedicines);
                if (!stockUpdated)
                {
                    return BadRequest("Erro ao atualizar o estoque. Verifique se há estoque suficiente.");
                }

                // calcula o total do pedido após atualizar os medicamentos
                existingOrder.OrderTotal = await CalculateOrderTotal(existingOrder.OrderId);

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating order: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var order = await _repositoryOrder.GetByIdAsync(id, q => q.Include(om => om.OrdersMedicines));
                if (order == null)
                    return NotFound($"order with ID {id} not found.");

                // restaura o estoque antes de remover os registros
                var restored = await RestoreStockQuantity(id);
                if (!restored)
                    return BadRequest($"Failed to restore stock for Order ID {id}.");

                // remove todas as associações na tabela de junção
                _context.OrdersMedicines.RemoveRange(_context.OrdersMedicines.Where(o => o.OrderId == id));
                await _context.SaveChangesAsync();

                await _repositoryOrder.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Failed to delete: {ex.Message}");
            }
        }
        private async Task<decimal> CalculateOrderTotal(int orderId)
        {
            try
            {
                var ordersMedicines = await _context.OrdersMedicines
                    .Where(om => om.OrderId == orderId)
                    .Include(om => om.Medicine)
                    .ToListAsync();

                if (ordersMedicines == null || !ordersMedicines.Any())
                    throw new InvalidOperationException($"Order {orderId} has no medicines associated.");

                decimal total = ordersMedicines
                    .Where(om => om.Medicine != null)
                    .Sum(om => om.Medicine.Price * om.Quantity);

                return total;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error to calculate order total {orderId}: {ex.Message}");
                throw;
            }
        }
        private async Task<bool> UpdateStockQuantityForCreate(int orderId, List<OrderMedicineDto> addedMedicines)
        {
            try
            {
                // Deduz do estoque os medicamentos que foram adicionados ao pedido
                foreach (var addedMedicine in addedMedicines)
                {
                    var medicine = await _context.Medicines
                        .FirstOrDefaultAsync(m => m.MedicineId == addedMedicine.MedicineId);

                    if (medicine != null)
                    {
                        // Verifica se há estoque suficiente
                        if (medicine.StockQuantity < addedMedicine.Quantity)
                            return false;

                        // Deduz a quantidade do estoque
                        medicine.StockQuantity -= addedMedicine.Quantity;
                    }
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock for order {orderId}: {ex.Message}");
                return false;
            }
        }
        private async Task<bool> UpdateStockQuantity(int orderId, List<OrderMedicine> removedMedicines, List<OrderMedicineDto> updatedMedicines)
        {
            try
            {
                // Adiciona de volta ao estoque os medicamentos removidos no Update
                foreach (var removedMedicine in removedMedicines)
                {
                    var medicine = await _context.Medicines.FirstOrDefaultAsync(m => m.MedicineId == removedMedicine.MedicineId);
                    if (medicine != null)
                    {
                        // Adiciona ao estoque a quantidade removida
                        medicine.StockQuantity += removedMedicine.Quantity;
                    }
                }

                // Adiciona ao estoque a diferença de quantidade para os medicamentos que tiveram a quantidade reduzida
                foreach (var updatedMedicine in updatedMedicines)
                {
                    // BUSCA A QUANTIDADE ORIGINAL DA ORDEM ANTES DA ALTERAÇÃO
                    var originalOrderMedicine = await _context.OrdersMedicines
                        .AsNoTracking() // Garante que o EF Core não pegue uma versão modificada
                        .FirstOrDefaultAsync(om => om.OrderId == orderId && om.MedicineId == updatedMedicine.MedicineId);

                    if (originalOrderMedicine != null)
                    {
                        int originalQuantity = originalOrderMedicine.Quantity; // Quantidade antes da atualização
                        int newQuantity = updatedMedicine.Quantity; // Nova quantidade informada

                        var medicine = await _context.Medicines
                                .FirstOrDefaultAsync(m => m.MedicineId == updatedMedicine.MedicineId);

                        // Se a nova quantidade for menor que a original, devolve a diferença ao estoque
                        if (newQuantity < originalQuantity)
                        {
                            if (medicine != null)
                            {
                                medicine.StockQuantity += (originalQuantity - newQuantity);
                            }
                        }
                        if (newQuantity > originalQuantity)
                        {
                            if (medicine != null)
                            {
                                medicine.StockQuantity -= (newQuantity - originalQuantity);
                            }
                        }
                    }
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock for order {orderId}: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> RestoreStockQuantity(int orderId)
        {
            try
            {
                var ordersMedicines = await _context.OrdersMedicines
                    .Where(om => om.OrderId == orderId)
                    .Include(om => om.Medicine)
                    .ToListAsync();

                if (ordersMedicines == null || !ordersMedicines.Any())
                    return false;

                foreach (var orderMedicine in ordersMedicines)
                {
                    if (orderMedicine.Medicine == null)
                        continue;

                    // Repor o estoque com a quantidade que foi retirada
                    orderMedicine.Medicine.StockQuantity += orderMedicine.Quantity;
                }

                // Salva as alterações no banco de dados
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error to restore stock quantity {orderId}: {ex.Message}");
                return false;
            }
        }
    }
}
