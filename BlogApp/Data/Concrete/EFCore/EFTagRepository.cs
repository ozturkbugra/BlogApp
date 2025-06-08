using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EFCore
{
    public class EFTagRepository : ITagRepository
    {
        private BlogContext _context;

        public EFTagRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Tag> Tags => _context.Tags;

        public void CreateTag(Tag Tag)
        {
            _context.Add(Tag);
            _context.SaveChanges();
        }
    }
}
