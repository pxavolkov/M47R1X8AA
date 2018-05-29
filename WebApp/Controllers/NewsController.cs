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
    //[Authorize]
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var account = MainContext.Users.Include(u => u.Profile).SingleOrDefault(a => a.Id == userId);
            if (account != null)
            {
                account.Profile = MainContext.Profiles.Include(p => p.Balance).SingleOrDefault(r => r.ID == account.Profile.ID);
            }

            var allNews = MainContext.News.OrderByDescending(n => n.CreateDate).ToList();
            var readNews = MainContext.ReadNews.Where(r => r.UserId == account.Id).ToList();
            foreach (var n in allNews)
            {
                if (readNews.All(r => r.NewsId != n.ID))
                {
                    MainContext.ReadNews.Add(new ReadNews {NewsId = n.ID, UserId = account.Id});
                }
            }

            MainContext.SaveChanges();
            return View(allNews);
        }
    }
}