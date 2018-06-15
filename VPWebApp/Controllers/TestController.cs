using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPWebApp.Shared;

namespace VPWebApp.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AuthenticateView()
        {
            return View();
        }

        public ActionResult ConfidentialsView()
        {
            return View();
        }

        public string RequestConfidentialsService()
        {
            Shared.CustomMessageHandler httpMessageHandler = new Shared.CustomMessageHandler();
            httpMessageHandler.Email = this.Request.Query[CustomRequestHeaders.Email];

            using (HttpClient client = new HttpClient(httpMessageHandler))
            {
                Task<string> response = client.GetStringAsync("https://localhost:5001/api/confidentials");
                response.Wait();
                return response.Result;
            }
        }
    }
}