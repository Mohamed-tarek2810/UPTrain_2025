namespace UPTrain.Models
{
    public class LessonCompletion
    {
        public int Id { get; set; }
        public int LessonId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CompletedAt { get; set; }

        public virtual Lesson? Lesson { get; set; }
        public virtual User? User { get; set; }
    }
}
