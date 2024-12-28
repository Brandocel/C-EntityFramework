using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace HokaProvedorWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _service;

        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }

        [HttpGet("ObtenerProveedores")]
        public async Task<IActionResult> ObtenerProveedores(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago)
        {
            try
            {
                var proveedores = await _service.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedor, formaPago);
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al obtener proveedores: {ex.Message}" });
            }
        }

        [HttpPost("GuardarAbono")]
        public async Task<IActionResult> GuardarAbono(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono)
        {
            try
            {
                var success = await _service.GuardarAbonoAsync(folioEntrada, abono, formaPago, fechaAbono);
                if (success)
                {
                    return Ok(new { message = "Abono guardado exitosamente." });
                }
                return BadRequest(new { message = "Error al guardar el abono." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al guardar el abono: {ex.Message}" });
            }
        }

        [HttpPost("GuardarFacturaPdf")]
        public async Task<IActionResult> GuardarFacturaPdf([FromForm] byte[] facturaPdf, string nombreRazonSocial)
        {
            try
            {
                var success = await _service.GuardarFacturaPdfAsync(facturaPdf, nombreRazonSocial);
                if (success)
                {
                    return Ok(new { message = "Factura guardada exitosamente." });
                }
                return BadRequest(new { message = "Error al guardar la factura." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al guardar la factura: {ex.Message}" });
            }
        }

        [HttpPost("GuardarComprobantePdf")]
        public async Task<IActionResult> GuardarComprobantePdf([FromForm] byte[] comprobantePdf, string nombreRazonSocial)
        {
            try
            {
                var success = await _service.GuardarComprobantePdfAsync(comprobantePdf, nombreRazonSocial);
                if (success)
                {
                    return Ok(new { message = "Comprobante guardado exitosamente." });
                }
                return BadRequest(new { message = "Error al guardar el comprobante." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al guardar el comprobante: {ex.Message}" });
            }
        }

        [HttpPut("ActualizarProveedor")]
        public async Task<IActionResult> ActualizarProveedor([FromBody] ProveedorViewModel proveedor)
        {
            try
            {
                var success = await _service.ActualizarProveedorAsync(proveedor);
                if (success)
                {
                    return Ok(new { message = "Proveedor actualizado exitosamente." });
                }
                return BadRequest(new { message = "Error al actualizar el proveedor." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al actualizar el proveedor: {ex.Message}" });
            }
        }

        [HttpDelete("EliminarProveedor/{folioEntrada}")]
        public async Task<IActionResult> EliminarProveedor(int folioEntrada)
        {
            try
            {
                var success = await _service.EliminarProveedorAsync(folioEntrada);
                if (success)
                {
                    return Ok(new { message = "Proveedor eliminado exitosamente." });
                }
                return BadRequest(new { message = "Error al eliminar el proveedor." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al eliminar el proveedor: {ex.Message}" });
            }
        }
    }
}
