namespace Web.BindingModels
{
    public class AddImageCommand
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
    }
}
