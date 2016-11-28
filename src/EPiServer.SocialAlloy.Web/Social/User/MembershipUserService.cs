using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EPiServer.SocialAlloy.Web.Social.User
{
    public class MembershipUserService
    {
        public MembershipUserService()
        {
        }

        public User Get(Reference userReference)
        {
            var user = GetTheCurrentUser(userReference.ToProviderUserKey());
            user.Reference = userReference;
            return user;
        }

        private User GetTheCurrentUser(Object id)
        {
            var membershipUser = id is string && string.IsNullOrWhiteSpace((id as string))
                ? null
                : Membership.GetUser(id);

            return new User
            {
                Name = membershipUser != null
                    ? membershipUser.UserName.Substring(membershipUser.UserName.IndexOf(@"\") + 1)
                    : string.Empty
            };
        }
    }
}