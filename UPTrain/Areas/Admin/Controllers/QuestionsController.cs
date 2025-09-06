using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;
using System.Linq;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly IQuizRepository _quizRepo;

        public QuestionsController(IQuestionRepository questionRepo, IQuizRepository quizRepo)
        {
            _questionRepo = questionRepo;
            _quizRepo = quizRepo;
        }

        public async Task<IActionResult> Index(int? quizId = null)
        {
            var questions = quizId.HasValue
                ? await _questionRepo.GetAllAsync(q => q.QuizId == quizId.Value, q => q.Quiz)
                : await _questionRepo.GetAllAsync(null, q => q.Quiz);

            ViewBag.QuizId = quizId;

            var quizzes = await _quizRepo.GetAllAsync(null, q => q.Course);
            ViewBag.Quizzes = new SelectList(quizzes.Select(q => new {
                QuizId = q.QuizId,
                DisplayText = $"{q.Title} - {q.Course?.Title}"
            }), "QuizId", "DisplayText", quizId);

            return View(questions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var quizzes = await _quizRepo.GetAllAsync(null, q => q.Course);
            ViewBag.Quizzes = new SelectList(quizzes.Select(q => new {
                QuizId = q.QuizId,
                DisplayText = $"{q.Title} - {q.Course?.Title}"
            }), "QuizId", "DisplayText");

            var question = new Question();
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question)
        {
          
            {
                await _questionRepo.AddAsync(question);
                var result = await _questionRepo.CommitAsync();
                if (result)
                    return RedirectToAction("Index", new { quizId = question.QuizId });
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ السؤال.");
            }

       
            var quizzes = await _quizRepo.GetAllAsync(null, q => q.Course);
            ViewBag.Quizzes = new SelectList(quizzes.Select(q => new {
                QuizId = q.QuizId,
                DisplayText = $"{q.Title} - {q.Course?.Title}"
            }), "QuizId", "DisplayText", question.QuizId);

            return View(question);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionRepo.GetOneAsync(q => q.QuestionId == id, q => q.Quiz);
            if (question == null) return NotFound();

            var quizzes = await _quizRepo.GetAllAsync(null, q => q.Course);
            ViewBag.Quizzes = new SelectList(quizzes.Select(q => new {
                QuizId = q.QuizId,
                DisplayText = $"{q.Title} - {q.Course?.Title}"
            }), "QuizId", "DisplayText", question.QuizId);

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Question question)
        {
           
            {
                await _questionRepo.Update(question);
                var result = await _questionRepo.CommitAsync();
                if (result)
                    return RedirectToAction("Index", new { quizId = question.QuizId });
                ModelState.AddModelError("", "حدث خطأ أثناء تحديث السؤال.");
            }

            var quizzes = await _quizRepo.GetAllAsync(null, q => q.Course);
            ViewBag.Quizzes = new SelectList(quizzes.Select(q => new {
                QuizId = q.QuizId,
                DisplayText = $"{q.Title} - {q.Course?.Title}"
            }), "QuizId", "DisplayText", question.QuizId);

            return View(question);
        }

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var question = await _questionRepo.GetOneAsync(c => c.QuestionId == id);

            if (question is not null)
            {
                await _questionRepo.Delete(question);
                await _questionRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}