using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPiServer.SocialAlloy.Web.Social.Blocks
{
    /// <summary>
    /// The RatingBlock class defines the configuration used for rendering rating views.
    /// </summary>
    [ContentType(DisplayName = "Rating Block", 
                 GUID = "069e2c52-fd48-49c5-8993-7a0347ea1f78", 
                 Description = "Configures the frontend view properties of a rating block")]
    public class RatingBlock : BlockData
    {
        /// <summary>
        /// Configures the heading that should be used when displaying the block view in the frontend.
        /// </summary>
        [Display(
        GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        /// <summary>
        /// Configures whether the heading should be displayed in the block's frontend view.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual bool ShowHeading { get; set; }

        /// <summary>
        /// Configures whether an activity should be sent to the Episerver Social 
        /// Activity Streams system when a rating a submitted using the rating block.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual bool SendActivity { get; set; }

        /// <summary>
        /// Configures the list of possible rating values that can be submitted using this rating block.
        /// </summary>
        [Editable(false)]
        [ScaffoldColumn(false)]
        public virtual IList<RatingSetting> RatingSettings { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Ratings and Statistics";

            // By default do not display a heading on the rating block
            ShowHeading = false;

            // By default send a rating activity to the Episerver Social 
            // Activity Streams system when a rating a submitted.
            SendActivity = true;

            // For the sake of the simplicity of this sample we allow items 
            // to be rated on a scale of 1 through 5 by initializing this
            // non-editable property list.
            RatingSettings = new List<RatingSetting>();
            RatingSettings.Add(new RatingSetting { Value = 1 });
            RatingSettings.Add(new RatingSetting { Value = 2 });
            RatingSettings.Add(new RatingSetting { Value = 3 });
            RatingSettings.Add(new RatingSetting { Value = 4 });
            RatingSettings.Add(new RatingSetting { Value = 5 });
        }
    }
}