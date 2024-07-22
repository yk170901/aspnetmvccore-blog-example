using Microsoft.AspNetCore.Mvc;
using Project.Models.Domain;
using Project.Repositories;

namespace Project.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;

        public BlogController(IBlogPostRepository blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            BlogPost? foundBlogPost = await _blogPostRepository.GetAsync(urlHandle);

            return View(foundBlogPost);
        }
    }
}
