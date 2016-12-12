using EPiServer.Core;
using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        /// A error message that should be flashed in the view.
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// Members displayed in the view will be associated with the group name provided in the admin view.
        /// </summary>
        public string  GroupName { get; set; }

        /// <summary>
        /// List of the users associated with the group
        /// </summary>
        public List<Composite<Member, MemberExtensionData>> MemberList { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the membership display block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the page containing the membership display  block.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the membership display  form.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; } 
    }
}

