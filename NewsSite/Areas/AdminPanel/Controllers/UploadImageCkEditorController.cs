using Common.UploudImage;
using Microsoft.AspNetCore.Mvc;
using Service.Services.News;
using static Common.UploudImage.UploadFiles;

namespace NewsSite.Areas.AdminPanel.Controllers
{
    public class UploadImageCkEditorController : Controller
    {

        private readonly IUploadFiles uploadFiles;
        public UploadImageCkEditorController(IUploadFiles uploadFiles)
        {
            this.uploadFiles = uploadFiles;
        }

        [Route("/Upload/About")]
        public IActionResult UploudImg(IFormFile upload)
        {
            if (upload == null)
                BadRequest();

            var imageName = uploadFiles.SaveFileAndReturnName(upload, Directories.AboutContentImage);

            return Json(new { Uploaded = true, url = Directories.GetAboutContentImage(imageName)});
        }
    }
}
