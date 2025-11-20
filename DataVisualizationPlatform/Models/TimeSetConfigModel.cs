using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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

        public TimeSetConfigModel()
        {
            // 监听集合变化，确保UI能够更新
            _timeSlots.CollectionChanged += TimeSlots_CollectionChanged;
        }

        private void TimeSlots_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // 当集合内容变化时，通知UI TimeSlots属性已改变
            OnPropertyChanged(nameof(TimeSlots));
        }

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
                    // 取消订阅旧集合的事件
                    if (_timeSlots != null)
                    {
                        _timeSlots.CollectionChanged -= TimeSlots_CollectionChanged;
                    }

                    _timeSlots = value;

                    // 订阅新集合的事件
                    if (_timeSlots != null)
                    {
                        _timeSlots.CollectionChanged += TimeSlots_CollectionChanged;
                    }

                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 创建当前对象的深拷贝
        /// </summary>
        public TimeSetConfigModel Clone()
        {
            var clone = new TimeSetConfigModel
            {
                Set_Id = this.Set_Id
            };

            // 深拷贝TimeSlots集合
            foreach (var slot in this.TimeSlots)
            {
                clone.TimeSlots.Add(slot);
            }

            return clone;
        }

        /// <summary>
        /// 从另一个对象复制所有属性值
        /// </summary>
        public void CopyFrom(TimeSetConfigModel source)
        {
            this.Set_Id = source.Set_Id;

            // 清空并复制TimeSlots
            this.TimeSlots.Clear();
            foreach (var slot in source.TimeSlots)
            {
                this.TimeSlots.Add(slot);
            }
        }
    }
}
