using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;
using System;
using System.Linq;
using EPiServer.SocialAlloy.Web.Social.Adapters.Groups;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// Defines the operations that can be issued against the EPiServer.Social.Groups.MemberService.
    /// </summary>
    public class SocialMemberRepository : ISocialMemberRepository
    {
        private readonly IMemberService memberService;
        private SocialMemberAdapter socialMemberAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialMemberRepository(IMemberService memberService)
        {
            this.memberService = memberService;
            this.socialMemberAdapter = new SocialMemberAdapter();
        }

        /// <summary>
        /// Adds a member to the EPiServer Social member repository.
        /// </summary>
        /// <param name="socialMember">The member to add.</param>
        /// <param name="memberExtension">The member extension data to add.</param>
        /// <returns>The added member.</returns>
        public SocialCompositeMember Add(SocialMember socialMember, MemberExtensionData memberExtension)
        {
            SocialCompositeMember addedSocialCompositeMember = null;

            try
            {
                var member = new Member(socialMember.UserReference, socialMember.GroupId);
                var addedCompositeMember = this.memberService.Add<MemberExtensionData>(member, memberExtension);
                var addedSocialMember = socialMemberAdapter.Adapt(addedCompositeMember.Data);
                addedSocialCompositeMember = socialMemberAdapter.Adapt(addedSocialMember, addedCompositeMember.Extension);

                if (addedSocialCompositeMember == null)
                    throw new SocialRepositoryException("The new member could not be added. Please try again");
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

            return addedSocialCompositeMember;
        }

        /// <summary>
        /// Retrieves a page of members from the EPiServer Social member repository.
        /// </summary>
        /// <param name="socialMemberFilter">The social filter used to properly construct the composite filter used to return members.</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        public IEnumerable<SocialCompositeMember> Get(SocialMemberFilter socialMemberFilter)
        {
            IEnumerable<SocialCompositeMember> returnedMembers = null;

            try
            {
                var pageInfo = new PageInfo { PageSize = socialMemberFilter.PageSize };
                var memberFilter = new MemberFilter { Group = socialMemberFilter.GroupId };
                var compositeFilter = new CompositeCriteria<MemberFilter, MemberExtensionData>() { Filter = memberFilter, PageInfo = pageInfo };
                var compositeMember = this.memberService.Get(compositeFilter).Results;
                returnedMembers = compositeMember.Select(x => socialMemberAdapter.Adapt(socialMemberAdapter.Adapt(x.Data), x.Extension));
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

            return returnedMembers;
        }

    }
}