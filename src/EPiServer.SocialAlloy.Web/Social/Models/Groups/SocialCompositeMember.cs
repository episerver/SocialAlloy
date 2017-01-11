using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The SocialCompositeMember represents the structure containing the member model used by 
    /// the SocialAlloy site along with extension data containing other member-related information 
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