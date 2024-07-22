using Project.Models.Domain;

namespace Project.Repositories
{
    public interface ITagRepository
    {
        // async in mind
        Task<Tag?> GetAsync(Guid id);
        
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag> AddAsync(Tag tag);

        Task<Tag?> UpdateAsync(Tag tag);

        Task<Tag?> DeleteAsync(Guid id);
    }
}
