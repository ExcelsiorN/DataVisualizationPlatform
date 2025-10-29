using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataVisualizationPlatform.Models
{
    /// <summary>
    /// 设备统计数据模型
    /// </summary>
    public class EquipmentStatistics : INotifyPropertyChanged
    {
        private string _equipmentId;
        private int _totalOrders;
        private double _fixedDurationRatio;
        private double _faultRate;
        private string _trendIndicator;
        private string _trendColor;

        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentId
        {
            get => _equipmentId;
            set => SetProperty(ref _equipmentId, value);
        }

        /// <summary>
        /// 累计预约单数量
        /// </summary>
        public int TotalOrders
        {
            get => _totalOrders;
            set => SetProperty(ref _totalOrders, value);
        }

        /// <summary>
        /// 固定时长占比（百分比）
        /// </summary>
        public double FixedDurationRatio
        {
            get => _fixedDurationRatio;
            set => SetProperty(ref _fixedDurationRatio, value);
        }

        /// <summary>
        /// 故障率（百分比）
        /// </summary>
        public double FaultRate
        {
            get => _faultRate;
            set => SetProperty(ref _faultRate, value);
        }

        /// <summary>
        /// 趋势指标文本（如"↗ 12%"）
        /// </summary>
        public string TrendIndicator
        {
            get => _trendIndicator;
            set => SetProperty(ref _trendIndicator, value);
        }

        /// <summary>
        /// 趋势颜色（用于绑定）
        /// </summary>
        public string TrendColor
        {
            get => _trendColor;
            set => SetProperty(ref _trendColor, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}