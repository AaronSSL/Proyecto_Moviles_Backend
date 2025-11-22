using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;
using System.Linq; // Necesario para el filtrado en GetByProfile

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillGapsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public SkillGapsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        // GET: api/SkillGaps/by-profile/{profileId:guid}
        // Devuelve todas las brechas detectadas para un perfil específico.
        [HttpGet("by-profile/{profileId:guid}")]
        public async Task<IActionResult> GetByProfile(Guid profileId)
        {
            // Nota: Este método obtiene TODOS los registros y filtra en memoria, 
            // similar a tu ProfileSkillsController. Es mejor optimizar la consulta en el servicio si la tabla es grande.
            var items = await _supabase.GetAllAsync<SkillGap>("skill_gaps");
            var filtered = items.FindAll(x => x.ProfileId == profileId);
            return Ok(filtered);
        }

        // POST: api/SkillGaps
        // Añade un nuevo registro de brecha.
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SkillGap skillGap)
        {
            var created = await _supabase.CreateAsync("skill_gaps", skillGap);
            // Redirecciona al endpoint de consulta por perfil tras la creación.
            return CreatedAtAction(nameof(GetByProfile), new { profileId = skillGap.ProfileId }, created);
        }

        // DELETE: api/SkillGaps
        // Elimina una brecha específica (usando la clave compuesta ProfileId y SkillId).
        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] SkillGap skillGap)
        {
            // Se utiliza la misma lógica de clave compuesta que en tu ProfileSkillsController
            // Asumiendo que la combinación ProfileId y SkillId es única para cada brecha (como clave compuesta).
            string profileKey = skillGap.ProfileId.ToString();
            string skillKey = skillGap.SkillId.ToString();

            // Formato de Supabase/PostgREST para DELETE con múltiples filtros:
            await _supabase.DeleteAsync(
                "skill_gaps", 
                "profile_id", 
                profileKey + "&skill_id=eq." + skillKey
            );
            
            return NoContent();
        }
        
       
    }
}