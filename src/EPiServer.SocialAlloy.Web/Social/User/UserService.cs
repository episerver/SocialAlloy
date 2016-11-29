using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPiServer.Social.Common;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EPiServer.Cms.UI.AspNetIdentity;

namespace EPiServer.SocialAlloy.Web.Social.User
{
    public class UserService : IUserService
    {
        public Reference GetUserReference(IPrincipal user)
        {
            string userId = String.Empty;
            try
            {
                userId = user.Identity.GetUserId();
            }
            catch
            {

            }
            return String.IsNullOrWhiteSpace(userId) ?
                   Reference.Empty :
                   Reference.Create(userId);
        }

        /// <summary>
        /// Return the user the underlying datastore to that whose 
        /// identifier matches by the specified reference identifier.
        /// </summary>
        /// <param name="id">User reference to search by</param>
        /// <returns></returns>
        public User GetUser(Reference id)
        {
            IdentityUser user = null;
            try
            {
                var userManager = new UserManager<IdentityUser>(
                        new UserStore<IdentityUser>(new ApplicationDbContext<IdentityUser>()));
                user = userManager.FindById(id.Id);
            }
            catch { }
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
