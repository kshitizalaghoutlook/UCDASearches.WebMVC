using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UCDASearches.WebMVC.Models
{
    public class BatchSearchViewModel
    {
        public string Province { get; set; } = "";
        public string VinBatch { get; set; } = "";
        public bool OntarioLien { get; set; }
        public bool AutoCheck { get; set; }
        public bool OntarioHistory { get; set; }
        public bool Oop { get; set; }
        public bool Carfax { get; set; }
        public bool ExportCheck { get; set; }
        public List<SearchItem> Items { get; set; } = new();
        public IEnumerable<SelectListItem> Provinces { get; } = new List<SelectListItem>
        {
            new("Alberta", "Alberta"),
            new("British Columbia", "British Columbia"),
            new("Manitoba", "Manitoba"),
            new("New Brunswick", "New Brunswick"),
            new("Newfoundland and Labrador", "Newfoundland and Labrador"),
            new("Nova Scotia", "Nova Scotia"),
            new("Ontario", "Ontario"),
            new("Prince Edward Island", "Prince Edward Island"),
            new("Quebec", "Quebec"),
            new("Saskatchewan", "Saskatchewan"),
            new("Northwest Territories", "Northwest Territories"),
            new("Nunavut", "Nunavut"),
            new("Yukon", "Yukon"),
        };
    }
}
