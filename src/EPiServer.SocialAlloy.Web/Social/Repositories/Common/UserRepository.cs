using EPiServer.SocialAlloy.Web.Social.Common.Models;
using EPiServer.SocialAlloy.Web.Social.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Security.Principal;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// This class is used to perform common user functions used by Social samples to obtain user 
    /// reference of a user from Identity or display the name of 
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private UserManager<IdentityUser> manager;

        public UserRepository(UserManager<IdentityUser> manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Returns the id of the user from the identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public string GetUserId(IPrincipal user)
        {
            var userId = user.Identity.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return string.Empty;
            }

            return userId;
        }

        /// <summary>
        /// Queries the underlying datastore and returns the name of the user whose 
        /// identifier matches the specified reference identifier.
        /// </summary>
        /// <param name="id">User Id to search by</param>
        /// <returns>The user name.</returns>
        public string GetUserName(string id)
        {
            var userName = User.Anonymous.Name;

            if (!String.IsNullOrWhiteSpace(id))
            {
                var user = manager.FindById(id);
                if (user != null)
                {
                    userName = user.UserName;
                }
            }
            return userName;
        }

        /// <summary>
        /// Creates a unique uri to be associated with any authenticated user looking to gain admission to a group 
        /// </summary>
        /// <param name="user">The id of the user that is trying to join a group</param>
        /// <returns></returns>
        public string CreateAuthenticatedUri(string user)
        {
            return
               string.Format(
                   "social://{0}/{1}",
                   "Authenticated",
                   user
               );
        }

        /// <summary>
        /// Creates a unique uri to be associated with any anonymous user looking to gain admission to a group 
        /// </summary>
        /// <param name="user">The name of the user that is trying to join a group</param>
        /// <returns></returns>
        public string CreateAnonymousUri(string user)
        {
            return
               string.Format(
                   "social://{0}/{1}",
                   "Anonymous",
                   user
               );
        }

        /// <summary>
        /// Returns a boolean that reflects whether the uri provided is for an anonymous user or not 
        /// </summary>
        /// <param name="user">The unique uri of the user</param>
        /// <returns>boolean</returns>
        public bool IsAnonymous(string user)
        {
            return user.StartsWith("social://Anonymous/");
        }

        /// <summary>
        /// Returns only the provided username from the anonymous user
        /// </summary>
        /// <param name="user">The unique uri of the user</param>
        /// <returns>Substring of original uri</returns>
        public string GetAnonymousName(string user)
        {
            return user.Replace("social://Anonymous/", "");
        }

        /// <summary>
        /// Returns only user id that was originally retrieved from the identity
        /// </summary>
        /// <param name="user">The unique uri of the user</param>
        /// <returns>Substring of original uri</returns>
        public string GetAuthenticatedId(string user)
        {
            return user.Replace("social://Authenticated/", "");
        }

        /// <summary>
        /// Determines if the user is anonymous and then retrieves the last section of the uri
        /// </summary>
        /// <param name="user">The unique uri of the user</param>
        /// <returns>Substring of original uri</returns>
        public string ParseUserUri(string user)
        {
            return IsAnonymous(user)
                ? this.GetAnonymousName(user)
                : this.GetUserName(this.GetAuthenticatedId(user));
        }
    }
}
