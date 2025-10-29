using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace DataVisualizationPlatform.ViewModels
{
    class HomePageAViewModel : INotifyPropertyChanged

    {
        private readonly Json _jsonData = new Json();
        public ObservableCollection<LineDataPoint> LineData { get; set; }

        private readonly DispatcherTimer _animationTimer;
        private DateTime _animationStartTime;
        private double _animationProgress;

        public HomePageAViewModel()
        {
            InitializeData();
            
            _animationTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS
            };
            _animationTimer.Tick += AnimationTimer_Tick;

            // 自动启动动画
            StartAnimation();
        }

        #region Properties

        private ObservableCollection<LineDataPoint> LoadLineData()
        {
            var reservations = JsonConvert.DeserializeObject<List<HomePageModel>>(_jsonData._ReservationList);
            if (reservations == null) return new ObservableCollection<LineDataPoint>();

            // 把合法的已完成订单转成 DateTime
            var completedDates = reservations
                .Where(r => r.Res_Status == "已完成")
                .Select(r => DateTime.TryParse(r.Res_Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)
                                ? (DateTime?)dt
                                : null)
                .Where(dt => dt.HasValue)
                .Select(dt => dt.Value)
                .ToList();

            if (completedDates.Count == 0)
                return new ObservableCollection<LineDataPoint>();

            // 分组统计每个月数量
            var grouped = completedDates
                .GroupBy(d => d.Year * 100 + d.Month)
                .ToDictionary(g => g.Key, g => g.Count());

            // 找出最小和最大月份
            DateTime minDate = new DateTime(completedDates.Min(d => d).Year, completedDates.Min(d => d).Month, 1);
            DateTime maxDate = new DateTime(completedDates.Max(d => d).Year, completedDates.Max(d => d).Month, 1);

            var result = new ObservableCollection<LineDataPoint>();

            for (DateTime dt = minDate; dt <= maxDate; dt = dt.AddMonths(1))
            {   
                int key = dt.Year * 100 + dt.Month;
                int count = grouped.ContainsKey(key) ? grouped[key] : 0;

                // 调试输出
                Debug.WriteLine($"年月: {dt:yyyy-MM}, 订单数量: {count}");

                result.Add(new LineDataPoint
                {
                    X = key,   // 例如 202309
                    Y = count,
                    Label = dt.ToString("yyyy-MM")
                });
            }

            return result;
        }




        public double AnimationProgress
        {
            get => _animationProgress;
            set => SetProperty(ref _animationProgress, value);
        }

        public int AnimationDuration { get; } = 3000; // 3秒

        #endregion

        #region Private Methods

        private void InitializeData()
        {
            LineData = LoadLineData();
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

            if (progress >= 1.0)
            {
                _animationTimer.Stop();
            }
        }

        // 缓动函数
        private static double EaseInOut(double t)
        {
            return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        #endregion

        #region INotifyPropertyChanged Implementation

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

        #endregion
    }

    #region Data Models

    public class LineDataPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Label { get; set; }

        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
