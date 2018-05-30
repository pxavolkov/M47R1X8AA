using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Model;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        private AccountManager _userManager;
        private ApplicationSignInManager _signInManager;
        private MainContext _mainContext;

        public AccountManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<AccountManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public MainContext MainContext
        {
            get
            {
                return _mainContext ?? HttpContext.GetOwinContext().Get<MainContext>();
            }
            private set
            {
                _mainContext = value;
            }
        }

        protected Account GetCurrentUserAccount()
        {
            var userId = User.Identity.GetUserId();
            var account = GetAccount(userId);
            return account;
        }

        protected Account GetAccount(string userId)
        {
            var account = MainContext.Users.Include(u => u.Profile).Include(u => u.Profile.Balance)
                .SingleOrDefault(a => a.Id == userId);
            var miningTime = account?.Profile?.Balance?.MiningTime;
            if (miningTime != null && miningTime < DateTime.Now)
            {
                account.Profile.Balance.MiningTime = null;
                var setting = System.Configuration.ConfigurationManager.AppSettings["MiningCredits"];
                account.Profile.Balance.Current += int.Parse(setting);
                MainContext.SaveChanges();
            }

            return account;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

    }
}