using CsvHelper;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace CsvVisualizer.Core
{
    /// <summary>
    /// Csv file processor.
    /// </summary>
    class CsvProcessor
    {
        private static readonly Regex CsvPathPattern = new Regex(@".+\.csv$", RegexOptions.Compiled);

        /// <summary>
        /// Try get DataTable from csv file.
        /// </summary>
        /// <param name="csvPath">Csv file path.</param>
        /// <param name="dataTable">DataTable with csv data.</param>
        /// <returns>Is parsing successful.</returns>
        public static bool TryGetCsvData(string csvPath, Collection<string> errorsCollector, out DataTable dataTable)
        {
            dataTable = null;

            if (!CsvPathPattern.IsMatch(csvPath))
            {
                errorsCollector.Add("Provided path is not valid.");
                return false;
            }

            if (!File.Exists(csvPath))
            {
                errorsCollector.Add("File does not exist.");
                return false;
            }

            try
            {
                dataTable = GetCsvData(csvPath);
            }
            catch (Exception e)
            {
                errorsCollector.Add(e.Message);
                return false;
            }

            if (dataTable.Rows.Count == 0)
            {
                errorsCollector.Add("File does not contain rows.");
                return false;
            }

            if (dataTable.Columns.Count == 0)
            {
                errorsCollector.Add("File does not contain columns.");
                return false;
            }

            return true;
        }

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
