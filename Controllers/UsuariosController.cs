using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GlabsHelps.Data;
using GlabsHelps.Models;

namespace GlabsHelps.Controllers
{
    public class UsuariosController : Controller
    {
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string clave)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(clave))
            {
                var cli = from s in Usuario.UsuarioList() select s;
                var user = cli.FirstOrDefault(e => e.CorreoElectronico == email && e.Password == clave);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.CorreoElectronico, false);
                    return RedirectToAction("Index", "Clientes");
                }
                else
                {
                    return Index("No esta registrado, por favor registrese");
                }
            }
            else
            {
                return Index("Llene los campos para iniciar sesión");
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdUsuario,NombreUsuario,CorreoElectronico,Password")] Usuario usua)
        {
            if (ModelState.IsValid)
            {
                usua.Guardar();
                return RedirectToAction("Index");
            }

            return View(usua);
        }

        public ActionResult CerrarSesion()
        {
            Logout();
            return RedirectToAction("Index");
        }
        public void Logout()
        {
            FormsAuthentication.SignOut();
            RedirectToAction("Index", "Home");
        }
    }
}
