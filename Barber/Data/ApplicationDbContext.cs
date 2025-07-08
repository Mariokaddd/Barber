
using Barber.Data.enums;
using Barber.Data.models;
using Barber.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Barber.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Hairstyle> Hairstyles { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WorkingSchedule> WorkingSchedules { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AdminSchedule> AdminSchedules { get; set; }
        public DbSet<Reservation> Reservations { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WorkingSchedule>(entity =>
            {
                entity.HasKey(ws => ws.Id);

                entity.Property(ws => ws.Date)
                      .IsRequired();

                entity.Property(ws => ws.Time)
                      .IsRequired();

                entity.HasIndex(ws => new { ws.Date, ws.Time })
                      .IsUnique();
            });

            builder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Date)
                      .IsRequired();

                entity.Property(a => a.Time)
                      .IsRequired();

                entity.Property(a => a.UserId)
                      .IsRequired();

                entity.Property(a => a.HairstyleId)
                      .IsRequired();

                entity.HasIndex(a => new { a.Date, a.Time })
                      .IsUnique();

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Appointments)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Hairstyle)
                      .WithMany(h => h.Appointments)
                      .HasForeignKey(a => a.HairstyleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Appointment>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.Property(a => a.Date)
                      .IsRequired();
                entity.Property(a => a.Time)
                      .IsRequired();
                entity.Property(a => a.UserId)
                      .IsRequired();
                entity.Property(a => a.HairstyleId)
                      .IsRequired();

                entity.HasOne(a => a.User)
                      .WithMany(u => u.Appointments)
                      .HasForeignKey(a => a.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Hairstyle)
                      .WithMany(h => h.Appointments)
                      .HasForeignKey(a => a.HairstyleId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(a => new { a.Date, a.Time })
                      .IsUnique();
            });

            builder.Entity<WorkingSchedule>(entity =>
            {
                entity.HasKey(ws => ws.Id);

                entity.Property(ws => ws.Date)
                      .IsRequired();
                entity.Property(ws => ws.Time)
                      .IsRequired();

                entity.HasIndex(ws => new { ws.Date, ws.Time })
                      .IsUnique();
            });

            builder.Entity<Hairstyle>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.Title)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.HasIndex(h => h.Title)
                      .IsUnique();

                entity.Property(h => h.Description)
                      .IsRequired()
                      .HasMaxLength(300);

                entity.Property(h => h.ImageUrl)
                      .IsRequired();

                entity.Property(h => h.Gender)
                      .IsRequired()
                      .HasConversion<int>();

                entity.HasMany(h => h.Appointments)
                      .WithOne(a => a.Hairstyle)
                      .HasForeignKey(a => a.HairstyleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ContactMessage>(entity =>
            {
                entity.HasKey(cm => cm.Id);

                entity.Property(cm => cm.Message)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(cm => cm.SentAt)
                      .IsRequired();

                entity.HasOne(cm => cm.User)
                      .WithMany(u => u.ContactMessages)
                      .HasForeignKey(cm => cm.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Favorite>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.Property(f => f.UserId)
                      .IsRequired();

                entity.Property(f => f.HairstyleId)
                      .IsRequired();

                entity.HasOne(f => f.User)
                      .WithMany(u => u.Favorites)
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Hairstyle)
                      .WithMany(h => h.Favorites)
                      .HasForeignKey(f => f.HairstyleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            //seedData
            builder.Entity<Hairstyle>().HasData(
        new Hairstyle
        {
            Id = 1,
            Title = "Buzzcut",
            Description = "Къса машинка подстрижка с дължина около 2–3 мм, която акцентира върху формата на главата и е лесна за поддръжка.",
            ImageUrl = "/images/hairstyles/BuzzCut.jpg",
            Gender = Gender.Male
        },
        new Hairstyle
        {
            Id = 2,
            Title = "Момичешки плитки",
            Description = "Дълги плитки за женствен и романтичен вид.",
            ImageUrl = "/images/hairstyles/plitki.jpg",
            Gender = Gender.Female
        },
        new Hairstyle
        {
            Id = 3,
            Title = "Мъжки квифф",
            Description = "Стилна прическа с обем отпред и по-къси страни.",
            ImageUrl = "/images/hairstyles/kviff.jpg",
            Gender = Gender.Male
        },
        new Hairstyle
        {
            Id = 4,
            Title = "Женски боб",
            Description = "Къса и елегантна боб прическа до брадичката.",
            ImageUrl = "/images/hairstyles/Bob.jpg",
            Gender = Gender.Female
        }
    );
        }

    }
}
