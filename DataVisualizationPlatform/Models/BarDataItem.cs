using System.ComponentModel;

namespace DataVisualizationPlatform.Models
{
    public class BarDataItem : INotifyPropertyChanged
    {
        private string _label = string.Empty;
        private double _part1;
        private double _part2;
        private string _falData = string.Empty;
        private string _falEqid = string.Empty;
        private int _falId;
        private string _falType = string.Empty;
        private string _falInfo = string.Empty;
        private string _falDetail = string.Empty;
        private string _falRemark = string.Empty;

        public string Label
        {
            get => _label;
            set
            {
                if (_label != value)
                {
                    _label = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        public double Part1
        {
            get => _part1;
            set
            {
                if (_part1 != value)
                {
                    _part1 = value;
                    OnPropertyChanged(nameof(Part1));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Part2
        {
            get => _part2;
            set
            {
                if (_part2 != value)
                {
                    _part2 = value;
                    OnPropertyChanged(nameof(Part2));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Total => Part1 + Part2;

        public string Fal_Data
        {
            get => _falData;
            set
            {
                if (_falData != value)
                {
                    _falData = value;
                    OnPropertyChanged(nameof(Fal_Data));
                }
            }
        }

        public string Fal_Eqid
        {
            get => _falEqid;
            set
            {
                if (_falEqid != value)
                {
                    _falEqid = value;
                    OnPropertyChanged(nameof(Fal_Eqid));
                }
            }
        }

        public int Fal_Id
        {
            get => _falId;
            set
            {
                if (_falId != value)
                {
                    _falId = value;
                    OnPropertyChanged(nameof(Fal_Id));
                }
            }
        }

        public string Fal_Type
        {
            get => _falType;
            set
            {
                if (_falType != value)
                {
                    _falType = value;
                    OnPropertyChanged(nameof(Fal_Type));
                }
            }
        }

        public string Fal_Info
        {
            get => _falInfo;
            set
            {
                if (_falInfo != value)
                {
                    _falInfo = value;
                    OnPropertyChanged(nameof(Fal_Info));
                }
            }
        }

        public string Fal_Detail
        {
            get => _falDetail;
            set
            {
                if (_falDetail != value)
                {
                    _falDetail = value;
                    OnPropertyChanged(nameof(Fal_Detail));
                }
            }
        }

        public string Fal_Remark
        {
            get => _falRemark;
            set
            {
                if (_falRemark != value)
                {
                    _falRemark = value;
                    OnPropertyChanged(nameof(Fal_Remark));
                }
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
        public BarDataItem Clone()
        {
            return new BarDataItem
            {
                Label = this.Label,
                Part1 = this.Part1,
                Part2 = this.Part2,
                Fal_Data = this.Fal_Data,
                Fal_Eqid = this.Fal_Eqid,
                Fal_Id = this.Fal_Id,
                Fal_Type = this.Fal_Type,
                Fal_Info = this.Fal_Info,
                Fal_Detail = this.Fal_Detail,
                Fal_Remark = this.Fal_Remark
            };
        }

        /// <summary>
        /// 从另一个对象复制所有属性值
        /// </summary>
        public void CopyFrom(BarDataItem source)
        {
            this.Label = source.Label;
            this.Part1 = source.Part1;
            this.Part2 = source.Part2;
            this.Fal_Data = source.Fal_Data;
            this.Fal_Eqid = source.Fal_Eqid;
            this.Fal_Id = source.Fal_Id;
            this.Fal_Type = source.Fal_Type;
            this.Fal_Info = source.Fal_Info;
            this.Fal_Detail = source.Fal_Detail;
            this.Fal_Remark = source.Fal_Remark;
        }
    }
}
