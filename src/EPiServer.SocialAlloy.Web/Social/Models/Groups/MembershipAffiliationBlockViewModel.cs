using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// The MembershipAffiliationBlockViewModel class represents the model that will be used to
    /// feed data to the MembershipAffiliation block view.
    /// </summary>
    public class MembershipAffiliationBlockViewModel
        {
            public MembershipAffiliationBlockViewModel(MembershipAffiliationBlock currentBlock)
            {
                Heading = currentBlock.Heading;
                ShowHeading = currentBlock.ShowHeading;
                Messages = new List<MessageViewModel>();
                Groups = new List<SocialGroup>();
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
            public List<SocialGroup> Groups { get; set; }

            /// <summary>
            /// Gets and sets message details to be displayed to the user.
            /// </summary>
            public List<MessageViewModel> Messages { get; set; }
        }
    }

