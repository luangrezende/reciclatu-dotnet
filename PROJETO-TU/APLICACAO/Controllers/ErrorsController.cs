using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APLICACAO.Controllers
{
    public class ErrorsController : Controller
    {
        //VIEWS ..............................................
        [HttpGet]
        public ActionResult NotFound404(Exception exception)
        {
            Response.StatusCode = 404;
            Response.ContentType = "text/html";
            return View(exception);
        }

        [HttpGet]
        public ActionResult ServerError500(Exception exception)
        {
            Response.StatusCode = 500;
            Response.ContentType = "text/html";
            return View(exception);
        }

        [HttpGet]
        public ActionResult DefaultError()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}