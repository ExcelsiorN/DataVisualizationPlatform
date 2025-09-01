using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DataVisualizationPlatform.Views
{
    public partial class MainMenu : Page
    {
        private const int TotalPages = 3;
        private double _pageWidth;
        private int _currentPage = 0;
        private bool _isDragging = false;
        private Point _startPoint;
        private double _startLeft;

        public MainMenu()
        {
            InitializeComponent();
            this.Loaded += MainMenu_Loaded;
            this.SizeChanged += MainMenu_SizeChanged;
        }

        private void MainMenu_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
            _pageWidth = this.ActualWidth;
            UpdatePageSizes();
            UpdateIndicators();
            SetCurrentPage(0, false);
        }

        private void MainMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width > 0)
            {
                _pageWidth = e.NewSize.Width;
                UpdatePageSizes();
                SetCurrentPage(_currentPage, false);
            }
        }

        private void UpdatePageSizes()
        {
            if (_pageWidth <= 0 || PagesContainer == null) return;

            foreach (Border border in PagesContainer.Children)
            {
                border.Width = _pageWidth;
                border.Height = this.ActualHeight;
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(MainCanvas);
            _startLeft = Canvas.GetLeft(PagesContainer);
            MainCanvas.CaptureMouse();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            Point currentPoint = e.GetPosition(MainCanvas);
            double deltaX = currentPoint.X - _startPoint.X;
            double newLeft = _startLeft + deltaX;

            Canvas.SetLeft(PagesContainer, newLeft);
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDragging) return;

            _isDragging = false;
            MainCanvas.ReleaseMouseCapture();

            Point endPoint = e.GetPosition(MainCanvas);
            double deltaX = endPoint.X - _startPoint.X;

            // 判断滑动方向和距离
            if (Math.Abs(deltaX) > _pageWidth * 0.3) // 滑动距离超过30%才切换
            {
                if (deltaX > 0) // 向右滑动，显示上一页
                {
                    NavigateToPreviousPage();
                }
                else // 向左滑动，显示下一页
                {
                    NavigateToNextPage();
                }
            }
            else
            {
                // 滑动距离不够，回到当前页
                SetCurrentPage(_currentPage, true);
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                Canvas_MouseLeftButtonUp(sender, null);
            }
        }

        private void NavigateToNextPage()
        {
            if (_currentPage == TotalPages - 1) // 从页面3到页面1
            {
                // 使用动画滑动到复制的页面1，然后无缝跳回真正的页面1
                SetCurrentPageWithCycleAnimation(TotalPages, 0, true);
            }
            else
            {
                _currentPage = (_currentPage + 1) % TotalPages;
                SetCurrentPage(_currentPage, true);
            }
        }

        private void NavigateToPreviousPage()
        {
            if (_currentPage == 0) // 从页面1到页面3
            {
                // 先立即跳到复制页面1的位置，然后动画滑动到页面3
                Canvas.SetLeft(PagesContainer, -TotalPages * _pageWidth);
                _currentPage = TotalPages - 1;
                SetCurrentPage(_currentPage, true);
            }
            else
            {
                _currentPage = (_currentPage - 1 + TotalPages) % TotalPages;
                SetCurrentPage(_currentPage, true);
            }
        }

        private void SetCurrentPageWithCycleAnimation(int animateToIndex, int finalPageIndex, bool isForward)
        {
            if (_pageWidth <= 0) return;

            double targetLeft = -animateToIndex * _pageWidth;

            var animation = new DoubleAnimation
            {
                From = Canvas.GetLeft(PagesContainer),
                To = targetLeft,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            animation.Completed += (s, e) => {
                // 动画完成后，立即跳到真正的目标页面
                _currentPage = finalPageIndex;
                Canvas.SetLeft(PagesContainer, -finalPageIndex * _pageWidth);
                UpdateIndicators();
            };

            PagesContainer.BeginAnimation(Canvas.LeftProperty, animation);
            // 先更新指示器显示最终页面
            _currentPage = finalPageIndex;
            UpdateIndicators();
        }

        private void SetCurrentPage(int pageIndex, bool animated)
        {
            if (_pageWidth <= 0) return;

            double targetLeft = -pageIndex * _pageWidth;

            if (animated)
            {
                var animation = new DoubleAnimation
                {
                    From = Canvas.GetLeft(PagesContainer),
                    To = targetLeft,
                    Duration = TimeSpan.FromMilliseconds(300),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                PagesContainer.BeginAnimation(Canvas.LeftProperty, animation);
            }
            else
            {
                Canvas.SetLeft(PagesContainer, targetLeft);
            }

            UpdateIndicators();
        }

        private void UpdateIndicators()
        {
            if (Indicator1 == null) return;

            // 重置所有指示器
            Indicator1.Fill = new SolidColorBrush(Colors.Gray);
            Indicator2.Fill = new SolidColorBrush(Colors.Gray);
            Indicator3.Fill = new SolidColorBrush(Colors.Gray);

            // 设置当前页面的指示器
            switch (_currentPage)
            {
                case 0:
                    Indicator1.Fill = new SolidColorBrush(Colors.White);
                    break;
                case 1:
                    Indicator2.Fill = new SolidColorBrush(Colors.White);
                    break;
                case 2:
                    Indicator3.Fill = new SolidColorBrush(Colors.White);
                    break;
            }
        }
    }
}