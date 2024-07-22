namespace Project.Repositories
{
    public interface IImageRepository
    {
        /// <returns>URL for Image</returns>
        Task<string> UploadAsync(IFormFile file);

    }
}
