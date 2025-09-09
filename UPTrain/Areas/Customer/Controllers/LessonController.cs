using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;
using System.Linq;

namespace UPTrain.Controllers
{
    [Area("Customer")]
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ICourseRepository _courseRepository;

        public LessonController(
            ILessonRepository lessonRepository,
            IQuizRepository quizRepository,
            IQuestionRepository questionRepository,
            ICourseRepository courseRepository)
        {
            _lessonRepository = lessonRepository;
            _quizRepository = quizRepository;
            _questionRepository = questionRepository;
            _courseRepository = courseRepository;
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

        
        public async Task<IActionResult> LessonQuiz(int lessonId)
        {
            var lesson = await _lessonRepository.GetOneAsync(l => l.LessonId == lessonId);
            if (lesson == null) return NotFound("Lesson not found");

           
            var quiz = await _quizRepository.GetOneAsync(q => q.CourseId == lesson.CourseId);
            if (quiz == null) return NotFound("Quiz not found for this course");

            var questions = await _questionRepository.GetAllAsync(q => q.QuizId == quiz.QuizId);

            ViewBag.Lesson = lesson;
            ViewBag.Questions = questions.OrderBy(q => q.QuestionId).ToList();

            return View("StartQuiz", quiz); 
        }

        public async Task<IActionResult> StartQuiz(int lessonId)
        {
           
            var lesson = await _lessonRepository.GetOneAsync(l => l.LessonId == lessonId);
            if (lesson == null) return NotFound("Lesson not found");

            
            var quiz = await _quizRepository.GetOneAsync(q => q.QuizId == lessonId);
            if (quiz == null) return NotFound("Quiz not found for this lesson");

           
            var questions = await _questionRepository.GetAllAsync(q => q.QuizId == quiz.QuizId);

            ViewBag.Questions = questions.OrderBy(q => q.QuestionId).ToList();
            ViewBag.LessonId = lessonId;

            return View("StartQuiz", quiz);
        }

    }
}
