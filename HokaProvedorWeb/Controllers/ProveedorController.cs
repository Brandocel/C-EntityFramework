using HokaProvedorWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _service;

        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago)
        {
            var proveedores = await _service.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedor, formaPago);
            return View(proveedores);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarAbono(int folioEntrada, decimal abono, string formaPago)
        {
            var success = await _service.GuardarAbonoAsync(folioEntrada, abono, formaPago, DateTime.Now);
            return Json(new { success });
        }

        [HttpPost]
        public async Task<IActionResult> SubirFactura(IFormFile facturaFile, string nombreRazonSocial)
        {
            var success = await _service.SubirFacturaAsync(facturaFile, nombreRazonSocial);
            return Json(new { success });
        }
    }
}
