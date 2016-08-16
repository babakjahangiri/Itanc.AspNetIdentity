using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Security.Claims;
using Itanc.AspNetIdentity.AdoNet;
using Itanc.AspNetIdentity.Entities;

namespace Itanc.AspNetIdentity
{
    /// <summary>
    /// This Class implements the key ASP.NET Identity user store iterfaces
    /// </summary>
    public class UserStore<TUser> : IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserEmailStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserTwoFactorStore<TUser, string>,
        IUserLockoutStore<TUser, string>,
        IUserStore<TUser>
        where TUser : IdentityUser
    {

        private readonly UsersTable<TUser> _userTable;
        private readonly RolesTable _roleTable;
        private readonly  UsersRolesTable _userRolesTable;
        private readonly UsersClaimsTable _userClaimsTable;
        private readonly UsersLoginsTable _userLoginsTable;
        public SqlServerDatabase Database { get; set; }

        public UserStore(SqlServerDatabase database)
        {
             Database = database;
            _userTable = new UsersTable<TUser>(database);
            _roleTable = new RolesTable(database);
            _userRolesTable = new UsersRolesTable(database);
            _userClaimsTable = new UsersClaimsTable(database);
            _userLoginsTable = new UsersLoginsTable(database);
        }

        public Task CreateAsync(TUser user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _userTable.Insert(user);

            return Task.FromResult<object>(null);
          
        }


        public Task<TUser> FindByIdAsync(string userId)
        {

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Null or empty argument: userId");
            }

            var result = _userTable.GetUserById(userId);
            return Task.FromResult<TUser>(result ?? null);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Null or empty argument: userName");
            }

            var user = _userTable.GetUserByName(userName);

            // ReSharper disable once SuspiciousTypeConversion.Global
            //List<TUser> result = user as List<TUser>;

            //if (result != null && result.Count == 1)
            //{
            //    return Task.FromResult<TUser>(result[0]);
            //}

            return Task.FromResult<TUser>(user);
        }

        public Task UpdateAsync(TUser user)
        {

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _userTable.Update(user);

            return Task.FromResult<object>(null);

        }

        public Task DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _userTable.Delete(user);
            return Task.FromResult<object>(null);
            
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            var passwordHash = _userTable.GetPasswordHash(user.Id);

            return Task.FromResult<string>(passwordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            var hasPassword = !string.IsNullOrEmpty(_userTable.GetPasswordHash(user.Id));

            return Task.FromResult<bool>(bool.Parse(hasPassword.ToString()));
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }


        public void Dispose()
        {
            if (Database == null)
            {
                return;
            }

            Database.Dispose();
            Database = null;
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return
                Task.FromResult(user.LockoutEndDateUtc.HasValue
                    ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                    : new DateTimeOffset());
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount++;
            _userTable.Update(user);

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            user.AccessFailedCount = 0;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var identity = _userClaimsTable.FindByUserId(user.Id);

            return Task.FromResult<IList<Claim>>(identity.Claims.ToList());
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            _userClaimsTable.Insert(claim, user.Id);

            return Task.FromResult<object>(null);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            _userClaimsTable.Delete(user, claim);

            return Task.FromResult<object>(null);
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            var roleId = _roleTable.GetRoleId(roleName);
            if (!string.IsNullOrEmpty(roleId))
            {
                _userRolesTable.Insert(user, roleId);
            }

            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Argument cannot be null or empty: roleName.");
            }

            var roleId = _roleTable.GetRoleId(roleName);
            if (!string.IsNullOrEmpty(roleId))
            {
                _userRolesTable.Delete(user.Id, roleId);
            }

            return Task.FromResult<object>(null);

        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = _userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null)
                {
                    return Task.FromResult<IList<string>>(roles);
                }
            }

            return Task.FromResult<IList<string>>(null);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(roleName))
            {
                throw new ArgumentNullException(nameof(user));
            }

            var roles = _userRolesTable.FindByUserId(user.Id);
            {
                if (roles != null && roles.Contains(roleName))
                {
                    return Task.FromResult<bool>(true);
                }
            }

            return Task.FromResult<bool>(false);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            var passwordHash = _userTable.GetPasswordHash(user.Id);
            return Task.FromResult<string>(passwordHash);

        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            var hasPassword = !string.IsNullOrEmpty(_userTable.GetPasswordHash(user.Id));

            return Task.FromResult<bool>(bool.Parse(hasPassword.ToString()));
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            user.Email = email;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            _userTable.Update(user);

            return Task.FromResult(0);

        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            var result = _userTable.GetUserByEmail(email) as TUser;
            return Task.FromResult<TUser>(result ?? null);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            _userTable.Update(user);

            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            _userLoginsTable.Insert(user, login);

            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            _userLoginsTable.Delete(user, login);

            return Task.FromResult<object>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            var userLogins = new List<UserLoginInfo>();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var logins = _userLoginsTable.FindByUserId(user.Id);
            return Task.FromResult<IList<UserLoginInfo>>(logins ?? null);
        }

        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var userId = _userLoginsTable.FindUserIdByLogin(login);
            if (userId != null)
            {
                var user = _userTable.GetUserById(userId) as TUser;
                if (user != null)
                {
                    return Task.FromResult<TUser>(user);
                }
            }

            return Task.FromResult<TUser>(null);
        }
    }
}