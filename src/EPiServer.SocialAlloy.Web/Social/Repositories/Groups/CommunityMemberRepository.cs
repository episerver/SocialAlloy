using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.ExtensionData.Membership;
using EPiServer.SocialAlloy.Web.Social.Adapters.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// CommunityMemberRepository persists and retrieves community member data to and from the Episerver Social Framework
    /// </summary>
    public class CommunityMemberRepository : ICommunityMemberRepository
    {
        private readonly IMemberService memberService;
        private readonly CommunityMemberAdapter communityMemberAdapter;
        private readonly MemberFilters memberFilters;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommunityMemberRepository(IMemberService memberService)
        {
            this.memberService = memberService;
            this.communityMemberAdapter = new CommunityMemberAdapter();
            this.memberFilters = new MemberFilters();
        }

        /// <summary>
        /// Adds a member to the Episerver Social Framework.
        /// </summary>
        /// <param name="communityMember">The member to add.</param>
        /// <param name="memberExtension">The member extension data to add.</param>
        /// <returns>The added member.</returns>
        public CommunityMember Add(CommunityMember communityMember)
        {
            try
            {
                var userReference = Reference.Create(communityMember.User);
                var groupId = GroupId.Create(communityMember.GroupId);
                var member = new Member(userReference, groupId);
                var extensionData = new MemberExtensionData(communityMember.Email, communityMember.Company);
                var addedCompositeMember = this.memberService.Add<MemberExtensionData>(member, extensionData);
                var addedSocialMember = communityMemberAdapter.Adapt(addedCompositeMember.Data, addedCompositeMember.Extension);
                if (addedSocialMember == null)
                    throw new SocialRepositoryException("The new member could not be added. Please try again");
                return addedSocialMember;
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
        /// Retrieves a page of community members from the Episerver Social Framework.
        /// </summary>
        /// <param name="communityMemberFilter">The filter by which to retrieve members by</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        public IEnumerable<CommunityMember> Get(CommunityMemberFilter communityMemberFilter)
        {
            try
            {
                var filter = BuildCriteria(communityMemberFilter);

                var members = this.memberService.Get<MemberExtensionData>(filter).Results;
                return members.Select(x => communityMemberAdapter.Adapt(x.Data, x.Extension));
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
        /// Build the appropriate criteria based the provided CommunityMemberFilter.
        /// The member filter will either contain a group id or a logged in user id. 
        /// If neither is provided an exception is thrown.
        /// </summary>
        /// <param name="communityMemberFilter">The provided member filter</param>
        /// <returns>A composite criteria of type MemberFilter and MemberExtensionData</returns>
        private Criteria BuildCriteria(CommunityMemberFilter communityMemberFilter)
        {
            var filters = new List<FilterExpression>();

            if (!string.IsNullOrEmpty(communityMemberFilter.CommunityId))
            {
                filters.Add(this.memberFilters.Group.EqualTo(GroupId.Create(communityMemberFilter.CommunityId)));
            }
            else if (!string.IsNullOrEmpty(communityMemberFilter.UserId))
            {
                filters.Add(this.memberFilters.User.EqualTo(Reference.Create(communityMemberFilter.UserId)));
            }
            else
            {
                throw new SocialException("This implementation of a CommunityMemberFilter should only contain either a CommunityId or a UserReference.");
            }

            filters.Add(this.memberFilters.Extension.Type.Is<MemberExtensionData>());

            return new Criteria
            {
                Filter = new AndExpression(filters),
                PageInfo = new PageInfo { PageSize = communityMemberFilter.PageSize },
                OrderBy = new List<SortInfo> { new SortInfo(MemberSortFields.Id, false) }
            };
        }
    }
}