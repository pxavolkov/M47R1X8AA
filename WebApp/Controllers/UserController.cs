using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Model;
using WebApp.Models;

namespace WebApp.Controllers
{
    //[Authorize]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            var models = GetUsers("");
            return View(models);
        }

        public ActionResult Search(string name)
        {
            var models = GetUsers(name);
            return View("Index", models);
        }

        public void Transfer(int value, string userId)
        {
            if (value > 0)
            {
                var account = GetCurrentUserAccount();
                var accountTo = MainContext.Users.Include(u => u.Profile).SingleOrDefault(a => a.Id == userId);
                if (accountTo != null)
                {
                    accountTo.Profile = MainContext.Profiles.Include(p => p.Balance).SingleOrDefault(r => r.ID == accountTo.Profile.ID);
                }
                if (account == null || accountTo == null || account.Profile.Balance.Current < value)
                {
                    throw new ArgumentException();
                }

                accountTo.Profile.Balance.Current += value;
                account.Profile.Balance.Current -= value;
                MainContext.SaveChanges();
            }
        }

        private List<UserViewModel> GetUsers(string name)
        {
            var users = LoadUsers(name);
            var models = users.Select(u => new UserViewModel(u.Id, u.Profile.FirstName, u.Profile.LastName,
                string.IsNullOrEmpty(u.Profile.PhotoPath) ? String.Empty : $"/Content/Avatars/{u.Profile.PhotoPath}")).ToList();
            return models;
        }

        private List<Account> LoadUsers(string name)
        {
            name = name?.Trim().ToUpper();
            var q = MainContext.Users.Include(u => u.Profile);
            if (!string.IsNullOrEmpty(name))
            {
                q = q.Where(u => u.Profile.FirstName.ToUpper().Contains(name) ||
                                 u.Profile.LastName.ToUpper().Contains(name));
            }

            q = q.OrderBy(u => u.Profile.FirstName);
            return q.ToList();
        }

        private Account GetCurrentUserAccount()
        {
            var userId = User.Identity.GetUserId();
            var account = MainContext.Users.Include(u => u.Profile).SingleOrDefault(a => a.Id == userId);
            if (account != null)
            {
                account.Profile = MainContext.Profiles.Include(p => p.Balance).SingleOrDefault(r => r.ID == account.Profile.ID);
            }
            return account;
        }
    }
}