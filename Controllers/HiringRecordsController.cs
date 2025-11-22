using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HiringRecordsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public HiringRecordsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }
        
        // GET: api/HiringRecords
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _supabase.GetAllAsync<HiringRecord>("hiring_records");
            return Ok(items);
        }

        // GET: api/HiringRecords/{id:guid}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
             var items = await _supabase.GetAllAsync<HiringRecord>("hiring_records");
             // Se asume el filtrado en memoria.
             var item = items.FirstOrDefault(x => x.HiringRecordId == id); 
             if (item == null) return NotFound();
             return Ok(item);
        }
        
        // POST: api/HiringRecords
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] HiringRecord hiringRecord)
        {
            var created = await _supabase.CreateAsync("hiring_records", hiringRecord);

            return CreatedAtAction(nameof(GetById), new { id = created!.HiringRecordId }, created);
        }

        // DELETE: api/HiringRecords/{id:guid}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Remove(Guid id)
        {
            // Eliminación por la clave primaria 'hiring_record_id'
            await _supabase.DeleteAsync("hiring_records", "hiring_record_id", "eq." + id.ToString());
            return NoContent();
        }
    }
}