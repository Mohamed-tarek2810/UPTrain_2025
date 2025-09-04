using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UPTrain.Models
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(200)]
        public string? Title { get; set; }

        [StringLength(10000)]
        public string? Description { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        [Display(Name = "Created By")]
        public string? CreatedById { get; set; }   // FK UserId

        [ForeignKey("CreatedById")]
        public User? CreatedBy { get; set; }   // Navigation property

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
        public ICollection<Point> Points { get; set; } = new List<Point>();
        // update user 
    }
}
