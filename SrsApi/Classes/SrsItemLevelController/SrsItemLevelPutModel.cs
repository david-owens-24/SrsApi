using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsItemLevelPutModel
    {
        [Required]
        public string Name { get; set; }
    }
}
