using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Master")]
    public class MasterController : BaseController
    {
        public ActionResult Register()
        {
            var accounts = MainContext.Users.Include(u => u.Profile).ToList();

            return View(accounts);
        }
    }
}