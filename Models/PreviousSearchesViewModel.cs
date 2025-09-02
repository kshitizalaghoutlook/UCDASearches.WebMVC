using System.ComponentModel.DataAnnotations;

namespace UCDASearches.WebMVC.Models
{
    public class PreviousSearch
    {
        public int? RequestID { get; set; }
        public int? UID { get; set; }
        public string? Vin { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string? Account { get; set; }
        public string? Operator { get; set; }
        public short? AutoCheck { get; set; }
        public short? Lien { get; set; }
        public short? History { get; set; }
        public short? OOPS { get; set; }
        public DateTime? ExcaDate { get; set; }
        public short? EXCA { get; set; }
        public short? IRE { get; set; }
        public short? Carfax { get; set; }
        public short? CPIC { get; set; }
        public DateTime? CPICTime { get; set; }
        public short? CAMVAP { get; set; }
        public byte? LNONpath { get; set; }
        public DateTime? LNONcompleted { get; set; }



    }

    public class PreviousSearchesViewModel
    {
        [Display(Name = "Request #")]
        public string? RequestId { get; set; }
<<<<<<< HEAD


        public int? Uid { get; set; }

        public string? Account { get; set; }

        public string? Operator { get; set; }


=======
>>>>>>> 01191a00804d66c3d5eda77a5bb8e61fa85602a0
        public string? Vin { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime? Date { get; set; }

        public List<PreviousSearch> Results { get; set; } = new();
    }
}
