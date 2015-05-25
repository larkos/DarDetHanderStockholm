using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventsIStockholm.Models;
namespace EventsIStockholm.Extension
{
    public class MyEventComparer
    {

        public bool Equals(MyEvent x, MyEvent y)
        {
            return x.EventName.Equals(y.EventName);
        }

        public int GetHashCode(MyEvent obj)
        {
            return obj.EventName.GetHashCode();
        }


    }

    
    }
