using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postrepository;

        public PostsController(IPostRepository postRepository)
        {
            _postrepository = postRepository;
        }



        public async Task<IActionResult> Index(string url)
        {
            var posts = _postrepository.Posts;

            if (!string.IsNullOrEmpty(url))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == url));
            }
            return View(
                new PostsViewModel
                {
                    Posts = await posts.ToListAsync(),

                }

            );
        }

        public async Task<IActionResult> Details(string url)
        {
            return View(await _postrepository.Posts.Include(x=> x.Tags).Include(x=> x.Comments).ThenInclude(x=> x.User).FirstOrDefaultAsync(p => p.Url == url));
        }
    }
}
