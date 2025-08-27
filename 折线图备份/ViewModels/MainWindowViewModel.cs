using DataVisualizationPlatform.Views;
using DataVisualizationPlatform.Commands;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging; // 引入消息机制

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
        public ICommand NavigateCommand { get; }

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
                }
            }
        }

        public MainViewModel()
        {
            NavigateCommand = new RelayCommand<object>(Navigate);
            CurrentPage = new MainMenu(); 

            // 注册消息接收
            WeakReferenceMessenger.Default.Register<ChangePageMessage>(this, (r, m) =>
            {
                CurrentPage = m.NewPage;
                Console.WriteLine("CurrentPage 已更新");
            });
        }

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
                case "Data":
                    CurrentPage = new Data();
                    break;
                case "MainMenu":
                    CurrentPage = new MainMenu();
                    break;
                default:
                    break;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
