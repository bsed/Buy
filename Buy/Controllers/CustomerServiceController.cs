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
        [Authorize(Roles = SysRole.CustomerServiceManageRead)]
        public ActionResult Index()
        {
            Sidebar();
            var model = new CustomerViewModel()
            {
                Value = Bll.SystemSettings.CustomerService,
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SysRole.CustomerServiceManageEdit)]
        public ActionResult Index(CustomerViewModel model)
        {
            Bll.SystemSettings.CustomerService = model.Value;
            return RedirectToAction("Index");
        }
    }
}