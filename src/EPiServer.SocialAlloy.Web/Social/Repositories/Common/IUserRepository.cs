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

        /// <summary>
        /// Determines if the user is anonymous and then retrieves the last section of the uri
        /// </summary>
        /// <param name="user">The unique uri of the user</param>
        /// <returns>Substring of original uri</returns>
        string ParseUserUri(string user);

        /// <summary>
        /// Creates a unique uri to be associated with any authenticated user looking to gain admission to a group 
        /// </summary>
        /// <param name="user">The id of the user that is trying to join a group</param>
        /// <returns></returns>
        string CreateAuthenticatedUri(string user);

        /// <summary>
        /// Creates a unique uri to be associated with any anonymous user looking to gain admission to a group 
        /// </summary>
        /// <param name="user">The name of the user that is trying to join a group</param>
        /// <returns></returns>
        string CreateAnonymousUri(string user);

        /// <summary>
        /// Returns only user id that was originally retrieved from the identity
        /// </summary>
        /// <param name="user">The unique uri of the user</param>
        /// <returns>Substring of original uri</returns>
        string GetAuthenticatedId(string user);
    }
}
