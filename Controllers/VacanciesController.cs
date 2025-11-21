using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacanciesController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public VacanciesController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _supabase.GetAllAsync<Vacancy>("vacancies");
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _supabase.GetByIdAsync<Vacancy>("vacancies", "id", id.ToString());
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vacancy vacancy)
        {
            var created = await _supabase.CreateAsync("vacancies", vacancy);
            return CreatedAtAction(nameof(GetById), new { id = created?.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vacancy vacancy)
        {
            var updated = await _supabase.UpdateAsync("vacancies", "id", id.ToString(), vacancy);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _supabase.DeleteAsync("vacancies", "id", id.ToString());
            return NoContent();
        }
    }
}
