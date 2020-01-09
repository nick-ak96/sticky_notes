using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Models.Exceptions;
using api.Repositories;

namespace api.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task AddUserToOrganizationAsync(long requester, long organizationId, long userId, OrganizationAccessType accessType,
                CancellationToken cancellationToken)
        {
            if (requester.Equals(userId))
                throw new ConflictException("Cannot add youself to an organization");
            var organization = await this.GetOrganizationAsync(organizationId, cancellationToken);
            if (!organization.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the organization {organizationId}");

            var result = await _organizationRepository.AddUserToOrganizationAsync(organizationId, userId, accessType, cancellationToken);
            if (result < 1)
                throw new Exception($"Error adding user {userId} to organization {organizationId}");
        }

        public async Task<Organization> CreateOrganizationAsync(long requester, OrganizationCreate organization, CancellationToken cancellationToken)
        {
            var newOrganization = new Organization(organization, requester);
            var organizationId = await _organizationRepository.CreateOrganizationAsync(newOrganization, cancellationToken);
            return await this.GetOrganizationAsync(organizationId, cancellationToken);
        }

        public async Task DeleteOrganizationAsync(long requester, long organizationId, CancellationToken cancellationToken)
        {
            var organization = await this.GetOrganizationAsync(organizationId, cancellationToken);
            if (!organization.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the organization {organizationId}");
            var result = await _organizationRepository.DeleteOrganizationAsync(organizationId, cancellationToken);
            if (result < 1)
                throw new Exception($"Error deleting the organization {organizationId}");
        }

        public async Task<Organization> GetOrganizationAsync(long organizationId, CancellationToken cancellationToken)
        {
            var organization = await _organizationRepository.GetOrganizationAsync(organizationId, cancellationToken);
            if (organization == null)
                throw new NotFoundException($"Organization {organizationId} does not exist");
			organization.Members = await _organizationRepository.GetOrganizationMembersAsync(organizationId, cancellationToken);
            return organization;
        }

        public async Task<IEnumerable<Organization>> GetUserOrganizationsAsync(long requester, CancellationToken cancellationToken)
        {
            var organizations = await _organizationRepository.GetUserOrganizationsAsync(requester, cancellationToken);
            foreach (var org in organizations)
            {
                org.Members = await _organizationRepository.GetOrganizationMembersAsync(org.Id, cancellationToken);
            }
            return organizations;
        }

        public async Task<OrganizationAccessType> GetUserAccessTypeAsync(long organizationId, long userId, CancellationToken cancellationToken)
        {
            var accessType = await _organizationRepository.GetUserAccessTypeAsync(organizationId, userId, cancellationToken);
            if (!accessType.HasValue)
                throw new NotFoundException($"User {userId} does not belong to organization {organizationId}");
            return accessType.Value;
        }

        public async Task RemoveUserFromOrganizationAsync(long requester, long organizationId, long userId, CancellationToken cancellationToken)
        {
            if (requester.Equals(userId))
                throw new ConflictException("Cannot remove yourself from organization");
            var organization = await this.GetOrganizationAsync(organizationId, cancellationToken);
            if (!organization.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of organization {organizationId}");
            await this.GetUserAccessTypeAsync(organizationId, userId, cancellationToken);
            var result = await _organizationRepository.RemoveUserFromOrganizationAsync(organizationId, userId, cancellationToken);
            if (result < 1)
                throw new Exception($"Error removing user {userId} from organization {organizationId}");
        }
        public async Task<Organization> UpdateOrganizationAsync(long requester, long organizationId, OrganizationUpdate organization, CancellationToken cancellationToken)
        {
            var oldOrganization = await this.GetOrganizationAsync(organizationId, cancellationToken);
            if (!oldOrganization.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of the organization {organizationId}");
            oldOrganization.Update(organization);
            var result = await _organizationRepository.UpdateOrganizationAsync(oldOrganization, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating organization {oldOrganization.Id}");
            return await this.GetOrganizationAsync(oldOrganization.Id, cancellationToken);
        }

        public async Task UpdateUserOrganizationAccessAsync(long requester, long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken)
        {
            var organization = await this.GetOrganizationAsync(organizationId, cancellationToken);
            if (!organization.CreatedBy.Equals(requester))
                throw new ForbiddenException($"User {requester} is not an owner of organization {organizationId}");
            await this.GetUserAccessTypeAsync(organizationId, userId, cancellationToken);
            var result = await _organizationRepository.UpdateUserOrganizationAccessAsync(organizationId, userId, accessType, cancellationToken);
            if (result < 1)
                throw new Exception($"Error updating organization access for user {userId} and organization {organizationId}");
        }
    }
}
