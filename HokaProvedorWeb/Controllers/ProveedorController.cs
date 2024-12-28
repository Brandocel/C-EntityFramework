using Microsoft.AspNetCore.Mvc;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Controllers
{
    [Route("api/[controller]")]
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _service;

        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }

        [HttpGet("Proveedores")]
        public async Task<IActionResult> Proveedores(DateTime? fechaInicio, DateTime? fechaFin, string? proveedorNombre, string? formaPago)
        {
            ViewBag.ListaProveedores = new List<SelectListItem>
            {
                new SelectListItem { Text = "Todos", Value = "todos" },
                new SelectListItem { Text = "Proveedor 1", Value = "Proveedor 1" },
                new SelectListItem { Text = "Proveedor 2", Value = "Proveedor 2" }
            };

            var proveedores = await _service.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
            return View(proveedores);
        }

        [HttpPost("GuardarAbono")]
        public async Task<IActionResult> GuardarAbono(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono)
        {
            try
            {
                var success = await _service.GuardarAbonoAsync(folioEntrada, abono, formaPago, fechaAbono);
                if (success)
                {
                    return Ok(new { success = true, message = "Abono guardado exitosamente." });
                }
                return BadRequest(new { success = false, message = "No se pudo guardar el abono." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
            }
        }
    }
}
