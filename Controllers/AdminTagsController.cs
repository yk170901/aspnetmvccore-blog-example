using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models.Domain;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    public class AdminTagsController : Controller
    {
        // constructor injection for connecting to db
        private readonly BlogDbContext _blogDbContext;
        
        public AdminTagsController(BlogDbContext blogDbContext)
        {
                _blogDbContext = blogDbContext;
        }

        #region Add
        // AdminTags/Add
        [HttpGet]
        public IActionResult Add()
        {
            // view for adding a tag
            return View();
        }

        [HttpPost]
        // [ActionName("Add")] // the name is Add.cshtml; thus, Add. can use when the method name is dif from the action name
        public IActionResult Add(AddTagRequest reqValue)
        {
            #region Method 1 : Manual Reading Inputs
            // Request.Form[value of the property "name" of the input element]
            // string name = Request.Form["name"].ToString();
            // string displayName = Request.Form["displayName"].ToString();
            #endregion

            #region Method 2(Better) : Model Binding
            // string name = reqValue.Name;
            // string displayName = reqValue.DisplayName;
            #endregion

            Tag tag = new Tag()
            {
                Name = reqValue.Name,
                DisplayName = reqValue.DisplayName
            };

            // DbContext.TableName.Action(Value);
            _blogDbContext.Tags.Add(tag);
            _blogDbContext.SaveChanges();

            return RedirectToAction("List"); // when the method has [ActionName("Name")] the value is Name; otherwise, the name of the method should be the value
            //return View("Add");
        }
        #endregion

        #region Get
        [HttpGet]
        public IActionResult List()
        {
            List<Tag> tags = _blogDbContext.Tags.ToList();

            return View(tags);
        }

        #endregion

    }
}
