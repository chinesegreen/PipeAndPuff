namespace Web.BindingModels
{
    public class EditShowcaseCommand
    {
        public IFormFile? Image1 { get; set; }
        public IFormFile? Image2 { get; set; }
        public IFormFile? Image3 { get; set; }
        public List<string> Links { get; set; }
        public List<string> Subtitles { get; set; }
        public List<string> Titles { get; set; }
        public IFormFile? CImage1 { get; set; }
        public IFormFile? CImage2 { get; set; }
        public IFormFile? CImage3 { get; set; }
        public IFormFile? CImage4 { get; set; }
        public List<string> CLinks { get; set; }
        public List<string> CSubtitles { get; set; }
        public List<string> CTitles { get; set; }
    }
}
