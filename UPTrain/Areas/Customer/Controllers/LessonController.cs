using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;

namespace UPTrain.Controllers
{
    [Area("Customer")]
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonController(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<IActionResult> Lessons()
        {
            var lessons = await _lessonRepository.GetAllAsync();
            return View(lessons);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var lesson = await _lessonRepository.GetOneAsync(l => l.LessonId == id);
            if (lesson == null)
            {
                return NotFound();
            }
            return View(lesson);
        }
    }
}
