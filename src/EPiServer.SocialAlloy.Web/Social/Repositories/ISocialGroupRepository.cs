using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public interface ISocialGroupRepository
    {
        /// <summary>
        /// Adds a group to the underlying group repository.
        /// </summary>
        /// <param name="group">The group to add.</param>
        /// <returns>The added group.</returns>
        Group Add(Group group);

    }
}
