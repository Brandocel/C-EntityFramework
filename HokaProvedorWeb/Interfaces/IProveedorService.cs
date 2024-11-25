

using HokaProvedorWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IProveedorService
    {
        Task<List<ProveedorViewModel>> ObtenerProveedoresAsync();
        Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor);
    }
}
