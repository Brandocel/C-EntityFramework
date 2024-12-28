using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HokaProvedorWeb.Data.Entities
{
    [Table("ProveedorAbonos")] // Nombre exacto de la tabla en la base de datos
    public class ProveedorAbono
    {
        [Key]
        public int Id { get; set; } // Llave primaria autoincremental

        [Required]
        public int FolioEntrada { get; set; } // Relacionado con la tabla Proveedores (llave foránea)

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Abono { get; set; } // Monto del abono

        [Required]
        [MaxLength(50)]
        public string FormaPago { get; set; } // Efectivo, Transferencia, etc.

        [Required]
        public DateTime FechaAbono { get; set; } // Fecha del abono

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; } // Saldo restante después del abono

        [MaxLength(150)]
        public string NombreRazonSocial { get; set; } // Para identificar el proveedor

        // Relación con la entidad Proveedor
        [ForeignKey("FolioEntrada")]
        [NotMapped]
        public Proveedor Proveedor { get; set; } // Navegación a Proveedor
    }
}
