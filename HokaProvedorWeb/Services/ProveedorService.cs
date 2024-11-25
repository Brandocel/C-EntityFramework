using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;

        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProveedorViewModel>> ObtenerProveedoresAsync()
        {
           
            var proveedores = await _repository.ObtenerTodosAsync();


            return proveedores.Select(p => new ProveedorViewModel
            {
                Provedor = p.Provedor,
                Nombre = p.Nombre,
                NombreRazonSocial = p.NombreRazonSocial,
                Ciudad = p.Ciudad,
                Estado = p.Estado,
                Telefonos = p.Telefonos,
                Email = p.Email,
                LimiteCreditoValor = (float?)p.LimiteCreditoValor
            }).ToList();
        }

        public async Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor)
        {
            return await _repository.GuardarProveedorAsync(proveedor);
        }

      
    }
}
