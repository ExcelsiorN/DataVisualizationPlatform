using System.ComponentModel;

namespace DataVisualizationPlatform.Models
{
    public class EquipmentInfoModel : INotifyPropertyChanged
    {
        private bool _isFlipped = false;

        public string Equ_Id { get; set; } = string.Empty;
        public string Equ_Name { get; set; } = string.Empty;
        public string Equ_OnlineStatus { get; set; } = string.Empty;
        public string Equ_AvailableBookingPeriod { get; set; } = string.Empty;
        public string Equ_TotalOperationTime { get; set; } = string.Empty;
        public string Equ_FixedDurationThisYear { get; set; } = string.Empty;
        public string Equ_UsedFixedDurationThisYear { get; set; } = string.Empty;
        public string Equ_UsageRateThisYear { get; set; } = string.Empty;
        public string Equ_DeploymentAddress { get; set; } = string.Empty;

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

        // 计算使用率百分比（用于饼图）
        public double UsageRatePercentage
        {
            get
            {
                if (double.TryParse(Equ_UsageRateThisYear.Replace("%", ""), out double rate))
                {
                    return rate;
                }
                return 0;
            }
        }

        // 计算已用时长百分比（用于饼图）
        public double UsedDurationPercentage
        {
            get
            {
                // 去除“小时”并尝试转换为 double
                string ?usedStr = Equ_UsedFixedDurationThisYear?.Replace("小时", "");
                string ?totalStr = Equ_FixedDurationThisYear?.Replace("小时", "");

                if (double.TryParse(usedStr, out double used) &&
                    double.TryParse(totalStr, out double total) &&
                    total > 0)
                {
                    return (used / total) * 100;
                }

                return 0;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}