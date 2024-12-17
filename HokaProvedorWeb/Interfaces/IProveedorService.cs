

using HokaProvedorWeb.Models;
using HokaProvedorWeb.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IProveedorService
    {
        Task<List<Models.ViewModels.ProveedorViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago);
        Task<bool> GuardarAbonoAsync(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono);
        Task<bool> SubirFacturaAsync(IFormFile facturaFile, string nombreRazonSocial);
        Task<bool> SubirComprobanteAsync(IFormFile comprobantePagoFile, string nombreRazonSocial);

        //Task<List<ProveedorViewModel>> ObtenerProveedoresAsync();
        Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor);
    }
}
