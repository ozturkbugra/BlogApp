using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postrepository;

        public PostsController(IPostRepository postRepository)
        {
            _postrepository = postRepository;
        }



        public IActionResult Index()
        {
            return View(
                new PostsViewModel
                {
                    Posts = _postrepository.Posts.ToList(),

                }

            );
        }
    }
}
