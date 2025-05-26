using MantenimientoEscolarCliente.Models;
using System.Text.Json;
namespace MantenimientoEscolarCliente.Services
{
    public class SolicitudService
    {
        private readonly HttpClient _httpClient;

        public SolicitudService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<List<SolicitudViewModel>> ObtenerTodasAsync()
        {
            var response = await _httpClient.GetAsync("api/solicitudes");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content); // Para depuración

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var solicitudes = JsonSerializer.Deserialize<List<SolicitudViewModel>>(content, options);
                return solicitudes ?? new List<SolicitudViewModel>();
            }
            else
            {
                // Manejo de errores según sea necesario
                return new List<SolicitudViewModel>();
            }
        }


        public async Task<SolicitudViewModel> ObtenerPorIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/solicitudes/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content); // Para depuración

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var solicitud = JsonSerializer.Deserialize<SolicitudViewModel>(content, options);
                return solicitud;
            }
            else
            {
                // Manejo de errores según sea necesario
                return null;
            }
        }


    }
}
