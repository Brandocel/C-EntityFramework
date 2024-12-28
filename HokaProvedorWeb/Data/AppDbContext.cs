using HokaProvedorWeb.Data.Entities;
using HokaProvedorWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace HokaProvedorWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proveedor> Provedores { get; set; }
        public DbSet<Concepto> Conceptos { get; set; }
        public DbSet<ProveedorAbono> ProveedorAbonos { get; set; }
        public IEnumerable<object> Proveedores { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Proveedor>().ToTable("provedores");
            //modelBuilder.Entity<Proveedor>().HasKey(p => p.Provedor); // Usa el campo `provedor` como clave primaria

            //modelBuilder.Entity<Proveedor>()
            //    .Property(p => p.FacturaPdf)
            //    .HasColumnName("pdf_factura");

            //modelBuilder.Entity<Proveedor>()
            //    .Property(p => p.ComprobantePagoPdf)
            //    .HasColumnName("pdf_comprobante");

            modelBuilder.Entity<ProveedorViewModel>(entity =>
            {
                // Configurar el mapeo de propiedades
                entity.HasNoKey(); // Indicar que no tiene clave primaria
                entity.ToTable("provedores"); // Nombre de la tabla

                entity.Property(e => e.Provedor).HasColumnName("provedor");
                //entity.Property(e => e.Nombre).HasColumnName("nombre");
                entity.Property(e => e.NombreRazonSocial).HasColumnName("nombre_rasonsocial");
                entity.Property(e => e.Observaciones).HasColumnName("Observaciones");
                entity.Property(e => e.Total).HasColumnName("stotal");
                entity.Property(e => e.Abono).HasColumnName("Abono");
                entity.Property(e => e.FechaFactura).HasColumnName("Fecha_factura");
                entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_Vencimiento");
                entity.Property(e => e.UUID).HasColumnName("UUID");
                entity.Property(e => e.Importe).HasColumnName("importe");
                entity.Property(e => e.FolioEntrada).HasColumnName("Folio_entrada");
                entity.Property(e => e.FormaPago).HasColumnName("FormaPago");
            });


            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ConceptoPago).HasColumnName("Concepto");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
