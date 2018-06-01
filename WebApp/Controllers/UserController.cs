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
            var users = GetUsers("");
            return View(new SearchViewModel{Users = users, SearchTerm = ""});
        }

        public ActionResult Search(string name)
        {
            var users = GetUsers(name);
            return View("Index", new SearchViewModel { Users = users, SearchTerm = name });
        }

        public JsonResult Transfer(int value, string userId)
        {
            if (value > 0)
            {
                var account = GetCurrentUserAccount();
                var accountTo = GetAccount(userId);
                if (account == null || accountTo == null)
                {
                    return Json(new { success = false, error = "Ошибка. Попробуйте еще раз" });
                }

                if (account.Profile.Balance.Current < value)
                {
                    return Json(new { success = false, error = "Недостаточно средств" });
                }
                
                if (value == 25) throw new Exception();

                accountTo.Profile.Balance.Current += value;
                account.Profile.Balance.Current -= value;
                MainContext.SaveChanges();
            }

            return Json(new {success = true});
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
    }
}