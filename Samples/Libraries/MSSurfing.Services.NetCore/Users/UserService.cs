using Microsoft.EntityFrameworkCore.Engine;
using MSSurfing.Data;
using MSSurfing.Data.Entity.Users;
using System;
using System.Linq;

namespace MSSurfing.Services.Users
{
    public partial class UserService : IUserService
    {
        #region Constants
        //
        #endregion

        #region Fields
        private readonly IRepository<User> _userRepository;
        #endregion

        #region Ctor
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Utilities
        #endregion

        #region Search & Get

        public IPagedList<User> Search(string username = null, string mobilephone = null, bool? isActive = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userRepository.Table;

            query = query.Where(e => !e.Deleted);
            if (!string.IsNullOrEmpty(username))
                query = query.Where(e => e.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            query = query.OrderBy(e => e.Username);
            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        public IPagedList<User> GetOnlineUsers(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(Guid Id)
        {
            if (Guid.Empty == Id)
                return null;

            return _userRepository.Table.FirstOrDefault(e => e.Id == Id);
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            return _userRepository.Table.FirstOrDefault(e => e.Username == username);
        }
        #endregion

        #region Insert / Update / Delete
        public bool DeleteUser(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            User.Deleted = true;
            return _userRepository.Update(User) > 0;
        }

        public bool InsertUser(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            return _userRepository.Insert(User) > 0;
        }

        public bool UpdateUser(User User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            return _userRepository.Update(User) > 0;
        }
        #endregion
    }
}
