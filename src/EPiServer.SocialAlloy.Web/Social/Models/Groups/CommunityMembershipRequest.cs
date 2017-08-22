using System;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// CommunityMembershipRequest represents the workflow items that have been added for moderation.  
    /// </summary>
    public class CommunityMembershipRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityMembershipRequest()
        {
            this.Actions = new List<string>();
        }

        /// <summary>
        /// Gets or sets the state of the request.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the ID of the workflow describing
        /// the moderation process for this request.
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// Gets or sets the date on which this item was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets a collection of actions, which can be
        /// taken on this item, given its current state.
        /// </summary>
        public IEnumerable<string> Actions { get; set; }

        /// <summary>
        /// Gets or sets a reference to the user requesting
        /// membership to the group.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the ID of the group to which the user
        /// has requested membership.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the name of the user
        /// </summary>
        public string UserName { get; set; }
    }
}