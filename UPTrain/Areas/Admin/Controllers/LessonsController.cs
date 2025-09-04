using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UPTrain.IRepositories;
using UPTrain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index(int? courseId)
        {
            var lessons = courseId.HasValue
                ? await _lessonRepo.GetAllAsync(l => l.CourseId == courseId.Value, l => l.Course)
                : await _lessonRepo.GetAllAsync(null, l => l.Course);

     
            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", courseId);
            ViewBag.SelectedCourseId = courseId;

            if (courseId.HasValue)
            {
                var selectedCourse = courses.FirstOrDefault(c => c.CourseId == courseId.Value);
                ViewBag.SelectedCourseName = selectedCourse?.Title;
            }

            return View(lessons);
        }
        //-----------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> Create(int? courseId)
        {
    
            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", courseId);

            var lesson = new Lesson();
            if (courseId.HasValue)
            {
                lesson.CourseId = courseId.Value;
          
                var existingLessons = await _lessonRepo.GetAllAsync(l => l.CourseId == courseId.Value);
                lesson.OrderIndex = existingLessons.Any() ? existingLessons.Max(l => l.OrderIndex) + 1 : 1;
            }

            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson)
        {
        
            {
                try
                {
                    await _lessonRepo.AddAsync(lesson);
                    var result = await _lessonRepo.CommitAsync();

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Lesson created successfully!";
                        return RedirectToAction("Index", new { courseId = lesson.CourseId });
                    }

                    ModelState.AddModelError("", "An error occurred while saving the lesson.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

           
            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", lesson.CourseId);
            return View(lesson);
        }
        //------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonRepo.GetOneAsync(l => l.LessonId == id, l => l.Course);
            if (lesson == null)
                return NotFound();


            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", lesson.CourseId);

            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lesson lesson)
        {
            if (id != lesson.LessonId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _lessonRepo.Update(lesson);
                    var result = await _lessonRepo.CommitAsync();

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Lesson updated successfully!";
                        return RedirectToAction("Index", new { courseId = lesson.CourseId });
                    }

                    ModelState.AddModelError("", "An error occurred while updating the lesson.");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

   
            var courses = await _courseRepo.GetAllAsync();
            ViewBag.Courses = new SelectList(courses, "CourseId", "Title", lesson.CourseId);
            return View(lesson);
        }



        //--------------------------------------------------


        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var lesson = await _lessonRepo.GetOneAsync(c => c.LessonId == id);

            if (lesson is not null)
            {
                await _lessonRepo.Delete(lesson);
                await _lessonRepo.CommitAsync();

                TempData["SuccessMessage"] = "Course deleted successfully!";
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }




    }
}