using KellermanSoftware.CompareNetObjects;
using SrsApi.DbContext;
using SrsApi.DbModels;

namespace SrsApi.Interfaces
{
    public interface IBaseChangedValuesChecker<T> where T : BaseDbModel
    {
        ComparisonResult CheckForChangedValues(T oldValue, T newValue);
    }
}
