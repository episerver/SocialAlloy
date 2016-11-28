namespace EPiServer.SocialAlloy.Web.Social.Models
{
    /// <summary>
    /// The SocialComment class describes a comment model used by the SocialAlloy site.
    /// </summary>
    public class SocialComment
    {
        public string Author { get; set; }
        public string Body { get; set; }
        public string Target { get; set; }
        public string Created { get; set; }
    }
}