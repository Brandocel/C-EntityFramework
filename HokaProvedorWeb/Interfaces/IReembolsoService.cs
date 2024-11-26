using HokaProvedorWeb.Models;

namespace HokaProvedorWeb.Interfaces
{
    public interface IReembolsoService
    {
        ReembolsoViewModel GetReembolsoViewModel();
        string ExtraerUUIDDesdeXML(IFormFile xmlFile);
        bool GuardarUUIDEnEntradaM(string uuid);
        bool GuardarReembolso(ReembolsoViewModel model);
    }
}
