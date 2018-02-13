using System;
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

                int height = image.Height;
                int width = image.Width;

                file.SaveAs(path);

                Session["UploadedFile"] = new AvatarViewModel
                {
                    Path = Path.Combine("/Content/Avatars/temp", Path.GetFileName(path)),
                    Width = width,
                    Height = height
                };
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

            var userId = User.Identity.GetUserId();
            var account = MainContext.Users.Include(u => u.Profile).SingleOrDefault(a => a.Id == userId);
            if (account == null)
            {
                return RedirectToAction("LogOff", "Account");
            }

            account.Profile.PhotoPath = Path.GetFileName(outputPath);
            MainContext.SaveChanges();

            return RedirectToAction("Index", "Profile");
        }

        private void Resize(string imageFile, string outputFile, double scaleFactor, Rectangle cropRectangle)
        {
            using (var srcImage = Image.FromFile(imageFile))
            {
                var newWidth = (int)(srcImage.Width * scaleFactor);
                var newHeight = (int)(srcImage.Height * scaleFactor);
                using (var newImage = new Bitmap(newWidth, newHeight))
                using (var graphics = Graphics.FromImage(newImage))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(srcImage, cropRectangle, new Rectangle(0, 0, 170, 170), GraphicsUnit.Pixel);
                    newImage.Save(outputFile);
                }
            }
        }
    }
}