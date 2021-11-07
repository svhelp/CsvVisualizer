using System.Collections.Generic;

namespace CsvVisualizer.Models
{
    /// <summary>
    /// Chart data model.
    /// </summary>
    class ChartData
    {
        /// <summary>
        /// Chart title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// X axis name.
        /// </summary>
        public string XName { get; set; }

        /// <summary>
        /// Y axis name.
        /// </summary>
        public string YName { get; set; }

        /// <summary>
        /// Chart series data.
        /// </summary>
        public List<ChartPointData> Series { get; set; }
    }
}
