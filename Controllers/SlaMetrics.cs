using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SlaMetricsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public SlaMetricsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        // GET: api/SlaMetrics
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _supabase.GetAllAsync<SlaMetric>("sla_metrics");
            return Ok(items);
        }

        // GET: api/SlaMetrics/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _supabase.GetByIdAsync<SlaMetric>("sla_metrics", "id", id.ToString());
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST: api/SlaMetrics
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SlaMetric metric)
        {
            var created = await _supabase.CreateAsync("sla_metrics", metric);
            return CreatedAtAction(nameof(GetById), new { id = created?.Id }, created);
        }

        // PUT: api/SlaMetrics/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SlaMetric metric)
        {
            var updated = await _supabase.UpdateAsync("sla_metrics", "id", id.ToString(), metric);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE: api/SlaMetrics/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _supabase.DeleteAsync("sla_metrics", "id", id.ToString());
            return NoContent();
        }
    }
}