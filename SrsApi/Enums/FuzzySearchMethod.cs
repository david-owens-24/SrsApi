namespace SrsApi.Enums
{
    public enum FuzzySearchMethod
    {
        ExactMatch = 1,
        SimpleRatio,
        PartialRatio,
        TokenSortRatio,
        TokenSetRatio,
        TokenInitialismRatio,
        TokenAbbreviationRatio,
        WeightedRatio
    }
}
