using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;
using System.Linq;
using static EPiServer.SocialAlloy.Web.Social.Models.SocialCommentFilter;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// The SocialCommentRepository class defines the operations that can be issued
    /// against the EPiServer Social comment repository.
    /// </summary>
    public class SocialCommentRepository : ISocialCommentRepository
    {
        private readonly IUserRepository userRepository;
        private readonly ICommentService commentService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialCommentRepository(IUserRepository userRepository, ICommentService commentService)
        {
            this.userRepository = userRepository;
            this.commentService = commentService;
        }

        /// <summary>
        /// Adds a comment to the EPiServer Social comment repository.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>The added comment.</returns>
        public Comment Add(SocialComment comment)
        {
            var newComment = AdaptComment(comment);
            Comment addedComment = null;

            try
            {
                addedComment = this.commentService.Add(newComment);
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return addedComment;
        }

        /// <summary>
        /// Gets comments from the EPiServer Social comment repository based on a filter.
        /// </summary>
        /// <param name="filter">The application comment filtering specification.</param>
        /// <returns>A list of comments.</returns>
        public IEnumerable<SocialComment> Get(SocialCommentFilter filter)
        {
            var comments = new List<Comment>();
            var visibility = AdaptVisibilityFilter(filter.Visibility);

            try
            {
                comments = this.commentService.Get(
                    new Criteria<CommentFilter>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = filter.PageSize
                        },
                        Filter = new CommentFilter
                        {
                            Visibility = visibility
                        }
                        ,
                        OrderBy = { new SortInfo(CommentSortFields.Created, false) }
                    }
                ).Results.ToList();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return AdaptSocialComment(comments);
        }

        /// <summary>
        /// Adapt the application SocialComment to the EPiServer Social Comment 
        /// </summary>
        /// <param name="comment">The application's SocialComment.</param>
        /// <returns>The EPiServer Social Comment.</returns>
        private Comment AdaptComment(SocialComment comment)
        {
            return new Comment(Reference.Create(comment.Target), Reference.Create(comment.Author), comment.Body, true);
        }

        /// <summary>
        /// Adapt a list of EPiServer Social Comment to application's SocialComment.
        /// </summary>
        /// <param name="comments">The list of EPiServer Social Comment.</param>
        /// <returns>The list of application SocialComment.</returns>
        private IEnumerable<SocialComment> AdaptSocialComment(List<Comment> comments)
        {
            return comments.Select(c =>
                new SocialComment
                {
                    Author = this.userRepository.GetUserName(c.Author.Id),
                    Body = c.Body,
                    Target = c.Parent.ToString(),
                    Created = c.Created
                }
            );
        }

        /// <summary>
        /// Adapt the application's VisibilityFilter filter to the EPiServer Social Visibility filter.
        /// </summary>
        /// <param name="filter">The application's VisibilityFilter</param>
        /// <returns>EPiServer Social Visibility filter.</returns>
        private Visibility AdaptVisibilityFilter(VisibilityFilter filter)
        {
            Visibility visibility = Visibility.All;

            switch (filter)
            {
                case SocialCommentFilter.VisibilityFilter.Visible:
                    visibility = Visibility.Visible;
                    break;

                case SocialCommentFilter.VisibilityFilter.NotVisible:
                    visibility = Visibility.NotVisible;
                    break;

                default:
                    break;
            }

            return visibility;
        }
    }
}