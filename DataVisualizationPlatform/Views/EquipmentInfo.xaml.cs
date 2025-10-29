using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    public partial class EquipmentInfo : Page
    {
        public EquipmentInfo(EquipmentInfoViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
