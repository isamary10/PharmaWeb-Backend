using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;
using PharmaWeb.Repositories;

namespace PharmaWeb.Controllers
{
    [Route("api/client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IRepository<Client> _clientRepository;

        public ClientController(IRepository<Client> repository)
        {
            _clientRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetAll()
        {
            try
            {
                var result = await _clientRepository.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve the list of clients {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetById(int id)
        {
            try
            {
                var client = await _clientRepository.GetByIdAsync(id);
                return client != null ? Ok(client) : NotFound();
            }
            catch (Exception ex)
            {
                ; return StatusCode(500, $"Failed to get order {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            try
            {
                if (client == null)
                    return BadRequest("Client not found.");

                await _clientRepository.AddAsync(client);
                return CreatedAtAction(nameof(GetById), new { id = client.ClientId }, client);
            }
            catch (Exception ex)

            {
                return StatusCode(500, $"Failed to create order {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            try
            {
                if (id != client.ClientId)
                    return BadRequest("ID not found.");

                await _clientRepository.UpdateAsync(client);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update client {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clientRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Failed to delete: {ex.Message}");
            }
        }
    }
}
