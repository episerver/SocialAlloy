namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The view model used to display community member details.
    /// </summary>
    public class CommunityMemberViewModel
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="company">The company the member is assoicated with.</param>
        /// <param name="name">The name of the member.</param>
        public CommunityMemberViewModel(string company, string name)
        {
            Company = company;
            Name = name;
        }

        /// <summary>
        /// The company a member is associated with.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// The name of the member.
        /// </summary>
        public string Name { get; set; }
    }
}