using Microsoft.AspNetCore.Mvc;

namespace Web.BindingModels
{
    public class DeleteImageCommand
    {
        public int Id { get; set; }
        public int Position { get; set; }
    }
}
