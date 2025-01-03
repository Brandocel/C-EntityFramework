﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HokaProvedorWeb.Data;
using HokaProvedorWeb.Models;
using HokaProvedorWeb.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public ProveedorRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection"); // Obtener cadena desde configuración
        }

        public async Task<List<SelectListItem>> ObtenerListaProveedoresAsync()
        {
            var listaProveedores = new List<SelectListItem>();
            var query = "SELECT DISTINCT nombre_rasonsocial FROM provedores WHERE nombre_rasonsocial IS NOT NULL";

            using (var connection = _context.Database.GetDbConnection() as SqlConnection)
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            listaProveedores.Add(new SelectListItem
                            {
                                Text = reader["nombre_rasonsocial"].ToString(),
                                Value = reader["nombre_rasonsocial"].ToString()
                            });
                        }
                    }
                }
            }
            return listaProveedores;
        }
        public async Task<List<ProveedorViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago)
        {
            var query = @"
                SELECT 
                    ISNULL(p.provedor, 0) AS Provedor,
                    ISNULL(p.nombre_rasonsocial, '') AS NombreRazonSocial,
                    ISNULL(e.Observaciones, '') AS Observaciones,
                    ISNULL(e.stotal, 0) AS Total,
                    ISNULL(pa.Abono, 0) AS Abono,
                    ISNULL(e.Fecha_factura, '1900-01-01') AS FechaFactura,
                    ISNULL(e.fecha_Vencimiento, '1900-01-01') AS FechaVencimiento,
                    ISNULL(e.UUID, '') AS UUID,
                    ISNULL(e.importe, 0) AS Importe,
                    ISNULL(e.Folio_entrada, 0) AS FolioEntrada,
                    ISNULL(pa.FormaPago, '') AS FormaPago
                FROM provedores p
                LEFT JOIN entradaM e ON p.provedor = e.Almacen
                LEFT JOIN ProveedorAbono pa ON e.Folio_entrada = pa.folio_entrada
                WHERE (@FechaInicio IS NULL OR e.Fecha_factura >= @FechaInicio)
                  AND (@FechaFin IS NULL OR e.Fecha_factura <= @FechaFin)
            ";

            var proveedores = new List<ProveedorViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaInicio", fechaInicio ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FechaFin", fechaFin ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Proveedor", proveedor ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FormaPago", formaPago ?? (object)DBNull.Value);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            proveedores.Add(new ProveedorViewModel
                            {
                                Provedor = reader["Provedor"] as int?,
                                NombreRazonSocial = reader["NombreRazonSocial"] as string,
                                Observaciones = reader["Observaciones"] as string,
                                Total = reader["Total"] as decimal?,
                                Abono = reader["Abono"] as decimal?,
                                FechaFactura = reader["FechaFactura"] as DateTime?,
                                FechaVencimiento = reader["FechaVencimiento"] as DateTime?,
                                UUID = reader["UUID"] as string,
                                Importe = reader["Importe"] as decimal?,
                                FolioEntrada = reader["FolioEntrada"] as int?,
                                FormaPago = reader["FormaPago"] as string
                            });
                        }
                    }
                }
            }

            return proveedores;
        }
        public async Task<bool> GuardarAbonoAsync(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono)
        {
            var proveedor = await _context.Provedores.FirstOrDefaultAsync(p => p.FolioEntrada == folioEntrada.ToString());
            if (proveedor == null)
                return false;

            proveedor.Abono += abono;
            //proveedor. = fechaAbono;
            proveedor.FormaPago = formaPago;

            _context.Provedores.Update(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GuardarFacturaPdfAsync(byte[] facturaPdf, string nombreRazonSocial)
        {
            var proveedor = await _context.Provedores.FirstOrDefaultAsync(p => p.NombreRazonSocial == nombreRazonSocial);
            if (proveedor == null)
                return false;

            proveedor.FacturaPdf = facturaPdf;
            _context.Provedores.Update(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GuardarComprobantePdfAsync(byte[] comprobantePdf, string nombreRazonSocial)
        {
            var proveedor = await _context.Provedores.FirstOrDefaultAsync(p => p.NombreRazonSocial == nombreRazonSocial);
            if (proveedor == null)
                return false;

            proveedor.ComprobantePagoPdf = comprobantePdf;
            _context.Provedores.Update(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActualizarProveedorAsync(ProveedorViewModel proveedor)
        {
            var entity = await _context.Provedores.FirstOrDefaultAsync(p => p.FolioEntrada == proveedor.FolioEntrada.ToString());
            if (entity == null)
                return false;

            entity.Observaciones = proveedor.Observaciones;
            //entity.Total = proveedor.Total;
            //entity.FechaFactura = proveedor.FechaFactura;
            //entity.FechaVencimiento = proveedor.FechaVencimiento;

            _context.Provedores.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarProveedorAsync(int folioEntrada)
        {
            var proveedor = await _context.Provedores.FirstOrDefaultAsync(p => p.FolioEntrada == folioEntrada.ToString());
            if (proveedor == null)
                return false;

            _context.Provedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                    INSERT INTO provedores (rfc, nombre_rasonsocial, nombre, PdfSituacionFiscal) 
                    VALUES (@rfc, @nombreRazonSocial, @nombre, @pdfSituacionFiscal)";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@rfc", proveedor.RFC);
                        command.Parameters.AddWithValue("@nombreRazonSocial", proveedor.NombreRazonSocial);
                        command.Parameters.AddWithValue("@nombre", proveedor.Nombre ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@pdfSituacionFiscal", proveedor.PdfSituacionFiscal ?? (object)DBNull.Value);
                        await command.ExecuteNonQueryAsync();
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GuardarProveedorCompletoAsync(
            AltaProveedorViewModel model,
            byte[]? pdfFiscal,
            byte[]? pdfBanco,
            byte[]? xml)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Insertar en `provedores`
                var queryProveedor = @"
                    INSERT INTO provedores 
                    (rfc, nombre_rasonsocial, nombre, PdfSituacionFiscal, pdf_Comprobante, xml, Cuenta, Banco, ContactoVenta, ContactoCobranza) 
                    VALUES 
                    (@rfc, @nombreRazonSocial, @nombre, @pdfFiscal, @pdfBanco, @xml, @cuenta, @banco, @contactoVenta, @contactoCobranza)";

                using var commandProveedor = new SqlCommand(queryProveedor, connection, transaction);
                commandProveedor.Parameters.AddWithValue("@rfc", model.RFC ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@nombreRazonSocial", model.NombreRazonSocial ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@nombre", model.Nombre ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@pdfFiscal", pdfFiscal ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@pdfBanco", pdfBanco ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@xml", xml ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@cuenta", model.CuentaBancaria ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@banco", model.Banco ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@contactoVenta", model.ContactoVenta ?? (object)DBNull.Value);
                commandProveedor.Parameters.AddWithValue("@contactoCobranza", model.ContactoCobranza ?? (object)DBNull.Value);

                await commandProveedor.ExecuteNonQueryAsync();

                // Insertar en `entradaM`
                var queryEntradaM = @"
                    INSERT INTO entradaM (uuid, pdf_factura, pdf_comprobante)
                    VALUES (@uuid, @pdfFiscal, @pdfBanco)";

                using var commandEntradaM = new SqlCommand(queryEntradaM, connection, transaction);
                commandEntradaM.Parameters.AddWithValue("@uuid", Guid.NewGuid().ToString());
                commandEntradaM.Parameters.AddWithValue("@pdfFiscal", pdfFiscal ?? (object)DBNull.Value);
                commandEntradaM.Parameters.AddWithValue("@pdfBanco", pdfBanco ?? (object)DBNull.Value);

                await commandEntradaM.ExecuteNonQueryAsync();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
