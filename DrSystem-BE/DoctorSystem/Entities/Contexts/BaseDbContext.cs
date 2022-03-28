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

        public virtual DbSet<Client> _clients { get; set; }
        public virtual DbSet<Doctor> _doctors { get; set; }
        public virtual DbSet<Place> _places { get; set; }
        public virtual DbSet<City> _city { get; set; }
        public virtual DbSet<County> _county { get; set; }
        public virtual DbSet<Message> _messages { get; set; }
        public virtual DbSet<OfficeHours> _officehours { get; set; }
        public virtual DbSet<Appointment> _appointments { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string mySqlConnectionStr = _configuration.GetConnectionString("DefaultConnection");
            //string mySqlConnectionStr = _configuration.GetConnectionString("LocalConnection");

            optionsBuilder.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr), options => options.EnableRetryOnFailure());

            //optionsBuilder.UseNpgsql(posgresConnectionStr,
            //    sqlOptions =>
            //    {
            //        sqlOptions.EnableRetryOnFailure(
            //        maxRetryCount: 2,
            //        maxRetryDelay: TimeSpan.FromSeconds(30),
            //        errorCodesToAdd: null);
            //    });
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

           
/*
            // Add the shadow property to the model
            modelBuilder.Entity<City>()
                .Property<string>("CountyFK");
*/
            // Use the shadow property as a foreign key
            modelBuilder.Entity<City>()
                .HasOne(ci => ci.County)
                .WithMany(co => co.Cities)
                .HasForeignKey(ci => ci.CountyId);
            
/*
            // Add the shadow property to the model
            modelBuilder.Entity<Place>()
                .Property<string>("CityFK");
*/ 
            // Use the shadow property as a foreign key
            modelBuilder.Entity<Place>()
                .HasOne(p => p.City)
                .WithMany(ci => ci.Places)
                .HasForeignKey(p => p.CityId);


            modelBuilder.Entity<Message>()
                .HasOne(x => x.Reciever)
                .WithMany(u => u.MessagesRecieved);


            modelBuilder.Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(u => u.MessagesSent);

            base.OnModelCreating(modelBuilder);
        }

        //dotnet tool install --global dotnet-ef
        //ctrl+shift+b --> build
        //cd doc [TAB]
        //dotnet ef migrations add Teszt2 --context BaseDbContext
        //dotnet ef database update --context BaseDbContext
    }
}
