using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UPTrain.Models;

namespace UPTrain.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Courses> Courses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<UserLesson> UserLessons { get; set; }

        public DbSet<LessonCompletion> LessonCompletions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=db28545.public.databaseasp.net; Database=db28545; User Id=db28545; Password=5n_LB=3ex8W!; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;"
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // UserBadge (Many-to-Many between User & Badge)
            builder.Entity<UserBadge>()
                   .HasKey(ub => new { ub.UserId, ub.BadgeId });

            builder.Entity<UserBadge>()
                   .HasOne(ub => ub.User)
                   .WithMany(u => u.UserBadges)
                   .HasForeignKey(ub => ub.UserId);

            builder.Entity<UserBadge>()
                   .HasOne(ub => ub.Badge)
                   .WithMany(b => b.UserBadges)
                   .HasForeignKey(ub => ub.BadgeId);

            // Convert User.Role enum to int in DB
            builder.Entity<User>()
                   .Property(u => u.Role)
                   .HasConversion<int>();

            // UserLesson (Many-to-Many between User & Lesson)
            builder.Entity<UserLesson>()
                   .HasOne(ul => ul.Lesson)
                   .WithMany()
                   .HasForeignKey(ul => ul.LessonId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Lesson ↔ Quiz (One-to-One) without cascade delete
  
        }
    }
}
