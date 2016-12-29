using EPiServer.Core;
using EPiServer.Social.Groups.Core;
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
        /// Gets or sets the heading for the Group Admission block.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading .
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// Gets and sets message details to be displayed to the user
        /// </summary>
        public List<MessageViewModel> Messages { get; set; }
        
        /// <summary>
        /// Gets the name of the group that a member will be added to.
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets and sets the value of the user email to be added as member extensiond data.
        /// </summary>
        public string MemberEmail { get; set; }

        /// <summary>
        /// Gets and sets the value of the user's company to be added as member extension data.
        /// </summary>
        public string MemberCompany { get; set; }

        /// <summary>
        /// Gets and sets the value for i
        /// </summary>
        public bool IsModerated { get; set; }

        /// <summary>
        /// Gets and sets the GroupId of the group that a user will be added to. 
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets and sets the name of the group that a user will be added to. 
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the group admission block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the group admission form.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; } 
    }
}

