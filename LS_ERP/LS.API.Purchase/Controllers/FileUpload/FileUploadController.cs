using CIN.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using CIN.Application.SystemQuery;
using CIN.Application.Common;
using static System.Net.WebRequestMethods;
using Microsoft.AspNetCore.StaticFiles;

namespace LS.API.Purchase.Controllers
{

    public class FileUploadController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileUploadController(IWebHostEnvironment env, IOptions<AppSettingsJson> appSettings)
        {
            _env = env;
        }

        //[HttpPost("fileupload")]
        //public async Task<IActionResult> FileUpload([FromForm] FileUploadDto input)
        //{
        //    // do the magic here
        //    return NoContent();
        //}
        //[HttpPost("createDocument")]
        //public IActionResult CreateDocument()
        //{
        //    var files = HttpContext.Request.Form;
        //    foreach (var key in Request.Form.Keys)
        //    {
        //        var data = JsonConvert.DeserializeObject<FileUploadDto>(Request.Form[key]);
        //        var file = Request.Form.Files["file" + key];

        //    }
        //    return Ok();
        //}


        [HttpGet("getFilesByModulewiseId")]
        public async Task<IActionResult> GetFilesByModulewiseId([FromQuery] string sourceId, [FromQuery] string action)
        {
            return Ok(await Mediator.Send(new GetFilesByModulewiseId() { Input = new() { SourceId = sourceId, Action = action } }));
        }

        [HttpGet("downLoadFilesByFileName")]
        public async Task<IActionResult> DownLoadFilesByFileName([FromQuery] string fileName)
        {
            var webRoot = $"{_env.ContentRootPath}/files";
            var filePath = Path.Combine(webRoot, fileName);

            byte[] stream = await System.IO.File.ReadAllBytesAsync(filePath);

            //Determine the Content Type of the File.
            string mimeType = "";
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out mimeType);
            return new FileContentResult(stream, mimeType);
        }




        [HttpPost("testuploadfiles")]
        public async Task<IActionResult> Testuploadfiles()
        {
            var files = HttpContext.Request.Form.Files;
            var module = Convert.ToString(HttpContext.Request.Form["modulefile"]);
            FileUploadItem fileItem = new();

            if (module.HasValue())//&& module.Length > 40
            {
                fileItem = JsonConvert.DeserializeObject<FileUploadItem>(module);
                return Ok(fileItem);

                //var fileUpload = await Mediator.Send(new UploadingFile() { Input = null });
                //if (fileUpload.Id > 0)
                //{
                //    return Ok(fileItem);
                //}
                //return BadRequest(new ApiMessageDto { Message = fileUpload.Message });
            }

            return BadRequest(new ApiMessageDto { Message = ApiMessageInfo.Failed });

        }


        [HttpPost("uploadfiles")]
        public async Task<IActionResult> UploadfilesAsync()
        {
            try
            {
                List<TblErpSysFileUploadDto> fileUploads = new();
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        var guid = Guid.NewGuid().ToString();
                        string fileName = file.FileName;
                        string name = file.Name;

                        string description = Convert.ToString(HttpContext.Request.Form[name]);
                        description = string.IsNullOrEmpty(description) ? file.FileName : description;

                        guid = $"{guid}_{Path.GetExtension(file.FileName)}";
                        var webRoot = $"{_env.ContentRootPath}/files";
                        var filePath = Path.Combine(webRoot, guid);
                        fileUploads.Add(new() { Description = description, FileName = guid, Name = fileName, UploadedBy = String.Empty });

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                    }
                }

                var module = Convert.ToString(HttpContext.Request.Form["modulefile"]);
                FileUploadItem fileItem = new();
                if (module.HasValue())//&& module.Length > 40
                {
                    fileItem = JsonConvert.DeserializeObject<FileUploadItem>(module);
                    fileUploads.ForEach(file =>
                    {
                        file.SourceId = fileItem.SourceId;
                        file.Type = fileItem.Action;
                    });

                    var fileUpload = await Mediator.Send(new UploadingFile() { Input = fileUploads });
                    if (fileUpload.Id > 0)
                    {
                        return Ok(fileItem);
                    }
                    return BadRequest(new ApiMessageDto { Message = fileUpload.Message });
                }

                return Ok(module);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }

}

