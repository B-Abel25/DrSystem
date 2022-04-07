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
            //string mySqlConnectionStr = _configuration.GetConnectionString("RemoteMySqlConnection"); //remote
            //string mySqlConnectionStr = _configuration.GetConnectionString("LocalMySqlConnection"); //local
            
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string connStr;


            if (env == "Development")
            {
                //connStr = _configuration.GetConnectionString("DockerPostGresConnection");
                connStr = _configuration.GetConnectionString("LocalMySqlConnection");
                optionsBuilder.UseMySql(connStr, ServerVersion.AutoDetect(connStr), options => options.EnableRetryOnFailure());
            }
            else
            {
                var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                connUrl = connUrl.Replace("postgres://", string.Empty);
                string pgUserPass = connUrl.Split("@")[0];
                string pgHostPortDb = connUrl.Split("@")[1];
                string pgHostPort = pgHostPortDb.Split("/")[0];
                string pgDb = pgHostPortDb.Split("/")[1];
                string pgUser = pgUserPass.Split(":")[0];
                string pgPass = pgUserPass.Split(":")[1];
                string pgHost = pgHostPort.Split(":")[0];
                string pgPort = pgHostPort.Split(":")[1];

                connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;TrustServerCertificate=True";
                optionsBuilder.UseNpgsql(connStr, sqlOptions => sqlOptions.EnableRetryOnFailure());
            }            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(x => x.Reciever)
                .WithMany(u => u.MessagesRecieved);

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Sender)
                .WithMany(u => u.MessagesSent);

            modelBuilder.Entity<Client>(entity => {
                entity.HasIndex(e => e.MedNumber).IsUnique();
            });

            modelBuilder.Entity<Doctor>(entity => {
                entity.HasIndex(e => e.SealNumber).IsUnique();
            });

            /*modelBuilder.Entity<City>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });
            */
            modelBuilder.Entity<County>(entity => {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }

        //dotnet tool install --global dotnet-ef
        //ctrl+shift+b --> build
        //cd doc [TAB]
        //dotnet ef migrations add Teszt2 --context BaseDbContext
        //dotnet ef database update --context BaseDbContext


        //docker run --name dev -e POSTGRES_USER=root -e POSTGRES_PASSWORD=toor -p 5432:5432 -d postgres:latest
    }
}
