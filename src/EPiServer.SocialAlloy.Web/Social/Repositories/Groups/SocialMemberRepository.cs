using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using System.Collections.Generic;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    public class SocialMemberRepository : ISocialMemberRepository
    {
        private readonly IMemberService memberService;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialMemberRepository(IMemberService memberService)
        {
            this.memberService = memberService;
        }

        /// <summary>
        /// Adds a member to the EPiServer Social member repository.
        /// </summary>
        /// <param name="member">The member to add.</param>
        /// <returns>The added member.</returns>
        public Composite<Member, MemberExtensionData> Add(Member member, MemberExtensionData memberExtension)
        {
            Composite<Member, MemberExtensionData> addedMember = null;

            try
            {
                addedMember = this.memberService.Add<MemberExtensionData>(member, memberExtension);
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

            return addedMember;
        }

        /// <summary>
        /// Retrieves a page members from the EPiServer Social member repository.
        /// </summary>
        /// <param name="groupId">The groupId to search by.</param>
        /// <returns>The list of members of that group member.</returns>
        public IEnumerable<Composite<Member, MemberExtensionData>> Get(GroupId groupId)
        {
            IEnumerable<Composite<Member, MemberExtensionData>> membersInGroup = null;

            try
            {
                var memberFilter = new CompositeCriteria<MemberFilter, MemberExtensionData>() { Filter = new MemberFilter { Group = groupId } };
                membersInGroup = this.memberService.Get(memberFilter).Results;
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

            return membersInGroup;
        }

    }
}