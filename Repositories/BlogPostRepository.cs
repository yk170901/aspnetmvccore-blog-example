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

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            BlogPost? existingPost = await _blogDbContext.BlogPosts.FindAsync(id);

            if (existingPost != null)
            {
                _blogDbContext.BlogPosts.Remove(existingPost);
                await _blogDbContext.SaveChangesAsync();

                return existingPost;
            }

            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _blogDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> GetAsync(string urlHandle)
        {
            return await _blogDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
            // when not including tags Include(x => x.Tags), an error that says 'duplicate primary key' occurs
            //BlogPost? existingBlogPost = await _blogDbContext.BlogPosts.FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            BlogPost? existingBlogPost = await _blogDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost != null)
            {
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.Tags = blogPost.Tags;
                existingBlogPost.PageTitle = blogPost.PageTitle;

                await _blogDbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }
    }
}
