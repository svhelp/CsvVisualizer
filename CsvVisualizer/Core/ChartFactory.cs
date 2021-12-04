using CsvVisualizer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CsvVisualizer.Core
{
    /// <summary>
    /// Chart model factory.
    /// </summary>
    class ChartFactory
    {
        /// <summary>
        /// Get chart model from csv.
        /// </summary>
        /// <param name="headers">Csv headers.</param>
        /// <param name="csvData">Csv data.</param>
        /// <param name="targetHeader">Header to project on Y axis.</param>
        /// <param name="keyHeader">Header to project on X axis.</param>
        /// <param name="errorsCollector">Errors collector.</param>
        /// <param name="chartModel">Chart model.</param>
        /// <returns>Is successful.</returns>
        public static bool TryGetChartModel(List<string> headers, List<List<string>> csvData, string targetHeader, string keyHeader, Collection<string> errorsCollector, out ChartData chartModel)
        {
            int timeIndex = headers.IndexOf(keyHeader);
            int dataIndex = headers.IndexOf(targetHeader);
            int pointsCount = csvData[0].Count;

            var chartPoints = new List<ChartPointData>();

            for (int i = 0; i < pointsCount - 1; i++)
            {
                if (!double.TryParse(csvData[dataIndex][i], NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
                {
                    errorsCollector.Add($"Encountered non-digital value in {targetHeader} dataset.");

                    chartModel = null;
                    return false;
                }

                var point = new ChartPointData
                {
                    Time = csvData[timeIndex][i],
                    Value = value,
                };

                chartPoints.Add(point);
            }

            chartModel = new ChartData
            {
                Title = targetHeader,
                XName = keyHeader,
                YName = targetHeader,
                Series = chartPoints,
            };

            return true;
        }
    }
}
