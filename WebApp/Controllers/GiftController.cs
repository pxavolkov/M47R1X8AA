using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    [Authorize]
    public class GiftController : BaseController
    {
        // GET: Gift
        public ActionResult Credits(string id)
        {
            var gift = MainContext.Gifts.SingleOrDefault(g => g.Code == id);
            if (gift != null && gift.UsedUserID == null)
            {
                var account = GetCurrentUserAccount();
                gift.UsedUserID = account.Id;
                gift.UsedDate = DateTime.Now;
                account.Profile.Balance.Current += gift.CreditsBonus;
                MainContext.SaveChanges();
                return View("Credits100");
            }
            else
            {
                return View("WrongCode");
            }
            
        }
    }
}