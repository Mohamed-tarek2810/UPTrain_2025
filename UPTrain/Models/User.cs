using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace UPTrain.Models
{
   
    public class User : IdentityUser
    {
        
        [Required]
        [StringLength(100)]
        [Display(Name = "FullName")]
        public string FullName { get; set; } = string.Empty;
        public bool IsBlocked { get; set; } = false;
        public UserRole Role { get; set; } = UserRole.Customer;

        public virtual ICollection<Courses> CreatedCourses { get; set; } = new List<Courses>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Point> Points { get; set; } = new List<Point>();
        public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();

    }
    public enum UserRole
    {
        Customer,
        Admin
    }



}
