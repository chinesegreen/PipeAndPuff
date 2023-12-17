using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ILocalStorageService
    {
        public Task<string> SaveFile(IFormFile file, string type);
    }
}
