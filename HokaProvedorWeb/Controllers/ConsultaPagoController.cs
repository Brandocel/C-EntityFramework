using Microsoft.AspNetCore.Mvc;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Pdf;
using Org.BouncyCastle.Security;
using iText.Layout.Properties;



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
            var proveedores = await _service.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);

            using (var stream = new MemoryStream())
            {
                var writer = new PdfWriter(stream);
                var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Encabezado
                document.Add(new Paragraph("Reporte de Pagos")
                    .SetFontSize(18)
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                // Tabla
                var table = new Table(new float[] { 2, 2, 2, 2, 2 }).SetWidth(UnitValue.CreatePercentValue(100));

                table.AddHeaderCell("Fecha Factura");
                table.AddHeaderCell("Proveedor");
                table.AddHeaderCell("Razón Social");
                table.AddHeaderCell("Total");
                table.AddHeaderCell("Forma de Pago");

                foreach (var item in proveedores)
                {
                    table.AddCell(item.FechaFactura?.ToShortDateString() ?? "N/A");
                    table.AddCell(item.Proveedor ?? "N/A");
                    table.AddCell(item.NombreRazonSocial ?? "N/A");
                    table.AddCell(item.Total.HasValue ? item.Total.Value.ToString("C") : "N/A");
                    table.AddCell(item.FormaPago ?? "N/A");
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", "ReportePagos.pdf");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
        {
            var pagos = await _service.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);

            using (var workbook = new ClosedXML.Excel.XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Pagos");
                worksheet.Cell(1, 1).Value = "Fecha Factura";
                worksheet.Cell(1, 2).Value = "Proveedor";
                worksheet.Cell(1, 3).Value = "Razón Social";
                worksheet.Cell(1, 4).Value = "Total";
                worksheet.Cell(1, 5).Value = "Abono";

                int row = 2;
                foreach (var pago in pagos)
                {
                    worksheet.Cell(row, 1).Value = pago.FechaFactura?.ToShortDateString() ?? "N/A";
                    worksheet.Cell(row, 2).Value = pago.Proveedor ?? "N/A";
                    worksheet.Cell(row, 3).Value = pago.NombreRazonSocial ?? "N/A";
                    worksheet.Cell(row, 4).Value = pago.Total ?? 0;
                    worksheet.Cell(row, 5).Value = pago.Abono ?? 0;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ConsultaPago.xlsx");
                }
            }
        }


        [HttpGet]
        public async Task<JsonResult> GetAbonos(string nombreRazonSocial)
        {
            var abonos = await _service.ObtenerAbonosAsync(nombreRazonSocial);
            return Json(abonos);
        }

    }
}
