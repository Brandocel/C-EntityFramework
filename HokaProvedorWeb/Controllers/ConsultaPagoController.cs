using Microsoft.AspNetCore.Mvc;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HokaProvedorWeb.Controllers
{
    public class ConsultaPagoController : Controller
    {
        private readonly IConsultaPagoService _service;

        public ConsultaPagoController(IConsultaPagoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ConsultaPago()
        {
            ViewBag.ListaProveedores = await _service.ObtenerListaProveedoresAsync();
            return View(new List<ConsultaPagoViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> ConsultaPago(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
        {
            var proveedores = await _service.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
            ViewBag.ListaProveedores = await _service.ObtenerListaProveedoresAsync();
            ViewBag.ProveedorNombreSeleccionado = proveedorNombre;
            return View(proveedores);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToPdf(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
        {
            var pdfFile = await _service.ExportarPagosAPdfAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
            return File(pdfFile, "application/pdf", "ConsultaPagos.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
        {
            var excelFile = await _service.ExportarPagosAExcelAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ConsultaPago.xlsx");
        }

        [HttpGet]
        public async Task<JsonResult> GetAbonos(string nombreRazonSocial)
        {
            var abonos = await _service.ObtenerAbonosAsync(nombreRazonSocial);
            return Json(abonos);
        }
    }
}
