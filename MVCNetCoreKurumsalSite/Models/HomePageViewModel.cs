using Entities;

namespace MVCNetCoreKurumsalSiteProje.Models
{
    public class HomePageViewModel
    {
        public List<Slide>? Slides { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
