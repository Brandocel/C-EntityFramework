namespace HokaProvedorWeb.Data.Entities
{
    public class BancoProveedores
    {
        public int Id { get; set; }
        public string FormaPago { get; set; }
        public string Banco { get; set; }
        public string CuentaBancaria { get; set; }
        public string? ConceptoPago { get; set; }
        public string? NombreRazonSocial { get; set; }
        public string? Folio { get; set; }
        public byte[]? Pdf { get; set; }
        public string? TipoCuenta { get; set; }
        public string? Sucursal { get; set; }
        public string? FolioEntrada { get; set; }
    }
}
