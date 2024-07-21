namespace Project.Models.Domain
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        // tag can have multiple blog posts
        public ICollection<BlogPost> BlogPosts { get; set; }
    }
}
