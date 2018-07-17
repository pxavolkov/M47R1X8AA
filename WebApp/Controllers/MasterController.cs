using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using WebApp.Models;

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

        public ActionResult Toggle(string id)
        {
            var user = MainContext.Users.Include(u => u.Profile).FirstOrDefault(u => u.Id == id);
            if (user?.Profile != null)
            {
                user.Profile.IsCitizen = !user.Profile.IsCitizen;
                MainContext.SaveChanges();
            }
            return RedirectToAction("Register");
        }

        public ActionResult AddNews(string title, string body, string date)
        {
            var error = Validate(title, body, date);

            if (!string.IsNullOrEmpty(error))
            {
                var news = MainContext.News.OrderByDescending(n => n.CreateDate).ToList();
                var model = new NewsViewModel
                {
                    News = news.Select(n => new NewsWithError(n)).ToList(),
                    NewNews = new NewsWithError {Title = title, Text = body, Error = error}
                };
                return View("News", model);
            }

            DateTime dt;
            if (!DateTime.TryParse(date, out dt))
            {
                var news = MainContext.News.OrderByDescending(n => n.CreateDate).ToList();
                var model = new NewsViewModel
                {
                    News = news.Select(n => new NewsWithError(n)).ToList(),
                    NewNews = new NewsWithError {Title = title, Text = body, Error = "Пожалуйста, введите корректную дату"}
                };
                return View("News", model);
            }

            var ne = new News {Title = title, Text = body, CreateDate = dt};
            MainContext.News.Add(ne);
            MainContext.SaveChanges();

            return RedirectToAction("News");
        }

        private static string Validate(string title, string body, string date)
        {
            var error = string.Empty;
            DateTime dt;
            if (string.IsNullOrWhiteSpace(title))
            {
                error = "Пожалуйста, введите заголовок";
            }
            else if (string.IsNullOrWhiteSpace(body))
            {
                error = "Пожалуйста, введите сообщение";
            }
            else if (!DateTime.TryParseExact(date, "yyyy-MM-dd", null, DateTimeStyles.None, out dt))
            {
                error = "Пожалуйста, введите корректную дату";
            }
            return error;
        }

        public ActionResult News()
        {
            var news = MainContext.News.OrderByDescending(n => n.CreateDate).ToList();
            var model = new NewsViewModel { News = news.Select(n => new NewsWithError(n)).ToList(), NewNews = new NewsWithError()};
            return View(model);
        }

        public ActionResult EditNews(int id, string title, string body, string date)
        {
            var error = Validate(title, body, date);

            if (!string.IsNullOrEmpty(error))
            {
                var news = MainContext.News.OrderByDescending(n => n.CreateDate).ToList();
                var model = new NewsViewModel
                {
                    News = news.Select(n => new NewsWithError(n)).ToList()
                };
                var edited = model.News.FirstOrDefault(n => n.ID == id);
                if (edited != null)
                {
                    edited.Error = error;
                }
                return View("News", model);
            }
            else
            {
                var news = MainContext.News.OrderByDescending(n => n.CreateDate).ToList();
                var model = new NewsViewModel
                {
                    News = news.Select(n => new NewsWithError(n)).ToList()
                };
                var edited = model.News.FirstOrDefault(n => n.ID == id);
                if (edited != null)
                {
                    DateTime dt;
                    if (!DateTime.TryParse(date, out dt))
                    {

                        edited.Error = "Пожалуйста, введите корректную дату";

                        return View("News", model);
                    }
                    else
                    {
                        var existed = MainContext.News.FirstOrDefault(n => n.ID == id);
                        if (existed != null)
                        {
                            existed.Title = title;
                            existed.Text = body;
                            existed.CreateDate = dt;
                            MainContext.SaveChanges();
                        }
                    }
                }
            }
            return RedirectToAction("News");
        }
    }
}