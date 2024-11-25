using HokaProvedorWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> ObtenerTodosAsync();
        Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor);
    }
}
