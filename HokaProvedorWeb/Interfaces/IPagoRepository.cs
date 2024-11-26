using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Interfaces
{
    public interface IPagoRepository
    {
        Task<List<string>> ObtenerNombresProveedoresAsync();
        Task<List<SelectListItem>> ObtenerSucursalesAsync();
        Task<List<PagoViewModel>> ObtenerPagosAsync();
        Task<bool> GuardarPagoAsync(PagoViewModel model);
    }
}
