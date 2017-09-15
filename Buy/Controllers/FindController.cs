using Buy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class FindController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Find
        public ActionResult Index()
        {
            var shops = db.Shops.OrderBy(s => s.Sort).ToList();
            return View(shops);
        }
    }
}