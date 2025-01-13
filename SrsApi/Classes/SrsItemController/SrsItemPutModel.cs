using System.ComponentModel.DataAnnotations;

namespace SrsApi.Classes.SrsItemController
{
    public class SrsItemPutModel
    {
        [Required]
        public Guid SrsItemLevelUID { get; set; }

        public int Order { get; set; }
    }
}
