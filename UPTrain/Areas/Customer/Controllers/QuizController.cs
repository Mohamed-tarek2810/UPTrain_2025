using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;

namespace UPTrain.Controllers
{
    [Area("Customer")]
    public class QuizController : Controller
    {
        private readonly IQuizRepository _quizRepository;

        public QuizController(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

       
        public async Task<IActionResult> Quizs()
        {
            var quizzes = await _quizRepository.GetAllAsync();
            return View(quizzes);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var quiz = await _quizRepository.GetOneAsync(q => q.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }
    }
}
