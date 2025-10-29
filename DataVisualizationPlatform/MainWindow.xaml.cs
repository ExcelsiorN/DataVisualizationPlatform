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
        private DispatcherTimer? popupCloseTimer;
        private DispatcherTimer? blurRemoveTimer;
        private bool isBlurActive = false;
        private readonly object blurLock = new object();
        private string currentPopupContent = "";

        public MainWindow(MainWindowViewModel viewModel, INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = viewModel;
            _navigationService = navigationService;

            // 绑定导航服务的CurrentPage到UI
            _navigationService.CurrentPageChanged += OnCurrentPageChanged;

            InitializeTimers();
            InitializeEventHandlers();
            InitializeUnifiedPopup();
        }

        private void InitializeTimers()
        {
            // Popup 延迟关闭计时器
            popupCloseTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300)
            };
            popupCloseTimer.Tick += PopupCloseTimer_Tick;

            // 模糊效果移除延迟计时器
            blurRemoveTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(350)
            };
            blurRemoveTimer.Tick += BlurRemoveTimer_Tick;
        }

        private void InitializeEventHandlers()
        {
            // 数据按钮事件处理
            if (DataBtn != null)
            {
                DataBtn.MouseEnter += OnDataButtonMouseEnter;
                DataBtn.MouseLeave += OnNavigationItemMouseLeave;
            }

            // 统一Popup事件处理
            if (UnifiedPopup != null)
            {
                UnifiedPopup.MouseEnter += OnPopupMouseEnter;
                UnifiedPopup.MouseLeave += OnNavigationItemMouseLeave;
                UnifiedPopup.Closed += OnPopupClosed;
            }

            // 为Popup内容区域也添加鼠标事件
            if (DataPopupContent != null)
            {
                DataPopupContent.MouseEnter += OnPopupMouseEnter;
                DataPopupContent.MouseLeave += OnNavigationItemMouseLeave;
            }

            if (FaultReportPopupContent != null)
            {
                FaultReportPopupContent.MouseEnter += OnPopupMouseEnter;
                FaultReportPopupContent.MouseLeave += OnNavigationItemMouseLeave;
            }
        }

        private void InitializeUnifiedPopup()
        {
            // 初始化时隐藏两个内容面板
            if (DataPopupContent != null)
                DataPopupContent.Visibility = Visibility.Collapsed;

            if (FaultReportPopupContent != null)
                FaultReportPopupContent.Visibility = Visibility.Collapsed;
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
            StopAllTimers();
            ApplyBlurEffect();
            ShowUnifiedPopup("Data");
        }

        private void OnPopupMouseEnter(object sender, MouseEventArgs e)
        {
            // 立即停止所有计时器，防止 Popup 被关闭
            StopAllTimers();
            ApplyBlurEffect();
        }

        private void OnNavigationItemMouseLeave(object sender, MouseEventArgs e)
        {
            // 启动延迟检查
            popupCloseTimer?.Start();
            blurRemoveTimer?.Start();
        }

        // 保留这些方法以兼容 XAML 中的绑定
        private void ButtonOrPopup_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender == DataBtn)
                OnDataButtonMouseEnter(sender, e);
            else if (sender is Popup)
                OnPopupMouseEnter(sender, e);
        }

        private void ButtonOrPopup_MouseLeave(object sender, MouseEventArgs e)
        {
            OnNavigationItemMouseLeave(sender, e);
        }

        #endregion

        #region 统一Popup管理

        private void ShowUnifiedPopup(string contentType)
        {
            if (UnifiedPopup == null) return;

            // 确保停止所有计时器
            StopAllTimers();

            if (!UnifiedPopup.IsOpen)
            {
                UnifiedPopup.IsOpen = true;
            }

            // 如果内容类型没变，不需要切换
            if (currentPopupContent == contentType)
                return;

            currentPopupContent = contentType;

            // 使用动画切换内容
            var fadeOutDuration = TimeSpan.FromMilliseconds(100);
            var fadeInDuration = TimeSpan.FromMilliseconds(150);

            if (contentType == "Data" && DataPopupContent != null && FaultReportPopupContent != null)
            {
                // 如果故障报告内容可见，先淡出
                if (FaultReportPopupContent.Visibility == Visibility.Visible)
                {
                    var fadeOut = new DoubleAnimation(1, 0, fadeOutDuration);
                    fadeOut.Completed += (s, e) =>
                    {
                        FaultReportPopupContent.Visibility = Visibility.Collapsed;
                        ClearRadioButtonsInContainer(FaultReportPopupContent);

                        // 淡入数据内容
                        DataPopupContent.Visibility = Visibility.Visible;
                        var fadeIn = new DoubleAnimation(0, 1, fadeInDuration);
                        DataPopupContent.BeginAnimation(OpacityProperty, fadeIn);
                    };
                    FaultReportPopupContent.BeginAnimation(OpacityProperty, fadeOut);
                }
                else
                {
                    // 直接显示数据内容
                    DataPopupContent.Visibility = Visibility.Visible;
                    DataPopupContent.Opacity = 0;
                    var fadeIn = new DoubleAnimation(0, 1, fadeInDuration);
                    DataPopupContent.BeginAnimation(OpacityProperty, fadeIn);
                }
            }
        }

        private void CloseUnifiedPopup()
        {
            if (UnifiedPopup == null) return;

            UnifiedPopup.IsOpen = false;
            currentPopupContent = "";

            // 隐藏所有内容
            if (DataPopupContent != null)
            {
                DataPopupContent.Visibility = Visibility.Collapsed;
                ClearRadioButtonsInContainer(DataPopupContent);
            }
            if (FaultReportPopupContent != null)
            {
                FaultReportPopupContent.Visibility = Visibility.Collapsed;
                ClearRadioButtonsInContainer(FaultReportPopupContent);
            }
        }

        #endregion

        #region 计时器事件处理

        private void PopupCloseTimer_Tick(object? sender, EventArgs e)
        {
            popupCloseTimer?.Stop();

            // 双重检查 - 再次确认鼠标确实不在导航区域
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!IsMouseInNavigationArea())
                {
                    CloseUnifiedPopup();
                }
            }), DispatcherPriority.Background);
        }

        private void BlurRemoveTimer_Tick(object? sender, EventArgs e)
        {
            blurRemoveTimer?.Stop();

            // 双重检查 - 再次确认鼠标确实不在导航区域
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!IsMouseInNavigationArea())
                {
                    RemoveBlurEffect();
                }
            }), DispatcherPriority.Background);
        }

        #endregion

        #region 模糊效果处理

        private void ApplyBlurEffect()
        {
            if (Display == null) return;

            lock (blurLock)
            {
                if (isBlurActive) return;

                isBlurActive = true;
                if (NavigationPanel != null)
                    NavigationPanel.Background = System.Windows.Media.Brushes.White;

                if (!(Display.Effect is BlurEffect))
                {
                    var blur = new BlurEffect { Radius = 0 };
                    Display.Effect = blur;

                    var animation = new DoubleAnimation
                    {
                        From = 0,
                        To = 10,
                        Duration = TimeSpan.FromSeconds(0.4),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };

                    blur.BeginAnimation(BlurEffect.RadiusProperty, animation);
                }
            }
        }

        private void RemoveBlurEffect()
        {
            if (Display == null) return;

            lock (blurLock)
            {
                if (!isBlurActive) return;

                isBlurActive = false;
                if (NavigationPanel != null)
                    NavigationPanel.Background = System.Windows.Media.Brushes.Transparent;

                if (Display.Effect is BlurEffect blur)
                {
                    var animation = new DoubleAnimation
                    {
                        From = blur.Radius,
                        To = 0,
                        Duration = TimeSpan.FromSeconds(0.3),
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
                    };

                    animation.Completed += (s, _) =>
                    {
                        Display.Effect = null;
                    };

                    blur.BeginAnimation(BlurEffect.RadiusProperty, animation);
                }
            }
        }

        #endregion

        #region 辅助方法

        private bool IsMouseInNavigationArea()
        {
            // 检查按钮
            if (IsMouseOverElement(DataBtn) || (FaultReportBtn != null && IsMouseOverElement(FaultReportBtn)))
                return true;

            // 检查 Popup 及其内容
            if (UnifiedPopup != null && UnifiedPopup.IsOpen)
            {
                // 检查整个 Popup
                if (IsMouseOverElement(UnifiedPopup))
                    return true;

                // 也检查内容面板
                if ((DataPopupContent != null && DataPopupContent.Visibility == Visibility.Visible && IsMouseOverElement(DataPopupContent)) ||
                    (FaultReportPopupContent != null && FaultReportPopupContent.Visibility == Visibility.Visible && IsMouseOverElement(FaultReportPopupContent)))
                    return true;
            }

            return false;
        }

        private bool IsMouseOverElement(FrameworkElement? element)
        {
            if (element == null) return false;

            // 特殊处理 Popup
            if (element is Popup popup)
            {
                if (!popup.IsOpen) return false;

                // 获取 Popup 的子元素（通常是 Border）
                if (popup.Child is FrameworkElement child)
                {
                    try
                    {
                        var pos = Mouse.GetPosition(child);
                        return pos.X >= 0 && pos.Y >= 0 &&
                               pos.X <= child.ActualWidth &&
                               pos.Y <= child.ActualHeight;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }

            // 常规元素检测
            if (!element.IsVisible) return false;

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

        private void StopAllTimers()
        {
            popupCloseTimer?.Stop();
            blurRemoveTimer?.Stop();
        }

        private void OnPopupClosed(object? sender, EventArgs e)
        {
            CloseUnifiedPopup();
        }

        private void ClearRadioButtonsInContainer(DependencyObject? container)
        {
            if (container == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(container); i++)
            {
                var child = VisualTreeHelper.GetChild(container, i);

                if (child is RadioButton radioButton && radioButton.GroupName == "PopupMenuItems")
                {
                    radioButton.IsChecked = false;
                }
                else
                {
                    ClearRadioButtonsInContainer(child);
                }
            }
        }

        #endregion

        #region 窗口控制按钮

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
            // 清理资源
            StopAllTimers();

            // 取消事件订阅
            if (popupCloseTimer != null)
            {
                popupCloseTimer.Tick -= PopupCloseTimer_Tick;
            }
            if (blurRemoveTimer != null)
            {
                blurRemoveTimer.Tick -= BlurRemoveTimer_Tick;
            }

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
