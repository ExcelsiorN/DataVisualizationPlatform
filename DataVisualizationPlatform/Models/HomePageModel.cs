using DataVisualizationPlatform.Services;
using System.ComponentModel;


namespace DataVisualizationPlatform.Models
{
    public class HomePageModel : INotifyPropertyChanged
    {
        public string Res_ID { get; set; } = string.Empty;
        public string Res_OrderID { get; set; } = string.Empty;
        public string Res_Equipment { get; set; } = string.Empty;
        public string Res_Date { get; set; } = string.Empty;
        public string Res_Start { get; set; } = string.Empty;
        public string Res_End { get; set; } = string.Empty;
        public string Res_PaymentStatus { get; set; } = string.Empty;
        public string Res_PaymentInfo { get; set; } = string.Empty;
        public string Res_Status { get; set; } = string.Empty;
        public string Res_Detail { get; set; } = string.Empty;
        public string Res_StartDate { get; set; } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
