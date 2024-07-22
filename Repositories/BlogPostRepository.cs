using Project.Data;
using Project.Models.Domain;

namespace Project.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BlogDbContext _blogDbContext;
        public BlogPostRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext; 
        }


        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _blogDbContext.BlogPosts.AddAsync(blogPost);
            await _blogDbContext.SaveChangesAsync();

            return blogPost;
        }

        public Task<BlogPost?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
