using EPiServer.Core;
using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The GroupAdmissionBlockViewModel class represents the model that will be used to
    /// feed data to the Group Admission block view.
    /// </summary>
    public class GroupAdmissionBlockViewModel 
    {
        public string Heading { get; set; }
        /// <summary>
        /// A success message that should be flashed in the view.
        /// </summary>
        public string SubmitSuccessMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the view.
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        public string  GroupName { get; set; }
        /// <summary>
        /// Gets the name of the group that a member will be added to.
        /// </summary>
        public string Name { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the group admission block.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the page containing the group admission block.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the group admission form.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }
    }
}

