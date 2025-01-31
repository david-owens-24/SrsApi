namespace SrsApi.Enums
{
    public enum FuzzySearchMethod
    {
        /// <summary>
        /// Caseless exact match only
        /// </summary>
        ExactMatch = 1,

        /// <summary>
        /// Simple similar
        /// </summary>
        SimpleRatio,

        /// <summary>
        /// Somewhat similar inside another string
        /// </summary>
        PartialRatio,

        /// <summary>
        /// Words out of order
        /// </summary>
        TokenSortRatio,

        /// <summary>
        /// Words out of order inside another string
        /// </summary>
        PartialTokenSortRatio,

        /// <summary>
        /// Tokenise both strings, remove duplicate tokens, compare intersection/union
        /// </summary>
        TokenSetRatio,

        /// <summary>
        /// Tokenise both strings, remove duplicate tokens, compare intersection/union, but allow for partials
        /// </summary>
        PartialTokenSetRatio,

        /// <summary>
        /// For acronyms. Checks if the first letter of each character in the input string matches the first letter in each word in the compared string
        /// </summary>
        TokenInitialismRatio,

        /// <summary>
        /// For acronyms. Checks if the first letter of each character in the input string matches the first letter in each word in the compared string, but allows for more text around the match
        /// </summary>
        PartialTokenInitialismRatio,

        /// <summary>
        /// Checks for an abbreviation of one string in another
        /// </summary>
        TokenAbbreviationRatio,

        /// <summary>
        /// Checks for an abbreviation of one string in a part of another
        /// </summary>
        PartialTokenAbbreviationRatio,

        /// <summary>
        /// Checks for matching strings, but allows for typos and extra letters
        /// </summary>
        WeightedRatio
    }
}
