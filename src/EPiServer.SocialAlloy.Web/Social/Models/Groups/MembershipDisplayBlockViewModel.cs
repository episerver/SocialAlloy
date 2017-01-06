﻿using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The MembershipDisplayBlockViewModel class represents the model that will be used to
    /// feed data to the Membership Display block view.
    /// </summary>
    public class MembershipDisplayBlockViewModel 
    {
        /// <summary>
        /// Gets or sets the heading for the membership display block.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading .
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Members displayed in the view will be associated with the group name provided in the admin view.
        /// </summary>
        public string  GroupName { get; set; }

        /// <summary>
        /// List of the users associated with the group
        /// </summary>
        public List<SocialCompositeMember> MemberList { get; set; }

        /// <summary>
        /// Gets and sets message details to be displayed to the user
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }
    }
}

