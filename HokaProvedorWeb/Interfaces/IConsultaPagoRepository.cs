using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IConsultaPagoRepository
    {
        Task<List<ConsultaPagoViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago);
        Task<List<SelectListItem>> ObtenerListaProveedoresAsync();
        Task<List<ConsultaPagoViewModel>> ObtenerAbonosAsync(string nombreRazonSocial);
        Task<byte[]> ExportarPagosAPdfAsync(List<ConsultaPagoViewModel> pagos);
        Task<byte[]> ExportarPagosAExcelAsync(List<ConsultaPagoViewModel> pagos);
    }
}
