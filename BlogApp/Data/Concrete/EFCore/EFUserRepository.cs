using BlogApp.Data.Abstract;
using BlogApp.Entity;

namespace BlogApp.Data.Concrete.EFCore
{
    public class EFUserRepository : IUserRepository
    {
        private BlogContext _context;

        public EFUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User User)
        {
            _context.Add(User);
            _context.SaveChanges();
        }
    }
}
