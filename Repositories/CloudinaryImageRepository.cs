using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using System.Net;

namespace Project.Repositories
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageRepository()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));

            var envVar = Environment.GetEnvironmentVariable("CLOUDINARY_URL");
            _cloudinary = new Cloudinary(envVar);
            _cloudinary.Api.Secure = true;
        }

        public async Task<string?> UploadAsync(IFormFile file)
        {
            // code are from cloudinary 'get started' section
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.Name,
                // UseFilename = true,
                // UniqueFilename = false,
                // Overwrite = true
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult != null)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }
    }
}
