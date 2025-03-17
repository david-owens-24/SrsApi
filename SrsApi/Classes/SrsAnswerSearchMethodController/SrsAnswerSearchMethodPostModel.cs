using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsAnswerSearchMethodPostModel
    {
        [Required]
        public Guid SrsAnswerUID { get; set; }

        [Required]
        public int? MinumumAcceptedValue { get; set; }

        [Required]
        public FuzzySearchMethod? FuzzySearchMethod { get; set; }

    }
}
