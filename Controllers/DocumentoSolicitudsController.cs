using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlabsHelps.Data;
using GlabsHelps.Models;

namespace GlabsHelps.Controllers
{
    public class DocumentoSolicitudsController : Controller
    {
        public ActionResult Index()
        {
            return View(DocumentoSolicitud.DocumentoSolicitudList());
        }

        public ActionResult Index(string busqueda)
        {
            var solic = from s in DocumentoSolicitud.DocumentoSolicitudList() select s;
            if (!string.IsNullOrEmpty(busqueda))
            {
                return View(solic.Where(s => s.IdSolicitud.ToString().Contains(busqueda) || s.NombreDocumento.Contains(busqueda)));
            }
            else
            {
                return View(DocumentoSolicitud.DocumentoSolicitudList());
            }
        }

        public ActionResult Details(Decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentoSolicitud doc = new DocumentoSolicitud(id);
            if (doc.IdDocumento == 0)
            {
                return HttpNotFound();
            }
            return View(doc);
        }

        public ActionResult Create()
        {
            var soli = from s in Solicitudes.SolicitudesList() select s;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var sol in soli)
            {
                items.Add(new SelectListItem
                {
                    Value = sol.IdSolicitud.ToString(),
                    Text = sol.Descripcion,
                    Selected = false
                });
            }

            ViewBag.items = items;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdDocumento,IdSolicitud,NombreDocumento,FechaAgregado,IdUsuario,TipoDocumento")] DocumentoSolicitud doc)
        {
            if (ModelState.IsValid)
            {
                doc.Guardar();
                return RedirectToAction("Index");
            }

            return View(doc);
        }

        public ActionResult Edit(Decimal id)
        {
            var soli = from s in Solicitudes.SolicitudesList() select s;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var sol in soli)
            {
                items.Add(new SelectListItem
                {
                    Value = sol.IdSolicitud.ToString(),
                    Text = sol.Descripcion,
                    Selected = false
                });
            }

            ViewBag.items = items;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentoSolicitud doc = new DocumentoSolicitud(id);
            if (doc.IdDocumento == 0)
            {
                return HttpNotFound();
            }
            return View(doc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdDocumento,IdSolicitud,NombreDocumento,FechaAgregado,IdUsuario,TipoDocumento")] DocumentoSolicitud doc)
        {
            if (ModelState.IsValid)
            {
                doc.Guardar();
                return RedirectToAction("Index");
            }
            return View(doc);
        }

        public ActionResult Delete(Decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentoSolicitud doc = new DocumentoSolicitud(id);
            if (doc.IdDocumento == 0)
            {
                return HttpNotFound();
            }
            return View(doc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentoSolicitud doc = new DocumentoSolicitud(id);
            doc.Eliminar();
            return RedirectToAction("Index");
        }
    }
}
