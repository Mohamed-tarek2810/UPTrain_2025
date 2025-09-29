using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace UPTrain.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

       
        public async Task<IActionResult> Index()
        {
            var answers = await _answerRepository.GetAllAsync();
            return View(answers);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var answer = await _answerRepository.GetOneAsync(a => a.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }
            return View(answer);
        }
    }
}
