using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UploudImage
{
    public class UploadFiles : IUploadFiles
    {
        private readonly IHostingEnvironment _appEnvironment;

        public UploadFiles(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public string UploadFileFunc(IEnumerable<IFormFile> files, string uploadPath)
        {
            var upload = Path.Combine(_appEnvironment.WebRootPath, uploadPath);
            var filename = "";
            foreach (var item in files)
            {
                filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(item.FileName);
                using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                {
                    item.CopyTo(fs);
                }

            }
            return filename;
        }


        public string UploadAttachamentFunc(IEnumerable<IFormFile> files, string uploadPath, string username)
        {
            var upload = Path.Combine(_appEnvironment.WebRootPath, uploadPath);

            if (!Directory.Exists(upload + username))
            {
                Directory.CreateDirectory(upload + username);
            }
            upload = upload + username;

            var filename = "";
            foreach (var item in files)
            {
                filename = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(item.FileName);
                using (var fs = new FileStream(Path.Combine(upload, filename), FileMode.Create))
                {
                    item.CopyTo(fs);
                }

            }
            return filename;
        }


        public string SaveFileAndReturnName(IFormFile file, string savePath)
        {
            if (file == null)
                throw new Exception("File Is Null");

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), savePath.Replace("/", "\\"));
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fullPath = Path.Combine(folderPath, fileName);

            using var stram = new FileStream(fullPath, FileMode.Create);
            file.CopyTo(stram);
            return fileName;
        }

        public class Directories
        {
            public const string AboutContentImage = "wwwroot/upload/aboutimage";
            public static string GetAboutContentImage(string imageName) => $"{AboutContentImage.Replace("wwwroot", "")}/{imageName}";
        }

    }
}
