namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// A Community represents a group of site visitors who share common interest.
    /// </summary>
    public class Community
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The name of the community</param>
        /// <param name="description">A description for the community</param>
        public Community(string name, string description) : this("", name, description, "")
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The unique id of the community</param>
        /// <param name="name">The name of the community</param>
        /// <param name="description">A description for the community</param>
        public Community(string id, string name, string description): this(id, name, description, "")
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">The unique id of the community</param>
        /// <param name="name">The name of the community</param>
        /// <param name="description">A description for the community</param>
        public Community(string id, string name, string description, string pageLink)
        {
            Id = id;
            Name = name;
            Description = description;
            PageLink = pageLink;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PageLink { get; set; }
    }
}