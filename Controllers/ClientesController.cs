using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using GlabsHelps.Models;

namespace GlabsHelps.Controllers
{
    public class ClientesController : Controller
    {
        //int eso = 0;
        public ActionResult Index()
        {

            return View(Clientes.ClienteList());
        }

        [HttpPost]
        public ActionResult Index(string busqueda)
        {
            var cli = from s in Clientes.ClienteList() select s;
            if (!string.IsNullOrEmpty(busqueda))
            {
                return View(cli.Where( s => s.Nombre.Contains(busqueda)));
            }
            else
            {
                return View(Clientes.ClienteList());
            }
           
        }

        public ActionResult Details(Decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clien = new Clientes(id);
               
            if (clien.IdCliente==0)
            {
                return HttpNotFound();
            }
            return View(clien);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Nombre,Direccion,Telefono,CorreoCliente,Contacto,CelularContacto,CorreoContacto")] Clientes clie)
        {
            if (ModelState.IsValid)
            {
                clie.Guardar();
                return RedirectToAction("Index");
            }

            return View(clie);
        }

        public ActionResult Edit(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clien = new Clientes(id); 
            if (clien.IdCliente == 0)
            {
                return HttpNotFound();
            }
            return View(clien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Nombre,Direccion,Telefono,CorreoCliente,Contacto,CelularContacto,CorreoContacto")] Clientes clien)
        {
            if (ModelState.IsValid)
            {
                clien.Guardar();
                return RedirectToAction("Index");
            }
            return View(clien);
        }

        public ActionResult Delete(decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clien = new Clientes(id);
            if (clien.IdCliente == 0)
            {
                return HttpNotFound();
            }
            return View(clien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clien = new Clientes(id);
            clien.Eliminar();
            return RedirectToAction("Index");
        }

        public static bool ValidarCorreo(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
