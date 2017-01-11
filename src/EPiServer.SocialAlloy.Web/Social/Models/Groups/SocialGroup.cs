using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The SocialGroup describes the a group model used by the SocialAlloy site.
    /// </summary>
    public class SocialGroup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the social group</param>
        /// <param name="description">A description for the social group</param>
        public SocialGroup(string name, string description) : this("", name, description)
        {

        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The id of the social group</param>
        /// <param name="name">The name of the social group</param>
        /// <param name="description">A description for the social group</param>
        public SocialGroup(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}