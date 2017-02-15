using EPiServer.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The GroupAdmissionBlockViewModel class represents the model that will be used to
    /// feed data to the Group Admission block view.
    /// </summary>
    public class GroupAdmissionBlockViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupAdmissionBlockViewModel(GroupAdmissionBlock block, PageReference currentPageLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            CurrentPageLink = currentPageLink;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupAdmissionBlockViewModel()
        {
        }

        /// <summary>
        /// Gets or sets the heading for the Group Admission block.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading .
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets or sets message details to be displayed to the user.
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }

        /// <summary>
        /// Gets the name of the group that a member will be added to.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the member email to be added as member extension data.
        /// </summary>
        public string MemberEmail { get; set; }

        /// <summary>
        /// Gets or sets the members's company to be added as member extension data.
        /// </summary>
        public string MemberCompany { get; set; }

        /// <summary>
        /// Gets or sets the value for whether the group has member admission moderation.
        /// </summary>
        public bool IsModerated { get; set; }

        /// <summary>
        /// Gets or sets the value for whether the user is currently logged in.
        /// </summary>
        public bool UserIsLoggedIn { get; set; }

        /// <summary>
        /// Gets or sets the value of the current state that a user is in while being moderated for admission to a group. 
        /// </summary>
        public string ModeratedUserAdmissionState { get; set; }

        /// <summary>
        /// Gets or sets the GroupId of the group that a member will be added to. 
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the group that a member will be added to. 
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the group admission block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }
    }
}

