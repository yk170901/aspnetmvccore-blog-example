using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Domain;
using Project.Models.ViewModels;
using Project.Repositories;

namespace Project.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        // DO NOT ACCESS DB DIRECTLY
        // constructor injection for connecting to db
        //private readonly BlogDbContext _blogDbContext;
        //public AdminTagsController(BlogDbContext blogDbContext)
        //{
        //    _blogDbContext = blogDbContext;
        //}

        // USE REPOSITORY PATTERN INSTEAD
        private readonly ITagRepository _tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        #region Add
        // AdminTags/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        // [ActionName("Add")] // the name is Add.cshtml; thus, Add. can use when the method name is dif from the action name
        public async Task<IActionResult> Add(AddTagRequest reqValue)
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

            // Do not directly call db. use repository pattern
            //await _blogDbContext.Tags.AddAsync(tag);
            //await _blogDbContext.SaveChangesAsync();

            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List"); // when the method has [ActionName("Name")] the value is Name; otherwise, the name of the method should be the value
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<Tag> tags = await _tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) // asp-route-"id" -> "id" should be the parameter name
        {
            Tag? tag = await _tagRepository.GetAsync(id);

            if (tag != null)
            {
                // viewmodel
                EditTagRequest editTagRequest = new EditTagRequest()
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }
        #endregion

        // TODO: not httpput?
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest reqValue)
        {
            Tag tag = new Tag()
            {
                Id= reqValue.Id,
                Name= reqValue.Name,
                DisplayName= reqValue.DisplayName
            };

            Tag? updatedTag = await _tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                // show success notification

                // TODO: WHY
                //return View("List"); // DOESNT WORK shows no tag when redirected
                return RedirectToAction("List"); // WORKS
            }
            else
            {
                // show fail notification
                return RedirectToAction("Edit", reqValue.Id); // back to HttpGet Edit(Guid id) page
            }
        }

        // TODO : not httpdelete?
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest reqValue) // Guid id
        {
            Tag? tag = await _tagRepository.DeleteAsync(reqValue.Id);

            if(tag != null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit", reqValue.Id);
            }
        }
    }
}
