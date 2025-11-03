using DataVisualizationPlatform.Services.Navigation;
using DataVisualizationPlatform.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataVisualizationPlatform
{
    /// <summary>
    /// MainWindow - 主窗口
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INavigationService _navigationService;

        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _navigationService = navigationService;

            // 绑定导航服务的CurrentPage到UI
            _navigationService.CurrentPageChanged += OnCurrentPageChanged;
        }

        private void OnCurrentPageChanged(object? sender, Page? page)
        {
            // 更新Frame的内容
            if (MainContentFrame != null)
            {
                MainContentFrame.Content = page;
            }
        }

        #region 数据查看菜单事件

        private void DataBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            if (DataSubMenu != null)
            {
                DataSubMenu.Visibility = Visibility.Visible;
            }
        }

        private void DataBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                if (DataSubMenu != null && !IsMouseOver(DataSubMenu) && !IsMouseOver(DataBtn))
                {
                    DataSubMenu.Visibility = Visibility.Collapsed;
                }
            };
            timer.Start();
        }

        private void DataSubMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            if (DataSubMenu != null)
            {
                DataSubMenu.Visibility = Visibility.Visible;
            }
        }

        private void DataSubMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                if (DataSubMenu != null && !IsMouseOver(DataSubMenu) && !IsMouseOver(DataBtn))
                {
                    DataSubMenu.Visibility = Visibility.Collapsed;
                }
            };
            timer.Start();
        }

        private bool IsMouseOver(FrameworkElement element)
        {
            if (element == null || !element.IsVisible) return false;
            var pos = Mouse.GetPosition(element);
            return pos.X >= 0 && pos.Y >= 0 &&
                   pos.X <= element.ActualWidth && pos.Y <= element.ActualHeight;
        }

        #endregion

        #region 窗口控制按钮

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal
                : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _navigationService.CurrentPageChanged -= OnCurrentPageChanged;
        }
    }
}
