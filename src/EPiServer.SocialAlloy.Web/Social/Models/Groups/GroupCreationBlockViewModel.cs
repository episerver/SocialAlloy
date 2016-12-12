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
    /// The GroupCreationsBlockViewModel class represents the model that will be used to
    /// feed data to the GroupCreation block frontend view.
    /// </summary>
    public class GroupCreationBlockViewModel
    {
        /// <summary>
        /// Gets or sets the heading for the Group Creation block.
        /// </summary>
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets whether to show the block heading .
        /// </summary>
        public bool ShowHeading { get; set; }

        /// <summary>
        /// A success message that should be flashed in the view.
        /// </summary>
        public string SubmitSuccessMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the view.
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the group description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the group creation form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the page containing the group creation block.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the group creation form.
        /// </summary>
        public ContentReference CurrentBlockLink { get; set; }
    }
}

