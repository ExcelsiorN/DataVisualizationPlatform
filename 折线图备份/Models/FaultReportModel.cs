using System.ComponentModel;

namespace DataVisualizationPlatform.Models
{
    public class FaultReportModel : INotifyPropertyChanged
    {
        public string Equ_Id { get; set; } = string.Empty;
        public string Equ_Name { get; set; } = string.Empty;
        public string Equ_OnlineStatus { get; set; } = string.Empty;
        public string Equ_AvailableBookingPeriod { get; set; } = string.Empty;
        public string Equ_TotalOperationTime { get; set; } = string.Empty;
        public string Equ_FixedDurationThisYear { get; set; } = string.Empty;
        public string Equ_UsedFixedDurationThisYear { get; set; } = string.Empty;
        public string Equ_UsageRateThisYear { get; set; } = string.Empty;
        public string Equ_DeploymentAddress { get; set; } = string.Empty;

        private bool _isFlipped;
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

        private bool _isExpanded = false;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged(nameof(IsExpanded));
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