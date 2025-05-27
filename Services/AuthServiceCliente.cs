using MantenimientoEscolarCliente.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace MantenimientoEscolarCliente.Services
{
    public class AuthServiceCliente
    {
        private readonly HttpClient _httpClient;

        public AuthServiceCliente(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsuarioTokenResponse?> LoginAsync(string correo)
        {
            var login = new UsuarioLogin { Correo = correo };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", login);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UsuarioTokenResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return null;
        }
    }
}
