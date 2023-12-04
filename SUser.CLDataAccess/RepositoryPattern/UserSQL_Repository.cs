using Microsoft.EntityFrameworkCore;
using SUser.CLDataAccess.EFContext;
using SUser.CLDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUser.CLDataAccess.RepositoryPattern
{
    public class UserSQL_Repository : IUserRespository
    {
        private readonly MUserDbContext _mUserDbContext;

        public UserSQL_Repository(MUserDbContext mUserDbContext)
        {
            this._mUserDbContext = mUserDbContext;
        }

        public async Task<MUser> AddUserAsync(MUser mUser_New)
        {
            await _mUserDbContext.DbS_Users.AddAsync(mUser_New);
            _mUserDbContext.SaveChanges();
            return mUser_New;
        }

        public async void DeleteUserAsync(int Id)
        {
            MUser User_ToDelete = await _mUserDbContext.DbS_Users.FindAsync(Id);
            _mUserDbContext.DbS_Users.Remove(User_ToDelete);
            _mUserDbContext.SaveChanges();
        }

        public async Task<MUser> GetUserAsync(string Id)
        {
            MUser User_ToGet = await _mUserDbContext.DbS_Users.FindAsync(Id);
            return User_ToGet;
        }

        public async Task<IEnumerable<MUser>> Get_ALL_Async()
        {
            IEnumerable<MUser> mUsersList = await _mUserDbContext.DbS_Users.ToListAsync();
            return mUsersList;
        }

        public MUser UpdateUser(MUser mUser_Changes)
        {
            var User_ToUpdate = _mUserDbContext.DbS_Users.Attach(mUser_Changes);
            User_ToUpdate.State = EntityState.Modified;
            _mUserDbContext.SaveChanges(); 
            return mUser_Changes;
        }

        public void Dispose()
        {
            _mUserDbContext.Dispose();
        }
    }
}
