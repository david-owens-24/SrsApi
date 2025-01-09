using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemLevelController
{
    public class SrsItemLevelPostModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Level must be set to a value greater than {1}")]
        public int Level { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
