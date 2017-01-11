using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Social.Comments.Core;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.Social.Common;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// Defines the operations that can be issued against the EPiServer.Social.Groups.GroupService.
    /// </summary>
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
        public SocialGroup Add(SocialGroup socialGroup)
        {
            Group addedGroup = null;
            
            try
            {
                var group = new Group(socialGroup.Name, socialGroup.Description);
                addedGroup = this.groupService.Add(group);
                if (addedGroup == null)
                    throw new SocialRepositoryException("The new group could not be added. Please try again");
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

            return new SocialGroup(addedGroup.Id.Id, addedGroup.Name, addedGroup.Description);
        }

        /// <summary>
        /// Retrieves a group based on the name of the group provided.
        /// </summary>
        /// <param name="groupName">The name of the group that is to be retrieved from the underlying data store.</param>
        /// <returns>The requested group.</returns>
        public SocialGroup Get(string groupName)
        {
            SocialGroup socialGroup = null;

            try
            {
                var criteria = new Criteria<GroupFilter>
                {
                    Filter = new GroupFilter { Name = groupName },
                    PageInfo = new PageInfo {  PageSize = 1, PageOffset = 0}
                };
                var group = this.groupService.Get(criteria).Results.FirstOrDefault();
                if(group != null)
                {
                    socialGroup = new SocialGroup(group.Id.Id, group.Name, group.Description);
                }
                else
                {
                    throw new GroupDoesNotExistException("The group that has been specified for this block does not exist");
                }
                
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

            return socialGroup;
        }
    }
}