namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The GroupCreationFormViewModel class represents the model of a social group creation form view.
    /// </summary>
    public class GroupCreationFormViewModel
    {
        public GroupCreationFormViewModel()
        {
        }

        /// <summary>
        /// The group name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// The group description
        /// </summary>
        public string GroupDescription { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the group creation form.
        /// </summary>
        public string CurrentPageLink { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the page containing the group creation block.
        /// </summary>
        public string PageId { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the block containing the group creation form.
        /// </summary>
        public string CurrentBlockLink { get; set; }
    }
}