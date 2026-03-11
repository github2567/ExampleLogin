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
    public class CustomerDAL
    {
        private readonly ThaiBevDbContext _dbContext;
        public CustomerDAL(ThaiBevDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Customers>> GetCustomerAllList()
        {
            try
            {
                return await _dbContext.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error CustomerDAL:GetCustomerAllList", ex);
            }
        }
    }
}
