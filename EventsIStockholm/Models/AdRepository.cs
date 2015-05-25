using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Resources;
using EventsIStockholm.Extensions;
using EventsIStockholm.Models;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;


namespace EventsIStockholm.Models
{
    public class AdRepository
    {


        public static AdRepository AdRepo { get; set; }

        public List<MyEvent> Events { get; set; }
        public string LatestUpdated { get; set; }
        public bool IsUpdated { get; set; }
        public static void init()
        {
            if(AdRepo==null)
            {
                AdRepo = new AdRepository();
                AdRepo.Events = new List<MyEvent>();
                AdRepo.IsUpdated = false;
                AdRepo.LatestUpdated = new DateTime().ToString();
                TimerObj.Start();
            }
           
            AdRepo.UpdateCheck();
        }

        public void UpdateCheck()
        {
            if(LatestUpdated!=DateTime.Today.ToString())
            {
                Thread UpdateThread = new Thread(AdRepo.DoUpdate);
                UpdateThread.Start();
                
                LatestUpdated = DateTime.Today.ToString();
            }
        }

        public void DoUpdate()
        {
            MakeRequest("http://konvent.se/api/upcomingeventsbanner/","");
            List<Category> response = RequestCategorys("http://api.eventful.com/json/categories/list?app_key=XwxQTgn9JbWFP3Xw");

            foreach (Category c in response)
            {
                wait();
                MakeRequest("http://api.eventful.com/json/events/search?app_key=XwxQTgn9JbWFP3Xw&l=Sweden&c=", c.id);
            }

            string stopp;
           
        }

        private void wait()
        {
            Thread.Sleep(30000);
        }

        public List<Category> RequestCategorys(string url)
        {
            RootListObject listedcategories = new RootListObject();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:

                            using (Stream stream = response.GetResponseStream())
                            {
                                string json = "";
                                byte[] buffer = new byte[1048];
                                int read = 0;
                                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    json += System.Text.Encoding.UTF8.GetString(buffer, 0, read);
                                }
                                    listedcategories = json.FromJson<RootListObject>();
                                
                            }
                            break;
                        case HttpStatusCode.NoContent:


                            break;
                    }
                }
            }
            catch
            {

            }

            return listedcategories.category;
        }

       

        public List<MyEvent> MakeRequest(string url,string id)
        {

            DateTime fran = DateTime.Today;
            DateTime till = fran.AddDays(90);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://hardcoded");
            List<EventObject> retur = new List<EventObject>();
            RootObject retur_eventful = new RootObject();
            RootListObject listedcategories = new RootListObject();
            List<MyEvent> Events = new List<MyEvent>();

            if (id!="")
            {
                request = (HttpWebRequest)WebRequest.Create(url+id);
                request.Method = "GET";
            }
            else if(url=="http://konvent.se/api/upcomingeventsbanner/")
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
            }
            else if (url=="http://api.eventful.com/json/categories/list?app_key=XwxQTgn9JbWFP3Xw")
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
            }
           
           
            
            
            
            
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.OK:



                            using (Stream stream = response.GetResponseStream())
                            {
                                string json = "";
                                byte[] buffer = new byte[1048];
                                int read = 0;
                                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    json += System.Text.Encoding.UTF8.GetString(buffer, 0, read);
                                }

                                if (id != "")
                                {
                                    retur_eventful = json.FromJson<RootObject>();
                                }
                                else if (url == "http://konvent.se/api/upcomingeventsbanner/")
                                {
                                    retur = json.FromJson<List<EventObject>>();
                                    
                                }
                                else if (url == "http://api.eventful.com/json/categories/list?app_key=XwxQTgn9JbWFP3Xw")
                                {
                                    listedcategories = json.FromJson<RootListObject>();
                                }

                                string stopp;


                           
                            }
                            break;
                        case HttpStatusCode.NoContent:


                            break;
                    }
                }
            }
            catch
            {

            }

            if (id != "" && retur_eventful.events!=null)
            {
                Events = (from ev in retur_eventful.events.@event
                          where ev.image!=null && ev.description!=null
                          select new MyEvent()
                          {
                              MyID = ev.id,
                              EventName = ev.title,
                              EventDate = ev.start_time,
                              EventUrl = ev.url,
                              ImageUrl = ev.image.medium.url,
                              Location = ev.city_name,
                              LongDescription = ev.description,
                              ShortDescription =CreateShortdescription(ev.description)
                          }).ToList();
               
            }
            else
            {
                Events = (from ev in retur
                          select new MyEvent()
                          {
                              
                              EventName = ev.name,
                              EventDate = ev.time_start,
                              EventUrl = ev.link,
                              ImageUrl = ev.banner,
                              Location = ev.city,
                              LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
                              ShortDescription = CreateShortdescription("lorum")
                          }).ToList();
            }
           
            foreach(MyEvent me in Events)
            {
                if(me.EventName!=null && me.LongDescription!=null && me.EventDate!=null && me.ImageUrl!=null)
                AdRepo.Events.Add(me);
            }
            
            
            //for (int i = 0; i < 100;i++ )
            //{
            //    AdRepo.Events.Add(new MyEvent() { EventName = "Testevent", EventDate = DateTime.Now.ToString(), EventUrl = "mitt event", LongDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum", ShortDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor..." });
            //}
          
           return Events;
        }

        public string CreateShortdescription(string htmlslongtring)
        {
            //first strip the string from its html markup
            string retur = System.Text.RegularExpressions.Regex.Replace(htmlslongtring, "<.*?>", String.Empty);

            int TextLength = 500;

            if(retur.Length<=TextLength)
            {
                return retur;
            }
            
            
                char[] delimiters = new char[] { ' ', '.', ',', ':', ';' };
                int index = retur.LastIndexOfAny(delimiters, TextLength - 3);

                if (index > (TextLength / 2))
                {
                    return retur.Substring(0, index) + "...";
                }
                else
                {
                    return retur.Substring(0, TextLength - 3) + "...";
                }
            


            
        }
      
         //public class TimerObject
         //{
         //    public int value { get; set; }
         //    public System.Threading.Timer TimerReference { get; set; }
         //    public bool TimerCanceled { get; set; }

         //    private void TimerTask(object Stateobject)
         //    {
         //        TimerObject TOstate = (TimerObject)Stateobject;
         //        System.Threading.Interlocked.Increment(ref TOstate.value);
         //        System.Diagnostics.Debug.WriteLine("Launched new thread  " + DateTime.Now.ToString());
         //        if (TOstate.TimerCanceled)
         //        // Dispose Requested.
         //        {
         //            TOstate.TimerReference.Dispose();
         //            System.Diagnostics.Debug.WriteLine("Done  " + DateTime.Now.ToString());
         //        }
         //    }

         //    private void RunTimer()
         //    {
         //        TimerObject TObject = new TimerObject();
         //        TObject.TimerCanceled = false;
         //        TObject.value = 1;
         //        System.Threading.TimerCallback TimerDelegate = new System.Threading.TimerCallback(TimerTask);



         //        System.Threading.Timer TimerItem =
         //   new System.Threading.Timer(TimerDelegate, TObject, 2000, 2000);

         //        TObject.TimerReference = TimerItem;
         //        while (TObject.value < 10)
         //        {
         //            // Wait one second.
         //            System.Threading.Thread.Sleep(1000);
         //        }

         //        // Request Dispose of the timer object.
         //        TObject.TimerCanceled = true;

         //    }
         //}

      

    }



    public class MyEvent
    {
        public string MyID { get; set; }
        public string EventName {get;set;}
        public string EventDate { get; set; }
        public string Location { get; set; }

        
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string EventUrl { get; set; }
        public string ImageUrl { get; set; }
    }

   
}