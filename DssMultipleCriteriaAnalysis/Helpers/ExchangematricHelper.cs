using System.Collections.Generic;
using System.Linq;

namespace DssMultipleCriteriaAnalysis
{
    public static class ExchangematricHelper
    {
        public static void CreateMatrix()
        {
            var exchangesData = Repository.Exchanges.ToList();

            var matrixObject = new List<ExchangeRow>();
            foreach (var exchange in exchangesData)
            {
                matrixObject.Add(new ExchangeRow
                {
                    Edge = new Edge { Source = exchange.FromCountry ?? 0, Target = exchange.ToCountry ?? 0 },
                    ExportWeight = exchange.Export,
                    ImportWeight = exchange.Import,
                });
            }
            var zero = matrixObject.Where(x => x.ExportWeight == 0 || x.ImportWeight == 0).ToList();

            var m = matrixObject.Count(); //اندازه ماتریس تصمیم
            decimal[,] matrix = new decimal[m, m];

            int zeroCount = 0;
            for (int i = 0; i < matrixObject.Count(); i++)
            {
                matrix[i, 0] = matrixObject[i].ExportWeight;
                matrix[i, 1] = matrixObject[i].ImportWeight;

                if (matrix[i, 0] == 0 || matrix[i, 1] == 0)
                {
                    zeroCount++;
                }
            }
        }
    }
}
