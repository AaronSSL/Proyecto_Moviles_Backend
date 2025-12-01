using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Supabase;

namespace Api.Services
{
    public class SupabaseService
    {
        public Client Client { get; private set; }

        public SupabaseService(IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var url = config["Supabase:Url"];
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Configuration value 'Supabase:Url' is required and was not found.", nameof(config));

            var key = config["Supabase:Key"];

            Client = new Client(url, key, new SupabaseOptions
            {
                AutoConnectRealtime = true
            });

            Client.InitializeAsync().GetAwaiter().GetResult();
            // Prepare an HttpClient for REST calls to PostgREST
            _baseUrl = url.TrimEnd('/');
            _apiKey = key;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };

            if (!string.IsNullOrWhiteSpace(_apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string? _apiKey;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<List<T>> GetAllAsync<T>(string table)
        {
            var resp = await _httpClient.GetAsync($"/rest/v1/{table}?select=*");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
        }

        public async Task<T?> GetByIdAsync<T>(string table, string keyColumn, string keyValue)
        {
            // keyValue should be already URL-encoded if needed
            var resp = await _httpClient.GetAsync($"/rest/v1/{table}?{keyColumn}=eq.{Uri.EscapeDataString(keyValue)}&select=*");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            return list != null && list.Count > 0 ? list[0] : default;
        }

        public async Task<T?> CreateAsync<T>(string table, T item)
        {
            var body = JsonSerializer.Serialize(item);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/rest/v1/{table}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Prefer", "return=representation");

            var resp = await _httpClient.SendAsync(request);

            // --- DETECTOR DE ERRORES (Igual que en Update) ---
            if (!resp.IsSuccessStatusCode)
            {
                var errorContent = await resp.Content.ReadAsStringAsync();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n========================================");
                Console.WriteLine($"🔴 ERROR CREATE (409 CONFLICTO?):");
                Console.WriteLine($"Status: {resp.StatusCode}");
                Console.WriteLine($"Mensaje: {errorContent}");
                Console.WriteLine($"JSON: {body}");
                Console.WriteLine("========================================\n");
                Console.ResetColor();

                throw new HttpRequestException($"Supabase Create Error: {errorContent}");
            }
            // -------------------------------------------------

            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            return list != null && list.Count > 0 ? list[0] : default;
        }

    public async Task<T?> UpdateAsync<T>(string table, string keyColumn, string keyValue, T item)
    {
        var body = JsonSerializer.Serialize(item);
        
        // Usamos PATCH, que es lo estándar para actualizar parcialmente en Supabase
        var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/rest/v1/{table}?{keyColumn}=eq.{Uri.EscapeDataString(keyValue)}")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("Prefer", "return=representation");

        var resp = await _httpClient.SendAsync(request);

        // --- DETECTOR DE ERRORES ---
        if (!resp.IsSuccessStatusCode)
        {
            var errorContent = await resp.Content.ReadAsStringAsync();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n========================================");
            Console.WriteLine($"🔴 ERROR CRÍTICO AL ACTUALIZAR (UPDATE):");
            Console.WriteLine($"Status Code: {resp.StatusCode}");
            Console.WriteLine($"Mensaje de Supabase: {errorContent}");
            Console.WriteLine($"Datos enviados: {body}");
            Console.WriteLine("========================================\n");
            Console.ResetColor();

            // Lanzamos el error con el detalle para que no se pierda
            throw new HttpRequestException($"Supabase Update Error: {errorContent}");
        }
        // ---------------------------

        var json = await resp.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
        return list != null && list.Count > 0 ? list[0] : default;
    }

        public async Task DeleteAsync(string table, string keyColumn, string keyValue)
        {
            var resp = await _httpClient.DeleteAsync($"/rest/v1/{table}?{keyColumn}=eq.{Uri.EscapeDataString(keyValue)}");
            resp.EnsureSuccessStatusCode();
        }

        // Agrega esto en SupabaseService.cs

    // Método especial para borrar con DOBLE condición (Ej: profile_id AND skill_id)
    public async Task DeleteCompositeAsync(string table, string col1, string val1, string col2, string val2)
    {
        // Construimos la URL con DOS filtros: ?col1=eq.val1 & col2=eq.val2
        var url = $"/rest/v1/{table}?{col1}=eq.{Uri.EscapeDataString(val1)}&{col2}=eq.{Uri.EscapeDataString(val2)}";

        var resp = await _httpClient.DeleteAsync(url);

        // Detector de errores (Mismo estilo que los otros)
        if (!resp.IsSuccessStatusCode)
        {
            var errorContent = await resp.Content.ReadAsStringAsync();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"🔴 ERROR DELETE COMPOSITE: {errorContent}");
            Console.ResetColor();
            throw new HttpRequestException($"Supabase Delete Error: {errorContent}");
        }
    }


    }
}
