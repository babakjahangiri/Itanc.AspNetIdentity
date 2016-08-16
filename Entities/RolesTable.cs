using Itanc.AspNetIdentity.AdoNet;
using System;
using System.Data;

namespace Itanc.AspNetIdentity.Entities
{
    public class RolesTable
    {
        private readonly SqlServerDatabase _database;

        public RolesTable(SqlServerDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Deletes a role from the Roles table
        /// </summary>
        public int Delete(string roleId)
        {
            const string sqlQuery = "DELETE FROM AspNetRoles WHERE Id = @Id";
            var parameter = new DbSqlParameterCollection {new DbSqlParameter("@Id", roleId, SqlDbType.NVarChar)};
            return _database.ExecuteNonQuery(sqlQuery, parameter);
              
        }

        /// <summary>
        /// Inserts a new Role in the Roles table
        /// </summary>
        public int Insert(IdentityRole role)
        {
            const string sqlQuery = "INSERT INTO AspNetRoles (Id,Name) VALUES (@Id, @Name)";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id",  role.Id, SqlDbType.NVarChar),
                new DbSqlParameter("@Name", role.Name, SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery, parameter);
        }

        /// <summary>
        /// Returns a role name given the roleId
        /// </summary>

        public string GetRoleName(string roleId)
        {
            const string sqlQuery = "SELECT NAME FROM AspNetRoles WHERE Id = @Id";
            var parameter = new DbSqlParameterCollection { new DbSqlParameter("@Id", roleId, SqlDbType.NVarChar) };
            return Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery, parameter));

        }

        /// <summary>
        /// Returns the role Id given a role name
        /// </summary>
        /// <param name="roleName">Role's name</param>
        /// <returns>Role's Id</returns>
        public string GetRoleId(string roleName)
        {
            const string sqlQuery = "SELECT Id FROM AspNetRoles WHERE Name = @Name";
            var parameter = new DbSqlParameterCollection { new DbSqlParameter("@Name", roleName, SqlDbType.NVarChar) };
            var result = Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery,parameter));
            return string.IsNullOrEmpty(result) ? null : result;
        }


        /// <summary>
        /// Gets the IdentityRole given the role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public IdentityRole GetRoleById(string roleId)
        {
            var roleName = GetRoleName(roleId);
            IdentityRole role = null;

            if (roleName != null)
            {
                role = new IdentityRole(roleName,roleId);
            }

            return role;
        }

        /// <summary>
        /// Gets the IdentityRole given the role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public IdentityRole GetRoleByName(string roleName)
        {
            var roleId = GetRoleId(roleName);
            IdentityRole role = null;

            if (roleId != null)
            {
                role = new IdentityRole(roleName, roleId);
            }

            return role;
        }


        public int Update(IdentityRole role)
        {
            const string sqlQuery = "UPDATE AspNetRoles SET Name = @Name WHERE Id = @Id";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id", role.Id, SqlDbType.NVarChar),
                new DbSqlParameter("@Name", role.Name, SqlDbType.NVarChar)
            };
 
            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }
    }
}