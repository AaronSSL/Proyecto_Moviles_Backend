namespace Api.DTOs
{
    public class LoginDto
    {
        // Agregamos "= string.Empty;" al final
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}