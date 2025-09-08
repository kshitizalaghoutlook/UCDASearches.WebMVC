using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UCDASearches.WebMVC.Models
{
    public class BatchSearchViewModel
    {
        public string SearchType { get; set; } = "AutoCheck";
        public string Province { get; set; } = "";
        public string VinBatch { get; set; } = "";
        public List<SearchItem> Items { get; set; } = new();
        public IEnumerable<SelectListItem> SearchTypes { get; } = new List<SelectListItem>
        {
            new("AutoCheck", "AutoCheck"),
            new("Lien", "Lien"),
            new("Ontario History", "OntarioHistory"),
        };
        public IEnumerable<SelectListItem> Provinces { get; } = new List<SelectListItem>
        {
            new("Alberta", "AB"),
            new("British Columbia", "BC"),
            new("Manitoba", "MB"),
            new("New Brunswick", "NB"),
            new("Newfoundland and Labrador", "NL"),
            new("Nova Scotia", "NS"),
            new("Ontario", "ON"),
            new("Prince Edward Island", "PE"),
            new("Quebec", "QC"),
            new("Saskatchewan", "SK"),
            new("Northwest Territories", "NT"),
            new("Nunavut", "NU"),
            new("Yukon", "YT"),
        };
    }
}
