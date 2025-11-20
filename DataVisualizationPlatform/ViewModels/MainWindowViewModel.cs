using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataVisualizationPlatform.ViewModels
{
    /// <summary>
    /// 主窗口ViewModel
    /// 使用partial关键字配合CommunityToolkit源生成器
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        // 使用[ObservableProperty]特性自动生成属性
        [ObservableProperty]
        private bool _isButton1Visible;

        [ObservableProperty]
        private bool _isButton2Visible;

        [ObservableProperty]
        private System.Windows.Controls.Page? _currentPage;

        [ObservableProperty]
        private string? _selectedYear;

        [ObservableProperty]
        private bool _isBlurred;

        [ObservableProperty]
        private bool _isMagicBarVisible;

        [ObservableProperty]
        private Thickness _frameMargin = new Thickness(60, 40, 60, 50);

        [ObservableProperty]
        private int _magicBarSelectedIndex = -1;

        public ObservableCollection<MonthItem> Months { get; } = new();
        public ObservableCollection<YearItem> Years { get; } = new();

        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // 监听导航变化，更新按钮可见性
            _navigationService.CurrentPageChanged += OnCurrentPageChanged;

            // 订阅页面切换消息
            WeakReferenceMessenger.Default.Register<ChangePageMessage>(this, (recipient, message) =>
            {
                if (!string.IsNullOrEmpty(message.PageKey))
                {
                    _navigationService.NavigateTo(message.PageKey, message.Parameter);
                }
            });

            InitializeMonthsAndYears();

            // 导航到默认页面 
            _navigationService.NavigateTo("HomePageB");
        }

        partial void OnSelectedYearChanged(string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _navigationService.NavigateTo("Data", value);
            }
        }

        private void InitializeMonthsAndYears()
        {
            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = DateTime.Now;
            DateTime prevYear = DateTime.MinValue;

            for (DateTime date = start; date <= end; date = date.AddMonths(1))
            {
                string monthKey = date.ToString("yyyyMM");
                string monthDisplay = date.ToString("yyyy.MM");
                Months.Add(new MonthItem(monthKey, monthDisplay));

                if (date.Year != prevYear.Year)
                {
                    string yearKey = date.ToString("yyyy");
                    string yearDisplay = date.ToString("yyyy");
                    Years.Add(new YearItem(yearKey, yearDisplay));
                    prevYear = date;
                }
            }
        }

        private void OnCurrentPageChanged(object? sender, System.Windows.Controls.Page? page)
        {
            // 更新CurrentPage属性
            CurrentPage = page;

            if (page == null) return;

            // 根据当前页面类型更新按钮可见性
            var pageType = page.GetType();
            bool shouldShowButtons = pageType.Name is "HomePageA" or "HomePageB" or "HomePageC";

            IsButton1Visible = shouldShowButtons;
            IsButton2Visible = shouldShowButtons;

            // 根据当前页面类型更新MagicBar可见性
            bool shouldShowMagicBar = pageType.Name is "Edit" or "TimeSlotEdit" or "FaultEdit";
            IsMagicBarVisible = shouldShowMagicBar;

            // 调整Frame的Margin
            FrameMargin = shouldShowMagicBar
                ? new Thickness(20, 20, 20, 90)
                : new Thickness(30, 30, 30, 50);

            // 根据当前页面更新MagicBar选中索引
            MagicBarSelectedIndex = pageType.Name switch
            {
                "Edit" => 0,           // 设备编辑
                "TimeSlotEdit" => 1,   // 时段编辑
                "FaultEdit" => 2,      // 故障编辑
                _ => -1                // 其他页面不选中
            };
        }

        [RelayCommand]
        private void Navigate(string? target)
        {
            if (string.IsNullOrEmpty(target)) return;

            _navigationService.NavigateTo(target);
        }

        [RelayCommand]
        private void OpenYearlyData(object? parameter)
        {
            _navigationService.NavigateTo("Data", parameter);
        }

        [RelayCommand]
        private void OpenFaultReport(object? parameter)
        {
            _navigationService.NavigateTo("FaultReport", parameter);
        }

        [RelayCommand]
        private void OpenCurrentMonthData()
        {
            var now = DateTime.Now;
            _navigationService.NavigateTo("ReservationList", (now.Month, now.Year));
        }

        [RelayCommand]
        private void PreviousPage()
        {
            // 实现向前循环切换逻辑（C -> B -> A -> C）
            var currentPageType = _navigationService.CurrentPage?.GetType().Name;
            var previousPage = currentPageType switch
            {
                "HomePageA" => "HomePageC",
                "HomePageB" => "HomePageA",
                "HomePageC" => "HomePageB",
                _ => "HomePageB"
            };

            _navigationService.NavigateTo(previousPage);
        }

        [RelayCommand]
        private void NextPage()
        {
            // 实现向后循环切换逻辑（A -> B -> C -> A）
            var currentPageType = _navigationService.CurrentPage?.GetType().Name;
            var nextPage = currentPageType switch
            {
                "HomePageA" => "HomePageB",
                "HomePageB" => "HomePageC",
                "HomePageC" => "HomePageA",
                _ => "HomePageB"
            };

            _navigationService.NavigateTo(nextPage);
        }

        [RelayCommand]
        private void NavigateFromMagicBar(int? index)
        {
            if (index == null) return;

            var targetPage = index switch
            {
                0 => "Edit",           // 设备编辑
                1 => "TimeSlotEdit",   // 时段编辑
                2 => "FaultEdit",      // 故障编辑
                _ => null
            };

            if (!string.IsNullOrEmpty(targetPage))
            {
                _navigationService.NavigateTo(targetPage);
            }
        }

        public record MonthItem(string Key, string DisplayMonth);
        public record YearItem(string Key, string DisplayYear);

        public override void OnUnloaded()
        {
            base.OnUnloaded();
            _navigationService.CurrentPageChanged -= OnCurrentPageChanged;
            WeakReferenceMessenger.Default.Unregister<ChangePageMessage>(this);
        }
    }
}
