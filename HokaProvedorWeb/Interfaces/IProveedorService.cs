using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Interfaces
{
    public interface IProveedorService
    {
        Task<List<SelectListItem>> ObtenerListaProveedoresAsync();
        Task<List<ProveedorViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago);
        Task<bool> GuardarAbonoAsync(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono);
        Task<bool> GuardarFacturaPdfAsync(byte[] facturaPdf, string nombreRazonSocial);
        Task<bool> GuardarComprobantePdfAsync(byte[] comprobantePdf, string nombreRazonSocial);
        Task<bool> ActualizarProveedorAsync(ProveedorViewModel proveedor);
        Task<bool> EliminarProveedorAsync(int folioEntrada);
        public async Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor)
        {
            // Implementación básica o delega a otro método (si es redundante)
            return await GuardarProveedorCompletoAsync(proveedor, null, null, null);
        }
        Task<bool> GuardarProveedorCompletoAsync(AltaProveedorViewModel model, IFormFile constanciaFiscal, IFormFile comprobanteBanco, IFormFile xmlArchivo);

    }
}
