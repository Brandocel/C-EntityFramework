using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using HokaProvedorWeb.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Services
{
    public class PagoService : IPagoService
    {
        private readonly IPagoRepository _pagoRepository;

        public PagoService(IPagoRepository pagoRepository)
        {
            _pagoRepository = pagoRepository;
        }

        public async Task<List<PagoViewModel>> ObtenerPagosAsync()
        {
            return await _pagoRepository.ObtenerPagosAsync();
        }

        public async Task<List<string>> ObtenerNombresProveedoresAsync()
        {
            return await _pagoRepository.ObtenerNombresProveedoresAsync();
        }

        public async Task<List<SelectListItem>> ObtenerSucursalesAsync()
        {
            return await _pagoRepository.ObtenerSucursalesAsync();
        }

        public async Task<bool> GuardarPagoAsync(PagoViewModel model)
        {
            return await _pagoRepository.GuardarPagoAsync(model);
        }
    }
}
