using Microsoft.AspNetCore.Mvc;
using Project.Models.ViewModels;

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
            
            string name = reqValue.Name;
            string displayName = reqValue.DisplayName;
            #endregion

            return View("Add");
        }
    }
}
