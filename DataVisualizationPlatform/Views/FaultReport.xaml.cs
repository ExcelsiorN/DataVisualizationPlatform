using DataVisualizationPlatform.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// FaultReport.xaml 的交互逻辑
    /// </summary>
    public partial class FaultReport : Page
    {
        public FaultReport(FaultReportViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;

            // 订阅事件
            viewModel.RequestScrollToTop += () =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    FaultDisplay.ScrollToVerticalOffset(0);
                }), System.Windows.Threading.DispatcherPriority.Background);
            };
        }
    }
}
