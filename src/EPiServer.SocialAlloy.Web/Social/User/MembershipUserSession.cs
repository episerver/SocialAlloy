using EPiServer.SocialAlloy.Web.Social.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.User
{
    public class MembershipUserSession
    {
        public Func<object> GetCurrentUserId;
        public Func<string> GetCurrentUserName;
        private IPrincipal user;

        public MembershipUserSession(IPrincipal user)
        {
            this.user = user;
            this.GetCurrentUserId = this.GetTheCurrentUserId;
            this.GetCurrentUserName = this.GetTheCurrentUserName;
        }

        private object GetTheCurrentUserId()
        {
            var user = System.Web.Security.Membership.GetUser(this.user.Identity.Name);
            return user == null
                ? null
                : user.ProviderUserKey;
        }

        private string GetTheCurrentUserName()
        {
            var user = System.Web.Security.Membership.GetUser();
            return user == null
                ? null
                : user.UserName;
        }

        public User Current
        {
            get
            {
                var id = this.GetCurrentUserId();

                return id == null
                    ? User.Anonymous
                    : new User()
                    {
                        Reference = id.ToReference(),
                        Name = this.GetCurrentUserName()
                    };
            }
        }
    }
}