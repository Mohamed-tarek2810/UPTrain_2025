using Microsoft.AspNetCore.Mvc.Rendering;
using UPTrain.Models;

namespace UPTrain.ViewModels
{
    public class CourseWithUserVM
    {
        public Courses? Course { get; set; }

        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}
