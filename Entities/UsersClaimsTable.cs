using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using Itanc.AspNetIdentity.AdoNet;
 

namespace Itanc.AspNetIdentity.Entities
{

    public class UsersClaimsTable
    {

        private readonly SqlServerDatabase _database;

        public UsersClaimsTable(SqlServerDatabase database)
        {
            _database = database;
        }


        /// <summary>
        /// Returns a ClaimsIdentity instance given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public ClaimsIdentity FindByUserId(string userId)
        {
            var claims = new ClaimsIdentity();

            const string sqlQuery = "SELECT * FROM AspNetUserClaims WHERE UserId = @UserId ";
            var parameter = new DbSqlParameterCollection { new DbSqlParameter("@UserId", userId, SqlDbType.NVarChar) };
            var dt = _database.GetTable(sqlQuery,parameter);

            foreach (var claim in from DataRow dr in dt.Rows select new Claim(Convert.ToString(dr["ClaimType"]), Convert.ToString(dr["ClaimValue"])))
            {
                claims.AddClaim(claim);
            }

            return claims;
        }

        /// <summary>
        /// Deletes all claims from a user given a userId
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            const string sqlQuery = "DELETE FROM AspNetUserClaims WHERE UserId = @UserId ";
            var parameter = new DbSqlParameterCollection { new DbSqlParameter("@UserId", userId, SqlDbType.NVarChar) };
            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }


        /// <summary>
        /// Inserts a new claim in UserClaims table
        /// </summary>
        /// <param name="userClaim">User's claim to be added</param>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        public int Insert(Claim userClaim, string userId)
        {
            const string sqlQuery  = "INSERT INTO AspNetUserClaims (ClaimValue, ClaimType, UserId) VALUES (@ClaimValue, @ClaimType, @UserId)";
            var parameter = new DbSqlParameterCollection
            {   new DbSqlParameter("@UserId", userId, SqlDbType.NVarChar),
                new DbSqlParameter("@ClaimType", userId, SqlDbType.NVarChar),
                new DbSqlParameter("@ClaimValue", userId, SqlDbType.NVarChar)
            };
            
            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }

        /// <summary>
        /// Deletes a claim from a user 
        /// </summary>
        /// <param name="user">The user to have a claim deleted</param>
        /// <param name="claim">A claim to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, Claim claim)
        {
            const string sqlQuery = "DELETE FROM AspNetUserClaims WHERE (UserId = @UserId) AND (ClaimValue = @ClaimValue) AND (ClaimType = @ClaimType)";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserId", user.Id , SqlDbType.NVarChar),
                new DbSqlParameter("@ClaimValue", claim.Value , SqlDbType.NVarChar),
                new DbSqlParameter("@ClaimType", claim.Type , SqlDbType.NVarChar)
            };
          
            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }



    }
}