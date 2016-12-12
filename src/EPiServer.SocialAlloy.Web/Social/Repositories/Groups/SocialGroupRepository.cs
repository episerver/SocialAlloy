using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public class SocialGroupRepository : ISocialGroupRepository
    {
        private readonly IGroupService groupService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialGroupRepository(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        /// <summary>
        /// Adds a group to the EPiServer Social group repository.
        /// </summary>
        /// <param name="group">The group to add.</param>
        /// <returns>The added group.</returns>
        public Group Add(Group group)
        {
            Group addedGroup = null;

            try
            {
                addedGroup = this.groupService.Add(group);
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

            return addedGroup;
        }

        /// <summary>
        /// Retrieves a group based on the name of the group provided.
        /// </summary>
        /// <param name="groupName">The name of the group that is to be retrieved from the underlying data store.</param>
        /// <returns>The desired group.</returns>
        public Group Get(string groupName)
        {
            Group group = null;

            try
            {
                var criteria = new Criteria<GroupFilter>
                {
                    Filter = new GroupFilter { Name = groupName },
                    PageInfo = new PageInfo {  PageSize = 1, CalculateTotalCount = true, PageOffset = 0}
                };
                group = this.groupService.Get(criteria).Results.FirstOrDefault();
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

            return group;
        }
    }
}