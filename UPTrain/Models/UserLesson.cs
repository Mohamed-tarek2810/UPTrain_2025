using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UPTrain.Models
{
    public class UserLesson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = default!; 

        [Required]
        public int LessonId { get; set; }

        public bool IsCompleted { get; set; } = true;

        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        // Navigation (اختياري)
        public Lesson Lesson { get; set; } = default!;
        // public ApplicationUser User { get; set; } // لو عندك كلاس User مخصّص
    }
}
