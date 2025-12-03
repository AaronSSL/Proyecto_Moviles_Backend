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
            // 1. Traemos los perfiles
            var profiles = await _supabase.GetAllAsync<Profile>("profiles");

            // 2. Traemos TODOS los skills (Join manual en memoria)
            // NOTA: Si tienes miles de datos, esto debería optimizarse en el SupabaseService
            // para usar ?select=*,profile_skills(*)
            var allSkills = await _supabase.GetAllAsync<ProfileSkill>("profile_skills");

            // 3. Asignamos los skills a cada perfil correspondiente
            foreach (var profile in profiles)
            {
                if (profile.Id != null)
                {
                    // Buscamos los skills que coincidan con el ID del perfil
                    profile.ProfileSkills = allSkills
                        .Where(s => s.ProfileId == profile.Id)
                        .ToList();
                }
            }

            return Ok(profiles);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // 1. Traemos el perfil
            var profile = await _supabase.GetByIdAsync<Profile>("profiles", "id", id.ToString());
            
            if (profile == null) return NotFound();

            // 2. Traemos los skills (Igual que haces en ProfileSkillsController)
            var allSkills = await _supabase.GetAllAsync<ProfileSkill>("profile_skills");

            // 3. Filtramos solo los de este usuario y los asignamos
            profile.ProfileSkills = allSkills
                .Where(s => s.ProfileId == id)
                .ToList();

            return Ok(profile);
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
            // 1. Guardamos los skills nuevos en una variable temporal
            var skillsToUpdate = profile.ProfileSkills;

            // 2. Limpiamos la propiedad para poder actualizar la tabla 'profiles' sin errores
            profile.ProfileSkills = null;

            // 3. Actualizamos los datos base del perfil
            var updated = await _supabase.UpdateAsync("profiles", "id", id.ToString(), profile);
            
            if (updated == null) return NotFound();

            // 4. Lógica de Actualización de Skills
            if (skillsToUpdate != null)
            {
                try 
                {
                    // A. Borramos TODOS los skills anteriores de este usuario
                    // Asumiendo que tu DeleteAsync funciona como "DELETE FROM table WHERE col = val"
                    await _supabase.DeleteAsync("profile_skills", "profile_id", id.ToString());

                    // B. Insertamos los nuevos skills uno por uno
                    foreach (var skill in skillsToUpdate)
                    {
                        // Aseguramos que el ID sea el correcto
                        skill.ProfileId = id; 
                        await _supabase.CreateAsync("profile_skills", skill);
                    }

                    // C. Los volvemos a pegar al objeto de respuesta para verlo en el JSON
                    updated.ProfileSkills = skillsToUpdate;
                }
                catch (Exception ex)
                {
                    // Opcional: Manejar error si falla la actualización de skills
                    return StatusCode(500, $"Perfil actualizado, pero error en skills: {ex.Message}");
                }
            }

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