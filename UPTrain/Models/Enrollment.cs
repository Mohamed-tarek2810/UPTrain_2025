namespace UPTrain.Models
{
    public class Enrollment
    {

        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public Courses? Course { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public double ProgressPercentage { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
