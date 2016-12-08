using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class MemberExtensionData
    {
        public MemberExtensionData(string email, string company)
        {
            Email = email;
            Company = company;
        }

        public string  Email { get; set; }

        public string  Company { get; set; }
    }
}