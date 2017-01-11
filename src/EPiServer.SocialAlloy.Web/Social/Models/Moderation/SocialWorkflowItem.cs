using EPiServer.Social.Common;
using EPiServer.Social.Moderation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflowItem class describes a workflow item model used by the SocialAlloy site
    /// </summary>
    public class SocialWorkflowItem
    {
        public SocialWorkflowItem(string id, string state, string target)
        {
            Id = id;
            State = state;
            Target = target;
        }

        /// <summary>
        /// Gets or sets the id of the workflow that the SocialWorkflowItem is associated with
        /// </summary>
        public String Id { get; set; }

        /// <summary>
        /// Gets or sets the current state that a workflow item is in within the workflow
        /// </summary>
        public String State { get; set; }

        /// <summary>
        /// Gets or sets a reference to the member and group data for this workflow item
        /// </summary>
        public String Target { get; set; }
    }
}