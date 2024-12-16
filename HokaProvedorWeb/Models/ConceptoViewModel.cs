namespace HokaProvedorWeb.Models
{
    public class ConceptoViewModel
    {
        public string? ConceptoPago { get; set; }

        public ConceptoViewModel(string conceptoPago)
        {
            ConceptoPago = conceptoPago;
        }

        public bool? Validar()
        {
            return !string.IsNullOrWhiteSpace(ConceptoPago);
        }
    }
}
