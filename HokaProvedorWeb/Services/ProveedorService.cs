using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;

namespace HokaProvedorWeb.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;

        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProveedorViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago)
        {
            return await _repository.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedor, formaPago);
        }

        public async Task<bool> GuardarAbonoAsync(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono)
        {
            // Aquí puedes agregar lógica adicional, como validar que el abono no exceda el saldo
            return await _repository.GuardarAbonoAsync(folioEntrada, abono, formaPago, fechaAbono);
        }

        public async Task<bool> GuardarFacturaPdfAsync(byte[] facturaPdf, string nombreRazonSocial)
        {
            // Validar que el archivo no esté vacío
            if (facturaPdf == null || facturaPdf.Length == 0)
                throw new ArgumentException("El archivo de factura no puede estar vacío.");

            return await _repository.GuardarFacturaPdfAsync(facturaPdf, nombreRazonSocial);
        }

        public async Task<bool> GuardarComprobantePdfAsync(byte[] comprobantePdf, string nombreRazonSocial)
        {
            // Validar que el archivo no esté vacío
            if (comprobantePdf == null || comprobantePdf.Length == 0)
                throw new ArgumentException("El archivo de comprobante no puede estar vacío.");

            return await _repository.GuardarComprobantePdfAsync(comprobantePdf, nombreRazonSocial);
        }

        public async Task<bool> ActualizarProveedorAsync(ProveedorViewModel proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor));

            return await _repository.ActualizarProveedorAsync(proveedor);
        }

        public async Task<bool> EliminarProveedorAsync(int folioEntrada)
        {
            return await _repository.EliminarProveedorAsync(folioEntrada);
        }

        public async Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor)
        {
            // Implementación del método
            return await _repository.GuardarProveedorAsync(proveedor);
        }
    }
}
