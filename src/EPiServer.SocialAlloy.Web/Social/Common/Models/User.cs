using System;

namespace EPiServer.SocialAlloy.Web.Social.Common.Models
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
            Id = String.Empty;
            Name = String.Empty;
        }
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An identifier that can be used to retrieve a user from the membership provider.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Used to denote anonymous users with a name of Anonymous and an empty Reference. 
        /// </summary>
        public static User Anonymous
        {
            get
            {
                return new User
                {
                    Name = "Anonymous",
                    Id = String.Empty
                };
            }
        }
    }
}