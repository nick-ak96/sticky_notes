using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Repositories
{
    public interface IOrganizationRepository
    {
        Task<long> CreateOrganizationAsync(Organization organization, CancellationToken cancellationToken);

        Task<int> UpdateOrganizationAsync(Organization organization, CancellationToken cancellationToken);

        Task<int> DeleteOrganizationAsync(long organizationId, CancellationToken cancellationToken);

        Task<Organization> GetOrganizationAsync(long organizationId, CancellationToken cancellationToken);

        Task<IEnumerable<Organization>> GetUserOrganizationsAsync(long userId, CancellationToken cancellationToken);

        Task<OrganizationAccessType?> GetUserAccessTypeAsync(long organizationId, long userId, CancellationToken cancellationToken);

        Task<int> AddUserToOrganizationAsync(long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken);

        Task<int> RemoveUserFromOrganizationAsync(long organizationId, long userId, CancellationToken cancellationToken);

        Task<int> UpdateUserOrganizationAccessAsync(long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken);
    }
}
