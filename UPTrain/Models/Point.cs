namespace UPTrain.Models
{
    public class Point
    {
        public int PointId { get; set; }
        public string Source { get; set; } = string.Empty; // Lesson or Quiz
        public int Value { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int CourseId { get; set; }
        public Courses? Course { get; set; }
    }
}
