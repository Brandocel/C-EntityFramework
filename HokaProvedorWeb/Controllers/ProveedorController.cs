using HokaProvedorWeb.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _service;

        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _service.ObtenerProveedoresAsync();
            return View(proveedores);
        }
    }
}
