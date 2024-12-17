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



        //Cambios 

        public async Task<List<Models.ViewModels.ProveedorViewModel>> ObtenerProveedoresFiltradosAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago)
        {
            return await _context.Provedores
                .Where(p =>
                    (!fechaInicio.HasValue || p.FechaFactura >= fechaInicio) &&
                    (!fechaFin.HasValue || p.FechaFactura <= fechaFin) &&
                    (string.IsNullOrEmpty(proveedor) || p.NombreRazonSocial.Contains(proveedor)) &&
                    (string.IsNullOrEmpty(formaPago) || p.FormaPago == formaPago))
                .Select(p => new Models.ViewModels.ProveedorViewModel
                {
                    NombreRazonSocial = p.NombreRazonSocial,
                    Importe = p.Importe ?? 0,
                    Iva = p.Iva ?? 0,
                    Total = p.Total ?? 0,
                    FechaFactura = p.FechaFactura ?? DateTime.MinValue,
                    UUID = p.UUID,
                    FormaPago = p.FormaPago,
                    Observaciones = p.Observaciones
                }).ToListAsync();
        }


        public async Task<bool> GuardarAbonoAsync(int folioEntrada, decimal abono, string formaPago, DateTime fechaAbono)
        {
            var abonoEntity = new ProveedorAbono
            {
                FolioEntrada = folioEntrada,
                Abono = abono,
                FormaPago = formaPago,
                FechaAbono = fechaAbono
            };
            _context.ProveedorAbonos.Add(abonoEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubirArchivoAsync(IFormFile file, string nombreRazonSocial, string tipoArchivo)
        {
            var proveedor = await _context.Provedores.FirstOrDefaultAsync(p => p.NombreRazonSocial == nombreRazonSocial);
            if (proveedor == null) return false;

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            if (tipoArchivo == "factura")
                proveedor.FacturaPdf = ms.ToArray();
            else if (tipoArchivo == "comprobante")
                proveedor.ComprobantePdf = ms.ToArray();

            _context.Provedores.Update(proveedor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
