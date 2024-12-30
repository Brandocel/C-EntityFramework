using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IPagoService
    {
        Task<List<PagoViewModel>> ObtenerPagosAsync();
        Task<List<SelectListItem>> ObtenerNombresProveedoresAsync();
        Task<List<SelectListItem>> ObtenerSucursalesAsync();
        Task<bool> GuardarPagoAsync(PagoViewModel model);
    }
}
