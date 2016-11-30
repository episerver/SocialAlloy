using EPiServer.SocialAlloy.Web.Social.Blocks.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiServer.SocialAlloy.Web.Social.Models.Groups
{
    /// <summary>
    /// The CommentsBlockViewModel class represents the model that will be used to
    /// feed data to the comments block frontend view.
    /// </summary>
    public class GroupCreationBlockModel : SocialBlockViewModel<GroupCreationBlock>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="block"></param>
        /// <param name="form"></param>
        public GroupCreationBlockModel(GroupCreationBlock block,
                                      GroupCreationFormModel form)
            : base(form.CurrentPageLink, form.CurrentBlockLink)
        {
            Heading = block.Heading;
            GroupName = form.Name;
            GroupDescription = form.Description;
        }

        /// <summary>
        /// The heading for the frontend comments block display.
        /// </summary>
        public string Heading { get; }

        /// <summary>
        /// A success message that should be flashed in the view.
        /// </summary>
        public string SubmitSuccessMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the view.
        /// </summary>
        public string SubmitErrorMessage { get; set; }

        /// <summary>
        /// A error message that should be flashed in the message display view.
        /// </summary>
        public string DisplayErrorMessage { get; set; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        public string GroupName { get; }

        /// <summary>
        /// Gets the group description.
        /// </summary>
        public string GroupDescription { get; }
    }
}

