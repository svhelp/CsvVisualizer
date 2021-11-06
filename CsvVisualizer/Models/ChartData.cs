using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvVisualizer.Models
{
    class ChartData
    {
        public string Title { get; set; }

        public string XName { get; set; }

        public string YName { get; set; }

        public List<ChartPointData> Series { get; set; }
    }
}
