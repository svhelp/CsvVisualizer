using CsvVisualizer.Core;
using CsvVisualizer.Models;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CsvVisualizer
{
    class MainVindowViewModel
    {
        private string csvPath;
        private ObservableCollection<string> headers;
        private string selectedHeader;
        private ObservableCollection<ChartData> charts;

        public string CsvPath
        {
            get => csvPath; set
            {
                csvPath = value;
                OnPropertyChanged();
                OnCsvPathChanged();
            }
        }

        public ObservableCollection<string> Headers
        {
            get => headers ?? (headers = new ObservableCollection<string>()); set
            {
                headers = value;
                OnPropertyChanged();
            }
        }

        public string SelectedHeader
        {
            get => selectedHeader; set
            {
                selectedHeader = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ChartData> Charts
        {
            get => charts ?? (charts = new ObservableCollection<ChartData>()); set
            {
                charts = value;
                OnPropertyChanged();
            }
        }

        private List<List<string>> CsvData { get; set; }

        private void OnCsvPathChanged()
        {
            CsvData = new List<List<string>>();

            var csvData = CsvProcessor.GetCsvData(CsvPath);

            Headers.Clear();
            Charts.Clear();

            foreach (DataColumn column in csvData.Columns)
            {
                Headers.Add(column.ColumnName);

                var columnData = new List<string>();

                foreach (DataRow row in csvData.Rows)
                {
                    columnData.Add(row[column].ToString());
                }

                CsvData.Add(columnData);
            }

            SelectedHeader = Headers[0];

            Charts.Clear();

            foreach (var header in Headers)
            {
                if (header == SelectedHeader)
                {
                    continue;
                }

                var chartModel = GetChartModel(header);

                Charts.Add(chartModel);
            }
        }

        private ChartData GetChartModel(string targetHeader)
        {
            int timeIndex = Headers.IndexOf(SelectedHeader);
            int dataIndex = Headers.IndexOf(targetHeader);
            int pointsCount = CsvData[0].Count;

            var chartPoints = new List<ChartPointData>();

            for (int i = 0; i < pointsCount - 1; i++)
            {
                var point = new ChartPointData
                {
                    Time = CsvData[timeIndex][i],
                    Value = double.Parse(CsvData[dataIndex][i], NumberStyles.Any, CultureInfo.InvariantCulture),
                };

                chartPoints.Add(point);
            }

            return new ChartData
            {
                Title = targetHeader,
                XName = SelectedHeader,
                YName = targetHeader,
                Series = chartPoints,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
