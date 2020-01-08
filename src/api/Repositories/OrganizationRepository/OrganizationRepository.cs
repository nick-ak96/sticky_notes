using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using api.Models;
using Dapper;

namespace api.Repositories
{
    public class OrganizationRepository : BaseRepository, IOrganizationRepository
    {
        public OrganizationRepository(IDBProvider dbProvider) : base(dbProvider)
        {
        }

        public Task<int> AddUserToOrganizationAsync(long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					insert into user_organization (
							organization_id,
							user_id,
							access_type)
					values (
							@organizationId,
							@userId,
							@accessType)
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql,
                    new
                    {
                        organizationId,
                        userId,
                        accessType
                    }, cancellationToken: cancellationToken));
            }
        }

        public Task<long> CreateOrganizationAsync(Organization organization, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                string sql =
                    @"
                    insert into organization (
							name,
							profile_picture,
							created_by,
							insert_date,
							last_modified)
                    values (
							@Name,
							@ProfilePicture,
							@CreatedBy,
							@InsertDate,
							@LastModified);
					select last_insert_rowid();
                    ";
                con.Open();
                return con.QuerySingleAsync<long>(
                    new CommandDefinition(sql,
                        new
                        {
                            organization.Name,
                            organization.ProfilePicture,
                            organization.CreatedBy,
                            organization.InsertDate,
                            organization.LastModified
                        }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> DeleteOrganizationAsync(long organizationId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = "delete from organization where id = @organizationId";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { organizationId }, cancellationToken: cancellationToken));
            }
        }

        public Task<Organization> GetOrganizationAsync(long organizationId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					select
						id as Id,
						name as Name,
						profile_picture as ProfilePicture,
						created_by as CreatedBy,
						insert_date as InsertDate,
						last_modified as LastModified
					from
						organization
					where
						id = @organizationId
					";
                con.Open();
                return con.QueryFirstOrDefaultAsync<Organization>(new CommandDefinition(sql, new { organizationId }, cancellationToken: cancellationToken));
            }
        }

        public Task<OrganizationAccessType?> GetUserAccessTypeAsync(long organizationId, long userId, CancellationToken cancellationToken)
        {
			using (var con = CreateConnection())
			{
				var sql = @"
					select
						access_type
					from
						user_organization
					where
						organization_id = @organizationId and user_id = @userId
					";
				con.Open();
				return con.QueryFirstOrDefaultAsync<OrganizationAccessType?>(new CommandDefinition(sql, new { organizationId, userId },
					cancellationToken: cancellationToken));
			}
        }

        public Task<IEnumerable<Organization>> GetUserOrganizationsAsync(long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					select
						id as Id,
						name as Name,
						profile_picture as ProfilePicture,
						created_by as CreatedBy,
						insert_date as InsertDate,
						last_modified as LastModified
					from
						organization
					where
						created_by = @userId

					union

					select
						o.id as Id,
						o.name as Name,
						o.profile_picture as ProfilePicture,
						o.created_by as CreatedBy,
						o.insert_date as InsertDate,
						o.last_modified as LastModified
					from
						organization o
					inner join
						user_organization uo on o.id = uo.organization_id
					inner join
						user u on uo.user_id = u.id and u.id = @userId
					";
                con.Open();
                return con.QueryAsync<Organization>(new CommandDefinition(sql, new { userId }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> RemoveUserFromOrganizationAsync(long organizationId, long userId, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					delete from user_organization where organization_id = @organizationId and user_id = @userId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { organizationId, userId }, cancellationToken: cancellationToken));
            }
        }
        public Task<int> UpdateOrganizationAsync(Organization organization, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					update
						organization
					set
						name = @Name,
						profile_picture = @ProfilePicture,
						last_modified = @LastModified
					where
						id = @Id
					";
                con.Open();
                return con.ExecuteAsync(
                    new CommandDefinition(sql,
                        new
                        {
                            organization.Id,
                            organization.Name,
                            organization.ProfilePicture,
                            organization.LastModified
                        }, cancellationToken: cancellationToken));
            }
        }

        public Task<int> UpdateUserOrganizationAccessAsync(long organizationId, long userId, OrganizationAccessType accessType, CancellationToken cancellationToken)
        {
            using (var con = CreateConnection())
            {
                var sql = @"
					update
						user_organization
					set
						access_type = @accessType
					where
						organization_id = @organizationId
						and user_id = @userId
					";
                con.Open();
                return con.ExecuteAsync(new CommandDefinition(sql, new { organizationId, userId, accessType }));
            }
        }
    }
}
