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
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Take(100)
                ?? Enumerable.Empty<string>();

            foreach (var vin in vins)
            {
                var existing = _batchItems.FirstOrDefault(i =>
                    i.Vin.Equals(vin, System.StringComparison.OrdinalIgnoreCase));

                if (existing == null)
                {
                    // New VIN: carry over Province if provided
                    existing = new SearchItem
                    {
                        Vin = vin,
                        Province = string.IsNullOrWhiteSpace(model.Province) ? null : model.Province.Trim()
                    };
                    _batchItems.Add(existing);
                }
                else
                {
                    // Existing VIN: append-unique Province (CSV), null/empty safe
                    if (!string.IsNullOrWhiteSpace(model.Province))
                    {
                        var provinces = (existing.Province ?? string.Empty)
                            .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim())
                            .ToList();

                        var incoming = model.Province.Trim();
                        if (!provinces.Contains(incoming, System.StringComparer.OrdinalIgnoreCase))
                        {
                            provinces.Add(incoming);
                            existing.Province = string.Join(", ", provinces);
                        }
                        else if (string.IsNullOrWhiteSpace(existing.Province))
                        {
                            // If existing was empty, just set it
                            existing.Province = incoming;
                        }
                    }
                }

                // Back-compat: support legacy SearchType posts if present
                if (!string.IsNullOrWhiteSpace(model.SearchType))
                {
                    switch (model.SearchType)
                    {
                        case "Lien":            existing.OntarioLien = true;      break;
                        case "AutoCheck":       existing.AutoCheck = true;        break;
                        case "OntarioHistory":  existing.OntarioHistory = true;   break;
                        case "Oop":             existing.Oop = true;              break;
                        case "Carfax":          existing.Carfax = true;           break;
                        case "ExportCheck":     existing.ExportCheck = true;      break;
                    }
                }

                // Checkbox-based flags (current behavior)
                if (model.OntarioLien)    existing.OntarioLien = true;
                if (model.AutoCheck)      existing.AutoCheck = true;
                if (model.OntarioHistory) existing.OntarioHistory = true;
                if (model.Oop)            existing.Oop = true;
                if (model.Carfax)         existing.Carfax = true;
                if (model.ExportCheck)    existing.ExportCheck = true;
            }

            // Reset textarea for convenience; keep Province to speed batching
            model.VinBatch = string.Empty;
            // If you prefer to clear Province too, uncomment:
            // model.Province = string.Empty;

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
