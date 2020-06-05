using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class DBContext : IdentityDbContext<User
    , Role
    , Guid
    , IdentityUserClaim<Guid>
    , UserRole
    , IdentityUserLogin<Guid>
    , IdentityRoleClaim<Guid>
    , IdentityUserToken<Guid>>
    {

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }


        public DbSet<CarProducent> CarProducents { get; set; }

        public DbSet<CarModel> CarModels { get; set; }

        public DbSet<FuelType> FuelTypes { get; set; }

        public DbSet<Equipment> Equipments { get; set; }

        public DbSet<CarOffer> CarOffers { get; set; }

        public DbSet<File> Files { get; set; }



        public new DbSet<Role> Roles { get; set; }

        public new DbSet<User> Users { get; set; }
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupUser(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void SetupUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarProducent>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<CarModel>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<FuelType>()
             .HasKey(x => x.Id);

            modelBuilder.Entity<Equipment>()
             .HasKey(x => x.Id);

            modelBuilder.Entity<CarOffer>()
             .HasKey(x => x.Id);

            modelBuilder.Entity<File>()
             .HasKey(x => x.Id);

            modelBuilder.Entity<CarOfferEquipment>().HasKey(x => new { x.CarOfferId, x.EquipmentId });

            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<Role>().HasKey(x => x.Id);

          /*    modelBuilder.Entity<UserRole>()
               .HasKey(x => new { x.UserId, x.RoleId });*/

       /*     modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });*/

        }

       
        
     
     
     
    }
}

