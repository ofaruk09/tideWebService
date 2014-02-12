using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tideWebService.Models
{
    public class DayTideCollection
    {
        public string Date;
        public List<TidalEvent> EventData { get; set; }
    }
}
