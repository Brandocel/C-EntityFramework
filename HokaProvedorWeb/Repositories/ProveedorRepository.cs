using HokaProvedorWeb.Data;
using HokaProvedorWeb.Data.Entities;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionString;

        public ProveedorRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await _context.Provedores.ToListAsync();
        }

        public async Task<bool> GuardarProveedorAsync(AltaProveedorViewModel proveedor)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                    INSERT INTO provedores 
                    (rfc, nombre_rasonsocial, nombre, PdfSituacionFiscal) 
                    VALUES 
                    (@rfc, @nombreRazonSocial, @nombre, @pdfSituacionFiscal)";

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
    }
}
