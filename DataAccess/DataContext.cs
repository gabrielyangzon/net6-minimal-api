using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace DataAccess
{
   public class DataContext : DbContext
    {
     
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

       

        public DbSet<UserModel> UsersDb { get; set; }
    }
   

}


