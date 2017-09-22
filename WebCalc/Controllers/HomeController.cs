using CalcLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCalc.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var svc = Factory.CreateService();
            SetupExtButtons(svc as ICalcSvcEx);
            return View();
        }

        private void SetupExtButtons(ICalcSvcEx svc)
        {
            if (svc == null) return;
            ViewBag.ExtButtonText = new[] { 0, 1, 2, 3 }.Select(i => svc.GetExtButtonText(i+1)).ToArray();
        }

        [HttpPost]
        public ActionResult Index(string btn)
        {
            var ctx = Session["Context"] as ICalcContext;
            if (ctx == null)
            {
                ctx = Factory.CreateContext();
                Session["Context"] = ctx;
            }

            var btnName = $"Btn{btn}";
            CalcButton cb;
            if (!Enum.TryParse(btnName, out cb))
            {
                throw new ApplicationException($"ボタン'{btnName}'が解釈できません。");
            }

            var svc = Factory.CreateService();
            svc.OnButtonClick(ctx, cb);

            return new JsonResult() { Data = ctx };
        }
    }
}