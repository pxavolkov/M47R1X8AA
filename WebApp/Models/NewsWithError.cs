using Model;

namespace WebApp.Models
{
    public class NewsWithError: News
    {
        public string Error { get; set; }

        public NewsWithError(News news)
        {
            ID = news.ID;
            Title = news.Title;
            Text = news.Text;
            CreateDate = news.CreateDate;
        }

        public NewsWithError()
        {

        }
    }
}