using System;
using System.Data;
using System.Data.Common;
using Itanc.AspNetIdentity.AdoNet;

namespace Itanc.AspNetIdentity.Entities
{
    public class UsersTable<TUser>
        where TUser: IdentityUser
    {
        private readonly SqlServerDatabase _database;

        public UsersTable(SqlServerDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns the user's name given a user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(string userId)
        {
            const string sqlQuery = "SELECT UserName FROM AspNetUsers WHERE Id=@Id";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id",userId,SqlDbType.NVarChar)
            };
         
            return Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery,parameter));
        }


        /// <summary>
        /// Returns a User ID given a user name
        /// </summary>
        /// <param name="userName">The user's name</param>
        /// <returns></returns>
        public string GetUserId(string userName)
        {
            const string sqlQuery = "SELECT Id FROM AspNetUsers WHERE UserName = @UserName";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserName", userName, SqlDbType.NVarChar)
            };
     
            return Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery, parameter));

        }

        /// <summary>
        /// Returns an TUser given the user's id
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public TUser GetUserById(string userId)
        {
            TUser user = null;

            const string sqlQuery = "SELECT * FROM AspNetUsers WHERE Id = @Id";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id", userId, SqlDbType.NVarChar)
            };

            var dt = _database.GetTable(sqlQuery, parameter);

            if (dt.Rows.Count != 1)
                return null;

            var row = dt.Rows[0];
            user = (TUser)Activator.CreateInstance(typeof (TUser));
            
            user.Id = Convert.ToString(row["Id"]);
            user.UserName = Convert.ToString(row["UserName"]);
            user.PasswordHash = string.IsNullOrEmpty(Convert.ToString(row["PasswordHash"])) ? null : Convert.ToString(row["PasswordHash"]);
            user.SecurityStamp = string.IsNullOrEmpty(Convert.ToString(row["SecurityStamp"])) ? null : Convert.ToString(row["SecurityStamp"]);
            user.Email = string.IsNullOrEmpty(Convert.ToString(row["Email"])) ? null : Convert.ToString(row["Email"]);
            user.EmailConfirmed = Convert.ToBoolean(row["EmailConfirmed"]);
            user.PhoneNumber = string.IsNullOrEmpty(Convert.ToString(row["PhoneNumber"])) ? null : Convert.ToString(row["PhoneNumber"]);
            user.PhoneNumberConfirmed = Convert.ToBoolean(row["PhoneNumberConfirmed"]);
            user.LockoutEnabled = Convert.ToBoolean(row["LockoutEnabled"]);
            user.TwoFactorEnabled = Convert.ToBoolean(row["TwoFactorEnabled"]);
            user.LockoutEndDateUtc = string.IsNullOrEmpty(Convert.ToString(row["LockoutEndDateUtc"])) ? DateTime.Now : (Convert.ToDateTime(row["LockoutEndDateUtc"]));
            user.AccessFailedCount = string.IsNullOrEmpty(Convert.ToString(row["AccessFailedCount"])) ? 0 : Convert.ToInt32(row["AccessFailedCount"]);

            return user;
        }

        public TUser GetUserByName(string userName)
        {
            TUser user = null;

            const string sqlQuery = "SELECT * FROM AspNetUsers WHERE UserName = @UserName";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@UserName", userName, SqlDbType.NVarChar)
            };

            var dt = _database.GetTable(sqlQuery, parameter);

            if (dt.Rows.Count != 1)
                return null;

            var row = dt.Rows[0];
            user = (TUser)Activator.CreateInstance(typeof(TUser));

            user.Id = Convert.ToString(row["Id"]);
            user.UserName = Convert.ToString(row["UserName"]);
            user.PasswordHash = string.IsNullOrEmpty(Convert.ToString(row["PasswordHash"])) ? null : Convert.ToString(row["PasswordHash"]);
            user.SecurityStamp = string.IsNullOrEmpty(Convert.ToString(row["SecurityStamp"])) ? null : Convert.ToString(row["SecurityStamp"]);
            user.Email = string.IsNullOrEmpty(Convert.ToString(row["Email"])) ? null : Convert.ToString(row["Email"]);
            user.EmailConfirmed = Convert.ToBoolean(row["EmailConfirmed"]);
            user.PhoneNumber = string.IsNullOrEmpty(Convert.ToString(row["PhoneNumber"])) ? null : Convert.ToString(row["PhoneNumber"]);
            user.PhoneNumberConfirmed = Convert.ToBoolean(row["PhoneNumberConfirmed"]);
            user.LockoutEnabled = Convert.ToBoolean(row["LockoutEnabled"]);
            user.TwoFactorEnabled = Convert.ToBoolean(row["TwoFactorEnabled"]);
            user.LockoutEndDateUtc = string.IsNullOrEmpty(Convert.ToString(row["LockoutEndDateUtc"])) ? DateTime.Now : (Convert.ToDateTime(row["LockoutEndDateUtc"]));
            user.AccessFailedCount = string.IsNullOrEmpty(Convert.ToString(row["AccessFailedCount"])) ? 0 : Convert.ToInt32(row["AccessFailedCount"]);

            return user;
        }



        public TUser GetUserByEmail(string email)
        {
            TUser user = null;

            const string sqlQuery = "SELECT * FROM AspNetUsers WHERE Email = @Email";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Email", email, SqlDbType.NVarChar)
            };

            var dt = _database.GetTable(sqlQuery, parameter);

            if (dt.Rows.Count != 1)
                return null;

            var row = dt.Rows[0];

            user = (TUser)Activator.CreateInstance(typeof(TUser));

            user.Id = Convert.ToString(row["Id"]);
            user.UserName = Convert.ToString(row["UserName"]);
            user.PasswordHash = string.IsNullOrEmpty(Convert.ToString(row["PasswordHash"])) ? null : Convert.ToString(row["PasswordHash"]);
            user.SecurityStamp = string.IsNullOrEmpty(Convert.ToString(row["SecurityStamp"])) ? null : Convert.ToString(row["SecurityStamp"]);
            user.Email = string.IsNullOrEmpty(Convert.ToString(row["Email"])) ? null : Convert.ToString(row["Email"]);
            user.EmailConfirmed = Convert.ToBoolean(row["EmailConfirmed"]);
            user.PhoneNumber = string.IsNullOrEmpty(Convert.ToString(row["PhoneNumber"])) ? null : Convert.ToString(row["PhoneNumber"]);
            user.PhoneNumberConfirmed = Convert.ToBoolean(row["PhoneNumberConfirmed"]);
            user.LockoutEnabled = Convert.ToBoolean(row["LockoutEnabled"]);
            user.TwoFactorEnabled = Convert.ToBoolean(row["TwoFactorEnabled"]);
            user.LockoutEndDateUtc = string.IsNullOrEmpty(Convert.ToString(row["LockoutEndDateUtc"])) ? DateTime.Now : (Convert.ToDateTime(row["LockoutEndDateUtc"]));
            user.AccessFailedCount = string.IsNullOrEmpty(Convert.ToString(row["AccessFailedCount"])) ? 0 : Convert.ToInt32(row["AccessFailedCount"]);

            return user;
        }

        /// <summary>
        /// Return the user's password hash
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public string GetPasswordHash(string userId)
        {
            const string sqlQuery = "SELECT PasswordHash FROM AspNetUsers WHERE Id = @Id";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id", userId, SqlDbType.NVarChar)
            };

            var passHash = Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery, parameter));

            return string.IsNullOrEmpty(passHash) ? null : passHash;
        }

        /// <summary>
        /// Sets the user's password hash
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public int SetPasswordHash(string userId, string passwordHash)
        {
            const string sqlQuery = "UPDATE AspNetUsers SET PasswordHash = @PasswordHash where Id = @id";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@id", userId, SqlDbType.NVarChar),
                new DbSqlParameter("@PasswordHash", passwordHash, SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery, parameter);

        }

        /// <summary>
        /// Returns the user's security stamp
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetSecurityStamp(string userId)
        {
            const string sqlQuery = "SELECT SecurityStamp FROM AspNetUsers where Id = @id";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@id", userId, SqlDbType.NVarChar)
            };
          
            return Convert.ToString(_database.ExecuteReaderSingleResult(sqlQuery, parameter));
        }

        /// <summary>
        /// Inserts a new user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Insert(TUser user)
        {
            var queryMaker = new SqlQueryMaker { TableName = "AspNetUsers" };
            queryMaker.ValueParameters.Add(new ColumnItem("Id", "@Id"));
            queryMaker.ValueParameters.Add(new ColumnItem("UserName", "@UserName"));
            queryMaker.ValueParameters.Add(new ColumnItem("Email", "@Email"));
            queryMaker.ValueParameters.Add(new ColumnItem("EmailConfirmed", "@EmailConfirmed"));
            queryMaker.ValueParameters.Add(new ColumnItem("PasswordHash", "@PasswordHash"));
            queryMaker.ValueParameters.Add(new ColumnItem("SecurityStamp", "@SecurityStamp"));
            queryMaker.ValueParameters.Add(new ColumnItem("PhoneNumber", "@PhoneNumber"));
            queryMaker.ValueParameters.Add(new ColumnItem("PhoneNumberConfirmed", "@PhoneNumberConfirmed"));
            queryMaker.ValueParameters.Add(new ColumnItem("TwoFactorEnabled", "@TwoFactorEnabled"));
            queryMaker.ValueParameters.Add(new ColumnItem("LockoutEndDateUtc", "@LockoutEndDateUtc"));
            queryMaker.ValueParameters.Add(new ColumnItem("LockoutEnabled", "@LockoutEnabled"));
            queryMaker.ValueParameters.Add(new ColumnItem("AccessFailedCount", "@AccessFailedCount"));
          
            var sqlQuery = queryMaker.Insert();
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id", user.Id, SqlDbType.NVarChar),
                new DbSqlParameter("@UserName", user.UserName, SqlDbType.NVarChar),
                new DbSqlParameter("@Email", user.Email, SqlDbType.NVarChar),
                new DbSqlParameter("@EmailConfirmed", user.EmailConfirmed, SqlDbType.Bit),
                new DbSqlParameter("@PasswordHash", user.PasswordHash, SqlDbType.NVarChar),
                new DbSqlParameter("@SecurityStamp", user.SecurityStamp, SqlDbType.NVarChar),
                new DbSqlParameter("@PhoneNumber", user.PhoneNumber, SqlDbType.NVarChar),
                new DbSqlParameter("@PhoneNumberConfirmed", user.PhoneNumberConfirmed, SqlDbType.Bit),
                new DbSqlParameter("@TwoFactorEnabled", user.TwoFactorEnabled, SqlDbType.Bit),
                new DbSqlParameter("@LockoutEndDateUtc", user.LockoutEndDateUtc, SqlDbType.DateTime),
                new DbSqlParameter("@LockoutEnabled", user.LockoutEnabled, SqlDbType.Bit),
                new DbSqlParameter("@AccessFailedCount", user.AccessFailedCount, SqlDbType.Int)
            };
 
            return _database.ExecuteNonQuery(sqlQuery, parameter);

        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        private int Delete(string userId)
        {
             
            const string sqlQuery = "DELETE FROM AspNetUsers WHERE Id = @userId";
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@id", userId, SqlDbType.NVarChar)
            };

            return _database.ExecuteNonQuery(sqlQuery, parameter);
        }

        /// <summary>
        /// Deletes a user from the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Delete(TUser user)
        {
            return Delete(user.Id);
        }

        /// <summary>
        /// Updates a user in the Users table
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int Update(TUser user)
        {
            var queryMaker = new SqlQueryMaker { TableName = "AspNetUsers" };
            queryMaker.WhereParameters.Add(new ColumnItem("Id", "@Id"));
            queryMaker.ValueParameters.Add(new ColumnItem("UserName", "@UserName"));
            queryMaker.ValueParameters.Add(new ColumnItem("Email", "@Email"));
            queryMaker.ValueParameters.Add(new ColumnItem("EmailConfirmed", "@EmailConfirmed"));
            queryMaker.ValueParameters.Add(new ColumnItem("PasswordHash", "@PasswordHash"));
            queryMaker.ValueParameters.Add(new ColumnItem("SecurityStamp", "@SecurityStamp"));
            queryMaker.ValueParameters.Add(new ColumnItem("PhoneNumber", "@PhoneNumber"));
            queryMaker.ValueParameters.Add(new ColumnItem("PhoneNumberConfirmed", "@PhoneNumberConfirmed"));
            queryMaker.ValueParameters.Add(new ColumnItem("TwoFactorEnabled", "@TwoFactorEnabled"));
            queryMaker.ValueParameters.Add(new ColumnItem("LockoutEndDateUtc", "@LockoutEndDateUtc"));
            queryMaker.ValueParameters.Add(new ColumnItem("LockoutEnabled", "@LockoutEnabled"));
            queryMaker.ValueParameters.Add(new ColumnItem("AccessFailedCount", "@AccessFailedCount"));

            var sqlQuery = queryMaker.Update();
            var parameter = new DbSqlParameterCollection
            {
                new DbSqlParameter("@Id", user.Id, SqlDbType.NVarChar,128),
                new DbSqlParameter("@UserName", user.UserName, SqlDbType.NVarChar,256),
                new DbSqlParameter("@Email", user.Email, SqlDbType.NVarChar,true),
                new DbSqlParameter("@EmailConfirmed", user.EmailConfirmed, SqlDbType.Bit),
                new DbSqlParameter("@PasswordHash", user.PasswordHash, SqlDbType.NVarChar,true),
                new DbSqlParameter("@SecurityStamp", user.SecurityStamp, SqlDbType.NVarChar,true),
                new DbSqlParameter("@PhoneNumber", user.PhoneNumber,SqlDbType.NVarChar,true),
                new DbSqlParameter("@PhoneNumberConfirmed", user.PhoneNumberConfirmed, SqlDbType.Bit),
                new DbSqlParameter("@TwoFactorEnabled", user.TwoFactorEnabled, SqlDbType.Bit),
                new DbSqlParameter("@LockoutEndDateUtc", user.LockoutEndDateUtc, SqlDbType.DateTime,true),
                new DbSqlParameter("@LockoutEnabled", user.LockoutEnabled, SqlDbType.Bit,false),
                new DbSqlParameter("@AccessFailedCount", user.AccessFailedCount, SqlDbType.Int,false)

            };

           return _database.ExecuteNonQuery(sqlQuery, parameter);
        }

    }
}