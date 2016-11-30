using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
     /// The GroupCreationFormViewModel class represents the model of a social group creation form view.
     /// </summary>
        public class GroupCreationFormModel
        {
            public GroupCreationFormModel()
            {
            }

            public GroupCreationFormModel(PageReference currentPageLink, ContentReference currentBlockLink)
            {
                CurrentPageLink = currentPageLink;
                CurrentBlockLink = currentBlockLink;
            }

            /// <summary>
            /// The group name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The group description
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the reference link of the page containing the group creation form.
            /// </summary>
            public PageReference CurrentPageLink { get; set; }

            /// <summary>
            /// Gets or sets the reference link of the block containing the group creation form.
            /// </summary>
            public ContentReference CurrentBlockLink { get; set; }
        }
    }