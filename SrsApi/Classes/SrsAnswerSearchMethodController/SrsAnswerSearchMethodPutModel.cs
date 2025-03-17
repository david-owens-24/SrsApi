using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsAnswerSearchMethodPutModel
    {
        public int? MinumumAcceptedValue { get; set; }

        public FuzzySearchMethod? FuzzySearchMethod { get; set; }
    }
}
