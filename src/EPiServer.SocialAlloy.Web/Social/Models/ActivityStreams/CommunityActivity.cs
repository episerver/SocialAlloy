namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// Represents community activities on pages in the site.
    /// </summary>
    public class CommunityActivity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityActivity()
        {
            this.Body = null;
            this.Value = null;
        }

        /// <summary>
        /// Gets or sets the body of a page comment activity.
        /// </summary>
        public string Body;

        /// <summary>
        /// Gets or sets the value of a page rating activity.
        /// </summary>
        public int? Value;
    }
}