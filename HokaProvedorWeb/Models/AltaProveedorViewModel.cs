namespace HokaProvedorWeb.Models
{
    public class AltaProveedorViewModel
    {
        public string? RFC { get; set; }
        public string? NombreRazonSocial { get; set; }
        public string? Nombre { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? TipoPersona { get; set; }
        public string? TipoCuenta { get; set; }
        public string? CuentaBancaria { get; set; }
        public string? Banco { get; set; }
        public string? ContactoVenta { get; set; }
        public string? ContactoCobranza { get; set; }
        public byte[]? PdfSituacionFiscal { get; set; }
        public byte[]? PdfComprobanteBanco { get; set; }
        public byte[]? XmlArchivo { get; set; }
    }
}
