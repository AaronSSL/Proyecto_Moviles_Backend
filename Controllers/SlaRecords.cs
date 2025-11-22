using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlaRecordsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public SlaRecordsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        // GET: api/SlaRecords
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _supabase.GetAllAsync<SlaRecords>("sla_records");
            return Ok(items);
        }

        // GET: api/SlaRecords/101
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _supabase.GetByIdAsync<SlaRecords>("sla_records", "id", id.ToString());
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/SlaRecords
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SlaRecords record)
        {
            var created = await _supabase.CreateAsync("sla_records", record);
            return CreatedAtAction(nameof(GetById), new { id = created?.Id }, created);
        }

        // PUT: api/SlaRecords/101
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SlaRecords record)
        {
            var updated = await _supabase.UpdateAsync("sla_records", "id", id.ToString(), record);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/SlaRecords/101
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _supabase.DeleteAsync("sla_records", "id", id.ToString());
            return NoContent();
        }
    }
}