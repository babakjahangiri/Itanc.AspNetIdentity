using System.Collections.Generic;
using System.Data;
using System.Linq;
using Itanc.AspNetIdentity.AdoNet;

namespace Itanc.AspNetIdentity.Entities
{
    public class UsersRolesTable
    {
        private readonly SqlServerDatabase _database;

        public UsersRolesTable(SqlServerDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        public List<string> FindByUserId(string userId)
        {
            var roles = new List<string>();
            const string sqlQuery = "SELECT AspNetRoles.Name FROM AspNetUserRoles, AspNetRoles WHERE AspNetUserRoles.UserId = @UserId " +
                                    " AND AspNetUserRoles.RoleId = AspNetRoles.Id";

            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserId", userId, SqlDbType.NVarChar)
            };
         
            var dt = _database.GetTable(sqlQuery,parameter);
            
            return dt.AsEnumerable().ToList().Select(item => item[0].ToString()).ToList();

        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        public int Delete(string userId)
        {
            const string sqlQuery = "DELETE FROM AspNetUserRoles WHERE UserId = @UserId";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id", userId, SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery,parameter);
         }

        /// <summary>
        /// Delete a role from a user in the UserRoles table
        /// </summary>
        public int Delete(string userId ,string roleId)
        {
            const string sqlQuery = "DELETE FROM AspNetUserRoles WHERE (UserId = @UserId) AND (RoleId = @RoleId)";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserId", userId, SqlDbType.NVarChar),
                new DbSqlParameter("@RoleId", roleId, SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            var queryMaker = new SqlQueryMaker { TableName = "AspNetUserRoles" };
            queryMaker.ValueParameters.Add(new ColumnItem("UserId", "@UserId"));
            queryMaker.ValueParameters.Add(new ColumnItem("RoleId", "@RoleId"));
            var sqlQuery = queryMaker.Insert();

            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserId", user.Id, SqlDbType.NVarChar),
                new DbSqlParameter("@RoleId", roleId, SqlDbType.NVarChar)
            };
 
           return _database.ExecuteNonQuery(sqlQuery,parameter);
        }
    }
}