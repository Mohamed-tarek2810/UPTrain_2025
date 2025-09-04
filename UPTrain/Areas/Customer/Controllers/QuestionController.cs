using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using UPTrain.Repositories;
using System.Threading.Tasks;

namespace UPTrain.Controllers
{
    [Area("Customer")]
    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionController(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        
        public async Task<IActionResult> Questions()
        {
            var questions = await _questionRepository.GetAllAsync();
            return View(questions);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var question = await _questionRepository.GetOneAsync(q => q.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }
    }
}
