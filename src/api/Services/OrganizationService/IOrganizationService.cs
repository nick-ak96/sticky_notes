using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IOrganizationService
    {
        Task<Organization> CreateOrganizationAsync(long requester, OrganizationCreate organization, CancellationToken cancellationToken);

        Task<Organization> UpdateOrganizationAsync(long requester, long organizationId, OrganizationUpdate organization, CancellationToken cancellationToken);

        Task DeleteOrganizationAsync(long requester, long organizationId, CancellationToken cancellationToken);

        Task<Organization> GetOrganizationAsync(long organizationId, CancellationToken cancellationToken);

        Task<IEnumerable<Organization>> GetUserOrganizationsAsync(long userId, CancellationToken cancellationToken);

        Task<OrganizationAccessType> GetUserAccessTypeAsync(long organizationId, long userId, CancellationToken cancellationToken);

        Task AddUserToOrganizationAsync(long requester, long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken);

        Task RemoveUserFromOrganizationAsync(long requester, long organizationId, long userId, CancellationToken cancellationToken);

        Task UpdateUserOrganizationAccessAsync(long requester, long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken);
    }
}
