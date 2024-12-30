using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class ConsultaPagoService : IConsultaPagoService
{
    private readonly IConsultaPagoRepository _repository;

    public ConsultaPagoService(IConsultaPagoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ConsultaPagoViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
    {
        return await _repository.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
    }

    public async Task<List<SelectListItem>> ObtenerListaProveedoresAsync()
    {
        return await _repository.ObtenerListaProveedoresAsync();
    }

    public async Task<List<ConsultaPagoViewModel>> ObtenerAbonosAsync(string nombreRazonSocial)
    {
        return await _repository.ObtenerAbonosAsync(nombreRazonSocial);
    }


    public async Task<byte[]> ExportarPagosAPdfAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
    {
        var pagos = await _repository.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
        return await _repository.ExportarPagosAPdfAsync(pagos);
    }

    public async Task<byte[]> ExportarPagosAExcelAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
    {
        var pagos = await _repository.ObtenerProveedoresAsync(fechaInicio, fechaFin, proveedorNombre, formaPago);
        return await _repository.ExportarPagosAExcelAsync(pagos);
    }

}
