using Microsoft.AspNetCore.Mvc;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;

namespace UPTrain.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LessonsController : Controller
    {
        private readonly ILessonRepository _lessonRepo;
        private readonly ICourseRepository _courseRepo;

        public LessonsController(ILessonRepository lessonRepo, ICourseRepository courseRepo)
        {
            _lessonRepo = lessonRepo;
            _courseRepo = courseRepo;
        }

        public async Task<IActionResult> Index(int courseId)
        {
            var lessons = await _lessonRepo.GetAllAsync(
                l => l.CourseId == courseId,

                l => l.Course
            );

            ViewBag.CourseId = courseId;
            return View(lessons);
        }

        [HttpGet]
        public IActionResult Create(int courseId)
        {
            var lesson = new Lesson { CourseId = courseId };
            return View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                await _lessonRepo.AddAsync(lesson);
                var result = await _lessonRepo.CommitAsync();

                if (result)
                    return RedirectToAction("Index", new { courseId = lesson.CourseId });

                ModelState.AddModelError("", "An error occurred while saving the lesson.");
            }
            return View(lesson);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonRepo.GetOneAsync(l => l.LessonId == id);
            if (lesson == null)
                return NotFound();

            return View(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Lesson lesson)
        {
            if (id != lesson.LessonId)
                return NotFound();

            if (ModelState.IsValid)
            {
                await _lessonRepo.Update(lesson);
                var result = await _lessonRepo.CommitAsync();

                if (result)
                    return RedirectToAction("Index", new { courseId = lesson.CourseId });

                ModelState.AddModelError("", "An error occurred while updating the lesson.");
            }
            return View(lesson);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _lessonRepo.GetOneAsync(l => l.LessonId == id, l => l.Course);
            if (lesson == null)
                return NotFound();

            return View(lesson);
        }

  
    }
}
