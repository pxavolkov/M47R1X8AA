namespace WebApp.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarPath { get; set; }

        public UserViewModel(string userId, string firstName, string lastName, string avatarPath)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            AvatarPath = avatarPath;
        }
    }
}