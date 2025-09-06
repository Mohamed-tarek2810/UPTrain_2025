using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuizzesController : Controller
    {
        private readonly IQuizRepository _quizRepo;
        private readonly ICourseRepository _courseRepo;

        public QuizzesController(IQuizRepository quizRepo, ICourseRepository courseRepo)
        {
            _quizRepo = quizRepo;
            _courseRepo = courseRepo;
        }

        public async Task<IActionResult> Index(int? courseId = null)
        {
            var quizzes = courseId.HasValue
                ? await _quizRepo.GetAllAsync(q => q.CourseId == courseId.Value, q => q.Course)
                : await _quizRepo.GetAllAsync(null, q => q.Course);

            ViewBag.CourseId = courseId;

          
            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", courseId);

            return View(quizzes);
        }

        public async Task<IActionResult> Create()
        {
            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title");

            var quiz = new Quiz();
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
        
            {
                await _quizRepo.AddAsync(quiz);
                var result = await _quizRepo.CommitAsync();
                return RedirectToAction(nameof(Index), new { courseId = quiz.CourseId });
            }


    
       
        }

        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _quizRepo.GetOneAsync(q => q.QuizId == id, q => q.Course);
            if (quiz == null) return NotFound();

            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", quiz.CourseId);

            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Quiz quiz)
        {
         
            {
                await _quizRepo.Update(quiz);
                var result = await _quizRepo.CommitAsync();
                return RedirectToAction(nameof(Index), new { courseId = quiz.CourseId });
            }

          
         
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var quize = await _quizRepo.GetOneAsync(c => c.QuizId == id);

            if (quize is not null)
            {
                await _quizRepo.Delete(quize);
                await _quizRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}