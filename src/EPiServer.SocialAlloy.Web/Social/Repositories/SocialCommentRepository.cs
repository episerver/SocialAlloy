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
            catch (ArgumentNullException ex)
            {
                throw new SocialRepositoryException("ArgumentNullException: " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new SocialRepositoryException("ArgumentException: " + ex.Message, ex);
            }
            catch (InvalidCommentException ex)
            {
                throw new SocialRepositoryException("InvalidCommentException: " + ex.Message, ex);
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("SocialAuthenticationException: " + ex.Message, ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("MaximumDataSizeExceededException: " + ex.Message, ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("SocialCommunicationException: " + ex.Message, ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("SocialException: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new SocialRepositoryException("Exception: " + ex.Message, ex);
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
            catch (ArgumentNullException ex)
            {
                throw new SocialRepositoryException("ArgumentNullException: " + ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new SocialRepositoryException("ArgumentException: " + ex.Message, ex);
            }
            catch (InvalidCommentException ex)
            {
                throw new SocialRepositoryException("InvalidCommentException: " + ex.Message, ex);
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("SocialAuthenticationException: " + ex.Message, ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("MaximumDataSizeExceededException: " + ex.Message, ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("SocialCommunicationException: " + ex.Message, ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("SocialException: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new SocialRepositoryException("Exception: " + ex.Message, ex);
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