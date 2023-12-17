using Infrastructure.Interfaces;
using Infrastructure.S3Models;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        public async Task<string> SaveFile(IFormFile file, string type)
        {
            int maxWidth = 600;

            string unique = Guid.NewGuid().ToString();
            string name = unique + Path.GetExtension(file.FileName);
            string root = $"wwwroot/img/{type}/";
            string filePath = Path.Combine(root, name);

            using var stream = file.OpenReadStream();
            using var image = Image.Load(stream);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                if (image.Width > maxWidth)
                {
                    var thumbScaleFactor = maxWidth / image.Width;
                    image.Mutate(i => i.Resize(maxWidth, image.Height *
                        thumbScaleFactor));
                }

                await image.SaveAsPngAsync(fileStream);
            }

            return $"/img/{type}/{name}";
        }
    }
}
