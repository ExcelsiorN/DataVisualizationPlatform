using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    public partial class Data : Page
    {
        public Data(DataViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
