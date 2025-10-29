using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// HomePageB.xaml 的交互逻辑
    /// </summary>
    public partial class HomePageB : Page
    {
        public HomePageB(HomePageBViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
