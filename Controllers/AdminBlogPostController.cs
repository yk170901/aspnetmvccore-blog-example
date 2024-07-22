using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.Models.Domain;
using Project.Models.ViewModels;
using Project.Repositories;

namespace Project.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // get tags from repository
            var tags = await _tagRepository.GetAllAsync();

            AddBlogPostRequest model = new AddBlogPostRequest()
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.Id.ToString() })
            };

            return View(model); // add tags from db to AddBlogPostRequest model in Add.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest reqValue)
        {
            BlogPost blogPost = new BlogPost()
            {
                Author = reqValue.Author,
                Content = reqValue.Content,
                FeaturedImageUrl = reqValue.FeaturedImageUrl,
                Heading = reqValue.Heading,
                PageTitle = reqValue.PageTitle,
                PublishedDate = reqValue.PublishedDate,
                ShortDescription = reqValue.ShortDescription,
                UrlHandle = reqValue.UrlHandle,
                Visible = reqValue.Visible
            };

            List<Tag> selectedTags = new List<Tag>();

            // map tags fom selected tags
            foreach (string selectedTagId in reqValue.SelectedTags)
            {
                Guid id = Guid.Parse(selectedTagId);
                Tag? tag = await _tagRepository.GetAsync(id);
                
                if(tag != null)
                {
                    selectedTags.Add(tag);
                }
            }
            blogPost.Tags = selectedTags;

            await _blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<BlogPost> blogPosts = await _blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) // asp-route-"id"
        {
            BlogPost? blogPost = await _blogPostRepository.GetAsync(id);

            if (blogPost != null)
            {
                IEnumerable<Tag> allTags = await _tagRepository.GetAllAsync();

                EditBlogPostRequest model = new EditBlogPostRequest()
                {
                    Author = blogPost.Author,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    Heading = blogPost.Heading,
                    Id = blogPost.Id,
                    PageTitle = blogPost.PageTitle,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = allTags.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);
            }
            else
            {
                return View(null);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest reqValue)
        {
            List<Tag> tagsToAdd = new List<Tag>();

            foreach (string selectedTag in reqValue.SelectedTags) {
               if (Guid.TryParse(selectedTag, out Guid id))
                {
                    Tag? foundTag = await _tagRepository.GetAsync(id);
                    if (foundTag != null)
                    {
                        tagsToAdd.Add(foundTag);
                    }
                }
            }

            BlogPost blogPost = new BlogPost()
            {
                Id = reqValue.Id,
                Author = reqValue.Author,
                Content = reqValue.Content,
                FeaturedImageUrl = reqValue.FeaturedImageUrl,
                Heading = reqValue.Heading,
                UrlHandle = reqValue.UrlHandle,
                PageTitle = reqValue.PageTitle,
                PublishedDate = reqValue.PublishedDate,
                ShortDescription = reqValue.ShortDescription,
                Visible = reqValue.Visible,
                Tags = tagsToAdd
            };

            BlogPost? updatedBlogPost = await _blogPostRepository.UpdateAsync(blogPost);

            if(updatedBlogPost != null)
            {
                //return RedirectToAction("Edit");
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest reqValue)
        {
            BlogPost? affectedBlogPost = await _blogPostRepository.DeleteAsync(reqValue.Id);

            if (affectedBlogPost != null)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Edit");
            }
        }
    }
}
