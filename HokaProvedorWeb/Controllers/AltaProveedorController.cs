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
        public async Task<IActionResult> GuardarProveedor(AltaProveedorViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool success = await _service.GuardarProveedorAsync(model);
                if (success)
                {
                    TempData["Message"] = "Proveedor guardado correctamente.";
                    return RedirectToAction("AltaProveedor");
                }
                ModelState.AddModelError("", "Error al guardar el proveedor.");
            }
            return View("AltaProveedor", model);
        }
    }
}
