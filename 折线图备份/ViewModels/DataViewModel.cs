using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DataVisualizationPlatform.ViewModels
{
    public class DataViewModel : INotifyPropertyChanged
    {
        private readonly DataModel _model = new DataModel();

        // 辅助类定义
        public class LineSegment
        {
            public double X1 { get; set; }
            public double Y1 { get; set; }
            public double X2 { get; set; }
            public double Y2 { get; set; }
        }

        public class DataPoint
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int Value { get; set; }
        }

        public class MonthLabel
        {
            public double X { get; set; }
            public string Month { get; set; } = string.Empty;
        }

        public class MonthlyChartDataVM : INotifyPropertyChanged
        {
            public string Month { get; set; } = string.Empty;
            public int ReservationCount { get; set; }
            public int FaultCount { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // 折线图相关属性
        public ObservableCollection<MonthlyChartDataVM> MonthlyData { get; } = new();
        public ObservableCollection<LineSegment> ReservationLines { get; } = new();
        public ObservableCollection<LineSegment> FaultLines { get; } = new();
        public ObservableCollection<DataPoint> ReservationPoints { get; } = new();
        public ObservableCollection<DataPoint> FaultPoints { get; } = new();
        public ObservableCollection<LineSegment> GridLines { get; } = new();
        public ObservableCollection<MonthLabel> MonthLabels { get; } = new();

        // 图表尺寸和比例
        private double _chartWidth = 800;
        private double _chartHeight = 300;
        private double _centerLineY = 200; // 中心线位置
        private int _maxReservationCount = 10;
        private int _maxFaultCount = 10;

        public double ChartWidth
        {
            get => _chartWidth;
            set
            {
                _chartWidth = value;
                OnPropertyChanged(nameof(ChartWidth));
                UpdateChart();
            }
        }

        public double ChartHeight
        {
            get => _chartHeight;
            set
            {
                _chartHeight = value;
                _centerLineY = value * 3 / 4;
                OnPropertyChanged(nameof(ChartHeight));
                OnPropertyChanged(nameof(CenterLineY));
                UpdateChart();
            }
        }

        public double CenterLineY
        {
            get => _centerLineY;
            set
            {
                _centerLineY = value;
                OnPropertyChanged(nameof(CenterLineY));
            }
        }

        public int MaxReservationCount
        {
            get => _maxReservationCount;
            set
            {
                _maxReservationCount = value;
                OnPropertyChanged(nameof(MaxReservationCount));
            }
        }

        public int MaxFaultCount
        {
            get => _maxFaultCount;
            set
            {
                _maxFaultCount = value;
                OnPropertyChanged(nameof(MaxFaultCount));
            }
        }

        public DataViewModel()
        {
            LoadData();
        }

        // 处理Canvas尺寸变化的方法
        public void UpdateCanvasSize(double width, double height)
        {
            if (width > 0 && height > 0)
            {
                ChartWidth = width;
                ChartHeight = height;
            }
        }

        private void LoadData()
        {
            try
            {
                var data = _model.GetMonthlyData();

                System.Diagnostics.Debug.WriteLine($"获取到 {data.Count} 条数据");

                if (data.Count == 0)
                {
                    return;
                }

                // 计算最大值
                MaxReservationCount = data.Max(d => d.ReservationCount);
                MaxFaultCount = data.Max(d => d.FaultCount);

                // 确保最大值不为0
                if (MaxReservationCount == 0) MaxReservationCount = 1;
                if (MaxFaultCount == 0) MaxFaultCount = 1;

                MonthlyData.Clear();
                foreach (var item in data)
                {
                    var viewModel = new MonthlyChartDataVM
                    {
                        Month = item.Month,
                        ReservationCount = item.ReservationCount,
                        FaultCount = item.FaultCount
                    };
                    MonthlyData.Add(viewModel);
                }

                UpdateChart();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载数据时出错: {ex.Message}");
            }
        }

        private void UpdateChart()
        {
            if (MonthlyData.Count == 0) return;

            ClearChartData();
            CreateGridLines();
            CreateMonthLabels();
            CreateDataLines();
            CreateDataPoints();
        }

        private void ClearChartData()
        {
            ReservationLines.Clear();
            FaultLines.Clear();
            ReservationPoints.Clear();
            FaultPoints.Clear();
            GridLines.Clear();
            MonthLabels.Clear();
        }

        private void CreateGridLines()
        {
            // 水平网格线（每25%一条）
            for (int i = 1; i <= 3; i++)
            {
                double y = (ChartHeight / 4) * i;
                GridLines.Add(new LineSegment
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = ChartWidth,
                    Y2 = y
                });
            }

            // 垂直网格线（每个月一条）
            if (MonthlyData.Count > 1)
            {
                double stepX = ChartWidth / (MonthlyData.Count - 1);
                for (int i = 0; i < MonthlyData.Count; i++)
                {
                    double x = i * stepX;
                    GridLines.Add(new LineSegment
                    {
                        X1 = x,
                        Y1 = 0,
                        X2 = x,
                        Y2 = ChartHeight
                    });
                }
            }
        }

        private void CreateMonthLabels()
        {
            if (MonthlyData.Count <= 1) return;

            double stepX = ChartWidth / (MonthlyData.Count - 1);
            for (int i = 0; i < MonthlyData.Count; i++)
            {
                double x = i * stepX - 20; // 调整位置使标签居中
                MonthLabels.Add(new MonthLabel
                {
                    X = Math.Max(0, x), // 确保X坐标不为负数
                    Month = MonthlyData[i].Month
                });
            }
        }

        private void CreateDataLines()
        {
            if (MonthlyData.Count <= 1) return;

            double stepX = ChartWidth / (MonthlyData.Count - 1);

            // 创建预约数据折线（向上）- 使用3/4高度
            for (int i = 0; i < MonthlyData.Count - 1; i++)
            {
                double x1 = i * stepX;
                double x2 = (i + 1) * stepX;

                // 预约数据向上绘制（使用图表高度的3/4作为最大范围）
                double y1 = CenterLineY - (MonthlyData[i].ReservationCount / (double)MaxReservationCount * (ChartHeight * 3 / 4));
                double y2 = CenterLineY - (MonthlyData[i + 1].ReservationCount / (double)MaxReservationCount * (ChartHeight * 3 / 4));

                ReservationLines.Add(new LineSegment
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2
                });
            }

            // 创建故障数据折线（向下）- 使用1/4高度
            for (int i = 0; i < MonthlyData.Count - 1; i++)
            {
                double x1 = i * stepX;
                double x2 = (i + 1) * stepX;

                // 故障数据向下绘制（使用图表高度的1/4作为最大范围）
                double y1 = CenterLineY + (MonthlyData[i].FaultCount / (double)MaxFaultCount * (ChartHeight * 1 / 4));
                double y2 = CenterLineY + (MonthlyData[i + 1].FaultCount / (double)MaxFaultCount * (ChartHeight * 1 / 4));

                FaultLines.Add(new LineSegment
                {
                    X1 = x1,
                    Y1 = y1,
                    X2 = x2,
                    Y2 = y2
                });
            }
        }

        private void CreateDataPoints()
        {
            if (MonthlyData.Count == 0) return;

            double stepX = MonthlyData.Count == 1 ? ChartWidth / 2 : ChartWidth / (MonthlyData.Count - 1);

            // 创建预约数据点 - 使用3/4高度
            for (int i = 0; i < MonthlyData.Count; i++)
            {
                double x = MonthlyData.Count == 1 ? stepX : i * stepX;
                double y = CenterLineY - (MonthlyData[i].ReservationCount / (double)MaxReservationCount * (ChartHeight * 3 / 4));

                ReservationPoints.Add(new DataPoint
                {
                    X = x - 4,
                    Y = y - 4,
                    Value = MonthlyData[i].ReservationCount
                });
            }

            // 创建故障数据点 - 使用1/4高度
            for (int i = 0; i < MonthlyData.Count; i++)
            {
                double x = MonthlyData.Count == 1 ? stepX : i * stepX;
                double y = CenterLineY + (MonthlyData[i].FaultCount / (double)MaxFaultCount * (ChartHeight * 1 / 4));

                FaultPoints.Add(new DataPoint
                {
                    X = x - 4,
                    Y = y - 4,
                    Value = MonthlyData[i].FaultCount
                });
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}