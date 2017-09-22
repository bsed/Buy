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
            var segmenter = new JiebaSegmenter();
            var segments = segmenter.Cut(text, cutAll: true);
            var segments2 = segmenter.Cut(text);
            var segments3 = segmenter.CutForSearch(text);
            var posSeg = new PosSegmenter();

            var tokens = posSeg.Cut(text).Where(s => s.Flag.IndexOf('n') == 0).Select(s => s.Word).Distinct();
            return Json(Comm.ToJsonResult("Success", "", segments3.Where(s => s.Length > 1).Distinct()), JsonRequestBehavior.AllowGet);
        }
    }

}