namespace HokaProvedorWeb.Models
{
    public class AltaProveedorViewModel
    {
        // Datos del proveedor
        public string? RFC { get; set; }
        public string? NombreRazonSocial { get; set; }
        public string? Nombre { get; set; }
        public byte[]? PdfSituacionFiscal { get; set; }
        public string? TipoCuenta { get; set; }
        public string? CuentaBancaria { get; set; }
        public string? Banco { get; set; }
        public string? FormaPago { get; set; }
        public byte[]? PdfComprobanteBanco { get; set; }
    }
}
