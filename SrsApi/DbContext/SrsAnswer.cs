using SrsApi.DbModels;

namespace SrsApi.DbContext
{
    public class SrsAnswer : BaseDbModel
    {
        /// <summary>
        /// The Text that the user's input will be compared against.
        /// </summary>
        public virtual string AnswerText { get; set; }

        /// <summary>
        /// A list of all fuzzy search methods that will be used when checking if the user's input is acceptable.
        /// </summary>
        public virtual List<SrsAnswerFuzzySearchMethod> SearchMethods { get; set; }
    }
}
