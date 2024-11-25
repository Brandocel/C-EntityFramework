using HokaProvedorWeb.Interfaces;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Services
{
    public class ConceptoService : IConceptoService
    {
        private readonly IConceptoRepository _repository;

        public ConceptoService(IConceptoRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AgregarConceptoAsync(string conceptoPago)
        {
            return await _repository.AgregarConceptoAsync(conceptoPago);
        }
    }
}
