using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IConsultaPagoService
    {
        Task<List<ConsultaPagoViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago);
        Task<List<SelectListItem>> ObtenerListaProveedoresAsync();
        Task<List<ConsultaPagoViewModel>> ObtenerAbonosAsync(string nombreRazonSocial);
        Task<byte[]> ExportarPagosAPdfAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago);
        Task<byte[]> ExportarPagosAExcelAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago);
    }
}
