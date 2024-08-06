using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LS.API.Purchase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected string GetConnectionString()
        {
            var connectionString = HttpContext.Request.Headers["ConnectionString"].FirstOrDefault();
            if (connectionString is not null && !string.IsNullOrEmpty(connectionString))
            {
                byte[] connection = System.Convert.FromBase64String(connectionString);
                return System.Text.ASCIIEncoding.ASCII.GetString(connection);
            }
            return null;
        }
    }
}
