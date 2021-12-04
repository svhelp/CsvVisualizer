using CsvVisualizer.Core;
using CsvVisualizer.Core.Commands;
using CsvVisualizer.Models;
using CsvVisualizer.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CsvVisualizer
{
    /// <summary>
    /// Main window view model.
    /// </summary>
    class MainVindowViewModel : INotifyPropertyChanged
    {
        private string csvPath;
        private CsvHeaderViewModel selectedHeader;
        private ObservableCollection<CsvHeaderViewModel> headers;
        private ObservableCollection<ChartData> charts;
        private ObservableCollection<string> errors;
        private RelayCommand drawCharts;
        private RelayCommand hideError;

        /// <summary>
        /// Path to csv file.
        /// </summary>
        public string CsvPath
        {
            get => csvPath; set
            {
                csvPath = value;
                OnPropertyChanged();
                OnCsvPathChanged();
            }
        }

        /// <summary>
        /// Current errors list.
        /// </summary>
        public ObservableCollection<string> Errors
        {
            get => errors ?? (errors = new ObservableCollection<string>()); set
            {
                errors = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Csv headers.
        /// </summary>
        public ObservableCollection<CsvHeaderViewModel> Headers
        {
            get => headers ?? (headers = new ObservableCollection<CsvHeaderViewModel>()); set
            {
                headers = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Csv header considered as 'time'.
        /// </summary>
        public CsvHeaderViewModel SelectedHeader
        {
            get => selectedHeader; set
            {
                selectedHeader = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Chart models.
        /// </summary>
        public ObservableCollection<ChartData> Charts
        {
            get => charts ?? (charts = new ObservableCollection<ChartData>()); set
            {
                charts = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Draw charts command.
        /// </summary>
        public RelayCommand DrawCharts
        {
            get => drawCharts ?? (drawCharts = new RelayCommand(obj =>
            {
                Charts.Clear();

                var headers = Headers.Select(x => x.Value).ToList();

                foreach (var header in Headers.Where(x => x.IsSelected))
                {
                    if (header == SelectedHeader ||
                            !ChartFactory.TryGetChartModel(headers, CsvData, header.Value, SelectedHeader.Value, Errors, out ChartData chartModel))
                    {
                        continue;
                    }

                    Charts.Add(chartModel);
                }
            }));
        }

        /// <summary>
        /// Hide error command.
        /// </summary>
        public RelayCommand HideError
        { 
            get => hideError ?? (hideError = new RelayCommand(obj =>
            {
                var errorToRemove = obj as string;
                errors.Remove(errorToRemove);
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

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On property changed.
        /// </summary>
        /// <param name="prop">Changed prop name.</param>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
