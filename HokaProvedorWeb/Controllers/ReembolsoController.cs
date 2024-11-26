using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Models;
using HokaProvedorWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace HokaProvedorWeb.Controllers
{
    public class ReembolsoController : Controller
    {
        private readonly IReembolsoService _reembolsoService;

        public ReembolsoController(IReembolsoService reembolsoService)
        {
            _reembolsoService = reembolsoService;
        }

        public ActionResult Reembolso()
        {
            var model = _reembolsoService.GetReembolsoViewModel();
            return View(model);
        }

        [HttpPost]
        public JsonResult ExtraerUUID(IFormFile xmlFile)
        {
            var uuid = _reembolsoService.ExtraerUUIDDesdeXML(xmlFile);
            if (!string.IsNullOrEmpty(uuid))
            {
                var isSaved = _reembolsoService.GuardarUUIDEnEntradaM(uuid);
                return Json(new { UUID = uuid, Message = isSaved ? "UUID guardado correctamente en entradaM." : "Error al guardar el UUID en entradaM." });
            }
            return Json(new { Message = "No se pudo extraer el UUID del archivo XML." });
        }

        [HttpPost]
        public ActionResult Reembolso(ReembolsoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isSaved = _reembolsoService.GuardarReembolso(model);
                ViewBag.Message = isSaved ? "Los datos se guardaron correctamente." : "No se pudieron guardar los datos.";
                if (isSaved) ModelState.Clear();
            }
            else
            {
                ViewBag.Message = "Por favor, corrija los errores en el formulario.";
            }

            model = _reembolsoService.GetReembolsoViewModel();
            return View(model);
        }
    }
}
