using SrsApi.DbModels;

namespace SrsApi.DbContext
{
    public class SrsItemDetails : BaseDbModel
    {
        public virtual string Question { get; set; }
    }
}
