using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.UploudImage
{
    public interface IUploadFiles 
    {
        string UploadFileFunc(IEnumerable<IFormFile> files, string uploadPath);
        string UploadAttachamentFunc(IEnumerable<IFormFile> files, string uploadPath, string username);
        string SaveFileAndReturnName(IFormFile file, string savePath);


    }
}
