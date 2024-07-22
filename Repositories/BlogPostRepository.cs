using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
