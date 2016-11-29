using EPiServer.ServiceLocation;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{

    [ServiceConfiguration(ServiceType = typeof(ISocialCommentRepository))]
    public class SocialCommentRepository : ISocialCommentRepository
    {
        private readonly ICommentService commentService;

        public SocialCommentRepository()
        {
            this.commentService = ServiceLocator.Current.GetInstance<ICommentService>();
        }

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

        public IEnumerable<SocialComment> Get(SocialCommentFilter filter)
        {
            var comments = new List<Comment>();
            var visibility = GetVisibilityFilter(filter);

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

        private Comment AdaptComment(SocialComment comment)
        {
            return new Comment(Reference.Create(comment.Target), Reference.Create(comment.Author), comment.Body, true);
        }

        private IEnumerable<SocialComment> AdaptSocialComment(List<Comment> comments)
        {
            return comments.Select(c =>
                new SocialComment
                {
                    Author = c.Author.ToString(),
                    Body = c.Body,
                    Target = c.Parent.ToString(),
                    Created = c.Created
                }
            );
        }

        private Visibility GetVisibilityFilter(SocialCommentFilter filter)
        {
            Visibility visibility = Visibility.All;

            switch(filter.Visibility)
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