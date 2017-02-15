using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Social.Comments;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using System;
using System.Collections.Generic;

namespace epiAlloySite
{
    /// <summary>
    /// This class implements extension methods on an instance of Episerver IContent to support
    /// adding and retrieving Episerver Social comments posted on that instance.
    /// </summary>
    public static class CommentExtensions
    {
        private static ICommentService service;
        // Format specifiers for building target content and author strings used to form 
        // references in Episerver Social. 
        private const string userReferenceFormat = "user://{0}";
        private const string resourceReferenceFormat = "resource://{0}";
        
        /// <summary>
        /// Constructor
        /// </summary>
        static CommentExtensions()
        {
            service = ServiceLocator.Current.GetInstance<ICommentService>();
        }

        /// <summary> 
        /// An extension method of the EPiServer.Core.IContent type to retrieve an page 
        /// of Comments posted for an IContent instance.  
        /// </summary> 
        /// <param name="content">A component stored in the content repository. Example: a product.</param> 
        /// <param name="visible">The visibility filter by which to retrieve comments.</param>
        /// <param name="offset">The zero-based start index of the comment to retrieve.</param> 
        /// <param name="size">The maximum number of comments to retreive.</param> 
        /// <returns> 
        /// Returns a page of comments.
        /// </returns> 
        public static ResultPage<Comment> GetComments(this IContent content, Visibility visibile, int offset, int size)
        {
            var targetReference = Reference.Create(
                  String.Format(resourceReferenceFormat, content.ContentGuid.ToString()));

            var criteria = new Criteria<CommentFilter>
            {
                Filter = new CommentFilter
                {
                    Parent = targetReference,
                    Visibility = visibile
                },
                PageInfo = new PageInfo
                {
                    PageOffset = offset,
                    PageSize = size,
                    CalculateTotalCount = false
                },
                OrderBy = new List<SortInfo>
                {
                    new SortInfo(CommentSortFields.Created, false)
                }
            };


            return service.Get(criteria);
        }

        /// <summary>
        /// An extension method of the EPiServer.Core.IContent type to post a comment.
        /// </summary>
        /// <param name="content">An IContent instance stored in the content repository. Example: a product.</param>
        /// <param name="authorId">The unique identifier of the author who posted a comment.</param>
        /// <param name="comment">The body of the comment.</param>
        /// <param name="isVisible">Indicates whether or not this comment is visible.</param>
        /// <returns>the newly saved Comment instance.</returns>
        public static Comment PublishComment(this IContent content, string authorId, string body, bool isVisible)
        {
            var authorReference = String.IsNullOrWhiteSpace(authorId) ?
                                  Reference.Empty :
                                  Reference.Create(String.Format(userReferenceFormat, authorId));
            var targetReference = Reference.Create(String.Format(resourceReferenceFormat, content.ContentGuid.ToString()));

            var newComment = new Comment(targetReference, authorReference, body, isVisible);

            return service.Add(newComment);
        }
    }
}