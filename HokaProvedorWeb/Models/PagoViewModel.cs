using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HokaProvedorWeb.Models
{
    public class PagoViewModel
    {
        [Required]
        public string NombreRazonSocial { get; set; }

        [Required]
        public string FormaPago { get; set; }

        public string Banco { get; set; }

        public string CuentaBancaria { get; set; }

        [Required]
        public string ConceptoPago { get; set; }

        [Required]
        public string Sucursal { get; set; }

        public string FolioPago { get; set; }

        [Required]
        public string Folio { get; set; }

        [DataType(DataType.Date)]
        public DateTime DiasCredito { get; set; }

        public int Id { get; set; }

        public string Autorizacion { get; set; }

        // Propiedades de archivo
        public IFormFile PdfUpload { get; set; }
        public IFormFile XmlUpload { get; set; }

        public decimal? Importe { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Total { get; set; }

        // Opciones de selectores
        public List<SelectListItem> SucursalOptions { get; set; }
        public List<string> NombreRazonSocialOptions { get; set; }
        public List<PagoViewModel> Pagos { get; set; }

        public bool IsValid()
        {
            if (FormaPago == "Deposito")
            {
                if (string.IsNullOrEmpty(Banco) || string.IsNullOrEmpty(CuentaBancaria))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
