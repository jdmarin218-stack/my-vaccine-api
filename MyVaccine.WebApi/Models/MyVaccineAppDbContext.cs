using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyVaccine.WebApi.Models;

public class MyVaccineAppDbContext : IdentityDbContext<IdentityUser>
{
    public MyVaccineAppDbContext(DbContextOptions<MyVaccineAppDbContext> options) : base(options)
    {
    }

    public new DbSet<User> Users { get; set; }
    public DbSet<Dependent> Dependents { get; set; }
    public DbSet<VaccineCategory> VaccineCategories { get; set; }
    public DbSet<Vaccine> Vaccines { get; set; }
    public DbSet<VaccineRecord> VaccineRecords { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<FamilyGroup> FamilyGroups { get; set; }
    public DbSet<UsersAllergy> UsersAllergies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(255);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<FamilyGroup>()
            .HasMany(fg => fg.Users)
            .WithMany(u => u.FamilyGroups)
            .UsingEntity(j => j.ToTable("UserFamilyGroups"));

        modelBuilder.Entity<FamilyGroup>(entity =>
        {
            entity.Property(fg => fg.Name).IsRequired().HasMaxLength(255);
            entity.HasMany(fg => fg.Dependents)
                  .WithOne(d => d.FamilyGroup)
                  .HasForeignKey(d => d.FamilyGroupId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Dependent>(entity =>
        {
            entity.Property(d => d.Name).IsRequired().HasMaxLength(255);
        });

        modelBuilder.Entity<VaccineCategory>(entity =>
        {
            entity.Property(vc => vc.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Vaccine>(entity =>
        {
            entity.Property(v => v.Name).IsRequired().HasMaxLength(255);
            entity.HasMany(v => v.Categories)
                  .WithMany(vc => vc.Vaccines)
                  .UsingEntity(j => j.ToTable("VaccineCategoryVaccines"));
        });

        modelBuilder.Entity<VaccineRecord>(entity =>
        {
            entity.Property(vr => vr.AdministeredLocation).HasMaxLength(255);
            entity.Property(vr => vr.AdministeredBy).HasMaxLength(255);
            entity.HasOne(vr => vr.Dependent)
                .WithMany(d => d.VaccineRecords)
                .HasForeignKey(vr => vr.DependentId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(vr => vr.Vaccine)
                .WithMany(v => v.VaccineRecords)
                .HasForeignKey(vr => vr.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<UsersAllergy>()
            .HasKey(ua => new { ua.UserId, ua.AllergyId });

        modelBuilder.Entity<UsersAllergy>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.UsersAllergies)
            .HasForeignKey(ua => ua.UserId);

        modelBuilder.Entity<UsersAllergy>()
            .HasOne(ua => ua.Allergy)
            .WithMany(a => a.UsersAllergies)
            .HasForeignKey(ua => ua.AllergyId);
    }
}