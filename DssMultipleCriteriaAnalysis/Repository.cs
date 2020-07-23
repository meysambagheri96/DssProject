using System.Collections.Generic;
using System.Linq;

namespace DssMultipleCriteriaAnalysis
{
    public static class Repository
    {
        public static List<Exchanges> Exchanges { get; set; }
        public static List<Final> FinalWieghts { get; set; }
        public static List<Country> Countries { get; set; }
        static Repository()
        {
            var database = new DssDatasetContext();
            Countries = database.Country.ToList();
            FinalWieghts = database.Final.ToList();
            Exchanges = database.Exchanges.ToList();
        }
    }
}
