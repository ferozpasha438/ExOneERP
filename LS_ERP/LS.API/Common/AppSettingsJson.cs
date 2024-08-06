using Microsoft.AspNetCore.Http;

namespace LS.API
{
    public class FileUploadDto
    {
        public IFormFile[] Files { get; set; }
    }
}
