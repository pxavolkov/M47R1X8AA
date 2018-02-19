using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Model;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    //return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неверный email или пароль");
                    return View(model);
            }
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Approval()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = MainContext.Users.FirstOrDefault(a => a.Email == model.Email || a.UserName == model.Email);
                if (account != null)
                {
                    ModelState.AddModelError("Email", "Этот email уже зарегистрирован");
                    return View(model);
                }

                string path = null;
                if (model.Quenta != null && model.Quenta.ContentLength > 0 && model.Quenta.ContentLength < 1024*1024) //1 MB limit 
                {
                    path = Path.Combine(Server.MapPath("~/Upload/Quenta"), model.Quenta.FileName);
                    model.Quenta.SaveAs(path);
                }
                else if (model.Quenta != null && model.Quenta.ContentLength > 0)
                {
                    ModelState.AddModelError("Quenta", "Файл слишком большой");
                    return View(model);
                }

                var user = new Account {
                    UserName = model.Email,
                    Email = model.Email,
                    PlayerName = model.PlayerName,
                    PlayerAge = Convert.ToInt32(model.PlayerAge),
                    Info = model.Info,
                    Allergy = model.Allergy,
                    RegistrationDate = DateTime.Now,
                    LockoutEnabled = true,
                    LockoutEndDateUtc = new DateTime(2019, 1, 1), 
                    
                    Profile = new ProfileInfo
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Age = Convert.ToInt32(model.Age),
                        IsMale = model.Sex == 1,
                        QuentaPath = path
                    }
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Approval", "Account");
                }
                ModelState.AddModelError("", "Ошибка. Попробуйте зарегистрироваться еще раз или обратитесь к мастерам.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }



        #region Helpers

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Profile");
        }

        #endregion
    }
}