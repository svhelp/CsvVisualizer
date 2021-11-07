using CsvHelper;
using System.Data;
using System.Globalization;
using System.IO;

namespace CsvVisualizer.Core
{
    /// <summary>
    /// Csv file processor.
    /// </summary>
    class CsvProcessor
    {
        /// <summary>
        /// Get DataTable from csv file.
        /// </summary>
        /// <param name="csvPath">Csv file path.</param>
        /// <returns>DataTable with csv data.</returns>
        public static DataTable GetCsvData(string csvPath)
        {
            using var reader = new StreamReader(csvPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            // Do any configuration to `CsvReader` before creating CsvDataReader.
            using var dr = new CsvDataReader(csv);

            var dt = new DataTable();
            dt.Load(dr);

            return dt;
        }
    }
}
