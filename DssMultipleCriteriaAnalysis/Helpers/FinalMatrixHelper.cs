using System;
using System.Collections.Generic;
using System.Linq;

namespace DssMultipleCriteriaAnalysis
{
    public static class FinalMatrixHelper
    {
        public static int Dimension { get; set; }
        public static double[,] CreateMatrixDecimalMxM()
        {
            var countries = Repository.Countries.OrderBy(x => x.Id).ToList();
            List<Final> data = GetFinalValidData(countries);

            int zeroCount = 0;
            var matrix = new double[205, 205];

            for (int i = 0; i < data.Count(); i++)
            {
                try
                {
                    var from = countries.IndexOf(countries.FirstOrDefault(x => x.Id == (int)data[i].FromCountry));
                    var to = countries.IndexOf(countries.FirstOrDefault(x => x.Id == (int)data[i].ToCountry));
                    if (data[i].Weight == 0)
                        zeroCount++;
                    matrix[from, to] = Convert.ToDouble(data[i].Weight);
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return matrix;

        }

        public static void GetCustomMatrix()
        {
            var countries = Repository.Countries.OrderBy(x => x.Id).ToList();
            List<Final> data = GetFinalValidData(countries);
            var matrix = new double[205, 205];
            int zeroCount = 0;

            for (int i = 0; i < 205; i++)
            {
                for (int j = 0; j < 205; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                        continue;
                    }
                    var source = countries[i];
                    var target = countries[j];
                    var value = data.FirstOrDefault(x => x.FromCountry == source.Id && x.ToCountry == target.Id);
                    if (value == null)
                    {
                        zeroCount++;
                    }
                    else
                    {
                        var val = Convert.ToDouble(value.Weight);
                        if (val == 0) zeroCount++;
                        matrix[i, j] = val;
                    }
                }
            }

            var list = data.Select(x=>x.FromCountry).ToList();
            list.AddRange(data.Select(x => x.ToCountry).ToList());
            list = list.Distinct().ToList();

            var notFoundCountries = new List<int>();
            foreach (var item in list)
            {
                if (countries.Any(x => x.Id == item.Value))
                {
                    notFoundCountries.Add(item.Value);
                }
            }
            var doubles = data.Select(x => Convert.ToDouble(x.Weight)).ToArray();   //10045

            var m = 205; //اندازه ماتریس تصمیم  205

            var result = ConvertMatrix(doubles, m, m);
            for (int i = 0; i < 205; i++)
            {
                for (int j = 0; j < 205; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                        continue;
                    }

                    var value = data.Skip(i * j).FirstOrDefault();
                    if (value != null)
                    {
                        matrix[i, j] = (double)value.Weight;
                    }
                }
            }
        }
        public static double[,] CreateMatrixDecimalMxM2()
        {
            var countries = Repository.Countries.OrderBy(x => x.Id).ToList();
            List<Exchanges> data = Repository.Exchanges.ToList();

            data = data.Where(x =>
                         countries.Select(z => z.Id).Contains(x.FromCountry) &&
                         countries.Select(z => z.Id).Contains(x.ToCountry)).ToList();

            int zeroCount = 0;
            var matrix = new double[205, 205];

            for (int i = 0; i < data.Count(); i++)
            {
                try
                {
                    var from = countries.IndexOf(countries.FirstOrDefault(x => x.Id == (int)data[i].FromCountry));
                    var to = countries.IndexOf(countries.FirstOrDefault(x => x.Id == (int)data[i].ToCountry));
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
            return matrix;
        }

        public static List<Final> GetFinalValidData(List<Country> countries)
        {
            var data = Repository.FinalWieghts.OrderBy(x => x.FromCountry).ToList();
            var except = new List<Final>();
            foreach (var item in data)
            {
                if (!countries.Any(x => x.Id == (int)item.FromCountry) ||
                    !countries.Any(x => x.Id == (int)item.ToCountry))
                {
                    except.Add(item);
                }
            }
            foreach (var item in except)
            {
                data.Remove(item);
            }

            return data;
        }

        static double[,] ConvertMatrix(double[] flat, int m, int n)
        {
            if (flat.Length != m * n)
            {
                throw new ArgumentException("Invalid length");
            }
            double[,] ret = new double[m, n];
            // BlockCopy uses byte lengths: a double is 8 bytes
            Buffer.BlockCopy(flat, 0, ret, 0, flat.Length * sizeof(double));
            return ret;
        }
        public static double[,] CreateMatrixDoubleMxM()
        {
            var countries = Repository.Countries.ToList();
            var data = Repository.FinalWieghts.ToList();

            var m = data.Count(); //اندازه ماتریس تصمیم
            Dimension = m;
            double[,] matrix = new double[m, m];

            for (int i = 0; i < countries.Count(); i++)
            {
                for (int j = 0; j < countries.Count; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                        continue;
                    }

                    var value = data.FirstOrDefault(x =>
                        x.FromCountry == countries[i].Id && x.ToCountry == countries[j].Id);
                    if (value != null)
                    {
                        matrix[i, j] = Convert.ToDouble(value.Weight);
                    }
                }
            }
            return matrix;
        }

        public static decimal[,] CreateMx2()
        {
            var data = Repository.Exchanges.ToList();
            var grouping = data.GroupBy(x => x.FromCountry).Select(x => new
            {
                key = x.Key,
                exportSum = x.Select(a => a.Export).Sum(),
                importSum = x.Select(a => a.Import).Sum(),
            }).ToList();

            var matrix = new decimal[data.Count, 2];

            for (int i = 0; i < grouping.Count; i++)
            {
                matrix[i, 0] = grouping[i].exportSum;
                matrix[i, 1] = grouping[i].importSum;
            }

            return matrix;
        }
    }
}
