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
        /// Returns the user Id of the user from the identity.
        /// </summary>
        /// <param name="Identity">The user identity.</param>
        /// <returns>The user id.</returns>
        string GetUserId(IPrincipal Identity);

        /// <summary>
        /// Queries the underlying datastore and returns the name of the user whose 
        /// identifier matches the specified reference identifier.
        /// </summary>
        /// <param name="id">User Id to search by</param>
        /// <returns>The user name.</returns>
        string GetUserName(string id);
    }
}
