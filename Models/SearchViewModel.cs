using System.Collections.Generic;
using System.Linq;

namespace UCDASearches.WebMVC.Models
{
    public class SearchItem
    {
        public string Vin { get; set; } = "";
        public bool OntarioLien { get; set; }
        public bool AutoCheck { get; set; }
        public bool OntarioHistory { get; set; }
        public bool Oop { get; set; }
        public bool Carfax { get; set; }
        public bool ExportCheck { get; set; }
    }

    public class SearchViewModel
    {
        public List<SearchItem> Items { get; set; } =
            Enumerable.Range(0, 5).Select(_ => new SearchItem()).ToList();
    }
}
