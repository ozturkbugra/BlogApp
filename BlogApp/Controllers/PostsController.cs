using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postrepository;
        private ITagRepository _tagrepository;

        public PostsController(IPostRepository postRepository, ITagRepository tagrepository)
        {
            _postrepository = postRepository;
            _tagrepository = tagrepository;
        }



        public IActionResult Index()
        {
            return View(
                new PostsViewModel
                {
                    Posts = _postrepository.Posts.ToList(),
                    Tags = _tagrepository.Tags.ToList()

                }

            );
        }
    }
}
