using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// TimeSlotEdit.xaml 的交互逻辑
    /// </summary>
    public partial class TimeSlotEdit : Page
    {
        public TimeSlotEdit(TimeSlotEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
