using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Social.Common;
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
        /// Returns the user reference of the user from the identity.
        /// </summary>
        /// <param name="Identity"></param>
        /// <returns></returns>
        public Reference GetUserReference(IPrincipal user)
        {
            string userId = String.Empty;
            userId = user.Identity.GetUserId();

            return String.IsNullOrWhiteSpace(userId) ?
                   Reference.Empty :
                   Reference.Create(userId);
        }

        /// <summary>
        /// Queries the underlying datastore and returns the user whose identifier 
        /// matches the specified reference identifier.
        /// </summary>
        /// <param name="id">User reference to search by</param>
        /// <returns></returns>
        public User GetUser(Reference id)
        {
            //var userManager = new UserManager<IdentityUser>(
            //        new UserStore<IdentityUser>(new ApplicationDbContext<IdentityUser>()));
            var user = manager.FindById(id.Id);

            return user != null ?
                new User
                {
                    Name = user.UserName,
                    Reference = id
                } :
                User.Anonymous;
        }
    }
}
