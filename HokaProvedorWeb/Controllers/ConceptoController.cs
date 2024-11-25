using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Controllers
{
    public class ConceptoController : Controller
    {
        private readonly IConceptoService _service;

        public ConceptoController(IConceptoService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Concepto()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AgregarConcepto(string conceptoPago)
        {
            if (string.IsNullOrWhiteSpace(conceptoPago))
            {
                return Json(new { success = false, message = "El concepto es obligatorio." });
            }

            bool result = await _service.AgregarConceptoAsync(conceptoPago);

            if (result)
            {
                return Json(new { success = true, message = "Concepto agregado exitosamente." });
            }
            else
            {
                return Json(new { success = false, message = "Error al guardar el concepto." });
            }
        }
    }
}
