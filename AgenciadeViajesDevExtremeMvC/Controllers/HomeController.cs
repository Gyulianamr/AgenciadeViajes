using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgenciadeViajesDevExtremeMvC.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Cliente()
        {
            return View();


        }

       

        public ActionResult Paquete()
        {
            return View();


        }

        public ActionResult Reserva()
        {
            return View();


        }

        public ActionResult Cotizacion()
        {
            return View();


        }
    }
}