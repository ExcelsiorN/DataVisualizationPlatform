using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// HomePageA.xaml 的交互逻辑
    /// </summary>
    public partial class HomePageA : Page
    {
        private readonly HomePageAViewModel _viewModel;

        public HomePageA(HomePageAViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;

            // 页面加载时调用ViewModel的OnLoaded
            Loaded += (s, e) => _viewModel.OnLoaded();
            Unloaded += (s, e) => _viewModel.OnUnloaded();
        }
    }
}
