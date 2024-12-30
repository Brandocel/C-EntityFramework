using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HokaProvedorWeb.Models
{
    public class PagoViewModel
    {
        public int? Id { get; set; } 
        public string? NombreRazonSocial { get; set; }
        public IEnumerable<SelectListItem>? NombreRazonSocialOptions { get; set; }
        public string? ConceptoPago { get; set; }
        public string? FormaPago { get; set; }
        public string? Banco { get; set; }
        public string? CuentaBancaria { get; set; }
        public string? Sucursal { get; set; }
        public IEnumerable<SelectListItem>? SucursalOptions { get; set; }
        public decimal? Iva { get; set; }
        public decimal? Importe { get; set; }
        public decimal? Total { get; set; }
        public string? Observaciones { get; set; }
        public IFormFile PdfUpload { get; set; }
        public DateTime? DiasCredito { get; set; }
    }

}
