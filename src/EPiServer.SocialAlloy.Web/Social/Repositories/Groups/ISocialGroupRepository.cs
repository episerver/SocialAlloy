using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;

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
        /// Updates a group to the underlying group repository.
        /// </summary>
        /// <param name="group">The updated group.</param>
        /// <returns>The updated group.</returns>
        SocialGroup Update(SocialGroup group);

        /// <summary>
        /// Retrieves a group based on the name of the group provided.
        /// </summary>
        /// <param name="groupName">The name of the group that is to be retrieved from the underlying data store.</param>
        /// <returns>The desired group.</returns>
        SocialGroup Get(string groupName);

        /// <summary>
        /// Retrieves a group based on a list of group ids that are provided.
        /// </summary>
        /// <param name="groupIds">The groups ids that are to be retrieved from the underlying data store.</param>
        /// <returns>The requested groups.</returns>
        List<SocialGroup> Get(List<string> groupIds);
    }
}
