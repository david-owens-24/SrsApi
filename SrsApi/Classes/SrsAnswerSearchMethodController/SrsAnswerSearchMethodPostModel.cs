using SrsApi.Classes.SrsAnswerSearchMethodController;
using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsAnswerSearchMethodController
{
    public class SrsAnswerSearchMethodPostModel
    {
        [Required]
        public Guid SrsAnswerUID { get; set; }
        public List<SrsAnswerSearchMethodItem> SearchMethods { get; set; }

    }
}
