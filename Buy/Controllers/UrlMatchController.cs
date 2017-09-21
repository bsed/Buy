using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using System.Data.Entity;

namespace Buy.Controllers
{
    public class UrlMatchController : Controller
    {
        // GET: UrlMatch

        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Index(string keyWord, Enums.UrlMatchType type, int page = 1, int pageSize = 15)
        {
            List<UrlMatch> model = new List<UrlMatch>();
            switch (type)
            {
                default:
                    break;
            }
            return View(model);
        }
    }
}