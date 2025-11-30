using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration; // Asegúrate de tener este using
using Supabase;

namespace Api.Services
{
    public class SupabaseService
    {
        public Client Client { get; private set; }
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string? _apiKey;

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public SupabaseService(IConfiguration config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var url = config["Supabase:Url"];
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Configuration value 'Supabase:Url' is required.", nameof(config));

            var key = config["Supabase:Key"];

            Client = new Client(url, key, new SupabaseOptions
            {
                AutoConnectRealtime = true
            });

            // Inicialización asíncrona (fuego y olvido en constructor no es ideal, pero funcional para este caso)
            Client.InitializeAsync().ConfigureAwait(false);

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

        public async Task<List<T>> GetAllAsync<T>(string table)
        {
            var resp = await _httpClient.GetAsync($"/rest/v1/{table}?select=*");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
        }

        public async Task<T?> GetByIdAsync<T>(string table, string keyColumn, string keyValue)
        {
            var resp = await _httpClient.GetAsync($"/rest/v1/{table}?{keyColumn}=eq.{Uri.EscapeDataString(keyValue)}&select=*");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            return list != null && list.Count > 0 ? list[0] : default;
        }

        // --- MÉTODO MODIFICADO PARA DEBUG ---
        public async Task<T?> CreateAsync<T>(string table, T item)
        {
            var body = JsonSerializer.Serialize(item);
            var request = new HttpRequestMessage(HttpMethod.Post, $"/rest/v1/{table}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Prefer", "return=representation");

            var resp = await _httpClient.SendAsync(request);

            if (!resp.IsSuccessStatusCode)
            {
                // AQUÍ CAPTURAMOS EL ERROR REAL
                var errorContent = await resp.Content.ReadAsStringAsync();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n========================================");
                Console.WriteLine($"🔴 ERROR CRÍTICO DE SUPABASE ({resp.StatusCode}):");
                Console.WriteLine(errorContent);
                Console.WriteLine("----------------------------------------");
                Console.WriteLine($"JSON ENVIADO: {body}");
                Console.WriteLine("========================================\n");
                Console.ResetColor();

                throw new HttpRequestException($"Supabase Error: {resp.StatusCode} - {errorContent}");
            }

            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            return list != null && list.Count > 0 ? list[0] : default;
        }

        public async Task<T?> UpdateAsync<T>(string table, string keyColumn, string keyValue, T item)
        {
            var body = JsonSerializer.Serialize(item);
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"/rest/v1/{table}?{keyColumn}=eq.{Uri.EscapeDataString(keyValue)}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Add("Prefer", "return=representation");

            var resp = await _httpClient.SendAsync(request);
            
             if (!resp.IsSuccessStatusCode)
            {
                var errorContent = await resp.Content.ReadAsStringAsync();
                Console.WriteLine($"🔴 ERROR UPDATE: {errorContent}");
                throw new HttpRequestException($"Supabase Update Error: {errorContent}");
            }

            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<T>>(json, _jsonOptions);
            return list != null && list.Count > 0 ? list[0] : default;
        }

        public async Task DeleteAsync(string table, string keyColumn, string keyValue)
        {
            var resp = await _httpClient.DeleteAsync($"/rest/v1/{table}?{keyColumn}=eq.{Uri.EscapeDataString(keyValue)}");
            resp.EnsureSuccessStatusCode();
        }
    }
}