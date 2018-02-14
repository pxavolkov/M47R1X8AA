using System;
using Model;

namespace WebApp.Models
{
    public class ProfileViewModel
    {
        public ProfileInfo Profile { get; set; }

        public string FirstName => string.IsNullOrEmpty(Profile?.FirstName) ? "Unknown" : Profile.FirstName;
        public string LastName => string.IsNullOrEmpty(Profile?.LastName) ? "Unknown" : Profile.LastName;

        public string AvatarPath => string.IsNullOrEmpty(Profile?.PhotoPath) ? String.Empty : $"/Content/Avatars/{Profile.PhotoPath}";

        public string MiningTimeLeft
        {
            get
            {
                var now = DateTime.Now;
                if (Profile?.Balance?.MiningTime != null && Profile.Balance.MiningTime.Value > now)
                {
                    var diff = Profile.Balance.MiningTime.Value - now;
                    return diff.ToString(@"mm\:ss");
                }
                return "";
            }
        }

        public int CurrentBalance
        {
            get
            {
                var c = Profile?.Balance?.Current;
                if (!c.HasValue)
                {
                    return 0;
                }

                if (Profile.Balance.MiningTime.HasValue)
                {
                    c += 100;
                }

                return c.Value;
            }
        }

        public ProfileViewModel(ProfileInfo profile)
        {
            Profile = profile ?? new ProfileInfo();
        }
    }
}