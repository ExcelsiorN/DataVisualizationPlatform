using System.ComponentModel;

namespace DataVisualizationPlatform.Models
{
    public class EquipmentInfoModel : INotifyPropertyChanged
    {
        private bool _isFlipped = false;
        private string _equ_Id = string.Empty;
        private string _equ_Name = string.Empty;
        private string _equ_OnlineStatus = string.Empty;
        private string _equ_AvailableBookingPeriod = string.Empty;
        private string _equ_TotalOperationTime = string.Empty;
        private string _equ_FixedDurationThisYear = string.Empty;
        private string _equ_UsedFixedDurationThisYear = string.Empty;
        private string _equ_UsageRateThisYear = string.Empty;
        private string _equ_DeploymentAddress = string.Empty;

        public string Equ_Id
        {
            get => _equ_Id;
            set
            {
                if (_equ_Id != value)
                {
                    _equ_Id = value;
                    OnPropertyChanged(nameof(Equ_Id));
                }
            }
        }

        public string Equ_Name
        {
            get => _equ_Name;
            set
            {
                if (_equ_Name != value)
                {
                    _equ_Name = value;
                    OnPropertyChanged(nameof(Equ_Name));
                }
            }
        }

        public string Equ_OnlineStatus
        {
            get => _equ_OnlineStatus;
            set
            {
                if (_equ_OnlineStatus != value)
                {
                    _equ_OnlineStatus = value;
                    OnPropertyChanged(nameof(Equ_OnlineStatus));
                }
            }
        }

        public string Equ_AvailableBookingPeriod
        {
            get => _equ_AvailableBookingPeriod;
            set
            {
                if (_equ_AvailableBookingPeriod != value)
                {
                    _equ_AvailableBookingPeriod = value;
                    OnPropertyChanged(nameof(Equ_AvailableBookingPeriod));
                }
            }
        }

        public string Equ_TotalOperationTime
        {
            get => _equ_TotalOperationTime;
            set
            {
                if (_equ_TotalOperationTime != value)
                {
                    _equ_TotalOperationTime = value;
                    OnPropertyChanged(nameof(Equ_TotalOperationTime));
                }
            }
        }

        public string Equ_FixedDurationThisYear
        {
            get => _equ_FixedDurationThisYear;
            set
            {
                if (_equ_FixedDurationThisYear != value)
                {
                    _equ_FixedDurationThisYear = value;
                    OnPropertyChanged(nameof(Equ_FixedDurationThisYear));
                }
            }
        }

        public string Equ_UsedFixedDurationThisYear
        {
            get => _equ_UsedFixedDurationThisYear;
            set
            {
                if (_equ_UsedFixedDurationThisYear != value)
                {
                    _equ_UsedFixedDurationThisYear = value;
                    OnPropertyChanged(nameof(Equ_UsedFixedDurationThisYear));
                }
            }
        }

        public string Equ_UsageRateThisYear
        {
            get => _equ_UsageRateThisYear;
            set
            {
                if (_equ_UsageRateThisYear != value)
                {
                    _equ_UsageRateThisYear = value;
                    OnPropertyChanged(nameof(Equ_UsageRateThisYear));
                }
            }
        }

        public string Equ_DeploymentAddress
        {
            get => _equ_DeploymentAddress;
            set
            {
                if (_equ_DeploymentAddress != value)
                {
                    _equ_DeploymentAddress = value;
                    OnPropertyChanged(nameof(Equ_DeploymentAddress));
                }
            }
        }

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

        /// <summary>
        /// 创建当前对象的深拷贝
        /// </summary>
        public EquipmentInfoModel Clone()
        {
            return new EquipmentInfoModel
            {
                Equ_Id = this.Equ_Id,
                Equ_Name = this.Equ_Name,
                Equ_OnlineStatus = this.Equ_OnlineStatus,
                Equ_AvailableBookingPeriod = this.Equ_AvailableBookingPeriod,
                Equ_TotalOperationTime = this.Equ_TotalOperationTime,
                Equ_FixedDurationThisYear = this.Equ_FixedDurationThisYear,
                Equ_UsedFixedDurationThisYear = this.Equ_UsedFixedDurationThisYear,
                Equ_UsageRateThisYear = this.Equ_UsageRateThisYear,
                Equ_DeploymentAddress = this.Equ_DeploymentAddress
            };
        }

        /// <summary>
        /// 从另一个对象复制所有属性值
        /// </summary>
        public void CopyFrom(EquipmentInfoModel source)
        {
            this.Equ_Id = source.Equ_Id;
            this.Equ_Name = source.Equ_Name;
            this.Equ_OnlineStatus = source.Equ_OnlineStatus;
            this.Equ_AvailableBookingPeriod = source.Equ_AvailableBookingPeriod;
            this.Equ_TotalOperationTime = source.Equ_TotalOperationTime;
            this.Equ_FixedDurationThisYear = source.Equ_FixedDurationThisYear;
            this.Equ_UsedFixedDurationThisYear = source.Equ_UsedFixedDurationThisYear;
            this.Equ_UsageRateThisYear = source.Equ_UsageRateThisYear;
            this.Equ_DeploymentAddress = source.Equ_DeploymentAddress;
        }
    }
}