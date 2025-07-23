namespace TattooStudio.WebUI.ViewModels
{
    public class GalleryImageViewModel
    {
        public string ImageUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Description { get; set; }

        public GalleryImageViewModel(string imageUrl, string description = "")
        {
            ImageUrl = imageUrl;
            ThumbnailUrl = imageUrl;
            Description = description;
        }
    }
}