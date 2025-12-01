using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileSkillsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public ProfileSkillsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpGet("by-profile/{profileId:guid}")]
        public async Task<IActionResult> GetByProfile(Guid profileId)
        {
            var items = await _supabase.GetAllAsync<ProfileSkill>("profile_skills");
            var filtered = items.FindAll(x => x.ProfileId == profileId);
            return Ok(filtered);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProfileSkill profileSkill)
        {
            var created = await _supabase.CreateAsync("profile_skills", profileSkill);
            return CreatedAtAction(nameof(GetByProfile), new { profileId = profileSkill.ProfileId }, created);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Guid profile_id, [FromQuery] int skill_id)
        {
            // Usamos el nuevo método que acepta DOS llaves
            await _supabase.DeleteCompositeAsync(
                "profile_skills",       // Nombre de la tabla
                "profile_id",           // Primera columna llave
                profile_id.ToString(),  // Primer valor
                "skill_id",             // Segunda columna llave
                skill_id.ToString()     // Segundo valor
            );

            return NoContent();
        }
    }
}
