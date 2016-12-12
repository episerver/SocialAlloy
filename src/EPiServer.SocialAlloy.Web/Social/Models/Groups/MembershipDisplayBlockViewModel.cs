using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
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
    }
}

