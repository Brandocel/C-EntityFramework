using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using System.Xml;

namespace HokaProvedorWeb.Services
{
    public class ReembolsoService : IReembolsoService
    {
        private readonly IReembolsoRepository _reembolsoRepository;

        public ReembolsoService(IReembolsoRepository reembolsoRepository)
        {
            _reembolsoRepository = reembolsoRepository;
        }

        public ReembolsoViewModel GetReembolsoViewModel()
        {
            return new ReembolsoViewModel
            {
                NotaFacturaOptions = _reembolsoRepository.ObtenerOpciones("SELECT DISTINCT NotaFactura FROM Notas", "NotaFactura"),
                NombreRazonSocialOptions = _reembolsoRepository.ObtenerOpciones("SELECT DISTINCT nombre_rasonsocial FROM provedores", "nombre_rasonsocial"),
                CompraGastoOptions = _reembolsoRepository.ObtenerOpciones("SELECT DISTINCT Tipo FROM ComprasYGastos", "Tipo"),
                ConceptoOptions = _reembolsoRepository.ObtenerOpciones("SELECT DISTINCT Concepto FROM Conceptos", "Concepto"),
                FolioOptions = _reembolsoRepository.ObtenerOpciones("SELECT DISTINCT Folio FROM Bancos", "Folio")
            };
        }

        public string ExtraerUUIDDesdeXML(IFormFile xmlFile)
        {
            try
            {
                using (var stream = xmlFile.OpenReadStream()) // Aquí utilizamos OpenReadStream en lugar de InputStream
                {
                    var xmlDoc = new XmlDocument();
                    xmlDoc.Load(stream);

                    var namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
                    namespaceManager.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");

                    var uuidNode = xmlDoc.SelectSingleNode("//tfd:TimbreFiscalDigital", namespaceManager);
                    if (uuidNode?.Attributes?["UUID"] != null)
                    {
                        return uuidNode.Attributes["UUID"].Value;
                    }
                }
            }
            catch
            {
                // Manejo de errores
            }
            return null;
        }
        public bool GuardarUUIDEnEntradaM(string uuid)
        {
            return _reembolsoRepository.GuardarUUID(uuid);
        }

        public bool GuardarReembolso(ReembolsoViewModel model)
        {
            return _reembolsoRepository.GuardarReembolso(model);
        }
    }

}
