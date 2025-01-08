using KellermanSoftware.CompareNetObjects;
using SrsApi.DbModels;
using SrsApi.Interfaces;

namespace SrsApi.ChangedValuesCheckers
{
    public class BaseChangedValuesChecker<T> : IBaseChangedValuesChecker<T> where T : BaseDbModel
    {
        public ComparisonResult _comparisonResult { get; set; }

        public BaseChangedValuesChecker()
        {

        }

        public ComparisonResult CheckForChangedValues(T oldValue, T newValue)
        {
            CompareLogic compareLogic = new CompareLogic();

            //ignore stuff that the user can't set
            compareLogic.Config.IgnoreProperty<BaseDbModel>(x => x.Id);
            compareLogic.Config.IgnoreProperty<BaseDbModel>(x => x.UID);
            compareLogic.Config.IgnoreProperty<BaseDbModel>(x => x.Created);
            compareLogic.Config.IgnoreProperty<BaseDbModel>(x => x.CreatedBy);
            compareLogic.Config.IgnoreProperty<BaseDbModel>(x => x.Deleted);
            compareLogic.Config.IgnoreProperty<BaseDbModel>(x => x.DeletedBy);

            //TODO: make this configurable
            compareLogic.Config.MaxDifferences = 100;

            _comparisonResult = compareLogic.Compare(oldValue, newValue);

            return _comparisonResult;
        }
    }
}
