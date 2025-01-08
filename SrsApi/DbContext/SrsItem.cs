using SrsApi.DbModels;

namespace SrsApi.DbContext
{
    public class SrsItem : BaseDbModel
    {
        public virtual SrsItemLevel Level { get; set; }

        /// <summary>
        /// A list of potential answers for the SrsItem.
        /// </summary>
        public virtual List<SrsAnswer> Answers { get; set; }
    }
}
