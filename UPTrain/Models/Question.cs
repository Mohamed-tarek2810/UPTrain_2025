namespace UPTrain.Models
{
    public enum AnswerOption
    {
        A,
        B,
        C,
        D
    }

    public class Question
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;

        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;

        public AnswerOption CorrectAnswer { get; set; } 

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; } = null!; 

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
