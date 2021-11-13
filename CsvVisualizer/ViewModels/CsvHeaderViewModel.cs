namespace CsvVisualizer.ViewModels
{
    /// <summary>
    /// Csv header model.
    /// </summary>
    class CsvHeaderViewModel
    {
        /// <summary>
        /// Header value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Is value selected for chart.
        /// </summary>
        public bool IsSelected { get; set; } = true;
    }
}
