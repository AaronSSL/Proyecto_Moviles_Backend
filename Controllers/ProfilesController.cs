using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;
using System.Collections.Generic;

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
            // 1. Separar Skills
            var skillsToCreate = profile.ProfileSkills;
            profile.ProfileSkills = null;

            // 2. Crear Perfil (Supabase generará el UUID)
            var createdProfile = await _supabase.CreateAsync("profiles", profile);

            // Verificación de seguridad
            if (createdProfile == null || createdProfile.Id == null) 
                return BadRequest("Error: No se pudo crear el perfil o no se devolvió ID.");

            // 3. Crear Skills
            if (skillsToCreate != null && skillsToCreate.Count > 0)
            {
                foreach (var skill in skillsToCreate)
                {
                    // Asignamos el UUID real que nos dio Supabase
                    skill.ProfileId = createdProfile.Id.Value;

                    await _supabase.CreateAsync("profile_skills", skill);
                }
                
                // Pegamos la lista de nuevo para mostrarla en la respuesta
                createdProfile.ProfileSkills = skillsToCreate;
            }

            return CreatedAtAction(nameof(GetById), new { id = createdProfile.Id }, createdProfile);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Profile profile)
        {
            profile.ProfileSkills = null;
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