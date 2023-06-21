using CalcLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NeoCalc.WebCalc.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace WebCalc.Controllers
{
    public class HomeController : Controller
    {
        const string _SESSION_KEY = "_session_key";
        private IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memCache)
        {
            _memoryCache = memCache;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var svc = Factory.CreateService();
            ViewBag.SvcClassName = svc.GetType().Name;
            SetupExtButtons(svc as ICalcSvcEx);
            // uuidを生成しセッションに保存する。このuuidはインメモリキャッシュのキーになる。
            HttpContext.Session.SetString(_SESSION_KEY, Guid.NewGuid().ToString());
            return View();
        }

        private void SetupExtButtons(ICalcSvcEx svc)
        {
            if (svc == null) return;
            ViewBag.ExtButtonText = new[] { 0, 1, 2, 3 }.Select(i => svc.GetExtButtonText(i + 1)).ToArray();
        }

        [HttpPost]
        public ActionResult Index(string btn)
        {
            var sessionKey = HttpContext.Session.GetString(_SESSION_KEY) ?? string.Empty;
            // sessionKeyが空の場合は最初のページを開いていないのでリダイレクトする
            if (String.IsNullOrEmpty(sessionKey)){
                Redirect("Index");
            }
            //var ctx = Session["Context"] as ICalcContext;
            var ctx = GetObjectMemoryCache<ICalcContext>(sessionKey);
            if (ctx == null)
            {
                ctx = Factory.CreateContext();
                //Session["Context"] = ctx;
                SetObjectMemoryCache<ICalcContext>(sessionKey, ctx);
            }

            var btnName = $"Btn{btn}";
            CalcButton cb;
            if (!Enum.TryParse(btnName, out cb))
            {
                throw new ApplicationException($"ボタン'{btnName}'が解釈できません。");
            }

            var svc = Factory.CreateService();
            svc.OnButtonClick(ctx, cb);

            return new JsonResult( ctx );
        }

        private T? GetObjectMemoryCache<T>(string key)
        {
            _memoryCache.TryGetValue<T>(key, out var value);
            return value;
        }
        private void SetObjectMemoryCache<T>(string key, T value)
        {
            _memoryCache.Set<T>(key, value);
        }
    }
}