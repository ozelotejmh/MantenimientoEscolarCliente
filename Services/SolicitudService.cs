using MantenimientoEscolarCliente.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MantenimientoEscolarCliente.Services
{
    public class SolicitudService
    {
        private readonly HttpClient _httpClient;
        private string? _token;

        public SolicitudService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void EstablecerToken(string token)
        {
            _token = token;
        }

        private void AgregarTokenAHeaders()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            }
        }

        public async Task<List<SolicitudViewModel>> ObtenerTodasAsync()
        {
            if (string.IsNullOrEmpty(_token))
                return new List<SolicitudViewModel>();

            AgregarTokenAHeaders();

            var response = await _httpClient.GetAsync("api/solicitudes");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SolicitudViewModel>>(content, options) ?? new List<SolicitudViewModel>();
            }

            return new List<SolicitudViewModel>();
        }

        public async Task<List<SolicitudViewModel>> ObtenerPorIdAsync(int id)
        {
            AgregarTokenAHeaders();

            var response = await _httpClient.GetAsync($"api/solicitudes/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<SolicitudViewModel>>(content, options) ?? new List<SolicitudViewModel>();
            }

            return new List<SolicitudViewModel>();
        }

        public async Task CrearAsync(CrearSolicitudDTO solicitud)
        {
            AgregarTokenAHeaders();

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(solicitud, options);

            Console.WriteLine("JSON ENVIADO:");
            Console.WriteLine(json);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/solicitudes", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RESPUESTA:");
            Console.WriteLine(responseContent);

            response.EnsureSuccessStatusCode(); // si falla, lanza excepción
        }


        public async Task ActualizarAsync(ActualizarSolicitudDTO solicitud)
        {
            AgregarTokenAHeaders();

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(solicitud, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/solicitudes/{solicitud.idSolicitud}", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RESPUESTA UPDATE:");
            Console.WriteLine(responseContent);

            response.EnsureSuccessStatusCode();
        }


        public async Task EliminarAsync(int id)
        {
            AgregarTokenAHeaders();

            var response = await _httpClient.DeleteAsync($"api/solicitudes/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
