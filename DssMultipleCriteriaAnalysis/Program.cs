using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DssMultipleCriteriaAnalysis
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(SpecialFunctions.Erf(0.5));

            //ساخت ماتریس معیار ها از روی دیتاست
            //var matrix = FinalMatrixHelper.CreateMatrixDecimalMxM2();
            var countries = Repository.Countries.OrderBy(n => n.Id).ToList();
            List<Exchanges> data = Repository.Exchanges.ToList();

            data = data.Where(z =>
                         countries.Select(n => n.Id).Contains(z.FromCountry) &&
                         countries.Select(n => n.Id).Contains(z.ToCountry)).ToList();

            int zeroCount = 0;
            var matrix = new double[205, 205];

            for (int i = 0; i < data.Count(); i++)
            {
                try
                {
                    var from = countries.IndexOf(countries.FirstOrDefault(n => n.Id == (int)data[i].FromCountry));
                    var to = countries.IndexOf(countries.FirstOrDefault(n => n.Id == (int)data[i].ToCountry));
                    if (data[i].Export == 0 || data[i].Import == 0)
                        zeroCount++;
                    matrix[from, to] = Convert.ToDouble(data[i].Export);
                    matrix[to, from] = Convert.ToDouble(-data[i].Import);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            //محاسبه میانگین ستون ها جهت تعیین بردار وزن
            for (int j = 0; j < 205; j++)
            {
                double sumOfColumn = 0;
                for (int z = 0; z < 205; z++)
                {
                    sumOfColumn += matrix[z, j];
                }

                for (int i = 0; i < 205; i++)
                {
                    if (sumOfColumn != 0)
                        matrix[j, i] = matrix[j, i] / sumOfColumn;
                    else
                        matrix[j, i] = 0;
                }
            }

            //محاسبه بردار وزن به روش میانگین سطری
            var weights = new List<double>();

            for (int i = 0; i < 205; i++)
            {
                double sumOfRow = 0;
                for (int j = 0; j < 205; j++)
                {
                    sumOfRow += matrix[i, j];
                }
                weights.Add(sumOfRow / 205);
            }

            //ذخیره بردار وزن در فایل
            using (StreamWriter file = new StreamWriter("C:\\Users\\Meysam\\Desktop\\weights.txt"))
            {
                for (int i = 0; i < weights.Count; i++)
                {
                    if (weights[i] == 0)
                        weights[i] = 0.00001;
                    file.Write(weights[i] + ",");
                }
            }

            //ذخیره ماتریس مبادلات بین کشور ها در فایل
            var m = Matrix<double>.Build.SparseOfArray(matrix);
            var columns = m.AsColumnArrays().ToList();

            using (StreamWriter file = new StreamWriter("C:\\Users\\Meysam\\Desktop\\matrix.txt"))
            {
                for (int i = 0; i < 205; i++)
                {
                    for (int j = 0; j < 205; j++)
                    {
                        file.Write(matrix[i, j] + ",");
                    }
                    file.Write(Environment.NewLine);
                }
            }
            Console.ReadLine();

            GetWeightsDescriptionForFillingMatrix();

            for (int i = 0; i < 10045; i++)
            {
                for (int j = 0; j < 10045; j++)
                {
                    if (matrix[i, j] == 0)
                        zeroCount++;
                }
            }
            var result = matrix.To1DArray().Split<double>(8);
            var averageWeightsForVikor = new List<double>();
            foreach (var item in result)
            {
                var averageItem = item.ToList().Average();
                averageWeightsForVikor.Add(averageItem);
            }

            var x = Matrix<double>.Build.SparseOfArray(matrix).ToArray();
            Console.ReadLine();
        }

        private static List<double> GetWeightsDescriptionForFillingMatrix()
        {
            var allWeights = FinalMatrixHelper.GetFinalValidData(Repository.Countries).Select(x => x.Weight).ToArray();
            var avgList = ArrayHelper.Split(allWeights.AsEnumerable(), 8).ToList();
            var avgRes = new List<double>();
            foreach (var item in avgList)
            {
                avgRes.Add(Convert.ToDouble(item.Average()));
            }
            avgRes = avgRes.OrderBy(x => x).ToList();
            return avgRes;
        }
    }
}
