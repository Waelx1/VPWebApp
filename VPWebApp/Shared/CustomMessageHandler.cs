using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using VPWebApp.Models;

namespace VPWebApp.Shared
{
    class CustomMessageHandler : HttpClientHandler
    {
        public string Email { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            //Ajout de la date à la requête
            httpRequestMessage.Headers.Date = DateTime.UtcNow;

            //Ajout de l'email de l'utilisateur à la requête
            if (!httpRequestMessage.Headers.Contains(CustomRequestHeaders.Email))
                httpRequestMessage.Headers.Add(CustomRequestHeaders.Email, Email);

            //Calcul de la clé d'accès secrète de l'utilisateur
            string email = httpRequestMessage.Headers.GetValues(CustomRequestHeaders.Email).First();
            UserModel user = new Users().GetUsers().FirstOrDefault(x => x.Email == email);
            if (user == null) return base.SendAsync(httpRequestMessage, cancellationToken);
            string accessKey = EncodingMethods.GetHashedString(user.Password);

            //Création de la demande canonique
            string canonicalRequest = EncodingMethods.GetCanonicalRequest(httpRequestMessage);
            if (canonicalRequest == null) return base.SendAsync(httpRequestMessage, cancellationToken);

            //Création de la signature
            string signature = EncodingMethods.GetSignature(accessKey, canonicalRequest);
            if (signature == null) return base.SendAsync(httpRequestMessage, cancellationToken);

            //Création et ajout de l'entête d'autorisation 
            AuthenticationHeaderValue header = new AuthenticationHeaderValue(CustomRequestHeaders.AuthorizationScheme, signature);
            httpRequestMessage.Headers.Authorization = header;

            return base.SendAsync(httpRequestMessage, cancellationToken);
        }
    }
}
