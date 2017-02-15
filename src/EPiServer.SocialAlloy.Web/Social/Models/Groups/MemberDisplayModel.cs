using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    public class MemberDisplayModel
    {
        public MemberDisplayModel(string company, string name)
        {
            Company = company;
            Name = name;
        }

        public string Company { get; set; }

        public string Name { get; set; }
    }
}