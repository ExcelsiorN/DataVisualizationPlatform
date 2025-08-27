using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// FaultReport.xaml 的交互逻辑
    /// </summary>
    public partial class FaultReport : Page
    {
        public FaultReport()
        {
            InitializeComponent();
            this.DataContext = new FaultReportViewModel();
        }
    }
}
