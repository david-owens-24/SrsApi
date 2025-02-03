using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsAnswerPostModel
    {
        [Required]
        public Guid SrsItemUID { get; set; }

        public string? AnswerText { get; set; }

        public int? MinumumAcceptedValue { get; set; }

        public FuzzySearchMethod? FuzzySearchMethod { get; set; }

        public bool HasSrsItemSearchMethod()
        {
            if (!string.IsNullOrWhiteSpace(AnswerText) && MinumumAcceptedValue != null && FuzzySearchMethod != null)
            {
                return true;
            }

            return false;
        }
    }
}
