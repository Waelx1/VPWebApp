using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPWebApp.Models;

namespace VPWebApp.Controllers
{
    [Route("api/authenticate")]
    [Produces("application/json")]
    public class AuthenticateController : Controller
    {
        // POST: api/authenticate
        [HttpPost]
        public JsonResult Authenticate(UserModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                try
                {
                    bool result = new Users().GetUsers().FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password) != null;
                    return Json(result);
                }
                catch
                {
                    return Json(false);
                }
            }
            else
                return Json(false);
        }
    }
}
