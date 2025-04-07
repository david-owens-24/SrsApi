using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsAnswerSearchMethodController
{
    public class SrsAnswerSearchMethodItem
    {
        [Required]
        public int? MinimumAcceptedValue { get; set; }

        [Required]
        public FuzzySearchMethod? FuzzySearchMethod { get; set; }
    }
}
