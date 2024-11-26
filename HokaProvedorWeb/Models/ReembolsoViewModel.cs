using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Models
{
    public class ReembolsoViewModel
    {
        public string NotaFactura { get; set; }
        public string NombreRazonSocial { get; set; }
        public string Folio { get; set; }

        public string FolioEntrada { get; set; }

        public string UUID { get; set; }
        public string Importe { get; set; }
        public string Iva { get; set; }
        public string Total { get; set; }
        public string CompraGasto { get; set; }

        public string Autorizacion { get; set; }

        public string Observacion { get; set; }

        public string Concepto { get; set; }

        public DateTime FechaHora { get; set; }



        public List<SelectListItem> CompraGastoOptions { get; set; }
        public List<SelectListItem> ConceptoOptions { get; set; }

        // Propiedades de archivo
        public IFormFile PdfUpload { get; set; }
        public IFormFile XmlUpload { get; set; }

        public List<SelectListItem> NotaFacturaOptions { get; set; }
        public List<SelectListItem> NombreRazonSocialOptions { get; set; }
        public List<SelectListItem> FolioOptions { get; set; }
        public List<SelectListItem> ImpuestoOptions { get; set; }
        public List<SelectListItem> IvaOptions { get; set; }
        public List<SelectListItem> TotalOptions { get; set; }
        public string ProveedorId { get; internal set; }
        public string AlmacenId { get; internal set; }
    }
}
