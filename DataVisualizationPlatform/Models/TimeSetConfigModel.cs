using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataVisualizationPlatform.Models
{
    /// <summary>
    /// 预约时段配置模型
    /// </summary>
    public class TimeSetConfigModel : INotifyPropertyChanged
    {
        private string _setId = string.Empty;
        private ObservableCollection<string> _timeSlots = new();

        /// <summary>
        /// 配置ID
        /// </summary>
        public string Set_Id
        {
            get => _setId;
            set
            {
                if (_setId != value)
                {
                    _setId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// 时段列表
        /// </summary>
        public ObservableCollection<string> TimeSlots
        {
            get => _timeSlots;
            set
            {
                if (_timeSlots != value)
                {
                    _timeSlots = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
