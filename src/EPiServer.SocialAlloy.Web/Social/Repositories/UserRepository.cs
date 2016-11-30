using EPiServer.SocialAlloy.Web.Social.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
        /// Returns the user reference of the user from the identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public string GetUserId(IPrincipal user)
        {
            return user.Identity.GetUserId();
        }

        /// <summary>
        /// Queries the underlying datastore and returns the user whose identifier 
        /// matches the specified reference identifier.
        /// </summary>
        /// <param name="id">User Id to search by</param>
        /// <returns></returns>
        public User GetUser(string id)
        {
            var user = manager.FindById(id);

            return user != null ?
                new User
                {
                    Name = user.UserName,
                    Id = id
                } :
                User.Anonymous;
        }
    }
}
