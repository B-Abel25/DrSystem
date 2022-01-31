using DoctorSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoctorSystem.Entities.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public BaseDbContext()
        {
        }
        public BaseDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual DbSet<User> _users { get; set; }
        public virtual DbSet<Doctor> _doctors { get; set; }
        public virtual DbSet<Place> _place { get; set; }
        public virtual DbSet<County> _county { get; set; }
        public virtual DbSet<City> _city { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string mySqlConnectionStr = _configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr));

            //optionsBuilder.UseNpgsql(posgresConnectionStr,
            //    sqlOptions =>
            //    {
            //        sqlOptions.EnableRetryOnFailure(
            //        maxRetryCount: 2,
            //        maxRetryDelay: TimeSpan.FromSeconds(30),
            //        errorCodesToAdd: null);
            //    });
        }

        /*
        protected override void OnModelCreating(ModelBuilder objModelBuilder)
        {
            objModelBuilder.Entity<User>().ToTable("users");


            //TODO: Ez ide miért kell? 
            base.OnModelCreating(objModelBuilder);
        }
        */

        //ctrl+shift+b --> build
        //cd doc [TAB]
        //dotnet ef migrations add Teszt2 --context BaseDbContext
        //dotnet ef database update --context BaseDbContext
    }
}
