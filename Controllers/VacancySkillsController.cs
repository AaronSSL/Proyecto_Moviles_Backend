using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VacancySkillsController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public VacancySkillsController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpGet("by-vacancy/{vacancyId:int}")]
        public async Task<IActionResult> GetByVacancy(int vacancyId)
        {
            var items = await _supabase.GetAllAsync<VacancySkill>("vacancy_skills");
            var filtered = items.FindAll(x => x.VacancyId == vacancyId);
            return Ok(filtered);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] VacancySkill vacancySkill)
        {
            var created = await _supabase.CreateAsync("vacancy_skills", vacancySkill);
            return CreatedAtAction(nameof(GetByVacancy), new { vacancyId = vacancySkill.VacancyId }, created);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] VacancySkill vacancySkill)
        {
            // Delete by composite key (vacancy_id and skill_id)
            await _supabase.DeleteAsync("vacancy_skills", "vacancy_id", vacancySkill.VacancyId + "&skill_id=eq." + vacancySkill.SkillId);
            return NoContent();
        }
    }
}
