using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class GiftController : Controller
    {
        // GET: Gift
        public ActionResult Credits(string id)
        {
            return View("Credits100");
        }
    }
}