using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
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

        [HttpGet]
        public async Task<IActionResult> Pago()
        {
            var model = new PagoViewModel
            {
                NombreRazonSocialOptions = await _pagoService.ObtenerNombresProveedoresAsync(),
                SucursalOptions = await _pagoService.ObtenerSucursalesAsync(),
                Importe = 0,
                Total = 0
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
                TempData["Error"] = "Ocurrió un error al guardar el pago.";
            }

            model.NombreRazonSocialOptions = await _pagoService.ObtenerNombresProveedoresAsync();
            model.SucursalOptions = await _pagoService.ObtenerSucursalesAsync();

            return View(model);
        }

    }
}
