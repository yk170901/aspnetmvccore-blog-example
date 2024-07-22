using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            // DbContext.TableName.Action(Value);
            await _blogDbContext.Tags.AddAsync(tag);
            await _blogDbContext.SaveChangesAsync();

            return RedirectToAction("List"); // when the method has [ActionName("Name")] the value is Name; otherwise, the name of the method should be the value
            //return View("Add");
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<IActionResult> List()
        {
            List<Tag> tags = await _blogDbContext.Tags.ToListAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) // asp-route-"id" -> "id" should be the parameter name
        {
            // Classic
            // _blogDbContext.Tags.Find(id);
            
            // LINQ (May be better)
            Tag? tag = await _blogDbContext.Tags.FirstOrDefaultAsync<Tag>(x => x.Id == id);

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

            Tag? existingTag = await _blogDbContext.Tags.FindAsync(tag.Id);

            // TODO : if nothing changed, popup alert instead of applying the change
            if (existingTag != null)
            {
                existingTag.Name = reqValue.Name;
                existingTag.DisplayName = reqValue.DisplayName;

                await _blogDbContext.SaveChangesAsync();
                
                // show success notification
                return RedirectToAction("List");
            }

            // show fail notification
            return RedirectToAction("Edit", reqValue.Id); // back to HttpGet Edit(Guid id) page
        }

        // TODO : not httpdelete?
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest reqValue) // Guid id
        {
            Tag? tag = await _blogDbContext.Tags.FindAsync(reqValue.Id);

            if(tag != null)
            {
                _blogDbContext.Tags.Remove(tag);
                await _blogDbContext.SaveChangesAsync();
             
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", reqValue.Id);
        }
    }
}
