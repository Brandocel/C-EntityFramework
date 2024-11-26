using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                        DiasCredito = reader.IsDBNull(7) ? Convert.ToDateTime(reader["diascredito"]) : reader.GetDateTime(7),
                        Importe = reader.IsDBNull(8) ? (decimal?)null : reader.GetDecimal(8),
                        Total = reader.IsDBNull(9) ? (decimal?)null : reader.GetDecimal(9)
                    });
                }
            }
            return pagos;
        }

        public async Task<List<string>> ObtenerNombresProveedoresAsync()
        {
            var nombres = new List<string>();
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
                            // Verifica si el valor es nulo antes de intentar leerlo
                            if (!reader.IsDBNull(0))
                            {
                                nombres.Add(reader.GetString(0));
                            }
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
                    connection.Open();
                    var query = @"INSERT INTO BancoProveedores (formapago, banco, cuentabancaria, conceptopago, sucursal)
                                  VALUES (@FormaPago, @Banco, @CuentaBancaria, @ConceptoPago, @Sucursal)";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FormaPago", model.FormaPago);
                    command.Parameters.AddWithValue("@Banco", model.Banco);
                    command.Parameters.AddWithValue("@CuentaBancaria", model.CuentaBancaria);
                    command.Parameters.AddWithValue("@ConceptoPago", model.ConceptoPago);
                    command.Parameters.AddWithValue("@Sucursal", model.Sucursal);

                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
