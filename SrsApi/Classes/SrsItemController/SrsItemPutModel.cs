using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemController
{
    public class SrsItemPutModel
    {
        [Required]
        public string Name { get; set; }
    }
}
