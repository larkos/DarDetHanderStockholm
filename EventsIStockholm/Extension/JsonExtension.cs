using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsIStockholm.Extensions
{
    public static class JsonExtension
    {

        public static T FromJson<T>(this string src)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(src);
        }

        public static string ToJson(this object src)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(src);
        }

    }
}