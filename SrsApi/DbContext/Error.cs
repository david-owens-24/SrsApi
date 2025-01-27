using SrsApi.DbModels;

namespace SrsApi.DbContext
{
    public class Error : BaseDbModel
    {
        /// <summary>
        /// A JSON dump of the entire error
        /// </summary>
        public virtual string ExceptionText { get; set; }

    }
}
