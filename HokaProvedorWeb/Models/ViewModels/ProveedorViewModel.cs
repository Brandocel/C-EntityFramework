namespace HokaProvedorWeb.Models.ViewModels
{
    public class ProveedorViewModel
    {
        public int? Provedor { get; set; }
        public string? Nombre { get; set; }
        public string? NombreRazonSocial { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? Telefonos { get; set; }
        public string? Email { get; set; }
        public float? LimiteCreditoValor { get; set; }
        public decimal Importe { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaFactura { get; set; }
        public string? UUID { get; set; }
        public string? FormaPago { get; set; }
        public string? Observaciones { get; set; }
    }
}
