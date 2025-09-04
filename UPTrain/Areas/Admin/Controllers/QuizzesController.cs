using Microsoft.AspNetCore.Mvc;
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

    
        public async Task<IActionResult> Index(int courseId)
        {
            var quizzes = await _quizRepo.GetAllAsync(
                q => q.CourseId == courseId,
                q => q.Course
            );

            ViewBag.CourseId = courseId;
            return View(quizzes);
        }

        
        public IActionResult Create(int courseId)
        {
            var quiz = new Quiz { CourseId = courseId };
            return View(quiz);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                await _quizRepo.AddAsync(quiz);
                return RedirectToAction(nameof(Index), new { courseId = quiz.CourseId });
            }
            return View(quiz);
        }

    
        public async Task<IActionResult> Edit(int id)
        {
            var quiz = await _quizRepo.GetOneAsync(q=>q.QuizId == id);
            if (quiz == null) return NotFound();

            return View(quiz);
        }


        [HttpPost] 
        public async Task<IActionResult> Edit(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                await _quizRepo.Update(quiz);
                return RedirectToAction(nameof(Index), new { courseId = quiz.CourseId });
            }
            return View(quiz);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var quiz = await _quizRepo.GetOneAsync(q => q.QuizId == id);
            if (quiz == null) return NotFound();

            return View(quiz);
        }

  
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _quizRepo.GetOneAsync(q => q.QuizId == id);
            if (quiz != null)
            {
                await _quizRepo.Delete(quiz);
                return RedirectToAction(nameof(Index), new { courseId = quiz.CourseId });
            }

            return NotFound();
        }
    }
}
