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

        private DateTime _dragStartTime;

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _startPoint = e.GetPosition(MainCanvas);
            _startLeft = Canvas.GetLeft(PagesContainer);
            _dragStartTime = DateTime.Now;

            // 取消未完成动画
            PagesContainer.BeginAnimation(Canvas.LeftProperty, null);
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
            double elapsed = (DateTime.Now - _dragStartTime).TotalMilliseconds;
            double velocity = deltaX / Math.Max(elapsed, 1); // 防止除零

            // 判断滑动切换条件：距离 > 10% 或 速度足够快
            if (Math.Abs(deltaX) > _pageWidth * 0.1 || Math.Abs(velocity) > 1.0)
            {
                if (deltaX > 0)
                    NavigateToPreviousPage();
                else
                    NavigateToNextPage();
            }
            else
            {
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
            double currentLeft = Canvas.GetLeft(PagesContainer);

            // 只有当前确实在最后一页时才允许循环
            if (_currentPage == TotalPages - 1 && Math.Abs(currentLeft + (_currentPage * _pageWidth)) < _pageWidth * 0.5)
            {
                // 从页面3到页面1
                SetCurrentPageWithCycleAnimation(TotalPages, 0, true);
            }
            else
            {
                _currentPage = Math.Min(_currentPage + 1, TotalPages - 1);
                SetCurrentPage(_currentPage, true);
            }
        }


        private void NavigateToPreviousPage()
        {
            double currentLeft = Canvas.GetLeft(PagesContainer);

            // 只有当前确实在第一页时才允许循环
            if (_currentPage == 0 && Math.Abs(currentLeft) < _pageWidth * 0.5)
            {
                Canvas.SetLeft(PagesContainer, -TotalPages * _pageWidth);
                _currentPage = TotalPages - 1;
                SetCurrentPage(_currentPage, true);
            }
            else
            {
                _currentPage = Math.Max(_currentPage - 1, 0);
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
                // 修复关键：确保动画起点是当前页的标准位置，而不是拖拽中间位置
                double currentLeft = -_currentPage * _pageWidth;
                PagesContainer.BeginAnimation(Canvas.LeftProperty, null); // 取消可能未完成的动画
                Canvas.SetLeft(PagesContainer, currentLeft);

                var animation = new DoubleAnimation
                {
                    From = currentLeft, // 关键修改：从标准页位置开始
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
            if (Indicator1 == null || Indicator2 == null || Indicator3 == null) return;

            var indicators = new[] { Indicator1, Indicator2, Indicator3 };
            for (int i = 0; i < indicators.Length; i++)
            {
                indicators[i].Fill = (i == _currentPage) ? Brushes.White : Brushes.Gray;
            }
        }
    }
}