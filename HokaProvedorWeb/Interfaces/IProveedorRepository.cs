using HokaProvedorWeb.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IProveedorRepository
    {
        Task<List<Proveedor>> ObtenerTodosAsync();
    }
}
