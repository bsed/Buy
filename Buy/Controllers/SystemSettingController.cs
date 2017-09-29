using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    public class SystemSettingController : Controller
    {
        // GET: SystemSetting
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Clean()
        {
            Bll.SystemSettings.Clean();
            return View();
        }

    }
}