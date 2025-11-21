using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public ProfilesController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _supabase.GetAllAsync<Profile>("profiles");
            return Ok(items);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _supabase.GetByIdAsync<Profile>("profiles", "id", id.ToString());
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Profile profile)
        {
            var created = await _supabase.CreateAsync("profiles", profile);
            return CreatedAtAction(nameof(GetById), new { id = created?.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Profile profile)
        {
            var updated = await _supabase.UpdateAsync("profiles", "id", id.ToString(), profile);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _supabase.DeleteAsync("profiles", "id", id.ToString());
            return NoContent();
        }
    }
}
