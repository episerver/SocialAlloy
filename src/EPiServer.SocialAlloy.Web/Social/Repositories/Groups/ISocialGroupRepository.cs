using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The ISocialGroupRepository interface describes a component capable
    /// of persisting, and retrieving social group data from
    /// an underlying data store.
    /// </summary>
    public interface ISocialGroupRepository
    {
        /// <summary>
        /// Adds a group to the underlying group repository.
        /// </summary>
        /// <param name="group">The group to add.</param>
        /// <returns>The added group.</returns>
        SocialGroup Add(SocialGroup group);

        /// <summary>
        /// Retrieves a group based on the name of the group provided.
        /// </summary>
        /// <param name="groupName">The name of the group that is to be retrieved from the underlying data store.</param>
        /// <returns>The desired group.</returns>
        SocialGroup Get(string groupName);

    }
}
