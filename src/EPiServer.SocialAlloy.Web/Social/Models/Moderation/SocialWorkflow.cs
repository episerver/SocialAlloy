﻿using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Models.Moderation
{
    /// <summary>
    /// The SocialWorkflow describes a workflow model used by the SocialAlloy site.
    /// </summary>
    public class SocialWorkflow
    {
        public SocialWorkflow(string id, string name, string initialState )
        {
            this.Id = id;
            this.Name = name;
            this.InitialState = initialState;
        }
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the initial state of a workflow item that enters this social workflow
        /// </summary>
        public string InitialState { get; set; }

        /// <summary>
        /// Gets or sets the name of the workflow.
        /// </summary>
        public string Name { get; set; }
    }
}