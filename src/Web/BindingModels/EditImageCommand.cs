using Microsoft.AspNetCore.Mvc;

namespace Web.BindingModels
{
    public class EditImageCommand
    {
        public int Id { get; set; }
        public IFormFile Replacer { get; set; }
        public int Position { get; set; }
    }
}
