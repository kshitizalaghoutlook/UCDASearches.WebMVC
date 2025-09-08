using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UCDASearches.WebMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace UCDASearches.WebMVC.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private static readonly List<SearchItem> _batchItems = new();

        [HttpGet("/search")]
        public IActionResult Index() => View(new SearchViewModel());

        [HttpPost]
        public IActionResult Index(SearchViewModel model)
        {
            // TODO: perform searches with submitted data
            return View(model);
        }

        [HttpGet("/search/batch")]
        public IActionResult Batch()
        {
            var model = new BatchSearchViewModel
            {
                Items = _batchItems
            };
            return View(model);
        }

        [HttpPost("/search/batch")]
        public IActionResult Batch(BatchSearchViewModel model)
        {
            var vins = model.VinBatch?.Split('\n', '\r')
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
                .Take(100)
                ?? Enumerable.Empty<string>();

            foreach (var vin in vins)
            {
                var existing = _batchItems.FirstOrDefault(i => i.Vin.Equals(vin, System.StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    existing = new SearchItem { Vin = vin };
                    _batchItems.Add(existing);
                }

                switch (model.SearchType)
                {
                    case "Lien":
                        existing.OntarioLien = true;
                        if (!string.IsNullOrEmpty(model.Province))
                        {
                            var provinces = existing.Province
                                .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Trim())
                                .ToList();
                            if (!provinces.Contains(model.Province, System.StringComparer.OrdinalIgnoreCase))
                            {
                                provinces.Add(model.Province);
                                existing.Province = string.Join(", ", provinces);
                            }
                        }
                        break;
                    case "AutoCheck":
                        existing.AutoCheck = true;
                        break;
                    case "OntarioHistory":
                        existing.OntarioHistory = true;
                        break;
                }
            }

            model.VinBatch = string.Empty;
            model.Province = string.Empty;
            model.Items = _batchItems;
            return View(model);
        }

        [HttpPost("/search/batch/clear")]
        public IActionResult ClearBatch()
        {
            _batchItems.Clear();
            return RedirectToAction(nameof(Batch));
        }
    }
}
