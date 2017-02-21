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
    /// Defines the implementation of persisting, and retrieving community data 
    /// </summary>
    public class CommunityRepository : ICommunityRepository
    {
        private readonly IGroupService groupService;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityRepository(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        /// <summary>
        /// Adds a community to the Episerver Social Framework.
        /// </summary>
        /// <param name="community">The community to add.</param>
        /// <returns>The added community.</returns>
        public Community Add(Community community)
        {
            Composite<Group, GroupExtensionData> addedGroup = null;

            try
            {
                var group = new Group(community.Name, community.Description);
                var extension = new GroupExtensionData(community.PageLink);
                addedGroup = this.groupService.Add<GroupExtensionData>(group, extension);
                if (addedGroup == null)
                    throw new SocialRepositoryException("The new community could not be added. Please try again");
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return new Community(addedGroup.Data.Id.Id, addedGroup.Data.Name, addedGroup.Data.Description, addedGroup.Extension.PageLink);
        }

        /// <summary>
        /// Retrieves a community based on the name of the community provided.
        /// </summary>
        /// <param name="communityName">The name of the community that is to be retrieved from the underlying data store.</param>
        /// <returns>The requested community.</returns>
        public Community Get(string communityName)
        {
            Community community = null;

            try
            {
                var criteria = new Criteria<GroupFilter>
                {
                    Filter = new GroupFilter { Name = communityName },
                    PageInfo = new PageInfo { PageSize = 1, PageOffset = 0 }
                };
                var group = this.groupService.Get(criteria).Results.FirstOrDefault();
                if (group != null)
                {
                    community = new Community(group.Id.Id, group.Name, group.Description);
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return community;
        }

        /// <summary>
        /// Retrieves a community based on a list of community ids that are provided.
        /// </summary>
        /// <param name="communityIds">The groups ids that are to be retrieved from the underlying data store.</param>
        /// <returns>The requested groups.</returns>
        public List<Community> Get(List<string> communityIds)
        {
            List<Community> socialGroups = new List<Community>();
            try
            {
                var groupIdList = communityIds.Select(x => GroupId.Create(x)).ToList();
                var groupCount = groupIdList.Count();
                var criteria = new CompositeCriteria<GroupFilter, GroupExtensionData>
                {
                    Filter = new GroupFilter { GroupIds = groupIdList },
                    PageInfo = new PageInfo { PageSize = groupCount },
                    OrderBy = new List<SortInfo> { new SortInfo(GroupSortFields.Name, true) }
                };
                var returnedGroups = this.groupService.Get(criteria);
                socialGroups = returnedGroups.Results.Select(x => new Community(x.Data.Id.Id, x.Data.Name, x.Data.Description, x.Extension.PageLink)).ToList();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }
            catch (GroupDoesNotExistException ex)
            {
                throw new SocialRepositoryException("Episerver Social could not find the community requested.", ex);
            }


            return socialGroups;
        }

        /// <summary>
        /// Updates a community to the Episerver Social Framework.
        /// </summary>
        /// <param name="community">The community to update.</param>
        /// <returns>The updated community.</returns>
        public Community Update(Community community)
        {
            Composite<Group, GroupExtensionData> updatedGroup = null;

            try
            {
                var group = new Group(GroupId.Create(community.Id), community.Name, community.Description);
                var extension = new GroupExtensionData(community.PageLink);
                updatedGroup = this.groupService.Update<GroupExtensionData>(group, extension);
                if (updatedGroup == null)
                    throw new SocialRepositoryException("The new community could not be added. Please try again");
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return new Community(updatedGroup.Data.Id.Id, updatedGroup.Data.Name, updatedGroup.Data.Description, updatedGroup.Extension.PageLink);
        }
    }
}