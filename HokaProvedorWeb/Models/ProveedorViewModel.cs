using System;
using System.ComponentModel.DataAnnotations;

namespace HokaProvedorWeb.Models
{
    public class ProveedorViewModel
    {
        public int Provedor { get; set; } // ID del proveedor
        public string NombreRazonSocial { get; set; } // Razón social
        public string Observaciones { get; set; } // Observaciones sobre el proveedor
        public decimal Total { get; set; } // Total de la factura
        public decimal Abono { get; set; } // Abonos realizados
        public DateTime FechaFactura { get; set; } // Fecha de emisión de la factura
        public DateTime FechaVencimiento { get; set; } // Fecha de vencimiento
        public string UUID { get; set; } // Identificador UUID de la factura
        public decimal Importe { get; set; } // Importe antes de impuestos
        public decimal IVA { get; set; } // Impuesto aplicado
        public string FormaPago { get; set; } // Forma de pago
        public byte[] FacturaPdf { get; set; } // Archivo PDF de la factura
        public byte[] ComprobantePagoPdf { get; set; } // Archivo PDF del comprobante de pago
        public string FolioEntrada { get; set; } // Folio de entrada
        public DateTime FechaAbono { get; set; } // Fecha del último abono
    }
}
