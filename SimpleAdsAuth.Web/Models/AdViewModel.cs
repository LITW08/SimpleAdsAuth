using SimpleAdsAuth.Data;

namespace SimpleAdsAuth.Web.Models
{
    public class AdViewModel
    {
        public SimpleAd Ad { get; set; }
        public bool CanDelete { get; set; }
    }
}