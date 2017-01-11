namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Custom extension data used in saving of member details for members associated with groups.
    /// </summary>
    public class MemberExtensionData
    {
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
        /// Gets or sets the company the member works for.
        /// </summary>
        public string Company { get; set; }
    }
}