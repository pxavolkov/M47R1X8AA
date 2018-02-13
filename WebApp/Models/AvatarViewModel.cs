namespace WebApp.Models
{
    public class AvatarViewModel
    {
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int DisplayWidth
        {
            get { return Height > Width ? 500 * Width / Height : 500; }
        }

        public int DisplayHeight
        {
            get { return Width > Height ? 500 * Height / Width : 500; }
        }
    }
}