using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// A composite class combining of SocialMember and MemberExtensionData
    /// </summary>
    public class SocialCompositeMember
    {
        /// <summary>
        /// Gets and sets member data
        /// </summary>
        public SocialMember Member { get; set; }
       
        /// <summary>
        /// Gets and sets extension data for members
        /// </summary>
        public MemberExtensionData MemberExtension { get; set; }
    }
}