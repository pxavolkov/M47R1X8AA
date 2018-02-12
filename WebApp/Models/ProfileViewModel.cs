using System;
using Model;

namespace WebApp.Models
{
    public class ProfileViewModel
    {
        public ProfileInfo Profile { get; set; }

        public string AvatarPath => string.IsNullOrEmpty(Profile?.PhotoPath) ? String.Empty : $"/Content/Avatars/{Profile.PhotoPath}";

        public ProfileViewModel(ProfileInfo profile)
        {
            Profile = profile ?? new ProfileInfo();
        }
    }
}