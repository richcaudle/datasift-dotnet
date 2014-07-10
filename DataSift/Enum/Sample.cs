using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSift.Enum
{
    public enum Sample
    {
        [Description("10")]
        TenPercent,
        [Description("100")]
        OneHundredPercent
    }
}
