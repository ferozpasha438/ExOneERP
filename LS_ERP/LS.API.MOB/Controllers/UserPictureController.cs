using CIN.Application;
using CIN.Application.SystemQuery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LS.API.MOB.Controllers
{
    public class UserPictureController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        public UserPictureController(IOptions<AppMobileSettingsJson> appSettings, IWebHostEnvironment env) : base(appSettings)
        {
            _env = env;
        }


        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0 && files[0] != null && files[0].Length > 0)
                {
                    var file = files[0];

                    var guid = Guid.NewGuid().ToString();
                    string name = file.FileName;

                    //string description = Convert.ToString(HttpContext.Request.Form[name]);
                    //description = string.IsNullOrEmpty(description) ? file.FileName : description;

                    guid = $"{guid}_{name}_{ Path.GetExtension(file.FileName)}";
                    var webRoot = $"{_env.ContentRootPath}/files/userimgs";
                    var filePath = Path.Combine(webRoot, guid);

                    MobileCtrollerDto status = null;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        status = await Mediator.Send(new UpdateUserPicture() { Input = guid, User = UserInfo() });
                    }
                    if (status is not null)
                    {
                        byte[] imageArray = System.IO.File.ReadAllBytes(filePath);
                        status.Message = $"data:image/{Path.GetExtension(file.FileName).Replace(".","")};base64,{Convert.ToBase64String(imageArray)}";
                        return status.Status ? Ok(status) : BadRequest(status);
                    }
                }
            }
            catch (Exception ex)
            {

                return BadRequest(ApiMessageInfo.Status(0, false));
            }
            return BadRequest(ApiMessageInfo.Status(0, false));
        }


        ////[HttpPost]
        ////public async Task<IActionResult> UploadUserPicture(int id, string title, string context, string subcontext, IFormFile imageFile)
        ////{
        ////    AboutUsContextModel abUC = await _context.AboutUsContextModel.Include(lim => lim.Image).FirstOrDefaultAsync(limy => limy.AboutUs_Context_Id == id);
        ////    if (abUC == null)
        ////    {
        ////        return BadRequest("No such About Us Content!");
        ////    }
        ////    if (imageFile != null)
        ////    {
        ////        ImageModel imgfrmDB = abUC.Image;
        ////        if (imgfrmDB != null)
        ////        {
        ////            string cloudDomaim = "https://privacy-web.conveyor.cloud";
        ////            string uploadDrcty = _webEnvr.WebRootPath + "\\Images\\";

        ////            if (!Directory.Exists(uploadDrcty))
        ////            {
        ////                Directory.CreateDirectory(uploadDrcty);
        ////            }
        ////            string filePath = uploadDrcty + imageFile.FileName;

        ////            using (var fileStream = new FileStream(filePath, FileMode.Create))
        ////            {
        ////                await imageFile.CopyToAsync(fileStream);
        ////                await fileStream.FlushAsync();
        ////            }
        ////            using (var memoryStream = new MemoryStream())
        ////            {
        ////                await imageFile.CopyToAsync(memoryStream);
        ////                imgfrmDB.Image_Byte = memoryStream.ToArray();
        ////            }
        ////            imgfrmDB.ImagePath = cloudDomaim + "/Images/" + imageFile.FileName;
        ////            imgfrmDB.Modify_By = "CMS Admin";
        ////            imgfrmDB.Modity_dt = DateTime.Now;


        ////        }
        ////    }
        ////    abUC.Title = title;
        ////    abUC.Context = context;
        ////    abUC.SubContext = subcontext;
        ////    abUC.Modify_By = "CMS Admin";
        ////    abUC.Modity_dt = DateTime.Now;

        ////    _context.Entry(abUC).State = EntityState.Modified;
        ////    try
        ////    {
        ////        await _context.SaveChangesAsync();
        ////    }
        ////    catch (DbUpdateConcurrencyException)
        ////    {
        ////        if (!AboutUsContextModelExists(abUC.AboutUs_Context_Id))
        ////        {
        ////            return NotFound();
        ////        }
        ////        else
        ////        {
        ////            throw;
        ////        }
        ////    }
        ////    return Ok("About Us Content Delivered, Edit Successful");
        ////}

    }
}
