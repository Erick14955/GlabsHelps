using GlabsHelps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GlabsHelps.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string clave)
        {
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(clave))
            {
                var cli = from s in Usuario.UsuarioList() select s;
                var user = cli.FirstOrDefault(e => e.CorreoElectronico == email && e.Password == clave);
                if(user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.CorreoElectronico, true);
                    return RedirectToAction("About");   
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
        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult About([Bind(Include = "IdUsuario,NombreUsuario,CorreoElectronico,Password")] Usuario usua)
        {
            if (ModelState.IsValid)
            {
                usua.Guardar();
                return RedirectToAction("Index");
            }

            return View(usua);
        }

        public ActionResult Contact()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}