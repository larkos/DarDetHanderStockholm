using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace EventsIStockholm.Models
{
    public static class TimerObj
    {

        static Timer timer;
        public static int minutes = 60;
       

        public static void Start()
        {
           
            timer = new Timer(minutes*60000); // Set up the timer for 3 seconds
            timer.AutoReset = true;
            //
            // Type "_timer.Elapsed += " and press tab twice.
            //
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true; // Enable it
        }
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
           
            AdRepository.AdRepo.UpdateCheck();
        }

        
    }
}