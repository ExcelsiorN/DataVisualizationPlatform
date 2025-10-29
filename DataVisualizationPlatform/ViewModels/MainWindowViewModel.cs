using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services.Navigation;
using System;
using System.Collections.ObjectModel;

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
            if (page == null) return;

            // 根据当前页面类型更新按钮可见性
            var pageType = page.GetType();
            bool shouldShowButtons = pageType.Name is "HomePageA" or "HomePageB" or "HomePageC";

            IsButton1Visible = shouldShowButtons;
            IsButton2Visible = shouldShowButtons;
        }

        // 使用[RelayCommand]特性自动生成命令
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
