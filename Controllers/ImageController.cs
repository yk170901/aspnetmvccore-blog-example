using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Repositories;
using System.Net;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // no views to return; only (http) reponses
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        // exist for test + practice purpose^^
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("This is the GetImages API Call");
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            string imageUrl = await _imageRepository.UploadAsync(file);

            if(imageUrl == null)
            {
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }

            return new JsonResult(new { link = imageUrl });
        }
    }
}
