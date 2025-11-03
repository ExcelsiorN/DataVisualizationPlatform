using DataVisualizationPlatform.Services.Navigation;
using DataVisualizationPlatform.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Threading;

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

            InitializeEventHandlers();
            InitializeUnifiedPopup();
        }

        private void InitializeEventHandlers()
        {
            // 数据按钮事件处理
            if (DataBtn != null)
            {
                DataBtn.MouseEnter += OnDataButtonMouseEnter;
                DataBtn.MouseLeave += OnDataButtonMouseLeave;
            }

            // Data子菜单事件处理
            if (DataSubMenu != null)
            {
                DataSubMenu.MouseEnter += OnDataSubMenuMouseEnter;
                DataSubMenu.MouseLeave += OnDataSubMenuMouseLeave;
            }
        }

        private void InitializeUnifiedPopup()
        {
            // 初始化时隐藏子菜单
            if (DataSubMenu != null)
                DataSubMenu.Visibility = Visibility.Collapsed;
        }

        private void OnCurrentPageChanged(object? sender, Page? page)
        {
            // 更新Frame的内容
            if (MainContentFrame != null)
            {
                MainContentFrame.Content = page;
            }
        }

        #region 鼠标事件处理

        private void OnDataButtonMouseEnter(object sender, MouseEventArgs e)
        {
            // 显示子菜单
            if (DataSubMenu != null)
            {
                DataSubMenu.Visibility = Visibility.Visible;
            }
        }

        private void OnDataButtonMouseLeave(object sender, MouseEventArgs e)
        {
            // 延迟隐藏子菜单
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                if (DataSubMenu != null && !IsMouseOverElement(DataSubMenu) && !IsMouseOverElement(DataBtn))
                {
                    DataSubMenu.Visibility = Visibility.Collapsed;
                }
            };
            timer.Start();
        }

        private void OnDataSubMenuMouseEnter(object sender, MouseEventArgs e)
        {
            // 保持子菜单显示
            if (DataSubMenu != null)
            {
                DataSubMenu.Visibility = Visibility.Visible;
            }
        }

        private void OnDataSubMenuMouseLeave(object sender, MouseEventArgs e)
        {
            // 延迟隐藏子菜单
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                if (DataSubMenu != null && !IsMouseOverElement(DataSubMenu) && !IsMouseOverElement(DataBtn))
                {
                    DataSubMenu.Visibility = Visibility.Collapsed;
                }
            };
            timer.Start();
        }

        // 保留这些方法以兼容 XAML 中的绑定
        private void ButtonOrPopup_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == DataBtn)
                OnDataButtonMouseEnter(sender, e);
        }

        private void ButtonOrPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender == DataBtn)
                OnDataButtonMouseLeave(sender, e);
        }

        #endregion


        #region 辅助方法

        private bool IsMouseOverElement(FrameworkElement? element)
        {
            if (element == null || !element.IsVisible) return false;

            try
            {
                var pos = Mouse.GetPosition(element);
                return pos.X >= 0 && pos.Y >= 0 &&
                       pos.X <= element.ActualWidth &&
                       pos.Y <= element.ActualHeight;
            }
            catch
            {
                return false;
            }
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
