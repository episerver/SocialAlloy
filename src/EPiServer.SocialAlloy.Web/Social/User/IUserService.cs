using EPiServer.Social.Common;
using System.Security.Principal;

namespace EPiServer.SocialAlloy.Web.Social.User
{
    public interface IUserService
    {
        Reference GetUserReference(IPrincipal Identity);

        /// <summary>
        /// Return the user the underlying datastore to that whose 
        /// identifier matches by the specified reference identifier.
        /// </summary>
        /// <param name="id">User reference to search by</param>
        /// <returns></returns>
        User GetUser(Reference id);
    }
}
