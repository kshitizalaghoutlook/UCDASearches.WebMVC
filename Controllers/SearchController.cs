using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UCDASearches.WebMVC.Models;

namespace UCDASearches.WebMVC.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        [HttpGet("/search")]
        public IActionResult Index() => View(new SearchViewModel());

        [HttpPost]
        public IActionResult Index(SearchViewModel model)
        {
            // TODO: perform searches with submitted data
            return View(model);
        }
    }
}
