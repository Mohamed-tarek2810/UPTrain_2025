//using Microsoft.AspNetCore.Mvc;
//using UPTrain.IRepositories;
//using UPTrain.Models;

//namespace UPTrain.Controllers
//{
//    [Area("Customer")]
//    public class QuizController : Controller
//    {
//        private readonly IQuizRepository _quizRepository;
//        private readonly IQuestionRepository _questionRepository;
//        private readonly ILessonRepository _lessonRepository;

//        public QuizController(
//            IQuizRepository quizRepository,
//            IQuestionRepository questionRepository,
//            ILessonRepository lessonRepository)
//        {
//            _quizRepository = quizRepository;
//            _questionRepository = questionRepository;
//            _lessonRepository = lessonRepository;
//        }

  
  

 
//        [HttpPost]
//        public async Task<IActionResult> SubmitQuiz(int lessonId, Dictionary<int, string> answers)
//        {
       
//            var lesson = await _lessonRepository.GetOneAsync(l => l.LessonId == lessonId);
//            if (lesson == null) return NotFound("Lesson not found");

         
//            var quiz = await _quizRepository.GetOneAsync(q => q.CourseId == lesson.CourseId);
//            if (quiz == null) return NotFound("Quiz not found for this course");

      
//            var questions = quiz.Questions.ToList();

//            int correctAnswers = 0;
//            foreach (var question in questions)
//            {
//                if (answers.ContainsKey(question.QuestionId))
//                {
//                    var userAnswer = answers[question.QuestionId];
//                    if (Enum.TryParse<AnswerOption>(userAnswer, out var selectedOption))
//                    {
//                        if (selectedOption == question.CorrectAnswer)
//                            correctAnswers++;
//                    }
//                }
//            }

//            double percentage = questions.Any()
//                ? (double)correctAnswers / questions.Count() * 100
//                : 0;

       
//            ViewBag.CorrectAnswers = correctAnswers;
//            ViewBag.TotalQuestions = questions.Count();
//            ViewBag.Percentage = percentage;
//            ViewBag.Questions = questions;
//            ViewBag.UserAnswers = answers;

//            return View("QuizResult", quiz);
//        }
//    }
//}
