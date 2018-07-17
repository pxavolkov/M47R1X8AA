using System.Collections.Generic;
using Model;

namespace WebApp.Models
{
    public class NewsViewModel
    {
        public List<NewsWithError> News { get; set; }
        public NewsWithError NewNews { get; set; }
    }
}