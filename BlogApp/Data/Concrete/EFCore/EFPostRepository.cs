using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EFCore
{
    public class EFPostRepository : IPostRepository
    {
        private BlogContext _context;

        public EFPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Add(post);
            _context.SaveChanges();
        }
    }
}
