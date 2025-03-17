using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SrsApi.DbModels
{
    public class BaseDbModel
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual Guid UID { get; set; }
        public virtual DateTime? Created { get; set; }
        public virtual string? CreatedBy { get; set; }
        public virtual DateTime? Deleted { get; set; }
        public virtual string? DeletedBy { get; set; }

        public BaseDbModel()
        {
            if (UID == Guid.Empty)
            {
                UID = Guid.NewGuid();
            }
        }
    }
}
