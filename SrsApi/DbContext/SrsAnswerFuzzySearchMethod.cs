using SrsApi.DbModels;

namespace SrsApi.DbContext
{    
    public class SrsAnswerFuzzySearchMethod : BaseDbModel
    {
        /// <summary>
        /// An integer that is the minimum acceptable value for an SrsItemAnswer to be considered correct.
        /// </summary>
        public virtual int MinimumAcceptedValue { get; set; }

        /// <summary>
        /// The search method that will be used.
        /// </summary>
        public virtual SrsFuzzySearchMethod SearchMethod { get; set; }
    }
}
