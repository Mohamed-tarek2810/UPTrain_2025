namespace UPTrain.Models
{
    public class Lesson
    {
        public int LessonId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? VideoUrl { get; set; }
        public string? Content { get; set; }
        public int OrderIndex { get; set; }

        public int CourseId { get; set; }
        public Courses Course { get; set; } = null!;

     
   

        public ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();
    }
}

