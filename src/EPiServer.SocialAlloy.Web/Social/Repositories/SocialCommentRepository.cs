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
            var error = "";

            try
            {
                addedComment = this.commentService.Add(newComment);
            }
            catch (ArgumentNullException ex)
            {
                error = "ArgumentNullException: " + ex.Message;
            }
            catch (ArgumentException ex)
            {
                error = "ArgumentException: " + ex.Message;
            }
            catch (InvalidCommentException ex)
            {
                error = "InvalidCommentException: " + ex.Message;
            }
            catch (SocialAuthenticationException ex)
            {
                error = "SocialAuthenticationException: " + ex.Message;
            }
            catch (MaximumDataSizeExceededException ex)
            {
                error = "MaximumDataSizeExceededException: " + ex.Message;
            }
            catch (SocialCommunicationException ex)
            {
                error = "SocialCommunicationException: " + ex.Message;
            }
            catch (SocialException ex)
            {
                error = "SocialException: " + ex.Message;
            }
            catch (Exception ex)
            {
                error = "Exception: " + ex.Message;
            }

            if (!String.IsNullOrWhiteSpace(error))
            {
                throw new SocialRepositoryException(error);
            }

            return addedComment;
        }

        public IEnumerable<SocialComment> Get(SocialCommentFilter filter)
        {
            var error = "";
            var comments = new List<Comment>();

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
                            Visibility = filter.Visible ? Visibility.Visible : Visibility.All
                        }
                    }
                ).Results.ToList();
            }
            catch (ArgumentNullException ex)
            {
                error = "ArgumentNullException: " + ex.Message;
            }
            catch (ArgumentException ex)
            {
                error = "ArgumentException: " + ex.Message;
            }
            catch (InvalidCommentException ex)
            {
                error = "InvalidCommentException: " + ex.Message;
            }
            catch (SocialAuthenticationException ex)
            {
                error = "SocialAuthenticationException: " + ex.Message;
            }
            catch (MaximumDataSizeExceededException ex)
            {
                error = "MaximumDataSizeExceededException: " + ex.Message;
            }
            catch (SocialCommunicationException ex)
            {
                error = "SocialCommunicationException: " + ex.Message;
            }
            catch (SocialException ex)
            {
                error = "SocialException: " + ex.Message;
            }
            catch (Exception ex)
            {
                error = "Exception: " + ex.Message;
            }

            if (!String.IsNullOrWhiteSpace(error))
            {
                throw new SocialRepositoryException(error);
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
                    Created = c.Created.ToString()
                }
            );
        }
    }
}