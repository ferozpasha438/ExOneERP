using CIN.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.HRM.Admin.Controllers.Common
{
    public class FileUploadController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env, IOptions<AppSettingsJson> appSettings)
        {
            _env = env;
        }

        [HttpGet("downLoadFilesByFileName")]
        public async Task<IActionResult> DownLoadFilesByFileName([FromQuery] string folderName, [FromQuery] string fileName)
        {
            ///files/employeedocuments
            var webRoot = $"{_env.ContentRootPath}/files/{folderName}";
            var filePath = Path.Combine(webRoot, fileName);

            byte[] stream = await System.IO.File.ReadAllBytesAsync(filePath);

            //Determine the Content Type of the File.
            string mimeType = "";
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out mimeType);
            return new FileContentResult(stream, mimeType);
        }
    }
}
