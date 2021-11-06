using CsvHelper;
using System.Data;
using System.Globalization;
using System.IO;

namespace CsvVisualizer.Core
{
    class CsvProcessor
    {
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
