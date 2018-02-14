using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Model
{
    public class Account : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Account> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public string PlayerName { get; set; }
        public int PlayerAge { get; set; }
        public string Info { get; set; }
        public string Allergy { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ProfileInfo Profile { get; set; }
    }

    
}
