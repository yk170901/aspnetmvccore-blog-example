using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Domain;

namespace Project.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public TagRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _blogDbContext.Tags.AddAsync(tag);
            await _blogDbContext.SaveChangesAsync();

            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            Tag? existingTag = await _blogDbContext.Tags.FindAsync(id);

            if (existingTag != null)
            {
                _blogDbContext.Tags.Remove(existingTag);
                await _blogDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _blogDbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _blogDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag newTag)
        {
            Tag? existingTag = await _blogDbContext.Tags.FindAsync(newTag.Id);

            if (existingTag != null)
            {
                existingTag.Name = newTag.Name;
                existingTag.DisplayName = newTag.DisplayName;

                await _blogDbContext.SaveChangesAsync();

                return existingTag;
            }

            return null;
        }
    }
}
