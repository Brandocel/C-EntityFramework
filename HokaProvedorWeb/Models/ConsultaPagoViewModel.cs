namespace HokaProvedorWeb.Models
{
    public class ConsultaPagoViewModel
    {
        public string FolioEntrada { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaFactura { get; set; }
        public string UUID { get; set; }
        public decimal Impuesto { get; set; }
        public int Provedor { get; set; }
        public string NombreRazonSocial { get; set; }
        public string Concepto { get; set; }
        public string NotaFactura { get; set; }
        public string CompraGasto { get; set; }
        public DateTime DiasCredito { get; set; }

        public decimal Saldo { get; set; }

        public DateTime FechaAbono { get; set; } // Nueva propiedad
        public string RFC { get; set; }
        public decimal IVA { get; set; }
        public string Autorizacion { get; set; }
        public decimal Abono { get; set; }
        public string FormaPago { get; set; }
        public string Proveedor { get; internal set; }
        public decimal Importe { get; internal set; }
    }

}
