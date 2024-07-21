using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class AdminTagsController : Controller
    {
        // AdminTags/Add
        [HttpGet]
        public IActionResult Add()
        {
            // view for adding a tag
            return View();
        }
    }
}
