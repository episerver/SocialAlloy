using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models
{
    public class SocialMember
    {
        public SocialMember( Reference userReference, GroupId groupId)
        {
            UserReference = userReference;
            GroupId = groupId;
        }
        public Reference UserReference { get; set; }

        public GroupId GroupId { get; set; }
    }
}