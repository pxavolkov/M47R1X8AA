using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SearchViewModel
    {
        public string SearchTerm { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}