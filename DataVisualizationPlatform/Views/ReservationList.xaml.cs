using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    public partial class ReservationList : Page
    {
        public ReservationList(ReservationListViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
