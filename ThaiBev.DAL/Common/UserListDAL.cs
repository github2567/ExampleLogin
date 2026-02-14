using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThaiBev.DAL.Data;
using ThaiBev.Domain.Models;

namespace ThaiBev.DAL.Common
{
    public class UserListDAL
    {
        private readonly ThaiBevDbContext _dbContext;
        public UserListDAL(ThaiBevDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserList>> GetUserList()
        {
            try
            {
                return await _dbContext.UserList.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error UserListDAL:GetUserList", ex);
            }
        }

    }
}
