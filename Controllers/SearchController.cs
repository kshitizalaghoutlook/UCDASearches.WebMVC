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
                    existing = new SearchItem { Vin = vin, Province = model.Province };
                    _batchItems.Add(existing);
                }
                else
                {
// Province: append unique province values (CSV), but be null/empty safe
if (!string.IsNullOrWhiteSpace(model.Province))
{
    var provinces = (existing.Province ?? string.Empty)
        .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
        .Select(p => p.Trim())
        .ToList();

    if (!provinces.Contains(model.Province.Trim(), System.StringComparer.OrdinalIgnoreCase))
    {
        provinces.Add(model.Province.Trim());
        existing.Province = string.Join(", ", provinces);
    }
    else if (string.IsNullOrWhiteSpace(existing.Province))
    {
        // If existing was empty, just set it
        existing.Province = model.Province.Trim();
    }
}

if (model.OntarioLien)    existing.OntarioLien = true;
if (model.AutoCheck)      existing.AutoCheck = true;
if (model.OntarioHistory) existing.OntarioHistory = true;
if (model.Oop)            existing.Oop = true;

// Keep master’s flags too
if (model.Carfax)         existing.Carfax = true;
if (model.ExportCheck)    existing.ExportCheck = true;


            model.VinBatch = string.Empty;
            model.Items = _batchItems;
            return View(model);
        }
    }
}
