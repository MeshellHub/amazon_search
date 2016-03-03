using AmazonApp.Models;
using AmazonApp.Models.Networking;
using AmazonApp.Models.Networking.Responses;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AmazonApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            List<Category> categories = Categories.Get();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetExchangeRates()
        {
            ExchangeRateResponse response = Networking.GetExchangeRates();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInitialProducts(string searchIndex, string keyword)
        {
            ProductResponse response = Networking.GetInitialProducts(searchIndex, keyword);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAdditionalProducts(string searchIndex, string keyword)
        {
            ProductResponse response = Networking.GetAdditionalProducts(searchIndex, keyword);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}