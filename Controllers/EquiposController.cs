using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlabsHelps.Models;

namespace GlabsHelps.Controllers
{
    public class EquiposController : Controller
    {
        public ActionResult Index()
        {
            return View(Equipos.EquipoList());
        }

        [HttpPost]
        public ActionResult Index(string busqueda)
        {
            var equip = from s in Equipos.EquipoList() select s;
            if (!string.IsNullOrEmpty(busqueda))
            {
                return View(equip.Where(s => s.IdCliente.ToString().Contains(busqueda) || s.Descripcion.Contains(busqueda)));
            }
            else
            {
                return View(Equipos.EquipoList());
            }
        }

        public ActionResult Details(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equip = new Equipos(id);
            if (equip.IdEquipo == 0)
            {
                return HttpNotFound();
            }
            return View(equip);
        }

        public ActionResult Create()
        {
            var cli = from s in Clientes.ClienteList() select s;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var clien in cli)
            {
                items.Add(new SelectListItem
                {
                    Value = clien.IdCliente.ToString(),
                    Text = clien.Nombre,
                    Selected = false
                });
            }

            ViewBag.items = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEquipo,IdCliente,Descripcion,Responsable,DireccionAnyDesk,DireccionTeamViewer,IpEquipo,IpPublica,IpLocal,TipoEquipo,UsuarioEquipo,ClaveEquipo")] Equipos equip)
        {
            if (ModelState.IsValid)
            {
                equip.Guardar();
                return RedirectToAction("Index");
            }

            return View(equip);
        }

        public ActionResult Edit(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equip = new Equipos(id);
            if (equip.IdEquipo == 0)
            {
                return HttpNotFound();
            }
            return View(equip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEquipo,IdCliente,Descripcion,Responsable,DireccionAnyDesk,DireccionTeamViewer,IpEquipo,IpPublica,IpLocal,TipoEquipo,UsuarioEquipo,ClaveEquipo")] Equipos equip)
        {
            if (ModelState.IsValid)
            {
                equip.Guardar();
                return RedirectToAction("Index");
            }
            return View(equip);
        }

        // GET: Equipos/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equip = new Equipos(id);
            if (equip.IdEquipo == 0)
            {
                return HttpNotFound();
            }
            return View(equip);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            Equipos equip = new Equipos(id);
            equip.Eliminar();
            return RedirectToAction("Index");
        }
    }
}
