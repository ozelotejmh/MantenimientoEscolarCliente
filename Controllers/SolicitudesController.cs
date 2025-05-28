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

        public async Task<IActionResult> Editar(int id)
        {
            EstablecerTokenDesdeCookie();
            var solicitudes = await _solicitudService.ObtenerPorIdAsync(id);
            var solicitud = solicitudes?.FirstOrDefault();
            if (solicitud == null)
                return NotFound();

            return View(solicitud);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SolicitudViewModel solicitud)
        {
            if (!ModelState.IsValid)
                return View(solicitud);

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
                return View(solicitud);
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
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar solicitud: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
