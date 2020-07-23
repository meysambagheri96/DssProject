using System.Collections.Generic;
using System.Linq;

namespace DssMultipleCriteriaAnalysis
{
    public static class ArrayHelper
    {
        public static double[] To1DArray(this double[,] oGridCells)
        {
            double[] oResult = new double[oGridCells.Length];
            System.Buffer.BlockCopy(oGridCells, 0, oResult, 0, 16);
            return oResult;
        }

        public static List<List<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            return list.Select((item, index) => new { index, item })
                         .GroupBy(x => (x.index + 1) / (list.Count() / parts) + 1)
                         .Select(x => x.Select(y => y.item).ToList()).ToList();
        }
    }
}
