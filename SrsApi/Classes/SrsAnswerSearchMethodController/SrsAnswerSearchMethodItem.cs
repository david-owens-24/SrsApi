using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsAnswerSearchMethodController
{
    public class SrsAnswerSearchMethodItem
    {
        [Required]
        public int? MinumumAcceptedValue { get; set; }

        [Required]
        public FuzzySearchMethod? FuzzySearchMethod { get; set; }
    }
}
