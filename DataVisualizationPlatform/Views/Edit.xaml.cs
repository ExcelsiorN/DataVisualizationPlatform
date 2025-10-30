using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : Page
    {
        public Edit(EditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
