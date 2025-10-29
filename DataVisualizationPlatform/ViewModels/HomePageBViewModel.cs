using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Controls;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataVisualizationPlatform.ViewModels
{
    public class HomePageBViewModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();
        private readonly DispatcherTimer _animationTimer;
        private readonly DispatcherTimer _scrollTimer;
        public ObservableCollection<BarDataItem> FaultReportList { get; } = new();
        public ObservableCollection<ReservationListModel> DataList { get; } = new();
        public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();
        public ObservableCollection<EquipmentStatistics> CurrentEquipmentStats { get; } = new();
        private List<EquipmentStatistics> _allEquipmentStats = new();
        private int _currentEquipmentIndex = 0;
        private DateTime _animationStartTime;
        private double _animationProgress;
        public ICommand LogoutCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand ViewReportCommand { get; }

        public HomePageBViewModel()
        {
            InitializeData();

            _animationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            _animationTimer.Tick += AnimationTimer_Tick;

            // 初始化循环滚动定时器
            _scrollTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _scrollTimer.Tick += ScrollTimer_Tick;

            StartAnimation();
            LoadData();
            CalculateEquipmentStatistics();
            StartEquipmentScroll();

            LogoutCommand = new RelayCommand<object>(NavigateToLogin);
            ExportDataCommand = new RelayCommand<object>(ExportData);
            ViewReportCommand = new RelayCommand<object>(ViewReport);
        }

        #region Properties
        public ObservableCollection<BarDataItem> BarData { get; private set; }
        public double AnimationProgress
        {
            get => _animationProgress;
            set => SetProperty(ref _animationProgress, value);
        }
        public int AnimationDuration { get; } = 3000;
        #endregion

        #region Private Methods
        private void InitializeData()
        {
            BarData = ChartDataService.LoadBarData(_jsonData);
        }

        private void LoadData()
        {
            try
            {
                var faultReport = JsonConvert.DeserializeObject<List<BarDataItem>>(_jsonData._FaultReport);
                var data = JsonConvert.DeserializeObject<List<ReservationListModel>>(_jsonData._ReservationList);
                var equipment = JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(_jsonData._EquipmentInfo);
                FaultReportList.Clear();
                DataList.Clear();
                EquipmentList.Clear();

                if (faultReport != null)
                {
                    foreach (var item in faultReport)
                    {
                        if (item.Fal_Info == "待处理")
                            FaultReportList.Add(item);
                    }
                }
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        // 加载所有预约数据用于统计
                        DataList.Add(item);
                    }
                }
                if (equipment != null)
                {
                    foreach (var item in equipment)
                    {
                        EquipmentList.Add(item);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"加载设备信息失败: {ex.Message}");
            }
        }

        private void CalculateEquipmentStatistics()
        {
            _allEquipmentStats.Clear();

            // 为fntp-0到fntp-5的每个设备计算统计数据
            for (int i = 0; i <= EquipmentList.Count - 1; i++)
            {
                string equipmentId = $"fntp-{i}";
                var stats = new EquipmentStatistics
                {
                    EquipmentId = equipmentId,
                    TotalOrders = 0,
                    FixedDurationRatio = 0,
                    FaultRate = 0,
                    TrendIndicator = "→ 0%",
                    TrendColor = "#757575"
                };

                // 计算累计预约单数量
                if (DataList != null)
                {
                    stats.TotalOrders = DataList.Count(d => d.Res_Equipment == equipmentId);
                }

                // 计算固定时长占比
                var equipInfo = EquipmentList?.FirstOrDefault(e => e.Equ_Id == equipmentId);
                if (equipInfo != null)
                {
                    // 去除"小时"单位并转换为double
                    string fixedDurationStr = equipInfo.Equ_FixedDurationThisYear?.Replace("小时", "").Trim();
                    string usedDurationStr = equipInfo.Equ_UsedFixedDurationThisYear?.Replace("小时", "").Trim();

                    if (double.TryParse(fixedDurationStr, out double fixedDuration) &&
                        double.TryParse(usedDurationStr, out double usedDuration) &&
                        fixedDuration > 0)
                    {
                        stats.FixedDurationRatio = Math.Round(usedDuration / fixedDuration * 100, 1);
                    }
                }

                // 计算故障率
                if (FaultReportList != null && stats.TotalOrders > 0)
                {
                    int faultCount = FaultReportList.Count(f => f.Fal_Eqid == equipmentId);
                    stats.FaultRate = Math.Round((double)faultCount / stats.TotalOrders * 100, 2);
                }

                // 模拟趋势数据（实际应该基于历史数据计算）
                Random rand = new Random(i);
                double trend = rand.Next(-20, 30) / 10.0;
                if (trend > 0)
                {
                    stats.TrendIndicator = $"↗ {trend}%";
                    stats.TrendColor = "#4CAF50";
                }
                else if (trend < 0)
                {
                    stats.TrendIndicator = $"↘ {Math.Abs(trend)}%";
                    stats.TrendColor = "#F44336";
                }
                else
                {
                    stats.TrendIndicator = "→ 0%";
                    stats.TrendColor = "#757575";
                }

                _allEquipmentStats.Add(stats);
            }

            // 初始化显示前3个设备的数据
            UpdateCurrentEquipmentDisplay();
        }

        private void UpdateCurrentEquipmentDisplay()
        {
            CurrentEquipmentStats.Clear();

            // 显示3个设备的数据（循环显示）
            for (int i = 0; i < 3 && i < _allEquipmentStats.Count; i++)
            {
                int index = (_currentEquipmentIndex + i) % _allEquipmentStats.Count;
                CurrentEquipmentStats.Add(_allEquipmentStats[index]);
            }
        }

        private void StartEquipmentScroll()
        {
            if (_allEquipmentStats.Count > 3)
            {
                _scrollTimer.Start();
            }
        }

        private void ScrollTimer_Tick(object sender, EventArgs e)
        {
            _currentEquipmentIndex = (_currentEquipmentIndex + 1) % _allEquipmentStats.Count;
            UpdateCurrentEquipmentDisplay();
        }

        private void NavigateToLogin(object parameter)
        {
            var loginWindow = new Login();
            loginWindow.Show();

            // 查找并关闭MainWindow
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();
            mainWindow?.Close();
        }

        private void ExportData(object parameter)
        {
            // 实现数据导出功能
            MessageBox.Show("数据编辑功能正在开发中...", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ViewReport(object parameter)
        {
            // 实现查看报告功能
            MessageBox.Show("系统设置功能正在开发中...", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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