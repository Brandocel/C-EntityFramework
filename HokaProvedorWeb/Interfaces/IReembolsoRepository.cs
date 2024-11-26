using HokaProvedorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HokaProvedorWeb.Interfaces
{
    public interface IReembolsoRepository
    {
        List<SelectListItem> ObtenerOpciones(string query, string columnName);
        bool GuardarUUID(string uuid);
        bool GuardarReembolso(ReembolsoViewModel model);
    }
}
