using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class ConsultaPagoRepository : IConsultaPagoRepository
{
    private readonly string _connectionString;

    public ConsultaPagoRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<List<ConsultaPagoViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedorNombre, string formaPago)
    {
        var proveedores = new List<ConsultaPagoViewModel>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = @"
         SELECT 
            p.provedor AS Proveedor,
            p.nombre_rasonsocial AS NombreRazonSocial,
            p.rfc AS RFC,  -- Campo RFC de la tabla provedores
            e.Folio_entrada AS FolioEntrada,
            e.stotal AS Total,
            e.Fecha_factura AS FechaFactura,
            e.Observaciones AS Observaciones,
            e.Fecha_Vencimiento AS FechaVencimiento,
            e.UUID AS UUID,
            e.Impuesto AS Impuesto,
            e.importe AS Importe,
            p.Compra_Gasto AS CompraGasto,
            p.Nota_Factura AS NotaFactura,
            p.Concepto AS Concepto,
            pa.Abono AS Abono,
            pa.FormaPago AS FormaPago,
            pa.fecha_abono AS FechaAbono
        FROM provedores p
        LEFT JOIN entradaM e ON p.provedor = e.Almacen -- Asegúrate de que esta relación sea correcta
        LEFT JOIN dbo.ProveedorAbono pa ON e.Folio_entrada = pa.folio_entrada
        WHERE (@FechaInicio IS NULL OR e.Fecha_factura >= @FechaInicio)
          AND (@FechaFin IS NULL OR e.Fecha_factura <= @FechaFin)
          AND (p.nombre_rasonsocial IS NOT NULL AND p.nombre_rasonsocial <> '')";
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FechaInicio", fechaInicio ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@FechaFin", fechaFin ?? (object)DBNull.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        proveedores.Add(new ConsultaPagoViewModel
                        {
                            FolioEntrada = reader["FolioEntrada"].ToString(),
                            Proveedor = reader["Proveedor"].ToString(),
                            NombreRazonSocial = reader["NombreRazonSocial"].ToString(),
                            RFC = reader["RFC"].ToString(),  // Asignar el RFC del proveedor
                            Total = reader["Total"] != DBNull.Value ? Convert.ToDecimal(reader["Total"]) : 0,
                            Abono = reader["Abono"] != DBNull.Value ? Convert.ToDecimal(reader["Abono"]) : 0,
                            FechaFactura = reader["FechaFactura"] != DBNull.Value ? Convert.ToDateTime(reader["FechaFactura"]) : DateTime.MinValue,
                            FechaVencimiento = reader["FechaVencimiento"] != DBNull.Value ? Convert.ToDateTime(reader["FechaVencimiento"]) : DateTime.MinValue,
                            FechaAbono = reader["FechaAbono"] != DBNull.Value ? Convert.ToDateTime(reader["FechaAbono"]) : DateTime.MinValue,
                            UUID = reader["UUID"].ToString(),
                            Impuesto = reader["Impuesto"] != DBNull.Value ? Convert.ToDecimal(reader["Impuesto"]) : 0,
                            FormaPago = reader["FormaPago"] != DBNull.Value ? reader["FormaPago"].ToString() : string.Empty,
                            Importe = reader["Importe"] != DBNull.Value ? Convert.ToDecimal(reader["Importe"]) : 0,
                            CompraGasto = reader["CompraGasto"] != DBNull.Value ? reader["CompraGasto"].ToString() : string.Empty,  // Asignar CompraGasto como string
                            NotaFactura = reader["NotaFactura"] != DBNull.Value ? reader["NotaFactura"].ToString() : string.Empty,
                            Concepto = reader["Concepto"] != DBNull.Value ? reader["Concepto"].ToString() : string.Empty

                        });
                    }
                }
            }
        }
        return proveedores;
    }


    public async Task<List<SelectListItem>> ObtenerListaProveedoresAsync()
    {
        var proveedores = new List<SelectListItem>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT DISTINCT nombre_rasonsocial FROM provedores WHERE nombre_rasonsocial IS NOT NULL AND nombre_rasonsocial <> ''";
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        proveedores.Add(new SelectListItem
                        {
                            Value = reader["nombre_rasonsocial"].ToString(),
                            Text = reader["nombre_rasonsocial"].ToString()
                        });
                    }
                }
            }
        }
        return proveedores;
    }

    public async Task<List<ConsultaPagoViewModel>> ObtenerAbonosAsync(string nombreRazonSocial)
    {
        var abonos = new List<ConsultaPagoViewModel>();
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = @"
            SELECT 
                Abono, 
                Saldo, 
                fecha_abono AS FechaAbono, 
                FormaPago 
            FROM ProveedorAbono
            WHERE nombre_rasonsocial = @NombreRazonSocial";

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@NombreRazonSocial", nombreRazonSocial);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        abonos.Add(new ConsultaPagoViewModel
                        {
                            Abono = reader["Abono"] != DBNull.Value ? Convert.ToDecimal(reader["Abono"]) : (decimal?)null,
                            Saldo = reader["Saldo"] != DBNull.Value ? Convert.ToDecimal(reader["Saldo"]) : (decimal?)null,
                            FechaAbono = reader["FechaAbono"] != DBNull.Value ? Convert.ToDateTime(reader["FechaAbono"]) : (DateTime?)null,
                            FormaPago = reader["FormaPago"] != DBNull.Value ? reader["FormaPago"].ToString() : null
                        });

                    }
                }
            }
        }
        return abonos;
    }


    //public async Task<List<ConsultaPagoViewModel>> ObtenerAbonosAsync(string nombreRazonSocial)
    //{
    //    var abonos = new List<ConsultaPagoViewModel>();
    //    using (var connection = new SqlConnection(_connectionString))
    //    {
    //        await connection.OpenAsync();
    //        string query = @"
    //        SELECT 
    //            pa.Abono, 
    //            pa.Saldo, 
    //            pa.fecha_abono AS FechaAbono,
    //            pa.FormaPago,
    //            pa.Folio_entrada AS FolioEntrada
    //        FROM ProveedorAbono pa
    //        WHERE pa.nombre_rasonsocial = @NombreRazonSocial
    //        ORDER BY pa.fecha_abono ASC";

    //        using (var command = new SqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@NombreRazonSocial", nombreRazonSocial);
    //            using (var reader = await command.ExecuteReaderAsync())
    //            {
    //                while (await reader.ReadAsync())
    //                {
    //                    abonos.Add(new ConsultaPagoViewModel
    //                    {
    //                        Abono = reader["Abono"] != DBNull.Value ? Convert.ToDecimal(reader["Abono"]) : 0,
    //                        Saldo = reader["Saldo"] != DBNull.Value ? Convert.ToDecimal(reader["Saldo"]) : 0,
    //                        FechaAbono = reader["FechaAbono"] != DBNull.Value ? Convert.ToDateTime(reader["FechaAbono"]) : DateTime.MinValue,
    //                        FormaPago = reader["FormaPago"].ToString(),
    //                        FolioEntrada = reader["FolioEntrada"].ToString()
    //                    });
    //                }
    //            }
    //        }
    //    }
    //    return abonos;
    //}


    public async Task<byte[]> ExportarPagosAPdfAsync(List<ConsultaPagoViewModel> pagos)
    {
        // Implementa la lógica de exportación a PDF como antes.
        throw new NotImplementedException("ExportarPagosAPdfAsync no está implementado.");
    }

    public async Task<byte[]> ExportarPagosAExcelAsync(List<ConsultaPagoViewModel> pagos)
    {
        // Implementa la lógica de exportación a Excel como antes.
        throw new NotImplementedException("ExportarPagosAExcelAsync no está implementado.");
    }

}
