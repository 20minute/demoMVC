using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Festival1.Models;
using System.Net.Http;

namespace Festival1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<FestivalierViewModel> festivaliers = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9572/api/");

                var responseTask = client.GetAsync("festivaliers");
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<FestivalierViewModel>>();
                    readTask.Wait();

                    festivaliers = readTask.Result;
                }
                else
                {
                    festivaliers = Enumerable.Empty<FestivalierViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error");
                }
            }
            return View(festivaliers);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FestivalierViewModel festivalier)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9572/api/festivaliers");

                var postTask = client.PostAsJsonAsync<FestivalierViewModel>("festivaliers", festivalier);
                postTask.Wait();

                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(festivalier);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
    }
}