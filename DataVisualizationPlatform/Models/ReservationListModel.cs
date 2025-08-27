using System.ComponentModel;

namespace DataVisualizationPlatform.Models
{
    public class ReservationListModel : INotifyPropertyChanged
    {
        private bool _isFlipped = false;
        public int Res_ID { get; set; }
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

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                if (_isFlipped != value)
                {
                    _isFlipped = value;
                    OnPropertyChanged(nameof(IsFlipped));
                }
            }
        }

        private bool _isContentFlipped = false;

        public bool IsContentFlipped
        {
            get => _isContentFlipped;
            set
            {
                if (_isContentFlipped != value)
                {
                    _isContentFlipped = value;
                    OnPropertyChanged(nameof(IsContentFlipped));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}