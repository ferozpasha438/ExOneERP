using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LS.API.Zatca
{
    public class CsIdResultDto
    {
        public string requestID { get; set; }
        public string dispositionMessage { get; set; }
        public string binarySecurityToken { get; set; }
        public string secret { get; set; }
        public object errors { get; set; }
    }
    public class AppSettingsJson
    {
        public string UserImagePath { get; set; }
        public string LogoImagePath { get; set; }
        public string QRCodeImagePath { get; set; }
        public string ZatcaApi { get; set; }

    }
}
