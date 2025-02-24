using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaWeb.Models;
using PharmaWeb.Persistencia;
using PharmaWeb.Repositories;

namespace PharmaWeb.Controllers
{
    [Route("api/rawmaterial")]
    [ApiController]
    public class RawMaterialController : ControllerBase
    {
        private readonly IRepository<RawMaterial> _repository;
        private PharmaWebContext _context { get; set; }
        public RawMaterialController(IRepository<RawMaterial> repository, PharmaWebContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RawMaterial>>> GetAll()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve the list of raw materials {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RawMaterial>> GetById(int id)
        {
            try
            {
                var material = await _repository.GetByIdAsync(id);
                return material != null ? Ok(material) : NotFound();
            }
            catch (Exception ex)
            {
;                return StatusCode(500, $"Failed to get raw material {ex.Message}" );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RawMaterial rawMaterial)
        {
            try
            {
                await _repository.AddAsync(rawMaterial);
                return CreatedAtAction(nameof(GetById), new { id = rawMaterial.RawMaterialId }, rawMaterial);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create raw material {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RawMaterial rawMaterial)
        {
            try
            {
                if (id != rawMaterial.RawMaterialId)
                    return BadRequest("ID not found.");

                await _repository.UpdateAsync(rawMaterial);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update raw material {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var rawMaterial = await _repository.GetByIdAsync(id, q => q.Include(rm => rm.Composition));
                if (rawMaterial == null)
                    return NotFound($"RawMaterial with ID {id} not found.");

                // remove todas as associações na tabela de junção
                _context.MedicinesRawMaterials.RemoveRange(_context.MedicinesRawMaterials.Where(mr => mr.RawMaterialId == id));
                await _context.SaveChangesAsync();

                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"Failed to delete: {ex.Message}");
            }
        }
    }
}
