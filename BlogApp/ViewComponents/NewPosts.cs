using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class NewPosts : ViewComponent
    {
        private IPostRepository _postrepository;

        public NewPosts(IPostRepository postRepository)
        {
            _postrepository = postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _postrepository.Posts.OrderByDescending(x=> x.PusblishedOn).Take(5).ToListAsync());
        }
    }
}
