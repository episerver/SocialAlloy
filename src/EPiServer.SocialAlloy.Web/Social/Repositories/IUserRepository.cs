using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Security.Principal;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// This interface represents common user functions used by Social samples to obtain 
    /// a user reference of a user from Identity or retrieve a user by identifier.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Returns the user reference of the user from the identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        Reference GetUserReference(IPrincipal Identity);

        /// <summary>
        /// Queries the underlying datastore and returns the user whose identifier 
        /// matches the specified reference identifier.
        /// </summary>
        /// <param name="id">User reference to search by</param>
        /// <returns></returns>
        User GetUser(Reference id);
    }
}
