using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axpo.PowerTradeForecast.Infrastructure.Configuration
{
    public class AppSettings
    {
        public string TimeZone { get; set; }
        public int IntervalMinutes { get; set; }
        public string OutputFolder { get; set; }
    }
}
