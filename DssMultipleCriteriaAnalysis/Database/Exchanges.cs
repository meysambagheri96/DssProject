using System;
using System.Collections.Generic;

namespace DssMultipleCriteriaAnalysis
{
    public partial class Exchanges
    {
        public int? FromCountry { get; set; }
        public int? ToCountry { get; set; }
        public decimal Export { get; set; }
        public decimal Import { get; set; }
    }
}
