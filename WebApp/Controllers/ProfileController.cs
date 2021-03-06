﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ImageResizer;
using Microsoft.AspNet.Identity;
using Model;
using Model.Enum;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        public ActionResult Index()
        {
            var account = GetCurrentUserAccount();
            if (account == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            var readNews = MainContext.ReadNews.Where(r => r.UserId == account.Id).Select(r => r.NewsId).ToList();
            var unreadNews = MainContext.News.Where(u => !readNews.Contains(u.ID));
            
            var model = new ProfileViewModel(account.Profile, unreadNews.Count());
            model.MiningUrl = model.Profile.IsCitizen ? Url.Action("Mining", "Profile") : "#";
            model.NewsUrl = model.Profile.IsCitizen ? Url.Action("Index", "News") : "#";
            return View(model);
        }


        //public ActionResult Edit()
        //{
        //    var account = GetCurrentUserAccount();
        //    if (account == null)
        //    {
        //        return RedirectToAction("LogOff", "Account");
        //    }

        //    var model = new ProfileViewModel(account.Profile);
        //    return View(model);
        //}

        public ActionResult UploadPhoto()
        {
            return View((Session["UploadedFile"] as AvatarViewModel) ?? new AvatarViewModel());
        }

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                var ext = Path.GetExtension(file.FileName) ?? "jpg";
                string path = Path.Combine(Server.MapPath("~/Content/Avatars/temp"), Guid.NewGuid() + ext);
                while (System.IO.File.Exists(path))
                {
                    path = Path.Combine(Server.MapPath("~/Content/Avatars/temp"), Guid.NewGuid() + ext);
                }

                System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);

                var viewModel = new AvatarViewModel
                {
                    Path = Path.Combine("/Content/Avatars/temp", Path.GetFileName(path)),
                    Width = image.Width,
                    Height = image.Height
                };

                var r = new ResizeSettings();
                r.Width = viewModel.DisplayWidth;
                r.Height = viewModel.DisplayHeight;
                ImageResizer.ImageBuilder.Current.Build(image, path, r);

                //file.SaveAs(path);

                
                Session["UploadedFile"] = viewModel;
            }
            
            // after successfully uploading redirect the user
            return RedirectToAction("UploadPhoto", "Profile");
        }

        public ActionResult Crop(int x1, int y1, int x2, int y2, int w, int h)
        {
            var data = Session["UploadedFile"] as AvatarViewModel;
            if (data == null)
                return RedirectToAction("Index", "Profile");

            var ext = Path.GetExtension(data.Path) ?? "jpg";
            var inputPath = Path.Combine(Server.MapPath("~/Content/Avatars/temp"), Path.GetFileName(data.Path));
            var outputPath = Path.Combine(Server.MapPath("~/Content/Avatars"), User.Identity.GetUserId() + ext);
            double scaleFactor = (double) 170 / w;
            //Resize(inputPath, outputPath, scaleFactor, new Rectangle(x1, y1, w, h));
            var r = new ResizeSettings($"crop={x1},{y1},{x2},{y2}&width=170&height=170&scale=both");
            //r.CropTopLeft = new PointF(x1, y1);
            //r.getCustomCropSourceRect(new SizeF(w, h));
            //r.Width = 170;
            //r.Height = 170;
            ImageResizer.ImageBuilder.Current.Build(inputPath, outputPath, r);

            var account = GetCurrentUserAccount();
            if (account == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            account.Profile.PhotoPath = Path.GetFileName(outputPath);
            MainContext.SaveChanges();

            return RedirectToAction("Index", "Profile");
        }

        public ActionResult Update(string firstName, string lastName, string sex, string age)
        {
            int ageToSave;
            int.TryParse(age, out ageToSave);

            var account = GetCurrentUserAccount();
            if (account == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            account.Profile.Age = ageToSave;
            account.Profile.FirstName = firstName;
            account.Profile.LastName = lastName;
            account.Profile.IsMale = sex == "1";
            MainContext.SaveChanges();

            return RedirectToAction("Index", "Profile");
        }

        

        public ActionResult Mining()
        {
            var account = GetCurrentUserAccount();
            if (account?.Profile?.Balance != null && account.Profile.IsCitizen)
            {
                if (!account.Profile.Balance.MiningTime.HasValue || account.Profile.Balance.MiningTime < DateTime.Now)
                {
                    var setting = System.Configuration.ConfigurationManager.AppSettings["MiningTimeMinutes"];
                    account.Profile.Balance.MiningTime = DateTime.Now.AddMinutes(int.Parse(setting));
                    var settingAmount = System.Configuration.ConfigurationManager.AppSettings["MiningCredits"];
                    AddTransaction(account.Id, int.Parse(settingAmount), TransactionType.Mining);
                    MainContext.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Profile");
        }
    }
}