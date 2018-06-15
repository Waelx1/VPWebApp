using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VPWebApp.Shared
{
    public static class EncodingMethods
    {
        /// <summary>
        /// Créé et retourne une version canonique de la requête passée en paramètre (selon la norme AWS)
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <returns></returns>
        public static string GetCanonicalRequest(HttpRequestMessage httpRequestMessage)
        {
            string date, email;

            if (httpRequestMessage.Headers.Date.HasValue)
                date = httpRequestMessage.Headers.Date.Value.UtcDateTime.ToString(CultureInfo.InvariantCulture);
            else
                return null;

            if (httpRequestMessage.Headers.Contains(CustomRequestHeaders.Email))
                email = httpRequestMessage.Headers.GetValues(CustomRequestHeaders.Email).First();
            else
                return null;

            string httpMethod = httpRequestMessage.Method.Method + "\n";
            string canonicalURI = httpRequestMessage.RequestUri.AbsoluteUri + "\n";
            string canonicalHeaders = date + ";" + email.ToLower() + "\n";
            string signedHeaders = CustomRequestHeaders.Date.ToLower() + ";" + CustomRequestHeaders.Email.ToLower() + "\n";

            return String.Join("\n", httpMethod, canonicalURI, canonicalHeaders, signedHeaders);
        }

        /// <summary>
        /// Créé et retourne une signature (selon la norme AWS)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetSignature(string key, string value)
        {
            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Hash une chaine de caractères et la retourne
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetHashedString(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytes));
        }
    }
}
