using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIN.Application
{
    public static class EnumData
    {
        public static List<CustomSelectListItem> GetPayCodeTypeEnum()
        {
            return Enum.GetValues(typeof(PayCodeTypeEnum))
                 .Cast<PayCodeTypeEnum>()
                 .Select(v => new CustomSelectListItem { Text = v.ToString(), Value = ((int)v).ToString() })
                 .ToList();
        }
    }
}
