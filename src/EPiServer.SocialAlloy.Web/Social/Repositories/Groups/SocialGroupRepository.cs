using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.ExtensionData.Group;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;
using System.Linq;

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
            Composite<Group, GroupExtensionData> addedGroup = null;

            try
            {
                var group = new Group(socialGroup.Name, socialGroup.Description);
                var extension = new GroupExtensionData(socialGroup.PageLink);
                addedGroup = this.groupService.Add<GroupExtensionData>(group, extension);
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

            return new SocialGroup(addedGroup.Data.Id.Id, addedGroup.Data.Name, addedGroup.Data.Description, addedGroup.Extension.PageLink);
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

        /// <summary>
        /// Retrieves a group based on a list of group ids that are provided.
        /// </summary>
        /// <param name="groupIds">The groups ids that are to be retrieved from the underlying data store.</param>
        /// <returns>The requested groups.</returns>
        public List<SocialGroup> Get(List<string> groupIds)
        {
            List<SocialGroup> socialGroups = new List<SocialGroup>();
            try
            {
                var groupIdList = groupIds.Select(x => GroupId.Create(x)).ToList();
                var groupCount = groupIdList.Count();
                var criteria = new CompositeCriteria<GroupFilter, GroupExtensionData>
                {
                    Filter = new GroupFilter { GroupIds = groupIdList },
                    PageInfo = new PageInfo { PageSize= groupCount, CalculateTotalCount= true }
                };
                var returnedGroups = this.groupService.Get(criteria);
                if (returnedGroups.TotalCount > 0)
                {
                    socialGroups = returnedGroups.Results.Select(x => new SocialGroup(x.Data.Id.Id, x.Data.Name, x.Data.Description, x.Extension.PageLink)).ToList();
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

            return socialGroups;
        }

        /// <summary>
        /// Updates a group to the EPiServer Social group repository.
        /// </summary>
        /// <param name="group">The group to update.</param>
        /// <returns>The updated group.</returns>
        public SocialGroup Update(SocialGroup socialGroup)
        {
            Composite<Group, GroupExtensionData> updatedGroup = null;

            try
            {
                var group = new Group(GroupId.Create(socialGroup.Id), socialGroup.Name, socialGroup.Description);
                var extension = new GroupExtensionData(socialGroup.PageLink);
                updatedGroup = this.groupService.Update<GroupExtensionData>(group, extension);
                if (updatedGroup == null)
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

            return new SocialGroup(updatedGroup.Data.Id.Id, updatedGroup.Data.Name, updatedGroup.Data.Description, updatedGroup.Extension.PageLink);
        }
    }
}