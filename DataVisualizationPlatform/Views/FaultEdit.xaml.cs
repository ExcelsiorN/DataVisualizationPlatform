using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// FaultEdit.xaml 的交互逻辑
    /// </summary>
    public partial class FaultEdit : Page
    {
        public FaultEdit(FaultEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
