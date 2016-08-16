using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Itanc.AspNetIdentity.AdoNet;
using Microsoft.AspNet.Identity;

namespace Itanc.AspNetIdentity.Entities
{
    public class UsersLoginsTable
    {

        private readonly SqlServerDatabase _database;

        public UsersLoginsTable(SqlServerDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Deletes a login from a user in the UserLogins table
        /// </summary>
        /// <param name="user">User to have login deleted</param>
        /// <param name="login">Login to be deleted from user</param>
        /// <returns></returns>
        public int Delete(IdentityUser user, UserLoginInfo login)
        {
            const string sqlQuery = "DELETE FROM AspNetUserLogins WHERE UserId = @UserId AND LoginProvider=@LoginProvider AND ProviderKey=@ProviderKey ";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserId", user.Id, SqlDbType.NVarChar),
                new DbSqlParameter("@LoginProvider", login.LoginProvider, SqlDbType.NVarChar),
                new DbSqlParameter("@ProviderKey", login.ProviderKey , SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }

        /// <summary>
        /// Deletes all Logins from a user in the UserLogins table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            const string sqlQuery = "DELETE FROM AspNetUserLogins WHERE UserId = @UserId ";
            var parameter = new DbSqlParameterCollection{new DbSqlParameter("@UserId", userId , SqlDbType.NVarChar)};
            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }

        /// <summary>
        /// Inserts a new login in the UserLogins table
        /// </summary>
        /// <param name="user">User to have new login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, UserLoginInfo login)
        {
            const string sqlQuery = "INSERT INTO AspNetUserLogins ( LoginProvider, ProviderKey, UserId ) VALUES ( @LoginProvider, @ProviderKey, @UserId )";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@LoginProvider", login.LoginProvider , SqlDbType.NVarChar),
                new DbSqlParameter("@ProviderKey", login.ProviderKey , SqlDbType.NVarChar),
                new DbSqlParameter("@UserId", user.Id , SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery,parameter);
        }

        /// <summary>
        /// Return a userId given a user's login
        /// </summary>
        /// <param name="userLogin">The user's login info</param>
        /// <returns></returns>
        public string FindUserIdByLogin(UserLoginInfo userLogin)
        {
            const string sqlQuery = "SELECT UserId FROM AspNetUserLogins WHERE ( LoginProvider = @LoginProvider AND  ProviderKey = @ProviderKey )";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@LoginProvider", userLogin.LoginProvider, SqlDbType.NVarChar),
                new DbSqlParameter("@ProviderKey", userLogin.ProviderKey, SqlDbType.NVarChar)
            };

             return Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery,parameter));
        }

        /// <summary>
        /// Returns a list of user's logins
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<UserLoginInfo> FindByUserId(string userId)
        {
            const string sqlQuery = "SELECT * FROM AspNetUserLogins WHERE UserId = @UserId ";

            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserId", userId, SqlDbType.NVarChar)
            };

            var dt = _database.GetTable(sqlQuery,parameter);
            var userLoginInfoList = from DataRow dr in dt.Rows
                select new UserLoginInfo(Convert.ToString(dr["LoginProvider"]) , Convert.ToString(dr["ProviderKey"]));

            return userLoginInfoList.ToList();
        }


    }
}