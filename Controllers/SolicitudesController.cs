using Microsoft.AspNetCore.Mvc;
using MantenimientoEscolarCliente.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using MantenimientoEscolarCliente.Models;
using System.Linq;

namespace MantenimientoEscolarCliente.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly SolicitudService _solicitudService;

        public SolicitudesController(SolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        private void EstablecerTokenDesdeCookie()
        {
            if (Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                _solicitudService.EstablecerToken(token);
            }
        }

        public async Task<IActionResult> Index()
        {
            EstablecerTokenDesdeCookie();
            var solicitudes = await _solicitudService.ObtenerTodasAsync();
            return View(solicitudes);
        }

        public async Task<IActionResult> SolicitudesPorUsuario(int id)
        {
            EstablecerTokenDesdeCookie();
            var solicitudes = await _solicitudService.ObtenerPorIdAsync(id);
            if (solicitudes == null || !solicitudes.Any())
            {
                return NotFound();
            }

            return View("Index", solicitudes);
        }

        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(CrearSolicitudDTO solicitud)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Hay errores en el formulario.";
                return View(solicitud);
            }

            EstablecerTokenDesdeCookie();

            try
            {
                await _solicitudService.CrearAsync(solicitud);
                TempData["SuccessMessage"] = "Solicitud creada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear solicitud: {ex.Message}";
                return View(solicitud);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            EstablecerTokenDesdeCookie();
            var solicitudes = await _solicitudService.ObtenerPorIdAsync(id);
            var solicitud = solicitudes?.FirstOrDefault();
            if (solicitud == null)
                return NotFound();

            return View("Editar", solicitud);
        }

        //[HttpPost]
        public async Task<IActionResult> EditarVista(ActualizarSolicitudDTO solicitud)
        {
            if (!ModelState.IsValid)
                return View("Editar", solicitud);

            EstablecerTokenDesdeCookie();

            var actualizarDto = new ActualizarSolicitudDTO
            {
                idSolicitud = solicitud.idSolicitud,
                descripcion = solicitud.descripcion,
                ubicacion = solicitud.ubicacion,
                estado = solicitud.estado
            };

            try
            {
                await _solicitudService.ActualizarAsync(actualizarDto);
                TempData["SuccessMessage"] = "Solicitud actualizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar solicitud: {ex.Message}";
                return View("Editar", solicitud);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            EstablecerTokenDesdeCookie();

            try
            {
                await _solicitudService.EliminarAsync(id);
                TempData["SuccessMessage"] = "Solicitud eliminada correctamente.";
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "Error de conexión con el servidor: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Intentar obtener el mensaje de error que viene en la respuesta de la API
                string mensajeError = ex.Message;

                if (ex.InnerException != null)
                    mensajeError = ex.InnerException.Message;

                if (mensajeError.Contains("No se puede eliminar una solicitud"))
                {
                    TempData["ErrorMessage"] = "No se puede eliminar esta solicitud porque ya está asignada a un técnico.";
                }
                else if (mensajeError.Contains("Solo se pueden eliminar solicitudes en estado Pendiente"))
                {
                    TempData["ErrorMessage"] = "Solo se pueden eliminar solicitudes en estado Pendiente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al eliminar solicitud: " + mensajeError;
                }
            }

            return RedirectToAction("Index");
        }

    }
}