using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventsIStockholm.Models;

namespace EventsIStockholm.Controllers
{
    public class GalleryController : Controller
    {
        //
        // GET: /Gallery/

        public ActionResult Index()
        {
            List<MyEvent> ToReturn = ClosestEvents((from ev in AdRepository.AdRepo.Events where ev.Location=="Stockholm" select ev).ToList());
            GalleryModel GM = new GalleryModel() { searchtext = new Searchmodel() { searchtext="Sök här"}, GalleryEvents = ToReturn };
            ViewBag.Title = "Event i Stockholm";
                return View(GM);
        }

        [HttpPost]
        public PartialViewResult search(Searchmodel sm)
        {
            List<MyEvent> ToReturn = (from ev in AdRepository.AdRepo.Events where ev.EventName.Contains(sm.searchtext) && ev.Location=="Stockholm" select ev).ToList();
            ViewBag.Title = "Sökresultat";
            return PartialView("Cool",ToReturn);
        }

        public ActionResult category()
        {
            return View();
        }

        public List<MyEvent> ClosestEvents(List<MyEvent> Events)
        {
            List<MyEvent> Ordered = new List<MyEvent>();
           

            Ordered = (from ev in Events
                       orderby DateTime.Parse(ev.EventDate)
                     ascending
                       select ev).ToList();

           
            return Ordered;
        }

        public ActionResult EventSearch()
        {
            
            List<MyEvent> ToReturn=ClosestEvents(AdRepository.AdRepo.Events);
            GalleryModel gm = new GalleryModel() { searchtext = new Searchmodel() { searchtext = "" }, GalleryEvents = ToReturn };
            ViewBag.Title = "Sök bland alla event";
            return View(gm);
        }

        [HttpPost]
        public PartialViewResult EventSearch(Searchmodel sm)
        {
            List<MyEvent> ToReturn = (from ev in AdRepository.AdRepo.Events where ev.EventName.Contains(sm.searchtext) select ev).ToList();

            return PartialView("Cool", ToReturn);
        }

        public ActionResult OmSverigeEvent()
        {
            return View();
        }

    }
}
