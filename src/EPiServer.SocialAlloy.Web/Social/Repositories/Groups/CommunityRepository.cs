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
        private readonly GroupFilters groupFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityRepository(IGroupService groupService)
        {
            this.groupService = groupService;
            this.groupFilters = new GroupFilters();
        }

        /// <summary>
        /// Adds a community to the Episerver Social Framework.
        /// </summary>
        /// <param name="community">The community to add.</param>
        /// <returns>The added community.</returns>
        public Community Add(Community community)
        {
            try
            {
                var group = new Group(community.Name, community.Description);
                var extension = new GroupExtensionData(community.PageLink);
                var addedGroup = this.groupService.Add<GroupExtensionData>(group, extension);
                if (addedGroup == null)
                    throw new SocialRepositoryException("The new community could not be added. Please try again");

                return new Community(addedGroup.Data.Id.Id, addedGroup.Data.Name, addedGroup.Data.Description, addedGroup.Extension.PageLink);
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
        }

        /// <summary>
        /// Retrieves a community based on the name of the community provided.
        /// </summary>
        /// <param name="communityName">The name of the community that is to be retrieved from the underlying data store.</param>
        /// <returns>The requested community.</returns>
        public Community Get(string communityName)
        {
            try
            {
                var criteria = new Criteria
                {
                    Filter = this.groupFilters.Name.EqualTo(communityName),
                    PageInfo = new PageInfo { PageSize = 1, PageOffset = 0 }
                };

                Community community = null;
                var group = this.groupService.Get(criteria).Results.FirstOrDefault();
                if (group != null)
                {
                    community = new Community(group.Id.Id, group.Name, group.Description);
                }

                return community;
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
        }

        /// <summary>
        /// Retrieves a community based on a list of community ids that are provided.
        /// </summary>
        /// <param name="communityIds">The groups ids that are to be retrieved from the underlying data store.</param>
        /// <returns>The requested groups.</returns>
        public List<Community> Get(List<string> communityIds)
        {
            try
            {
                var groupIdList = communityIds.Select(x => GroupId.Create(x)).ToList();
                var groupCount = groupIdList.Count();

                var filters = new List<FilterExpression>();
                filters.Add(this.groupFilters.Id.Any(groupIdList));
                filters.Add(this.groupFilters.Extension.Type.Is<GroupExtensionData>());

                var criteria = new Criteria
                {
                    Filter = new AndExpression(filters),
                    PageInfo = new PageInfo { PageSize = groupCount },
                    OrderBy = new List<SortInfo> { new SortInfo(GroupSortFields.Name, true) }
                };

                var groups = this.groupService.Get<GroupExtensionData>(criteria);

                return groups.Results.Select(x => new Community(x.Data.Id.Id, x.Data.Name, x.Data.Description, x.Extension.PageLink)).ToList();
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
        }

        /// <summary>
        /// Updates a community to the Episerver Social Framework.
        /// </summary>
        /// <param name="community">The community to update.</param>
        /// <returns>The updated community.</returns>
        public Community Update(Community community)
        {
            try
            {
                var group = new Group(GroupId.Create(community.Id), community.Name, community.Description);
                var extension = new GroupExtensionData(community.PageLink);

                var updatedGroup = this.groupService.Update<GroupExtensionData>(group, extension);
                if (updatedGroup == null)
                    throw new SocialRepositoryException("The new community could not be added. Please try again");

                return new Community(updatedGroup.Data.Id.Id, updatedGroup.Data.Name, updatedGroup.Data.Description, updatedGroup.Extension.PageLink);
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
        }
    }
}