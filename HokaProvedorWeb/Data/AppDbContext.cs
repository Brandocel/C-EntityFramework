using HokaProvedorWeb.Data.Entities;
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
            modelBuilder.Entity<Proveedor>().ToTable("provedores");
            modelBuilder.Entity<Proveedor>().HasKey(p => p.Provedor); // Usa el campo `provedor` como clave primaria

            modelBuilder.Entity<Proveedor>()
                .Property(p => p.FacturaPdf)
                .HasColumnName("pdf_factura");

            modelBuilder.Entity<Proveedor>()
                .Property(p => p.ComprobantePagoPdf)
                .HasColumnName("pdf_comprobante");

            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ConceptoPago).HasColumnName("Concepto");
            });
        }
    }
}
