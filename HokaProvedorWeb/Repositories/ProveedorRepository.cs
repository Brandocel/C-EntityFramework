using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HokaProvedorWeb.Data;
using HokaProvedorWeb.Models;
using HokaProvedorWeb.Interfaces;
using Microsoft.Data.SqlClient;

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


        public async Task<List<ProveedorViewModel>> ObtenerProveedoresAsync(DateTime? fechaInicio, DateTime? fechaFin, string proveedor, string formaPago)
        {
            var query = _context.Provedores.AsQueryable();

            if (fechaInicio.HasValue)
                query = query.Where(p => p.FechaFactura >= fechaInicio);

            if (fechaFin.HasValue)
                query = query.Where(p => p.FechaFactura <= fechaFin);

            if (!string.IsNullOrEmpty(proveedor) && proveedor != "todos")
                query = query.Where(p => p.NombreRazonSocial.Contains(proveedor));

            if (!string.IsNullOrEmpty(formaPago))
                query = query.Where(p => p.FormaPago == formaPago);

            return await query
                .Select(p => new ProveedorViewModel
                {
                    Provedor = p.Provedor,
                    NombreRazonSocial = p.NombreRazonSocial,
                    Observaciones = p.Observaciones,
                    Total = p.Total,
                    Abono = p.Abono,
                    FechaFactura = p.FechaFactura,
                    FechaVencimiento = p.FechaVencimiento,
                    UUID = p.UUID,
                    Importe = p.Importe,
                    IVA = p.IVA,
                    FormaPago = p.FormaPago,
                    FacturaPdf = p.FacturaPdf,
                    ComprobantePagoPdf = p.ComprobantePagoPdf,
                    FolioEntrada = p.FolioEntrada,
                }).ToListAsync();
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
            entity.Total = proveedor.Total;
            entity.FechaFactura = proveedor.FechaFactura;
            entity.FechaVencimiento = proveedor.FechaVencimiento;

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
    }
}
