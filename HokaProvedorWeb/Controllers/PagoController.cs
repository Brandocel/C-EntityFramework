using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using HokaProvedorWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Controllers
{
    public class PagoController : Controller
    {
        private readonly IPagoService _pagoService;

        public PagoController(IPagoService pagoService)
        {
            _pagoService = pagoService;
        }

        public async Task<IActionResult> TipoSolicitud()
        {
            return View();
        }

        public async Task<IActionResult> Pago()
        {
            var model = new PagoViewModel
            {
                NombreRazonSocialOptions = await _pagoService.ObtenerNombresProveedoresAsync(),
                SucursalOptions = await _pagoService.ObtenerSucursalesAsync(),
                Pagos = await _pagoService.ObtenerPagosAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Pago(PagoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _pagoService.GuardarPagoAsync(model);
                if (success)
                {
                    TempData["Message"] = "Pago guardado correctamente.";
                    return RedirectToAction(nameof(Pago));
                }
            }

            TempData["Error"] = "Ocurrió un error al guardar el pago.";
            return View(model);
        }
    }
}
