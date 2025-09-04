using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(int quizId)
        {
            var questions = await _questionRepo.GetAllAsync(
                q => q.QuizId == quizId,
                q => q.Quiz
            );

            ViewBag.QuizId = quizId;
            ViewBag.QuizTitle = questions.FirstOrDefault()?.Quiz?.Title ?? "";
            return View(questions);
        }

        [HttpGet]
        public IActionResult Create(int quizId)
        {
            var question = new Question { QuizId = quizId };
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Question question)
        {
            if (ModelState.IsValid)
            {
                await _questionRepo.AddAsync(question);
                var result = await _questionRepo.CommitAsync();
                if (result)
                    return RedirectToAction("Index", new { quizId = question.QuizId });

                ModelState.AddModelError("", "An error occurred while saving the question.");
            }
            return View(question);
        }

 
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var question = await _questionRepo.GetOneAsync(q => q.QuestionId == id);
            if (question == null) return NotFound();
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Question question)
        {
            if (ModelState.IsValid)
            {
                await _questionRepo.Update(question);
                var result = await _questionRepo.CommitAsync();
                if (result)
                    return RedirectToAction("Index", new { quizId = question.QuizId });

                ModelState.AddModelError("", "An error occurred while updating the question.");
            }
            return View(question);
        }

  
        public async Task<IActionResult> Delete(int id)
        {
            var question = await _questionRepo.GetOneAsync(q => q.QuestionId == id, q => q.Quiz);
            if (question == null) return NotFound();
            return View(question);
        }

   
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _questionRepo.GetOneAsync(q => q.QuestionId == id);
            if (question != null)
            {
                int quizId = question.QuizId;
                await _questionRepo.Delete(question);
                await _questionRepo.CommitAsync();
                return RedirectToAction("Index", new { quizId });
            }
            return NotFound();
        }
    }
}
