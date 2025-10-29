using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace DataVisualizationPlatform.ViewModels
{
    // 用于消息传递的类
    public class ChangePageMessage
    {
        public Page NewPage { get; set; }
        public object Parameter { get; set; }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly List<Func<Page>> _pages;
        private int _currentPageIndex = 1;
        public IRelayCommand OpenYearlyDataCommand { get; }
        public IRelayCommand OpenFaultReportCommand { get; }
        public IRelayCommand OpenCurrentMonthDataCommand { get; }

        public ObservableCollection<MonthItem> Months { get; } = new();

        public ObservableCollection<YearItem> Years { get; } = new();

        public ICommand NavigateCommand { get; }
        public ICommand DataSelectCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }

        private Page _currentPage;
        public Page CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    UpdateButtonVisibility();
                }
            }
        }

        private bool _isButton1Visible = false;
        public bool IsButton1Visible
        {
            get => _isButton1Visible;
            set
            {
                if (_isButton1Visible != value)
                {
                    _isButton1Visible = value;
                    OnPropertyChanged(nameof(IsButton1Visible));
                }
            }
        }

        private bool _isButton2Visible = false;
        public bool IsButton2Visible
        {
            get => _isButton2Visible;
            set
            {
                if (_isButton2Visible != value)
                {
                    _isButton2Visible = value;
                    OnPropertyChanged(nameof(IsButton2Visible));
                }
            }
        }


        public MainViewModel()
        {
            // 初始化页面列表
            _pages = new List<Func<Page>>
            {
                () => new HomePageA(),
                () => new HomePageB(),
                () => new HomePageC()
            };

            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = DateTime.Now;
            DateTime prevYear = DateTime.MinValue;

            for (DateTime date = start; date <= end; date = date.AddMonths(1))
            {
                string monthKey = date.ToString("yyyyMM");
                string monthDisplay = date.ToString("yyyy.MM");
                Months.Add(new MonthItem(monthKey, monthDisplay));

                // 添加新的 YearItem
                if (date.Year != prevYear.Year)
                {
                    string yearKey = date.ToString("yyyy");
                    string yearDisplay = date.ToString("yyyy");
                    Years.Add(new YearItem(yearKey, yearDisplay));
                    prevYear = date;
                }
            }

            OpenYearlyDataCommand = new RelayCommand<object>((param) =>
            {
                WeakReferenceMessenger.Default.Send(new ChangePageMessage
                {
                    NewPage = new Data(),
                    Parameter = param
                });
                Console.WriteLine($"消息已发送，参数={param}");
            });

            OpenFaultReportCommand = new RelayCommand<object>((param) =>
            {
                WeakReferenceMessenger.Default.Send(new ChangePageMessage
                {
                    NewPage = new FaultReport(),
                    Parameter = param
                });
                MessageBox.Show($"消息已发送，参数={param}");
            });

            OpenCurrentMonthDataCommand = new RelayCommand(() =>
            {
                var now = DateTime.Now;
                WeakReferenceMessenger.Default.Send(new ChangePageMessage
                {
                    NewPage = new ReservationList(),
                    Parameter = (now.Month, now.Year)
                });
                Console.WriteLine($"消息已发送，参数=({now.Month}, {now.Year})");
            }); 

            NavigateCommand = new RelayCommand<object>(Navigate);
            DataSelectCommand = new RelayCommand<object>(DataSelect);
            PreviousPageCommand = new RelayCommand(NavigateToPreviousPage);
            NextPageCommand = new RelayCommand(NavigateToNextPage);

            CurrentPage = new HomePageB();

            // 注册消息接收
            WeakReferenceMessenger.Default.Register<ChangePageMessage>(this, (r, m) =>
            {
                CurrentPage = m.NewPage;
                Console.WriteLine("CurrentPage 已更新");
            });
        }

        public record MonthItem(string Key, string DisplayMonth);
        public record YearItem(string Key, string DisplayYear);

        public record DataItem();

        private void Navigate(object parameter)
        {
            string target = parameter?.ToString();

            switch (target)
            {
                case "EquipmentInfo":
                    CurrentPage = new EquipmentInfo();
                    break;
                case "ReservationList":
                    CurrentPage = new ReservationList();
                    break;
                case "FaultReport":
                    CurrentPage = new FaultReport();
                    break;
                case "MainMenu":
                    CurrentPage = new HomePageB();
                    break;
                default:
                    break;
            }
        }

        private void DataSelect(object parameter)
        {

        }
        private void NavigateToPreviousPage()
        {
            if (_pages.Count == 0) return;

            _currentPageIndex = (_currentPageIndex - 1 + _pages.Count) % _pages.Count;
            CurrentPage = _pages[_currentPageIndex]();
            Console.WriteLine($"切换到页面: {CurrentPage.GetType().Name} (索引: {_currentPageIndex})");
        }

        private void NavigateToNextPage()
        {
            if (_pages.Count == 0) return;

            _currentPageIndex = (_currentPageIndex + 1) % _pages.Count;
            CurrentPage = _pages[_currentPageIndex]();
            Console.WriteLine($"切换到页面: {CurrentPage.GetType().Name} (索引: {_currentPageIndex})");
        }

        private void UpdateButtonVisibility()
        {
            // 当前页面是PageA、PageB或PageC时显示按钮
            bool shouldShowButtons = CurrentPage is HomePageA || CurrentPage is HomePageB || CurrentPage is HomePageC;
            IsButton1Visible = shouldShowButtons;
            IsButton2Visible = shouldShowButtons;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}