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
        private ObservableCollection<SfChart> charts;
        private ObservableCollection<ChartPointData> singleChart;

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

        public ObservableCollection<SfChart> Charts
        {
            get => charts ?? (charts = new ObservableCollection<SfChart>()); set
            {
                charts = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ChartPointData> SingleChart
        {
            get => singleChart ?? (singleChart = new ObservableCollection<ChartPointData>()); set
            {
                singleChart = value;
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

            foreach (var header in Headers)
            {
                if (header == SelectedHeader || header != Headers[1])
                {
                    continue;
                }

                var chartModel = GetChartModel(header);

                Charts.Add(chartModel);
            }
        }

        private SfChart GetChartModel(string targetHeader)
        {
            int timeIndex = Headers.IndexOf(SelectedHeader);
            int dataIndex = Headers.IndexOf(targetHeader);
            int pointsCount = CsvData[0].Count;

            SingleChart.Clear();
            var points = new List<ChartPointData>();

            for (int i = 0; i < pointsCount - 1; i++)
            {
                var point = new ChartPointData
                {
                    Time = CsvData[timeIndex][i],
                    Value = double.Parse(CsvData[dataIndex][i], NumberStyles.Any, CultureInfo.InvariantCulture),
                };

                points.Add(point);
                SingleChart.Add(point);
            }

            SfChart chart = new SfChart();

            //Adding horizontal axis to the chart 

            CategoryAxis primaryAxis = new CategoryAxis();

            primaryAxis.Header = SelectedHeader;

            chart.PrimaryAxis = primaryAxis;


            //Adding vertical axis to the chart 

            NumericalAxis secondaryAxis = new NumericalAxis();

            secondaryAxis.Header = targetHeader;

            chart.SecondaryAxis = secondaryAxis;


            //Initialize the two series for SfChart
            LineSeries series = new LineSeries();

            series.ItemsSource = points;
            series.XBindingPath = "Time";
            series.YBindingPath = "Value";

            //Adding Series to the Chart Series Collection
            chart.Series.Add(series);

            return chart;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
