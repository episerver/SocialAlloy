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
        /// Email of the member 
        /// </summary>
        public string  Email { get; set; }

        /// <summary>
        /// Company the member works for.
        /// </summary>
        public string  Company { get; set; }
    }
}