using SrsApi.DbModels;

namespace SrsApi.DbContext
{
    public class SrsItem : BaseDbModel
    {
        /// <summary>
        /// The order in which this SrsItem should appear.
        /// </summary>
        public virtual int Order { get; set; }

        public virtual SrsItemLevel Level { get; set; }

        /// <summary>
        /// A list of potential answers for the SrsItem.
        /// </summary>
        public virtual List<SrsAnswer> Answers { get; set; }
    }
}
