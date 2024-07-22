using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.Domain;
using Project.Models.ViewModels;
using Project.Repositories;
using System.Diagnostics;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ITagRepository _tagRepository;

        public HomeController(ILogger<HomeController> logger, IBlogPostRepository blogPostRepository, ITagRepository tagRepository)
        {
            _logger = logger;
            _blogPostRepository = blogPostRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<BlogPost> blogPosts = await _blogPostRepository.GetAllAsync();
            IEnumerable<Tag> tags = await _tagRepository.GetAllAsync();

            return View(new HomeViewModel()
            {
                BlogPosts = blogPosts,
                Tags = tags
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
