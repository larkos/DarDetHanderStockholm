using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsIStockholm.Models
{
    public class EventObject
    {
        public string uri { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string adress { get; set; }
        public string time_start { get; set; }
        public string time_end {get;set;}
        public string banner { get; set; }
        public string link { get; set; }

    }
}