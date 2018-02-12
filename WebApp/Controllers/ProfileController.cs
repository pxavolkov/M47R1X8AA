using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Model;
using WebApp.Models;

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

            var model = new ProfileViewModel(account.Profile);
            return View(model);
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