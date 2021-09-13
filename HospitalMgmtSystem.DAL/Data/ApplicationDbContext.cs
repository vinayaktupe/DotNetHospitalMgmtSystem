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
        public ApplicationDbContext(){}

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<CasePaper> CasePapers { get; set; }
        public DbSet<CaseFile> CaseFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().Property(appUser => appUser.CreatedAt).HasDefaultValue(DateTime.Now);
            builder.Entity<ApplicationUser>().Property(appUser => appUser.IsAdmin).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().Property(appUser => appUser.IsActive).HasDefaultValue(true);


            builder.Entity<Doctor>().Property(doctor => doctor.IsActive).HasDefaultValue(false);

            //Patient MODEL DESIGN
            builder.Entity<Patient>().Property(patient => patient.IsActive).HasDefaultValue(false);

            //CasePaper MODEL DESIGN
            builder.Entity<CasePaper>().Property(paper => paper.IsSolved).HasDefaultValue(false);
            builder.Entity<CasePaper>().Property(paper => paper.ForSelf).HasDefaultValue(true);
            builder.Entity<CasePaper>().Property(paper => paper.IsActive).HasDefaultValue(true);
            builder.Entity<CasePaper>().Property(paper => paper.CreatedAt).HasDefaultValue(DateTime.Now);

            //CaseFile MODEL DESIGN
            builder.Entity<CaseFile>().Property(file => file.CreatedAt).HasDefaultValue(DateTime.Now);
            builder.Entity<CaseFile>().Property(file => file.IsActive).HasDefaultValue(true);


            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=vinayakt\\SQLEXPRESS;Database=HospitalMgmtSystem_Final;Trusted_Connection=True;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }
    }
    //public class UserDbContext : DbContext
    //{
    //    public UserDbContext(DbContextOptions<UserDbContext> options)
    //        : base(options)
    //    {
    //    }

    //    public DbSet<Doctor> Doctors { get; set; }
    //    public DbSet<Patient> Patients { get; set; }
    //    public DbSet<CasePaper> CasePapers { get; set; }
    //    public DbSet<CaseFile> CaseFiles { get; set; }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        //Doctor MODEL DESIGN
    //        modelBuilder.Entity<Doctor>().Property(doctor => doctor.IsActive).HasDefaultValue(false);

    //        //Patient MODEL DESIGN
    //        modelBuilder.Entity<Patient>().Property(patient => patient.IsActive).HasDefaultValue(false);

    //        //CasePaper MODEL DESIGN
    //        modelBuilder.Entity<CasePaper>().Property(paper => paper.IsSolved).HasDefaultValue(false);
    //        modelBuilder.Entity<CasePaper>().Property(paper => paper.ForSelf).HasDefaultValue(true);
    //        modelBuilder.Entity<CasePaper>().Property(paper => paper.IsActive).HasDefaultValue(true);
    //        modelBuilder.Entity<CasePaper>().Property(paper => paper.CreatedAt).HasDefaultValue(DateTime.Now);

    //        //CaseFile MODEL DESIGN
    //        modelBuilder.Entity<CaseFile>().Property(file => file.CreatedAt).HasDefaultValue(DateTime.Now);
    //        base.OnModelCreating(modelBuilder);
    //    }

    //}
}
