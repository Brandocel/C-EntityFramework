using HokaProvedorWeb.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HokaProvedorWeb.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Proveedor> Provedores { get; set; }
        public DbSet<Concepto> Conceptos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proveedor>(entity =>
            {
                // Establecer la clave primaria
                entity.HasKey(e => e.Provedor);

                // Mapeo de propiedades a columnas de la base de datos
                entity.Property(e => e.NombreRazonSocial).HasColumnName("nombre_rasonsocial");
                entity.Property(e => e.LimiteCreditoValor).HasColumnName("limite_credito_valor");
                entity.Property(e => e.LimiteCreditoDias).HasColumnName("limite_credito_dias");
                entity.Property(e => e.DiasCredito).HasColumnName("DiasCredito").HasColumnType("date");
                entity.Property(e => e.SumaCargo).HasColumnName("Suma_cargo");
                entity.Property(e => e.SumaAbono).HasColumnName("Suma_abono");
                entity.Property(e => e.DiasEntrega).HasColumnName("dias_entrega");
                entity.Property(e => e.TipoProveedor).HasColumnName("tipo_provedor");
                entity.Property(e => e.NotaFactura).HasColumnName("Nota_Factura");
                entity.Property(e => e.CompraGasto).HasColumnName("Compra_Gasto");
                entity.Property(e => e.DescuentoComercial).HasColumnName("Descuento_comercial");
                entity.Property(e => e.DescuentoPp).HasColumnName("Descuento_pp");
                entity.Property(e => e.FolioEntrada).HasColumnName("Folio_entrada");
                entity.Property(e => e.DatosBanco).HasColumnName("datos_banco");
                entity.Property(e => e.PdfFactura).HasColumnName("pdf_factura");
                entity.Property(e => e.PdfComprobante).HasColumnName("pdf_Comprobante");
                entity.Property(e => e.PdfSituacionFiscal).HasColumnName("PdfSituacionFiscal");
                entity.Property(e => e.FechaHora).HasColumnName("FechaHora");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Folio).HasColumnName("folio");

                // Configuración adicional (si es necesario)
            });

            modelBuilder.Entity<Concepto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ConceptoPago).HasColumnName("Concepto");
            });
        }
    }
}
