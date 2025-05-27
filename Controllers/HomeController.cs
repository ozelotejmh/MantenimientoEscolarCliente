using MantenimientoEscolarCliente.Models;
using MantenimientoEscolarCliente.Services;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly AuthServiceCliente _authService;

    public HomeController(AuthServiceCliente authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new UsuarioLogin());
    }


    [HttpPost]
    public async Task<IActionResult> Index(UsuarioLogin model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _authService.LoginAsync(model.Correo);

        if (result != null)
        {
            Response.Cookies.Append("AuthToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            TempData["Usuario"] = result.Nombre;
            TempData["Rol"] = result.TipoUsuario;

            return RedirectToAction("Index", "Solicitudes");
        }

        ModelState.AddModelError(string.Empty, "Correo inválido");
        return View(model);
    }

}
