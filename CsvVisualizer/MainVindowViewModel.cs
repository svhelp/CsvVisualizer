using CsvVisualizer.Core;
using CsvVisualizer.Core.Commands;
using CsvVisualizer.Models;
using CsvVisualizer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CsvVisualizer
{
    class MainVindowViewModel : INotifyPropertyChanged
    {
        private string csvPath;
        private ObservableCollection<CsvHeaderViewModel> headers;
        private CsvHeaderViewModel selectedHeader;
        private ObservableCollection<ChartData> charts;
        private RelayCommand drawCharts;
        private ObservableCollection<string> errors;

        public string CsvPath
        {
            get => csvPath; set
            {
                csvPath = value;
                OnPropertyChanged();
                OnCsvPathChanged();
            }
        }

        public ObservableCollection<string> Errors
        {
            get => errors ?? (errors = new ObservableCollection<string>()); set
            {
                errors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CsvHeaderViewModel> Headers
        {
            get => headers ?? (headers = new ObservableCollection<CsvHeaderViewModel>()); set
            {
                headers = value;
                OnPropertyChanged();
            }
        }

        public CsvHeaderViewModel SelectedHeader
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

        public RelayCommand DrawCharts
        {
            get => drawCharts ?? (drawCharts = new RelayCommand(obj =>
            {
                foreach (var header in Headers.Where(x => x.IsSelected))
                {
                    if (header == SelectedHeader)
                    {
                        continue;
                    }

                    var chartModel = GetChartModel(header);
                    Charts.Add(chartModel);
                }
            }));
        }

        private List<List<string>> CsvData { get; set; }

        private void OnCsvPathChanged()
        {
            CsvData = new List<List<string>>();
            Headers.Clear();
            Charts.Clear();
            Errors.Clear();

            if (!CsvProcessor.TryGetCsvData(CsvPath, Errors, out DataTable csvData))
            {
                return;
            }

            foreach (DataColumn column in csvData.Columns)
            {
                var headerModel = new CsvHeaderViewModel
                {
                    Value = column.ColumnName,
                };

                Headers.Add(headerModel);

                var columnData = new List<string>();

                foreach (DataRow row in csvData.Rows)
                {
                    columnData.Add(row[column].ToString());
                }

                CsvData.Add(columnData);
            }

            SelectedHeader = Headers.First();
        }

        private ChartData GetChartModel(CsvHeaderViewModel targetHeader)
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
                Title = targetHeader.Value,
                XName = SelectedHeader.Value,
                YName = targetHeader.Value,
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
