using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quiz.ViewModel
{
    public class ResponseChartViewModel
    {
        public int totalMark { get; set; }
        public string name { get; set; }
        public List<ChartView> Charts { get; set; } = new List<ChartView>();
    }
}