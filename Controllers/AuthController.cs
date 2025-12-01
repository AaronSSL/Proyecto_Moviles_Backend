using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Api.Services;
using Api.DTOs;   // Para usar el archivo que acabamos de crear
using Api.Models; // Para usar Profile

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SupabaseService _supabase;

        public AuthController(SupabaseService supabase)
        {
            _supabase = supabase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData)
        {
            // 1. Validamos que manden datos
            if (string.IsNullOrEmpty(loginData.Email) || string.IsNullOrEmpty(loginData.Password))
            {
                return BadRequest("Email y contraseña son obligatorios.");
            }

            try
            {
                Console.WriteLine($"🔍 Intentando login para: {loginData.Email}");

                // 2. Preguntamos a Supabase Auth si la contraseña es correcta
                var session = await _supabase.Client.Auth.SignIn(loginData.Email, loginData.Password);

                if (session == null || session.User == null)
                {
                    return Unauthorized(new { message = "Credenciales incorrectas o usuario no encontrado en Auth." });
                }

                var userId = session.User.Id;
                Console.WriteLine($"✅ Auth correcto. ID Usuario: {userId}");

                // 3. Buscamos el perfil en tu tabla 'profiles' para saber el ROL
                // Usamos el método GetByIdAsync que ya tienes funcionando
                var profile = await _supabase.GetByIdAsync<Profile>("profiles", "id", userId);

                if (profile == null)
                {
                    return NotFound(new { message = "Usuario existe en Auth, pero no tiene datos en la tabla 'profiles'." });
                }

                // 4. ¡Éxito! Devolvemos el perfil con el rol
                return Ok(profile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔴 Error en Login: {ex.Message}");
                // Tip: Supabase lanza error si el email no está confirmado o la pass está mal
                return BadRequest(new { message = $"Error de autenticación: {ex.Message}" });
            }
        }
    }
}