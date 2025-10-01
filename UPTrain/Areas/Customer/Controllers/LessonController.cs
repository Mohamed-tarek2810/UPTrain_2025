using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using UPTrain.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace UPTrain.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<LessonController> _logger;

        public LessonController(
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository,
            ICourseRepository courseRepository,
            ILogger<LessonController> logger)
        {
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Lessons()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return View(lessons);
        }

        public async Task<IActionResult> CourseLessons(int courseId)
        {
            var course = await _courseRepository.GetOneAsync(c => c.CourseId == courseId);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            var lessons = await _lessonRepository.GetAllAsync(l => l.CourseId == courseId);

            ViewBag.Title = course.Title;
            ViewBag.CourseId = courseId;

            return View("CourseLessons", lessons);
        }

        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _lessonRepository.GetOneAsync(l => l.LessonId == id);
            if (lesson == null)
            {
                return NotFound("Lesson not found");
            }

            var quiz = await _quizRepository.GetOneAsync(q => q.CourseId == lesson.CourseId);
            ViewBag.Quiz = quiz;

            return View("Details", lesson);
        }

        public async Task<IActionResult> StartQuiz(int courseId)
        {
            var course = await _courseRepository.GetOneAsync(c => c.CourseId == courseId);
            if (course == null) return NotFound("Course not found");

            var quiz = await _quizRepository.GetOneAsync(q => q.CourseId == courseId);
            if (quiz == null) return NotFound("Quiz not found for this course");

            var questions = await _questionRepository.GetAllAsync(q => q.QuizId == quiz.QuizId);

            ViewBag.Course = course;
            ViewBag.Questions = questions.OrderBy(q => q.QuestionId).ToList();

            return View("StartQuiz", quiz);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuiz(int courseId, IFormCollection form)
        {
            var course = await _courseRepository.GetOneAsync(c => c.CourseId == courseId);
            if (course == null) return NotFound("Course not found");

            var quiz = await _quizRepository.GetOneAsync(q => q.CourseId == courseId);
            if (quiz == null) return NotFound("Quiz not found");

            var questions = await _questionRepository.GetAllAsync(q => q.QuizId == quiz.QuizId);
            var questionList = questions.ToList();

            int correctAnswers = 0;
            var userAnswers = new Dictionary<int, string>();

            foreach (var question in questionList)
            {
                var formKey = $"question_{question.QuestionId}";

                if (form.ContainsKey(formKey))
                {
                    var userAnswer = form[formKey].ToString();
                    userAnswers[question.QuestionId] = userAnswer;

                    if (Enum.TryParse<AnswerOption>(userAnswer, out AnswerOption selected))
                    {
                        if (selected == question.CorrectAnswer)
                        {
                            correctAnswers++;
                        }
                    }
                }
                else
                {
                    userAnswers[question.QuestionId] = "No Answer";
                }
            }

            ViewBag.Course = course;
            ViewBag.Questions = questionList;
            ViewBag.UserAnswers = userAnswers;
            ViewBag.CorrectAnswers = correctAnswers;
            ViewBag.TotalQuestions = questionList.Count;
            ViewBag.Percentage = questionList.Count > 0
                ? (int)((correctAnswers * 100.0) / questionList.Count)
                : 0;

            return View("QuizResult", quiz);
        }

        public IActionResult Certificate()
        {
            return View();
        }
    }
}