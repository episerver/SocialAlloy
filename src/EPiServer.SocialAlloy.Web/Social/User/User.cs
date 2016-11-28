using EPiServer.Social.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.User
{
    /// <summary>
    /// A class encapsulates the Reference and the Name of a user.  
    /// </summary>
    public class User
    {
        /// <summary>
        /// A parameterless constructor of the User class that will populate the Reference with an empty Reference and empty string for the Name. 
        /// </summary>
        public User()
        {
            Reference = Reference.Empty;
            Name = String.Empty;
        }
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An identifier that can be used to retrieve a user from the membership provider.
        /// </summary>
        public Reference Reference { get; set; }

        /// <summary>
        /// Used to denote anonymous users. 
        /// When a user is not authenticated they will have a populated User object with a name of Anonymous and an empty Reference. 
        /// </summary>
        public static User Anonymous
        {
            get
            {
                return new User
                {
                    Name = "Anonymous",
                    Reference = Reference.Empty
                };
            }
        }
    }
}