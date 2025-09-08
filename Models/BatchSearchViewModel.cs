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
public bool Carfax { get; set; }          // from master
public bool ExportCheck { get; set; }      // from master

public List<SearchItem> Items { get; set; } = new();

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

        };
    }

