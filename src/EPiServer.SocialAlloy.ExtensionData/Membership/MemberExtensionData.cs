namespace EPiServer.SocialAlloy.ExtensionData.Membership
{
    /// <summary>
    /// Custom extension data used in saving of member details for members associated with groups.
    /// </summary>
    public class MemberExtensionData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MemberExtensionData()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="email">The email address of the member</param>
        /// <param name="company">The company that the member is associated with</param>
        public MemberExtensionData(string email, string company)
        {
            Email = email;
            Company = company;
        }

        /// <summary>
        /// Gets or sets the email of the member 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the company the member is associated with
        /// </summary>
        public string Company { get; set; }
    }
}
