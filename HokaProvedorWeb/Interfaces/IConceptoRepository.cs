using HokaProvedorWeb.Data.Entities;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IConceptoRepository
    {
        Task<bool> AgregarConceptoAsync(string conceptoPago);
    }
}
