using HospitalMgmtSystem.DAL.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalMgmtSystem.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().Property(appUser => appUser.CreatedAt).HasDefaultValue(DateTime.Now);
            builder.Entity<ApplicationUser>().Property(appUser => appUser.IsAdmin).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().Property(appUser => appUser.IsActive).HasDefaultValue(true);
            base.OnModelCreating(builder);
        }
    }
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<CasePaper> CasePapers { get; set; }
        public DbSet<CaseFile> CaseFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().Property(doctor => doctor.IsActive).HasDefaultValue(false);
            //modelBuilder.Entity<Doctor>().HasIndex(doctor => new { doctor.User }).IsUnique(unique: true);


            modelBuilder.Entity<Patient>().Property(patient => patient.IsActive).HasDefaultValue(false);
            //modelBuilder.Entity<Patient>().HasIndex(patient => new { patient.User }).IsUnique(unique: true);

            //CasePaper MODEL DESIGN
            modelBuilder.Entity<CasePaper>().Property(paper => paper.IsSolved).HasDefaultValue(false);
            modelBuilder.Entity<CasePaper>().Property(paper => paper.IsActive).HasDefaultValue(false);
            modelBuilder.Entity<CasePaper>().Property(paper => paper.CreatedAt).HasDefaultValue(DateTime.Now);

            modelBuilder.Entity<CaseFile>().Property(file => file.CreatedAt).HasDefaultValue(DateTime.Now);

            //modelBuilder.Entity<CasePaper>().HasIndex(paper => new { paper.ID, paper.Doctor, paper.Patient }).IsUnique(unique: true);
        }

    }
}
