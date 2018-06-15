using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using Microsoft.Net.Http.Headers;
using VPWebApp.Models;
using VPWebApp.Shared;

namespace VPWebApp.Controllers
{
    [Route("api/confidentials")]
    [Produces("application/json")]
    public class ConfidentialsController : Controller
    {
        // GET: api/Confidentials
        [HttpGet]
        public JsonResult Confidentials()
        {
            return Json(IsAuthenticated(this.HttpContext));
        }

        /// <summary>
        /// Valide l'authentification de la requête passée en paramètre
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public bool IsAuthenticated(HttpContext httpContext)
        {
            try
            {
                HttpRequestMessageFeature httpRequestMessageFeature = new HttpRequestMessageFeature(httpContext);
                HttpRequestMessage httpRequestMessage = httpRequestMessageFeature.HttpRequestMessage;

                if (!httpRequestMessage.Headers.Contains(CustomRequestHeaders.Email)) return false;
                if (!httpRequestMessage.Headers.Date.HasValue) return false;
                if (httpRequestMessage.Headers.Authorization == null) return false;
                if (httpRequestMessage.Headers.Authorization.Scheme != CustomRequestHeaders.AuthorizationScheme) return false;

                //Calcul de la clé d'accès secrète de l'utilisateur
                string email = httpRequestMessage.Headers.GetValues(CustomRequestHeaders.Email).First();
                UserModel user = new Users().GetUsers().FirstOrDefault(x => x.Email == email);
                if (user == null) return false;
                string accessKey = EncodingMethods.GetHashedString(user.Password);

                //Création de la demande canonique
                string canonicalRequest = EncodingMethods.GetCanonicalRequest(httpRequestMessage);
                if (canonicalRequest == null) return false;

                //Création de la signature
                string signature = EncodingMethods.GetSignature(accessKey, canonicalRequest);
                if (signature == null) return false;

                //Comparaison des deux signatures
                return httpRequestMessage.Headers.Authorization.Parameter == signature;
            }
            catch
            {
                return false;
            }
        }
    }
}
