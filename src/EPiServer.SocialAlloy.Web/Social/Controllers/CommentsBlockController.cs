using EPiServer.SocialAlloy.Web.Social.Comments.Blocks;
using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace EPiServer.SocialAlloy.Web.Social.Comments.Controllers
{
    /// <summary>
    /// The CommentsBlockController handles the rendering of comment block views.
    /// </summary>
    public class CommentsBlockController : BlockController<CommentsBlock>
    {
        public override ActionResult Index(CommentsBlock currentBlock)
        {
            return PartialView("~/Views/Social/CommentsBlock/CommentsDisplayView.cshtml", currentBlock);
        }
    }
}
