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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-F7MI2PIT\\MSSQLSERVERMO;Initial Catalog=UPTrainDB_DB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
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


            builder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<int>();
        }
    }
}
