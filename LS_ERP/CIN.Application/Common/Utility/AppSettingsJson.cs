using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application
{
    public class AppSettingsJson
    {
        public string SessionTimeOut { get; set; }
        public string LogoImagePath { get; set; }
        public string UserImagePath { get; set; }
        public string QRCodeImagePath { get; set; }
        public string InventoryApi { get; set; }
        public string ZatcaApi { get; set; }
        public string Certificate { get; set; }
        public string PrivateKey { get; set; }
        public string Authorization { get; set; }
        public int ReportCount { get; set; }

    }
    public class AppMobileSettingsJson : AppSettingsJson
    {       
        public decimal SiteLocationNvMeter { get; set; }
        public decimal SiteLocationPvMeter { get; set; }
        public decimal SiteLocationExtraMeter { get; set; }
    }
    public enum InvoiceStatusIdType
    {
            Invoice = 1,
            Credit = 2
    }


}
