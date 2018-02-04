using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Model;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var account = MainContext.Users.Include(u => u.Profile).SingleOrDefault(a => a.Id == userId);
            if (account == null)
            {
                return RedirectToAction("LogOff", "Account");
            }
            return View(account.Profile);
        }

        [AllowAnonymous]
        public ActionResult Approval()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}