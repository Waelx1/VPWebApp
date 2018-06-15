using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VPWebApp.Models
{
    public class Users
    {
        /// <summary>
        /// Retourne la liste des utilisateurs enregistrés
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetUsers()
        {
            return new List<UserModel>
            {
                new UserModel {Email = "pierre@vp.com", Password="pierre1234"},
                new UserModel {Email = "paul@vp.com", Password="paul1234"},
                new UserModel {Email = "jacques@vp.com", Password="jacques1234"},
                new UserModel {Email = "nicolas@vp.com", Password="nico1234"},
                new UserModel {Email = "christophe@vp.com", Password="chris1234"},
                new UserModel {Email = "wael@vp.com", Password="wael1234"},
            };
        }
    }
}