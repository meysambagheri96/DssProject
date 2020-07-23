namespace DssMultipleCriteriaAnalysis
{
    public class Edge
    {
        public int Source { get; set; }
        public int Target { get; set; }
    }

    public class ExchangeRow
    {
        //مسیر
        public Edge Edge { get; set; }
        
        //معیار اول: صادرات
        public decimal ExportWeight { get; set; }

        //معیار دوم: واردات
        public decimal ImportWeight { get; set; }

    }
}