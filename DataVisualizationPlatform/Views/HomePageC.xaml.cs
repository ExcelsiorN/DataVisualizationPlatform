using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataVisualizationPlatform.Views
{
    public partial class HomePageC : Page
    {
        private HomePageCViewModel _viewModel;

        public HomePageC()
        {
            InitializeComponent();
            MapControl.DragButton = MouseButton.Left;

            // 创建ViewModel并设置DataContext
            _viewModel = new HomePageCViewModel();
            this.DataContext = _viewModel;

            // 页面加载完成后设置地图控件引用
            this.Loaded += HomePageC_Loaded;
        }

        private void HomePageC_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // 将地图控件引用传递给ViewModel，以便添加标记
            _viewModel.SetMapControl(MapControl);
        }
    }
}