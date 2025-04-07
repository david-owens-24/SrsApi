using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsAnswerController
{
    public class SrsAnswerPostModel
    {
        [Required]
        public Guid SrsItemUID { get; set; }

        public string? AnswerText { get; set; }

        public int? MinimumAcceptedValue { get; set; }

        public FuzzySearchMethod? FuzzySearchMethod { get; set; }

        public bool HasSrsItemSearchMethod()
        {
            if (!string.IsNullOrWhiteSpace(AnswerText) && MinimumAcceptedValue != null && FuzzySearchMethod != null)
            {
                return true;
            }

            return false;
        }
    }
}
