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
            var countriesData = Repository.Countries.ToList();
         
            var Ri = new List<int>()
                { 1, 131, 132, 133 ,134, 135 ,136, 137 ,138 ,139, 140, 141, 142, 143 ,144, 145, 146, 147,
                  148, 149, 150, 151 ,130, 129 ,128, 127 ,105 ,106, 107, 108, 109, 110 ,111, 112, 113, 114,
                  152, 115, 117, 118 ,119, 120 ,121, 122 ,123 ,124, 125, 126, 116, 153 ,154, 155, 182, 183,
                  184, 185, 186, 187 ,188, 189 ,190, 191 ,181 ,192, 194, 195, 196, 197 ,198, 199, 200, 201,
                  202, 203, 193, 104 ,180, 178 ,156, 157 ,158 ,159, 160, 161, 162, 163 ,164, 165, 179, 166,
                  168, 169, 170, 171 ,172, 173 ,174, 175 ,176 ,177, 167, 204, 103, 101 , 28,  29,  30,  31,
                  32,  33,  34,  35 , 36,  37 , 38,  39 , 40 , 41,  42,  43,  44,  45 , 46,  47,  48,  27,
                  26,  25,  24,   2 ,  3,   4 ,  5,   6 ,  7 ,  8,   9,  10,  11,  49 , 12,  14,  15,  16,
                  17,  18,  19,  20 , 21,  22 , 23,  13 , 50 , 51,  52,  79,  80,  81 , 82,  83,  84,  85,
                  86,  87,  88,  78 , 89,  91 , 92,  93 , 94 , 95,  96,  97,  98,  99 ,100,  90, 102,  77,
                  75,  53,  54,  55 , 56,  57 , 58,  59 , 60 , 61,  62,  76,  63,  65 , 66,  67,  68,  69,
                  70,  71,  72,  73 , 74,  64 ,205 };
            var Si = new List<int>()
            {
                1, 131 ,132, 133, 134 ,135, 136, 137 ,138, 139 ,140, 141, 142 ,143 ,144 ,145, 146, 147,
              148, 149 ,150, 151, 130 ,129, 128, 127 ,105, 106 ,107, 108, 109 ,110 ,111 ,112, 113, 114,
              152, 115 ,117, 118, 119 ,120, 121, 122 ,123, 124 ,125, 126, 116 ,153 ,154 ,155, 182, 183,
              184, 185 ,186, 187, 188 ,189, 190, 191 ,181, 192 ,194, 195, 196 ,197 ,198 ,199, 200, 201,
              202, 203 ,193, 104, 180 ,178, 156, 157 ,158, 159 ,160, 161, 162 ,163 ,164 ,165, 179, 166,
              168, 169 ,170, 171, 172 ,173, 174, 175 ,176, 177 ,167, 204, 103 ,101 , 28 , 29,  30,  31,
               32,  33 , 34,  35,  36 , 37,  38,  39 , 40,  41 , 42,  43,  44 , 45 , 46 , 47,  48,  27,
               26,  25 , 24,   2,   3 ,  4,   5,   6 ,  7,   8 ,  9,  10,  11 , 49 , 12 , 14,  15,  16,
               17,  18 , 19,  20,  21 , 22,  23,  13 , 50,  51 , 52,  79,  80 , 81 , 82 , 83,  84,  85,
               86,  87 , 88,  78,  89 , 91,  92,  93 , 94,  95 , 96,  97,  98 , 99 ,100 , 90, 102,  77,
               75,  53 , 54,  55,  56 , 57,  58,  59 , 60,  61 , 62,  76,  63 , 65 , 66 , 67,  68,  69,
               70,  71 , 72,  73,  74 , 64, 205};

            var QResult = new List<(int index, double Qi)>();

            ///Qi ساختن بردار 
            ///Qk بر اساس فرمول 
            for (int i = 0; i < Si.Count(); i++)
            {
                try
                {
                    var q =
                     ((Si[i] - Si.Min()) / (Si.Max() - Si[i]) * 0.5) +
                     ((Ri.Min() - Ri[i] / Ri.Max() - Ri[i]) * (1 - 0.5));
                    QResult.Add((i, q));
                }
                catch (Exception ex)
                {
                    QResult.Add((i, 0));
                    continue;
                }
            }

            var orderedQi = QResult.OrderBy(z => z.Qi).ToList();
            int bestItem1Index = 0;
            int bestItem2Index = 0;
            var bestItems = new List<int>();

            for (int i = 0; i < QResult.Count(); i++)
            {
                bool condition1 = false;
                bool condition2 = false;
                double lastIndex = i;

                var qA1 = orderedQi[i];
                var qA2 = orderedQi[i + 1];

                ///چک کردن شرط اول: مزیت قابل قبول 
                if ((qA2.index - qA1.Qi) > (1 / (QResult.Count() - 1)))
                {
                    condition1 = true;
                    Console.WriteLine($"Q(A2) - Q(A1) > (1/(m-1)) => Q(A2)={qA2}");
                }

                ///چک کردن شرط دوم: ثبات قابل قبول در تصمیم گیری
                if ((Si[qA1.index] <= Si.Min()) ||
                    Ri[qA1.index] <= Ri.Min())
                {
                    condition2 = true;
                    Console.WriteLine($"Q(A1) is min value in Si or Ri");
                }

                //اگر یکی از شرط ها برقرار نباشد
                if ((condition1 == true && condition2 == false) ||
                    (condition1 == false && condition2 == true))
                {
                    //اگر شرط اول برقرار باشد و شرط دوم برقرار نباشد
                    if (condition2 == false && condition1 == true)
                    {
                        //گزینه اول و دوم پاسخ ما خواهند بود 
                        bestItem1Index = qA1.index;
                        bestItem2Index = qA2.index;
                        break;
                    }

                    //اگر شرط دوم برقرار باشد و شرط اول برقرار نباشد
                    //ممکن است چندین رتبه اول داشته باشیم
                    if (condition1 == false && condition2 == true)
                    {
                        //لیستی از جواب های رتبه برتر
                        var localBestIndexs = new List<int>();
                        for (int b = 0; b < orderedQi.Count(); b++)
                        {
                            if (QResult[b].Qi - qA1.Qi < 1 / (orderedQi.Count() - 1))
                            {
                                localBestIndexs.Add(QResult[b].index);
                            }
                        }
                        bestItem1Index = qA1.index;
                        bestItems = localBestIndexs;
                        break;
                    }
                    break;
                }
            }

            Console.WriteLine($"index 1: {bestItem1Index.ToString()} ," +
                $"country name: { countriesData[bestItem1Index].CountryName}");

            Console.WriteLine($"index 2: {bestItem2Index.ToString()} ," +
                $"country name: { countriesData[bestItem2Index].CountryName}");

            foreach (var item in bestItems)
            {
                try
                {
                    Console.WriteLine($"index: {item.ToString()}, " +
                        $"country name: {countriesData[item].CountryName}");
                }
                catch (Exception)
                {

                }
            }

            Console.ReadLine();
        }

        public static void MatrixAndWeight()
        {

            #region Matrix and weight
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
            #endregion
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
