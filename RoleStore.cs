using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Itanc.AspNetIdentity.AdoNet;
using Itanc.AspNetIdentity.Entities;
using Microsoft.AspNet.Identity;

namespace Itanc.AspNetIdentity
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces.
    /// </summary>
    public class RoleStore<TRole> : IRoleStore<TRole> where TRole : IdentityRole
    {
        private readonly RolesTable _rolesTable;

        public SqlServerDatabase Database { get; private set; }

        /// <summary>
        /// Constructor that takes a MySQLDatabase as argument 
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(SqlServerDatabase database)
        {
            Database = database;
            _rolesTable = new RolesTable(database);
        }


        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            _rolesTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            _rolesTable.Delete(role.Id);

            return Task.FromResult<object>(null);
        }

    
        public Task<TRole> FindByIdAsync(string roleId)
        {
            var result = _rolesTable.GetRoleById(roleId) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = _rolesTable.GetRoleByName(roleName) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            _rolesTable.Update(role);

            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            if (Database == null) return;
            Database.Dispose();
            Database = null;
        }

       
        
    }
}
