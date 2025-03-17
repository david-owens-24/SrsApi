using SrsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsItemDetailsPostModel
    {
        [Required]
        public Guid SrsItemUID { get; set; }

        public string? QuestionText { get; set; }
    }
}
