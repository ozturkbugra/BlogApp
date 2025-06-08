using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EFCore
{
    public class EFCommentRepository : ICommentRepository
    {
        private BlogContext _context;

        public EFCommentRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> Comments => _context.Comments;

        public void CreateComment(Comment comment)
        {
            _context.Add(comment);
            _context.SaveChanges();
        }
    }
}
