using CIN.Application;
using CIN.Application.Common;
using CIN.Application.SchoolMgtDto;
using CIN.Application.SchoolMgtQuery;
using CIN.Server;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.CINValidation.Controllers
{
    
    public class CINValidateController : ApiControllerBase
    {

          private readonly CINServerDbContext _cinDbContext;
        //private IConfiguration _Config;
        //private readonly IOptions<AppMobileSettingsJson> _appSettings;
        public CINValidateController(CINServerDbContext cinDbContext)
        {
           // _Config = config;
            _cinDbContext = cinDbContext;
          //  _appSettings = appSettings;
        }
        [HttpPost("ValidateCIN")]
            public async Task<IActionResult> ValidateCIN([FromBody] CheckParentLoginMetaDataDto Input)
            {
                var metaData = await _cinDbContext.MetaDataList.FirstOrDefaultAsync(e => e.CINNumber == Input.CINNumber);
                if ((metaData != null))
                {

                    byte[] connectionString = ASCIIEncoding.ASCII.GetBytes(metaData.DBConnectionString);
                    var DbConString = Convert.ToBase64String(connectionString);
                    
                    return Ok(new ValidateCinMessageDto { Message = "Successfully CIN Validated", 
                                                         Connectionstring = DbConString,
                                                           AdmUrl= metaData.AdmUrl,
                                                           FinUrl=metaData.FinUrl,
                                                           OpmUrl=metaData.OpmUrl,
                                                           InvUrl=metaData.InvUrl,
                                                           SndUrl=metaData.SndUrl,
                                                           PopUrl=metaData.PopUrl,
                                                           HrmUrl=metaData.HrmUrl,
                                                           HraUrl=metaData.HraUrl,
                                                           HrsUrl= metaData.HrsUrl,
                                                           FltUrl=metaData.FltUrl,
                                                           SchUrl=metaData.SchUrl,
                                                           ScpUrl=metaData.ScpUrl,
                                                           SctUrl=metaData.SctUrl,
                                                           PosUrl=metaData.PosUrl,
                                                           MfgUrl=metaData.MfgUrl,
                                                           CrmUrl=metaData.CrmUrl,
                                                           UtlUrl=metaData.UtlUrl,
                                                           Status = true });
                }

                return BadRequest(new ValidateCinFailedMessageDto { Message = "Invalid CIN Number", Status = false });
            }
    }
   
   
}
