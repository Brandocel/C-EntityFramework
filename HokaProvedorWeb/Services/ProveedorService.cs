﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repository;

        public ProveedorService(IProveedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SelectListItem>> ObtenerListaProveedoresAsync()
        {
            return await _repository.ObtenerListaProveedoresAsync();
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

        public async Task<bool> GuardarProveedorCompletoAsync(
           AltaProveedorViewModel model,
           IFormFile? constanciaFiscal,
           IFormFile? comprobanteBanco,
           IFormFile? xmlArchivo)
        {
            // Convertir archivos en bytes
            byte[]? pdfFiscal = constanciaFiscal != null ? await ConvertirArchivoABytes(constanciaFiscal) : null;
            byte[]? pdfBanco = comprobanteBanco != null ? await ConvertirArchivoABytes(comprobanteBanco) : null;
            byte[]? xml = xmlArchivo != null ? await ConvertirArchivoABytes(xmlArchivo) : null;

            // Llamar al repositorio para guardar los datos
            return await _repository.GuardarProveedorCompletoAsync(model, pdfFiscal, pdfBanco, xml);
        }

        private async Task<byte[]> ConvertirArchivoABytes(IFormFile archivo)
        {
            using var memoryStream = new MemoryStream();
            await archivo.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }


    }
}
