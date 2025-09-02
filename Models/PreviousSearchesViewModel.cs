using System.ComponentModel.DataAnnotations;

namespace UCDASearches.WebMVC.Models
{
    public class PreviousSearch
    {
        public int RequestID { get; set; }
        public string UID { get; set; } = string.Empty;
        public string Vin { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public string Account { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string AutoCheck { get; set; } = string.Empty;
        public string Lien { get; set; } = string.Empty;
        public string History { get; set; } = string.Empty;
        public string OOPS { get; set; } = string.Empty;

        public DateTime? ExcaDate { get; set; }
        public string EXCA { get; set; } = string.Empty;
        public string IRE { get; set; } = string.Empty;
        public string Carfax { get; set; } = string.Empty;
        public string CPIC { get; set; } = string.Empty;
        public DateTime? CPICTime { get; set; }
        public string CAMVAP { get; set; } = string.Empty;
        public string LNONpath { get; set; } = string.Empty;
        public DateTime? LNONcompleted { get; set; }

    }

    public class PreviousSearchesViewModel
    {
        [Display(Name = "Request #")]
        public string? RequestId { get; set; }
        public string? Vin { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        public List<PreviousSearch> Results { get; set; } = new();
    }
}
