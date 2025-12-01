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
    public class SugerenciasController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public SugerenciasController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // OJO: Verifica si tu tabla en Supabase se llama "Sugerencia" o "sugerencia"
            var items = await _supabase.GetAllAsync<Sugerencia>("Sugerencia");
            return Ok(items);
        }

        // Cambiamos la restricción de ruta de guid a long porque tu ID es numérico
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            // Convertimos el long a string para pasarlo al servicio
            var item = await _supabase.GetByIdAsync<Sugerencia>("Sugerencia", "id", id.ToString());
            
            if (item == null) return NotFound();
            
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Sugerencia sugerencia)
        {
            // 1. Crear Sugerencia 
            // Supabase generará el ID (autoincrement) y el user_id (si es null y hay contexto de auth)
            var createdSugerencia = await _supabase.CreateAsync("Sugerencia", sugerencia);

            // Verificación de seguridad
            if (createdSugerencia == null || createdSugerencia.Id == null) 
                return BadRequest("Error: No se pudo crear la sugerencia.");

            return CreatedAtAction(nameof(GetById), new { id = createdSugerencia.Id }, createdSugerencia);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] Sugerencia sugerencia)
        {
            // Nos aseguramos de no enviar el ID en el cuerpo para evitar conflictos
            sugerencia.Id = id; 
            
            var updated = await _supabase.UpdateAsync("Sugerencia", "id", id.ToString(), sugerencia);
            
            if (updated == null) return NotFound();
            
            return Ok(updated);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _supabase.DeleteAsync("Sugerencia", "id", id.ToString());
            return NoContent();
        }
    }
}