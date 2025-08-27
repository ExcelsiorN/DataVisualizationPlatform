using DataVisualizationPlatform.ViewModels;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Views
{
    public partial class Data : Page
    {
        public Data()
        {
            InitializeComponent();
            this.DataContext = new DataViewModel();

        }
    }
}