using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Controllers
{
    public class AltaProveedorController : Controller
    {
        private readonly IProveedorService _service;

        public AltaProveedorController(IProveedorService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult AltaProveedor()
        {
            return View(new AltaProveedorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GuardarProveedor(
            AltaProveedorViewModel model,
            IFormFile? PdfSituacionFiscal,
            IFormFile? PdfComprobanteBanco,
            IFormFile? XmlArchivo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool resultado = await _service.GuardarProveedorCompletoAsync(model, PdfSituacionFiscal, PdfComprobanteBanco, XmlArchivo);

                    if (resultado)
                    {
                        TempData["Message"] = "Proveedor guardado correctamente.";
                        return RedirectToAction("AltaProveedor");
                    }

                    ModelState.AddModelError("", "Error al guardar el proveedor.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            return View("AltaProveedor", model);
        }
    }
}
