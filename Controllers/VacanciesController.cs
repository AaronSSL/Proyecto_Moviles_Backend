using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Api.Models;
using System.Collections.Generic; // Necesario para List<>

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

        // --- MÉTODO CREATE CORREGIDO ---
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Vacancy vacancy)
        {
            // 1. Separar los skills del objeto principal
            // Si no lo separamos, Supabase intentará buscar una columna 'skills' en la tabla 'vacancies' y fallará.
            var skillsToCreate = vacancy.VacancySkills;
            vacancy.VacancySkills = null; 

            // 2. Crear la Vacante (Padre)
            var createdVacancy = await _supabase.CreateAsync("vacancies", vacancy);

            // Validación por si falla la creación de la vacante
            if (createdVacancy == null) 
            {
                return BadRequest("No se pudo crear la vacante.");
            }

            // 3. Crear los Skills (Hijos) usando el ID de la vacante recién creada
            if (skillsToCreate != null && skillsToCreate.Count > 0)
            {
                foreach (var skill in skillsToCreate)
                {
                    // Asignamos el ID generado por la BD a cada skill
                    skill.VacancyId = createdVacancy.Id;
                    
                    // Guardamos en la tabla intermedia
                    await _supabase.CreateAsync("vacancy_skills", skill);
                }

                // Opcional: Volvemos a poner la lista en la respuesta para que el frontend vea lo que envió
                createdVacancy.VacancySkills = skillsToCreate;
            }

            return CreatedAtAction(nameof(GetById), new { id = createdVacancy.Id }, createdVacancy);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Vacancy vacancy)
        {
            // Nota: Para el Update, usualmente solo actualizas datos de la vacante.
            // Si quisieras actualizar skills aquí, requeriría una lógica más compleja (borrar anteriores y poner nuevos).
            // Por ahora, dejamos que actualice solo la tabla vacancies.
            vacancy.VacancySkills = null; 

            var updated = await _supabase.UpdateAsync("vacancies", "id", id.ToString(), vacancy);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Nota: Si tu base de datos tiene "ON DELETE CASCADE", esto borrará los skills automáticamente.
            // Si no, deberías borrar los skills manualmente antes de borrar la vacante.
            await _supabase.DeleteAsync("vacancies", "id", id.ToString());
            return NoContent();
        }
    }
}