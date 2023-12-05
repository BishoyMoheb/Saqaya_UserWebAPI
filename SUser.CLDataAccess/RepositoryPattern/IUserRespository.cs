using SUser.CLDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.CLDataAccess.RepositoryPattern
{
    public interface IUserRespository
    {
        Task<MUser> GetUserAsync(string Id);
        // Get access token
        Task<MJwtSTokens> GetJSTokenAsync(string AToken);
        Task<IEnumerable<MUser>> Get_ALL_Async();
        // Add User 
        Task<MUser> AddUserAsync(MUser mUser_New);
        // Add User with its access token 
        void AddJwtSTokens(MJwtSTokens mJwtSTokens);
        // Update and Delete User
        MUser UpdateUser(MUser mUser_Changes);
        void DeleteUserAsync(int Id);
    }
}
