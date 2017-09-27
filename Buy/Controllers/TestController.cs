using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buy.Models;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using JiebaNet.Segmenter;
using JiebaNet.Analyser;
using JiebaNet.Segmenter.PosSeg;

namespace Buy.Controllers
{
    public class TestController : Controller
    {
        [AllowCrossSiteJson]
        public ActionResult Index(string text)
        {


            return Json(Comm.ToJsonResult("Success", ""), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FixCountType()
        {
            MoGuJie.Method.ReSetCidFile();
            return null;
        }
    }

}