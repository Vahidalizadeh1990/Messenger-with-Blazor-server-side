using BlazorInputFile;
using Microsoft.AspNetCore.Hosting;
using PrivateMessenger.Models.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Repository
{
    public class FileUpload:IFileUpload
    {
        private readonly IWebHostEnvironment owebHostEnvironment;

        public FileUpload(IWebHostEnvironment owebHostEnvironment)
        {
            this.owebHostEnvironment = owebHostEnvironment;
        }
        // Remove a file if it exists in wwwroot user profile folder
        public void Remove(string name)
        {
            var path = Path.Combine(owebHostEnvironment.WebRootPath, "UserProfile", name);
            var exist = System.IO.File.Exists(path);
            if (exist)
            {
                System.IO.File.Delete(path);
            }
            
            
        }

        // Upload a file in our user profile folder in wwwroot
        public async Task Upload(IFileListEntry file)
        {
            var path = Path.Combine(owebHostEnvironment.WebRootPath, "UserProfile", file.Name);
            var memoryStream=new MemoryStream();
            
            await file.Data.CopyToAsync(memoryStream);
            using (FileStream fileStream=new FileStream(path,FileMode.Create,FileAccess.Write))
            {
                memoryStream.WriteTo(fileStream);
            }
        }
    }
}
