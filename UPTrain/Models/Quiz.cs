namespace UPTrain.Models
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; } = string.Empty;

        public int CourseId { get; set; }
        public Courses Course { get; set; } = null!;



        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}

