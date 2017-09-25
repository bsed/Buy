using Buy.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buy.Controllers
{
    [Authorize]
    public class CustomerServiceController : Controller
    {
        private void Sidebar()
        {
            ViewBag.Sidebar = "客服设置";
        }

        private string UserID
        {

            get
            {
                return User.Identity.GetUserId();
            }
        }

        // GET: CustomerService
        public ActionResult Index()
        {
            Sidebar();
            var model = new SystemSetting()
            {
                Value = Bll.SystemSettings.CustomerService,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SystemSetting model)
        {
            Bll.SystemSettings.CustomerService = model.Value;
            return View(model);
        }
    }
}