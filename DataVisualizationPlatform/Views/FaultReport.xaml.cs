using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;
using System.Windows;

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
            var vm = new FaultReportViewModel();

            this.DataContext = vm;

            // 订阅事件
            vm.RequestScrollToTop += () =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    FaultDisplay.ScrollToVerticalOffset(0);
                }), System.Windows.Threading.DispatcherPriority.Background);
            };
        }


    }
}
