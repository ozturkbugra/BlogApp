using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.ViewComponents
{
    public class TagsMenu: ViewComponent
    {
        private ITagRepository _tagrepository;

        public TagsMenu(ITagRepository tagrepository)
        {
            _tagrepository = tagrepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _tagrepository.Tags.ToListAsync());
        }
    }
}
