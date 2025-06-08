using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EFCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postrepository;
        private ICommentRepository _commentrepository;

        public PostsController(IPostRepository postRepository, ICommentRepository commentrepository)
        {
            _postrepository = postRepository;
            _commentrepository = commentrepository;
        }



        public async Task<IActionResult> Index(string url)
        {
            var claims = User.Claims;

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

        [HttpPost]
        public JsonResult AddComment(int PostID, string Text)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);

            var entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostID = PostID,
                UserID = int.Parse(userID ?? ""),
            };
            _commentrepository.CreateComment(entity);

            return Json(new
            {
                userName,
                Text,
                entity.PublishedOn,
                avatar
            });

            //return Redirect("/posts/details/"+Url);
        }


        public ActionResult Create()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Posts");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Create(PostCreateViewModel model)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                _postrepository.CreatePost(new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    Description = model.Description,
                    Url = model.Url,
                    UserID = int.Parse(userID),
                    PusblishedOn = DateTime.Now,
                    Image = "mvc.png",
                    IsActive = false,


                });

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
