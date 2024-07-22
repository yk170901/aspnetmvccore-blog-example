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

            return RedirectToAction("Add");
            //return View("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            IEnumerable<BlogPost> blogPosts = await _blogPostRepository.GetAllAsync();

            return View(blogPosts);
        }

    }
}
