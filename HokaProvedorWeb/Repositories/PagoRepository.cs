using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Repositories
{
    public class PagoRepository : IPagoRepository
    {
        private readonly string _connectionString;

        public PagoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<PagoViewModel>> ObtenerPagosAsync()
        {
            var pagos = new List<PagoViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT bp.Id, p.nombre_rasonsocial, bp.formapago, bp.banco, bp.cuentabancaria, 
                              bp.conceptopago, bp.sucursal, p.diascredito, 
                              TRY_CAST(em.importe AS decimal(18, 2)) AS importe,
                              TRY_CAST(em.stotal AS decimal(18, 2)) AS Total
                              FROM BancoProveedores bp
                              INNER JOIN provedores p ON bp.nombre_rasonsocial = p.nombre_rasonsocial
                              LEFT JOIN entradaM em ON bp.Id = em.Folio_entrada";

                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    pagos.Add(new PagoViewModel
                    {
                        Id = reader.GetInt32(0),
                        NombreRazonSocial = reader.GetString(1),
                        FormaPago = reader.GetString(2),
                        Banco = reader.GetString(3),
                        CuentaBancaria = reader.GetString(4),
                        ConceptoPago = reader.GetString(5),
                        Sucursal = reader.GetString(6),
                        DiasCredito = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7),
                        Importe = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        Total = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9)
                    });
                }
            }
            return pagos;
        }

        public async Task<List<SelectListItem>> ObtenerNombresProveedoresAsync()
        {
            var nombres = new List<SelectListItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT DISTINCT nombre_rasonsocial FROM provedores";
                using (var command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Verifica si el valor es nulo antes de agregarlo a la lista
                            var nombreRazonSocial = reader.IsDBNull(0) ? "Sin Nombre" : reader.GetString(0);
                            nombres.Add(new SelectListItem
                            {
                                Value = nombreRazonSocial,
                                Text = nombreRazonSocial
                            });
                        }
                    }
                }
            }
            return nombres;
        }


        public async Task<List<SelectListItem>> ObtenerSucursalesAsync()
        {
            var sucursales = new List<SelectListItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT Nombre, ciudad FROM almacenes";
                var command = new SqlCommand(query, connection);
                connection.Open();
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    sucursales.Add(new SelectListItem
                    {
                        Value = reader.GetString(0),
                        Text = $"{reader.GetString(0)} - {reader.GetString(1)}"
                    });
                }
            }
            return sucursales;
        }

        public async Task<bool> GuardarPagoAsync(PagoViewModel model)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Procesar archivo PDF (si está presente)
                    byte[] pdfBytes = null;
                    if (model.PdfUpload != null && model.PdfUpload.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await model.PdfUpload.CopyToAsync(memoryStream);
                            pdfBytes = memoryStream.ToArray();
                        }
                    }

                    // Insertar en la tabla provedores
                    var queryProveedores = @"INSERT INTO provedores (nombre_rasonsocial, concepto, formapago, banco, CuentaBancaria, sucursal, iva, PdfSituacionFiscal, DiasCredito)
                                     VALUES (@NombreRazonSocial, @ConceptoPago, @FormaPago, @Banco, @CuentaBancaria, @Sucursal, @Iva, @Pdf, @DiasCredito)";
                    var commandProveedores = new SqlCommand(queryProveedores, connection);

                    commandProveedores.Parameters.AddWithValue("@NombreRazonSocial", model.NombreRazonSocial ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@ConceptoPago", model.ConceptoPago ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@FormaPago", model.FormaPago ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@Banco", model.Banco ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@CuentaBancaria", model.CuentaBancaria ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@Sucursal", model.Sucursal ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@Iva", model.Iva ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@Pdf", pdfBytes ?? (object)DBNull.Value);
                    commandProveedores.Parameters.AddWithValue("@DiasCredito", model.DiasCredito ?? (object)DBNull.Value);

                    await commandProveedores.ExecuteNonQueryAsync();

                    // Generar un valor para Folio_entrada (si no está en el modelo)
                    var folioEntrada = model.Id ?? new Random().Next(1, 1000000); // Genera un valor único o utiliza uno preexistente

                    // Insertar en la tabla entradaM
                    var queryEntradaM = @"INSERT INTO entradaM (Folio_entrada, observaciones, importe, stotal)
                                  VALUES (@FolioEntrada, @Observaciones, @Importe, @Total)";
                    var commandEntradaM = new SqlCommand(queryEntradaM, connection);

                    commandEntradaM.Parameters.AddWithValue("@FolioEntrada", folioEntrada);
                    commandEntradaM.Parameters.AddWithValue("@Observaciones", model.Observaciones ?? (object)DBNull.Value);
                    commandEntradaM.Parameters.AddWithValue("@Importe", model.Importe ?? (object)DBNull.Value);
                    commandEntradaM.Parameters.AddWithValue("@Total", model.Total ?? (object)DBNull.Value);

                    await commandEntradaM.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}
