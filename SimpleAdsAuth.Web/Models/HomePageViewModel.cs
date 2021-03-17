using System.Collections.Generic;

namespace SimpleAdsAuth.Web.Models
{
    public class HomePageViewModel
    {
        public List<AdViewModel> Ads { get; set; }
        public string Message { get; set; }
    }
}