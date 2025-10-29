using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Controls;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Views;

namespace DataVisualizationPlatform.ViewModels
{
    class DataViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _animationTimer;
        private readonly Json _jsonData = new Json();     
        private DateTime _animationStartTime;
        private double _animationProgress;
        private string _currentYear; 

        public IRelayCommand OpenMonthlyDataCommand { get; }

        public DataViewModel()
        {
            _currentYear = DateTime.Now.Year.ToString();

            WeakReferenceMessenger.Default.Register<ChangePageMessage>(this, (r, msg) =>
            {
                if (msg.Parameter != null)
                {
                    string targetYear = msg.Parameter.ToString();
                    _currentYear = targetYear; // 更新存储的年份
                    InitializeData(targetYear);
                }
            });

            OpenMonthlyDataCommand = new RelayCommand<object>((param) =>
            {
                
                if (int.TryParse(_currentYear, out int targetYear))
                {
                    if (int.TryParse(param?.ToString(), out int month))
                    {
                        WeakReferenceMessenger.Default.Send(new ChangePageMessage
                        {
                            NewPage = new ReservationList(),
                            Parameter = (month, targetYear)
                        });
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("月份参数无效，必须是 1-12 的数字！");
                    }
                }
            });

            _animationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            _animationTimer.Tick += AnimationTimer_Tick;
            StartAnimation();
        }

        #region Properties
        private ObservableCollection<BarDataItem> _barData;
        public ObservableCollection<BarDataItem> BarData
        {
            get => _barData;
            set => SetProperty(ref _barData, value);
        }

        public double AnimationProgress
        {
            get => _animationProgress;
            set => SetProperty(ref _animationProgress, value);
        }
        public int AnimationDuration { get; } = 3000;
        #endregion

        public ObservableCollection<MonthItem> Months { get; } = new()
    {
        new MonthItem("1", "1月"),
        new MonthItem("2", "2月"),
        new MonthItem("3", "3月"),
        new MonthItem("4", "4月"),
        new MonthItem("5", "5月"),
        new MonthItem("6", "6月"),
        new MonthItem("7", "7月"),
        new MonthItem("8", "8月"),
        new MonthItem("9", "9月"),
        new MonthItem("10", "10月"),
        new MonthItem("11", "11月"),
        new MonthItem("12", "12月")
    };
        public record MonthItem(string Key, string DisplayMonth);

        #region Private Methods
        private void InitializeData(string targetYear)
        {
            var allData = ChartDataService.LoadBarData(_jsonData);
            BarData = new ObservableCollection<Models.BarDataItem>(
                allData.Where(item => item.Label.StartsWith(targetYear + "-"))
            );
        }

        private void StartAnimation()
        {
            AnimationProgress = 0;
            _animationStartTime = DateTime.Now;
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            var elapsed = DateTime.Now - _animationStartTime;
            var progress = Math.Min(elapsed.TotalMilliseconds / AnimationDuration, 1.0);
            AnimationProgress = EaseInOut(progress);
            if (progress >= 1.0) _animationTimer.Stop();
        }

        private static double EaseInOut(double t) => t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
