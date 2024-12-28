using System;
using Microsoft.EntityFrameworkCore;

namespace HokaProvedorWeb.Models
{
    [Keyless]
    public class ProveedorViewModel
    {
        public int? Provedor { get; set; }
        public string? NombreRazonSocial { get; set; }
        public string? Observaciones { get; set; }
        public decimal? Total { get; set; }
        public decimal? Abono { get; set; }
        public DateTime? FechaFactura { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string? UUID { get; set; }
        public decimal? Importe { get; set; }
        public int? FolioEntrada { get; set; }
        public string? FormaPago { get; set; }

        // Métodos auxiliares para evitar null
        public int ProvedorValor => Provedor ?? 0;
        public string NombreRazonSocialValor => NombreRazonSocial ?? "Sin datos";
        public decimal TotalValor => Total ?? 0;
        public decimal AbonoValor => Abono ?? 0;
        public DateTime FechaFacturaValor => FechaFactura ?? DateTime.MinValue;
        public DateTime FechaVencimientoValor => FechaVencimiento ?? DateTime.MinValue;
    }
}
