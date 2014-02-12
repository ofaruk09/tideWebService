using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tideWebService.Models
{
    public class Port
    {
        public String portID { get; set; }
        public List<DayTideCollection> portForecast { get; set; }

    }
}