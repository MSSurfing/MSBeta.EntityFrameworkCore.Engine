using MSSurfing.Data;
using MSSurfing.Data.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSurfing.Services.Users
{
    public interface IUserService
    {
        #region Search & Get
        IPagedList<User> Search(string username = null, string mobilephone = null
            , bool? isActive = null
            , int pageIndex = 0, int pageSize = 2147483647); //or Int32.MaxValue

        User GetUserById(Guid Id);

        User GetUserByUsername(string username);
        #endregion

        #region Insert / Update / Delete 
        bool InsertUser(User User);

        bool UpdateUser(User User);

        bool DeleteUser(User User);
        #endregion
    }
}
