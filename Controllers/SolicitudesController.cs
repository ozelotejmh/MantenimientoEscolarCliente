using Microsoft.AspNetCore.Mvc;
using MantenimientoEscolarCliente.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using MantenimientoEscolarCliente.Models;
namespace MantenimientoEscolarCliente.Controllers
{
    public class SolicitudesController : Controller
    {
        private readonly SolicitudService _solicitudService;

        public SolicitudesController(SolicitudService solicitudService)
        {
            _solicitudService = solicitudService;
        }

        public async Task<IActionResult> Index()
        {
            var solicitudes = await _solicitudService.ObtenerTodasAsync();
            return View(solicitudes);
        }

        public async Task<IActionResult> Detalles(int id)
        {
            var solicitud = await _solicitudService.ObtenerPorIdAsync(id);
            if (solicitud == null)
            {
                return NotFound();
            }
            return View(solicitud);
        }
    }
}
