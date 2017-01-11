using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// MembershipRequestModel represents the workflow items that have been added for moderation.  
    /// </summary>
    public class MembershipRequestModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipRequestModel()
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
        /// Gets or sets SocialMember data for the user requesting
        /// membership to the group.
        /// </summary>
        public SocialMember Member { get; set; }

        /// <summary>
        /// Gets or sets extension data for the user requesting
        /// membership to the group.
        /// </summary>
        public MemberExtensionData ExtensionData { get; set; }
    }
}