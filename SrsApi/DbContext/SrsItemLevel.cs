using SrsApi.DbModels;

namespace SrsApi.DbContext
{
    public class SrsItemLevel : BaseDbModel
    {
        /// <summary>
        /// A number to determine the Level of the SrsItem. Generally, a higher level should be a more difficult question.
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// The Name of the SrsItemLevel. Should be a more user-friendly representation of the Level.
        /// </summary>
        public virtual string Name { get; set; }
    }
}
