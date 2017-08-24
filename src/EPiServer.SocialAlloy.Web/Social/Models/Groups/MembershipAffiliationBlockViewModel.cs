using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The MembershipAffiliationBlockViewModel class represents the model that will be used to
    /// feed data to the MembershipAffiliation block view.
    /// </summary>
    public class MembershipAffiliationBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="currentBlock">The current member affiliation block</param>
        public MembershipAffiliationBlockViewModel(MembershipAffiliationBlock currentBlock)
        {
            Heading = currentBlock.Heading;
            ShowHeading = currentBlock.ShowHeading;
            Messages = new List<MessageViewModel>();
            Groups = new List<Community>();
        }
        /// <summary>
        /// Gets or sets the heading for the membership display block.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading.
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// List of the groups that the current user is associated with.
        /// </summary>
        public List<Community> Groups { get; set; }

        /// <summary>
        /// Gets and sets message details to be displayed to the user.
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }
    }
}

