using System.Threading.Tasks;

namespace HokaProvedorWeb.Interfaces
{
    public interface IConceptoService
    {
        Task<bool> AgregarConceptoAsync(string conceptoPago);
    }
}
