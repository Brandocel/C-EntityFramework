namespace HokaProvedorWeb.Models.ViewModels
{
    public class ProveedorViewModel
    {
        public int FolioEntrada { get; set; }
        public string Proveedor { get; set; }
        public string NombreRazonSocial { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal Importe { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public decimal Saldo { get; set; }
        public string FormaPago { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Observaciones { get; set; }
        public string UUID { get; set; }
        public string Concepto { get; set; }
        public string Banco { get; set; }
        public string CuentaBancaria { get; set; }
    }

}
