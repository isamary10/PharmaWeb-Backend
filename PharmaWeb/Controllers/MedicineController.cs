using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Dto;
using PharmaWeb.Models;
using PharmaWeb.Persistencia;
using PharmaWeb.Repositories;

namespace PharmaWeb.Controllers
{
    [Route("api/medicine")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IRepository<Medicine> _repositoryMedicine;
        private readonly IRepository<RawMaterial> _repositoryRawMaterial;
        private PharmaWebContext _context { get; set; }

        public MedicineController(IRepository<Medicine> repositoryMedicine, IRepository<RawMaterial> repositoryRawMaterial, PharmaWebContext pharmaWebContext)
        {
            _repositoryMedicine = repositoryMedicine;
            _repositoryRawMaterial = repositoryRawMaterial;
            _context = pharmaWebContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetAll()
        {
            try
            {
                var medicines = await _repositoryMedicine.GetAllAsync(q => q.Include(m => m.Composition));
                return Ok(medicines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching medicines: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Medicine>> GetById(int id)
        {
            try
            {
                var medicine = await _repositoryMedicine.GetByIdAsync(id, q => q.Include(m => m.Composition));
                return medicine != null ? Ok(medicine) : NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching medicine: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MedicineCreateDto medicineDto)
        {
            try
            {
                var medicine = new Medicine
                {
                    Name = medicineDto.Name,
                    Description = medicineDto.Description,
                    Price = medicineDto.Price,
                    StockQuantity = medicineDto.StockQuantity,
                    Composition = new List<MedicineRawMaterial>()
                };

                await _repositoryMedicine.AddAsync(medicine);

                // verifica e adiciona os RawMaterials a composição
                foreach (var item in medicineDto.Composition)
                {
                    var rawMaterial = await _repositoryRawMaterial.GetByIdAsync(item.RawMaterialId);
                    if (rawMaterial == null)
                        return BadRequest($"RawMaterialId {item.RawMaterialId} not found.");

                    // adiciona relação na tabela de composição
                    var medicineRawMaterial = new MedicineRawMaterial
                    {
                        MedicineId = medicine.MedicineId, // Agora temos certeza de que esse ID existe
                        RawMaterialId = rawMaterial.RawMaterialId
                    };

                    _context.MedicinesRawMaterials.Add(medicineRawMaterial);
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = medicine.MedicineId }, medicine);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating medicine: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Medicine medicine)
        {
            try
            {
                if (id != medicine.MedicineId)
                    return BadRequest("ID inconsistente.");

                await _repositoryMedicine.UpdateAsync(medicine);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating medicine: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repositoryMedicine.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Failed to delete: {ex.Message}");
            }
        }
    }
}
