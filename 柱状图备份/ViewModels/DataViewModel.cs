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
        public ICommand FlipCardCommand { get; }

        public class MonthlyChartDataVM : INotifyPropertyChanged
        {
            public string Month { get; set; } = string.Empty;
            public int ReservationCount { get; set; }
            public int FaultCount { get; set; }

            // 绑定到图表的高度
            public double ReservationHeight { get; set; }
            public double FaultHeight { get; set; }

            // 翻转状态属性
            private bool _isFlipped = false;
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

        public ObservableCollection<MonthlyChartDataVM> MonthlyData { get; } = new();

        public DataViewModel()
        {
            FlipCardCommand = new RelayCommand<MonthlyChartDataVM>(FlipCard);
            LoadData();
        }

        private async void FlipCard(MonthlyChartDataVM monthlyData)
        {
            if (monthlyData != null)
            {
                if (!monthlyData.IsFlipped)
                {
                    // 翻转到背面：先开始动画，延时后切换内容
                    monthlyData.IsFlipped = true;
                    await Task.Delay(500); // 等待动画到中间点
                    monthlyData.IsContentFlipped = true;
                }
                else
                {
                    // 翻转到正面：先开始动画，延时后切换内容
                    monthlyData.IsFlipped = false;
                    await Task.Delay(500); // 等待动画到中间点
                    monthlyData.IsContentFlipped = false;
                }
            }
        }

        private void LoadData()
        {
            try
            {
                var data = _model.GetMonthlyData();

                // 调试输出
                System.Diagnostics.Debug.WriteLine($"获取到 {data.Count} 条数据");

                if (data.Count == 0)
                {
                    return;
                }

                // 计算最大值，用于比例缩放
                int maxFaultCount = data.Max(d => d.FaultCount);
                int maxReservationCount = data.Max(d => d.ReservationCount);
                int maxTotalCount = Math.Max(maxFaultCount, maxReservationCount);

                // 确保最大值不为0
                if (maxTotalCount == 0)
                {
                    maxTotalCount = 1;
                }

                // 每个Grid行的最大高度
                const double maxRowHeight = 100; // 每行最大100像素

                MonthlyData.Clear();
                foreach (var item in data)
                {
                    var viewModel = new MonthlyChartDataVM
                    {
                        Month = item.Month,
                        ReservationCount = item.ReservationCount,
                        FaultCount = item.FaultCount,
                        // 分别计算每个类型的高度，不是累加
                        ReservationHeight = item.ReservationCount == 0 ? 5 :
                            (item.ReservationCount / (double)maxTotalCount * maxRowHeight),
                        FaultHeight = item.FaultCount == 0 ? 5 :
                            (item.FaultCount / (double)maxTotalCount * maxRowHeight)
                    };

                    // 调试输出
                    System.Diagnostics.Debug.WriteLine(
                        $"月份: {viewModel.Month}, " +
                        $"故障: {viewModel.FaultCount}(高度:{viewModel.FaultHeight:F1}), " +
                        $"预约: {viewModel.ReservationCount}(高度:{viewModel.ReservationHeight:F1})");

                    MonthlyData.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载数据时出错: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}