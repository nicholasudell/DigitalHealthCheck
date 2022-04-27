using System.Collections.Generic;

namespace QMSUK.QRisk
{
    /// <summary>
    /// Converts 16+1 ethnicity into the groupings used in the QRISK algorithm.
    /// </summary>
    public static class EthnicityGroups
    {
        private static readonly IDictionary<Ethnicity, int> EthnicityGrouping = new Dictionary<Ethnicity, int>()
        {
            {Ethnicity.NotRecorded, 0 },
            {Ethnicity.British, 0},
            {Ethnicity.Irish,0 },
            {Ethnicity.OtherWhiteBackground,0 },
            {Ethnicity.WhiteAndBlackCaribbeanMixed, 8 },
            {Ethnicity.WhiteAndBlackAfricanMixed, 8 },
            {Ethnicity.WhiteAndAsianMixed, 8 },
            {Ethnicity.OtherMixed, 8 },
            {Ethnicity.Indian, 1},
            {Ethnicity.Pakistani, 2},
            {Ethnicity.Bangladeshi, 3},
            {Ethnicity.OtherAsian, 4},
            {Ethnicity.Caribbean, 5},
            {Ethnicity.BlackAfrican, 6},
            {Ethnicity.OtherBlack, 8},
            {Ethnicity.Chinese, 7},
            {Ethnicity.OtherEthnicGroup, 8 },
            {Ethnicity.NotStated, 0 }
        };

        /// <summary>
        /// Gets the Ethnicity Grouping for an ethnicity.
        /// </summary>
        /// <param name="ethnicity">The ethnicity.</param>
        /// <returns>The QRISK ethnicity grouping for the provided ethnicity.</returns>
        public static int GroupingFor(Ethnicity ethnicity) => EthnicityGrouping[ethnicity];
    }
}