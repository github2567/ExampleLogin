using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ThaiBev.Domain.Models;
using System.Reflection.Emit;

namespace ThaiBev.DAL.Data
{
    public partial class ThaiBevDbContext : DbContext
    {
        internal readonly object config;

        public ThaiBevDbContext()
        {
        }

        public ThaiBevDbContext(DbContextOptions<ThaiBevDbContext> options)
    : base(options)
        {
        }

        public virtual DbSet<UserList> UserList { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
