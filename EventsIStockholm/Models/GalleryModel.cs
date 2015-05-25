using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsIStockholm.Models
{
    public class GalleryModel
    {
        public Searchmodel searchtext {get;set;}
        public List<MyEvent> GalleryEvents { get; set; }
    }
}