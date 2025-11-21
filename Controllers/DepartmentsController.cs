using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public DepartmentsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _supabase.GetAllAsync<Department>("departments");
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _supabase.GetByIdAsync<Department>("departments", "id", id.ToString());
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Department department)
        {
            var created = await _supabase.CreateAsync("departments", department);
            return CreatedAtAction(nameof(GetById), new { id = created?.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Department department)
        {
            var updated = await _supabase.UpdateAsync("departments", "id", id.ToString(), department);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _supabase.DeleteAsync("departments", "id", id.ToString());
            return NoContent();
        }
    }
}
