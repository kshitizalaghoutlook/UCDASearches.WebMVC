using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UCDASearches.WebMVC.Models
{
    public class BatchSearchViewModel
    {
        // Legacy dropdown support (still honored by the controller for back-compat)
        public string SearchType { get; set; } = "AutoCheck";

        // Inputs
        public string Province { get; set; } = "";
        public string VinBatch { get; set; } = "";

        // Checkbox flags (current behavior)
        public bool OntarioLien { get; set; }
        public bool AutoCheck { get; set; }
        public bool OntarioHistory { get; set; }
        public bool Oop { get; set; }
        public bool Carfax { get; set; }        // from master
        public bool ExportCheck { get; set; }   // from master

        public List<SearchItem> Items { get; set; } = new();

        // Legacy dropdown (kept for compatibility; controller maps these to flags)
        public IEnumerable<SelectListItem> SearchTypes { get; } = new List<SelectListItem>
        {
            new("AutoCheck", "AutoCheck"),
            new("Lien", "Lien"),
            new("Ontario History", "OntarioHistory"),
        };

        // Province list uses 2-letter codes as values
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

