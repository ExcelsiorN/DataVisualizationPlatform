using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.ViewModels;
using System.Diagnostics;


namespace DataVisualizationPlatform.Controls
{
    #region 动画折线图控件
    public class AnimatedLineChart : Canvas
    {
        public static readonly DependencyProperty DataPointsProperty =
            DependencyProperty.Register(nameof(DataPoints), typeof(ObservableCollection<LineDataPoint>),
                typeof(AnimatedLineChart), new PropertyMetadata(null, OnDataChanged));

        public static readonly DependencyProperty AnimationProgressProperty =
            DependencyProperty.Register(nameof(AnimationProgress), typeof(double),
                typeof(AnimatedLineChart), new PropertyMetadata(0.0, OnAnimationProgressChanged));

        public ObservableCollection<LineDataPoint> DataPoints
        {
            get => (ObservableCollection<LineDataPoint>)GetValue(DataPointsProperty);
            set => SetValue(DataPointsProperty, value);
        }

        public double AnimationProgress
        {
            get => (double)GetValue(AnimationProgressProperty);
            set => SetValue(AnimationProgressProperty, value);
        }

        // 缓存UI元素
        private List<Line> _gridLines = new List<Line>();
        private List<TextBlock> _yAxisLabels = new List<TextBlock>();
        private List<TextBlock> _xAxisLabels = new List<TextBlock>();
        private Line _xAxis;
        private Line _yAxis;
        private Path _linePath;
        private PathGeometry _pathGeometry;
        private PathFigure _pathFigure;
        private List<Ellipse> _dataPointDots = new List<Ellipse>();
        private List<TextBlock> _valueLabels = new List<TextBlock>();
        private List<Point> _calculatedPoints = new List<Point>();

        // 缓存计算值
        private double _lastChartWidth;
        private double _lastChartHeight;
        private double _lastMaxY;
        private double _lastMinY;
        private bool _needsFullRedraw = true;
        private double _scaleX;
        private double _scaleY;
        private double _padding = 40;
        private double _bottomPadding = 60;

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedLineChart chart)
            {
                chart._needsFullRedraw = true;
                chart.UpdateChart();
            }
        }

        private static void OnAnimationProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedLineChart chart)
            {
                // 更新动画
                chart.UpdateLineAnimation();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _needsFullRedraw = true;
            UpdateChart();
        }

        private void UpdateChart()
        {
            if (DataPoints == null || DataPoints.Count == 0 || ActualWidth == 0 || ActualHeight == 0)
            {
                Children.Clear();
                return;
            }

            double chartWidth = ActualWidth - 2 * _padding;
            double chartHeight = ActualHeight - _padding - _bottomPadding;

            if (chartWidth <= 0 || chartHeight <= 0) return;

            double minY = DataPoints.Min(p => p.Y);
            double maxY = DataPoints.Max(p => p.Y);

            // 添加边距
            double range = maxY - minY;
            if (range == 0) range = 1;
            minY -= range * 0.1;
            maxY += range * 0.1;

            // 检查是否需要重绘
            bool dimensionsChanged = Math.Abs(_lastChartWidth - chartWidth) > 0.1 ||
                                    Math.Abs(_lastChartHeight - chartHeight) > 0.1;
            bool scaleChanged = Math.Abs(_lastMaxY - maxY) > 0.1 ||
                               Math.Abs(_lastMinY - minY) > 0.1;

            if (_needsFullRedraw || dimensionsChanged || scaleChanged)
            {
                _lastChartWidth = chartWidth;
                _lastChartHeight = chartHeight;
                _lastMaxY = maxY;
                _lastMinY = minY;

                // 计算缩放比例
                _scaleX = chartWidth / Math.Max(1, DataPoints.Count - 1);
                _scaleY = chartHeight / (maxY - minY);

                // 完全重绘静态元素
                FullRedraw(chartWidth, chartHeight, minY, maxY);
                _needsFullRedraw = false;
            }

            // 更新动画
            UpdateLineAnimation();
        }

        private void FullRedraw(double chartWidth, double chartHeight, double minY, double maxY)
        {
            Children.Clear();
            _gridLines.Clear();
            _yAxisLabels.Clear();
            _xAxisLabels.Clear();
            _dataPointDots.Clear();
            _valueLabels.Clear();
            _calculatedPoints.Clear();

            // 绘制网格线和Y轴标签
            DrawGridAndAxes(chartWidth, chartHeight, minY, maxY);

            // 预计算所有点的位置
            for (int i = 0; i < DataPoints.Count; i++)
            {
                var point = DataPoints[i];
                double x = _padding + i * _scaleX;
                double y = _padding + chartHeight - (point.Y - minY) * _scaleY;
                _calculatedPoints.Add(new Point(x, y));
            }

            // 创建折线路径（初始为空）
            _pathGeometry = new PathGeometry();
            _pathFigure = new PathFigure();
            _pathGeometry.Figures.Add(_pathFigure);

            _linePath = new Path
            {
                Data = _pathGeometry,
                Stroke = new LinearGradientBrush
                {
                    StartPoint = new Point(0, 0),
                    EndPoint = new Point(1, 0),
                    GradientStops = new GradientStopCollection
                    {
                        new GradientStop(Color.FromRgb(59, 130, 246), 0),
                        new GradientStop(Color.FromRgb(147, 51, 234), 1)
                    }
                },
                StrokeThickness = 3,
                Fill = Brushes.Transparent,
                StrokeLineJoin = PenLineJoin.Round,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round,
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    Color = Color.FromRgb(59, 130, 246),
                    BlurRadius = 10,
                    ShadowDepth = 0,
                    Opacity = 0.3
                }
            };
            Children.Add(_linePath);

            // 预创建所有数据点（初始隐藏）
            for (int i = 0; i < DataPoints.Count; i++)
            {
                var point = _calculatedPoints[i];

                // 创建数据点圆点
                var ellipse = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Color.FromRgb(59, 130, 246)),
                    Stroke = new SolidColorBrush(Colors.White),
                    StrokeThickness = 2,
                    Visibility = Visibility.Hidden,
                    Effect = new System.Windows.Media.Effects.DropShadowEffect
                    {
                        Color = Colors.Black,
                        BlurRadius = 4,
                        ShadowDepth = 2,
                        Opacity = 0.2
                    }
                };
                SetLeft(ellipse, point.X - 5);
                SetTop(ellipse, point.Y - 5);
                SetZIndex(ellipse, 10);
                _dataPointDots.Add(ellipse);
                Children.Add(ellipse);

                // 创建数值标签（初始隐藏）
                var valueLabel = new TextBlock
                {
                    Text = DataPoints[i].Y.ToString("F1"),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Color.FromRgb(75, 85, 99)),
                    FontWeight = FontWeights.SemiBold,
                    Visibility = Visibility.Hidden,
                    Background = new SolidColorBrush(Color.FromArgb(240, 255, 255, 255)),
                    Padding = new Thickness(4, 2, 4, 2)
                };

                valueLabel.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                double labelWidth = valueLabel.DesiredSize.Width;
                double labelHeight = valueLabel.DesiredSize.Height;

                SetLeft(valueLabel, point.X - labelWidth / 2);
                SetTop(valueLabel, point.Y - labelHeight - 12);
                SetZIndex(valueLabel, 11);
                _valueLabels.Add(valueLabel);
                Children.Add(valueLabel);

                // 创建X轴标签
                if (i % Math.Max(1, DataPoints.Count / 10) == 0 || i == DataPoints.Count - 1)
                {
                    var xLabel = new TextBlock
                    {
                        Text = DataPoints[i].Label ?? i.ToString(),
                        FontFamily = new FontFamily("Segoe UI"),
                        FontSize = 9,
                        Foreground = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                        TextAlignment = TextAlignment.Center
                    };

                    xLabel.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    double xLabelWidth = xLabel.DesiredSize.Width;

                    SetLeft(xLabel, point.X - xLabelWidth / 2);
                    SetTop(xLabel, _padding + chartHeight + 5);
                    _xAxisLabels.Add(xLabel);
                    Children.Add(xLabel);
                }
            }

            Debug.WriteLine($"[AnimatedLineChart] 创建了 {DataPoints.Count} 个数据点");
        }

        private void DrawGridAndAxes(double chartWidth, double chartHeight, double minY, double maxY)
        {
            // 绘制水平网格线和Y轴标签
            int gridLineCount = 5;
            for (int i = 0; i <= gridLineCount; i++)
            {
                double value = minY + (maxY - minY) * i / gridLineCount;
                double y = _padding + chartHeight - (value - minY) * _scaleY;

                // 网格线
                var gridLine = new Line
                {
                    X1 = _padding,
                    Y1 = y,
                    X2 = _padding + chartWidth,
                    Y2 = y,
                    Stroke = new SolidColorBrush(Color.FromRgb(243, 244, 246)),
                    StrokeThickness = 1,
                    StrokeDashArray = i % gridLineCount == 0 ? null : new DoubleCollection { 5, 5 }
                };
                SetZIndex(gridLine, 0);
                _gridLines.Add(gridLine);
                Children.Add(gridLine);

                // Y轴标签
                var label = new TextBlock
                {
                    Text = value.ToString("F1"),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                    TextAlignment = TextAlignment.Right
                };

                label.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                SetLeft(label, _padding - label.DesiredSize.Width - 8);
                SetTop(label, y - label.DesiredSize.Height / 2);
                _yAxisLabels.Add(label);
                Children.Add(label);
            }

            // 绘制垂直网格线（可选）
            for (int i = 0; i < DataPoints.Count; i++)
            {
                if (i % Math.Max(1, DataPoints.Count / 10) == 0)
                {
                    double x = _padding + i * _scaleX;
                    var vGridLine = new Line
                    {
                        X1 = x,
                        Y1 = _padding,
                        X2 = x,
                        Y2 = _padding + chartHeight,
                        Stroke = new SolidColorBrush(Color.FromRgb(243, 244, 246)),
                        StrokeThickness = 1,
                        StrokeDashArray = new DoubleCollection { 5, 5 }
                    };
                    SetZIndex(vGridLine, 0);
                    _gridLines.Add(vGridLine);
                    Children.Add(vGridLine);
                }
            }

            // X轴
            _xAxis = new Line
            {
                X1 = _padding,
                Y1 = _padding + chartHeight,
                X2 = _padding + chartWidth,
                Y2 = _padding + chartHeight,
                Stroke = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                StrokeThickness = 2
            };
            SetZIndex(_xAxis, 1);
            Children.Add(_xAxis);

            // Y轴
            _yAxis = new Line
            {
                X1 = _padding,
                Y1 = _padding,
                X2 = _padding,
                Y2 = _padding + chartHeight,
                Stroke = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                StrokeThickness = 2
            };
            SetZIndex(_yAxis, 1);
            Children.Add(_yAxis);
        }

        private void UpdateLineAnimation()
        {
            if (_calculatedPoints == null || _calculatedPoints.Count == 0 ||
                _pathFigure == null || _dataPointDots == null) return;

            // 计算当前应该显示多少个点
            int visiblePointCount = (int)Math.Ceiling(AnimationProgress * _calculatedPoints.Count);
            visiblePointCount = Math.Max(0, Math.Min(_calculatedPoints.Count, visiblePointCount));

            //Debug.WriteLine($"[UpdateLineAnimation] Progress={AnimationProgress:F3}, VisiblePoints={visiblePointCount}/{_calculatedPoints.Count}");

            // 清除现有路径
            _pathFigure.Segments.Clear();

            if (visiblePointCount > 0)
            {
                // 设置起始点
                _pathFigure.StartPoint = _calculatedPoints[0];

                // 构建路径段，使用贝塞尔曲线使线条更平滑
                for (int i = 1; i < visiblePointCount; i++)
                {
                    if (i < visiblePointCount - 1)
                    {
                        // 使用二次贝塞尔曲线连接点，创建平滑效果
                        var point1 = _calculatedPoints[i - 1];
                        var point2 = _calculatedPoints[i];
                        var point3 = i + 1 < _calculatedPoints.Count ? _calculatedPoints[i + 1] : _calculatedPoints[i];

                        // 计算控制点
                        double smoothing = 0.2;
                        var controlPoint1 = new Point(
                            point1.X + (point2.X - point1.X) * (1 - smoothing),
                            point1.Y + (point2.Y - point1.Y) * (1 - smoothing)
                        );
                        var controlPoint2 = new Point(
                            point2.X - (point3.X - point1.X) * smoothing,
                            point2.Y - (point3.Y - point1.Y) * smoothing
                        );

                        _pathFigure.Segments.Add(new BezierSegment(controlPoint1, controlPoint2, point2, true));
                    }
                    else
                    {
                        // 最后一个点使用直线
                        double partialProgress = AnimationProgress * _calculatedPoints.Count - (visiblePointCount - 1);
                        partialProgress = EaseOutCubic(Math.Min(1, Math.Max(0, partialProgress)));

                        var lastPoint = _calculatedPoints[i - 1];
                        var targetPoint = _calculatedPoints[i];
                        var animatedPoint = new Point(
                            lastPoint.X + (targetPoint.X - lastPoint.X) * partialProgress,
                            lastPoint.Y + (targetPoint.Y - lastPoint.Y) * partialProgress
                        );

                        _pathFigure.Segments.Add(new LineSegment(animatedPoint, true));
                    }
                }
            }

            // 更新数据点的可见性和动画
            for (int i = 0; i < _calculatedPoints.Count; i++)
            {
                // 计算每个点的动画进度
                double pointProgress = AnimationProgress * _calculatedPoints.Count - i;
                pointProgress = Math.Min(1, Math.Max(0, pointProgress));

                if (i < _dataPointDots.Count)
                {
                    var dot = _dataPointDots[i];
                    var label = _valueLabels[i];

                    if (pointProgress > 0)
                    {
                        // 显示点并应用缩放动画
                        dot.Visibility = Visibility.Visible;
                        double scale = EaseOutBack(pointProgress);
                        dot.RenderTransform = new ScaleTransform(scale, scale, 5, 5);
                        dot.Opacity = pointProgress;

                        // 显示标签（带延迟）
                        if (pointProgress > 0.5)
                        {
                            label.Visibility = Visibility.Visible;
                            label.Opacity = (pointProgress - 0.5) * 2;

                            // 标签上浮动画
                            var currentY = _calculatedPoints[i].Y - 12 - label.DesiredSize.Height;
                            var floatOffset = (1 - pointProgress) * 10;
                            SetTop(label, currentY - floatOffset);
                        }
                        else
                        {
                            label.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        dot.Visibility = Visibility.Hidden;
                        label.Visibility = Visibility.Hidden;
                    }
                }
            }

            // 确保动画完成时所有元素都正确显示
            if (AnimationProgress >= 0.99)
            {
                for (int i = 0; i < _calculatedPoints.Count; i++)
                {
                    if (i < _dataPointDots.Count)
                    {
                        _dataPointDots[i].Visibility = Visibility.Visible;
                        _dataPointDots[i].Opacity = 1;
                        _dataPointDots[i].RenderTransform = new ScaleTransform(1, 1, 5, 5);

                        _valueLabels[i].Visibility = Visibility.Visible;
                        _valueLabels[i].Opacity = 1;
                        SetTop(_valueLabels[i], _calculatedPoints[i].Y - 12 - _valueLabels[i].DesiredSize.Height);
                    }
                }
                Debug.WriteLine("[UpdateLineAnimation] 动画完成，所有元素显示");
            }
        }

        // 缓动函数 - 先快后慢
        private double EaseOutCubic(double t)
        {
            return 1 - Math.Pow(1 - t, 3);
        }

        // 缓动函数 - 带回弹效果（用于点的出现）
        private double EaseOutBack(double t)
        {
            const double c1 = 1.70158;
            const double c3 = c1 + 1;
            return 1 + c3 * Math.Pow(t - 1, 3) + c1 * Math.Pow(t - 1, 2);
        }
    }
    #endregion

    #region 动画柱状图控件
    public class AnimatedBarChart : Canvas
    {
        public static readonly DependencyProperty DataItemsProperty =
            DependencyProperty.Register(nameof(DataItems), typeof(ObservableCollection<BarDataItem>),
                typeof(AnimatedBarChart), new PropertyMetadata(null, OnDataChanged));

        public static readonly DependencyProperty AnimationProgressProperty =
            DependencyProperty.Register(nameof(AnimationProgress), typeof(double),
                typeof(AnimatedBarChart), new PropertyMetadata(0.0, OnAnimationProgressChanged));

        public ObservableCollection<BarDataItem> DataItems
        {
            get => (ObservableCollection<BarDataItem>)GetValue(DataItemsProperty);
            set => SetValue(DataItemsProperty, value);
        }

        public double AnimationProgress
        {
            get => (double)GetValue(AnimationProgressProperty);
            set => SetValue(AnimationProgressProperty, value);
        }

        // 缓存UI元素
        private List<Line> _gridLines = new List<Line>();
        private List<TextBlock> _axisLabels = new List<TextBlock>();
        private Line _xAxis;
        private Line _yAxis;
        private List<Rectangle> _barPart1Elements = new List<Rectangle>();
        private List<Rectangle> _barPart2Elements = new List<Rectangle>();
        private List<TextBlock> _valueLabels = new List<TextBlock>();
        private List<TextBlock> _faultLabels = new List<TextBlock>();
        private List<TextBlock> _xAxisLabels = new List<TextBlock>(); // 添加X轴标签缓存

        // 缓存计算值
        private double _lastChartWidth;
        private double _lastChartHeight;
        private double _lastMaxTotal;
        private bool _needsFullRedraw = true;

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedBarChart chart)
            {
                chart._needsFullRedraw = true;
                chart.UpdateChart();
            }
        }

        private static void OnAnimationProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AnimatedBarChart chart)
            {
                // 动画进度改变时只更新柱子，不重建整个图表
                chart.UpdateBarsOnly();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            _needsFullRedraw = true;
            UpdateChart();
        }

        private void UpdateChart()
        {
            if (DataItems == null || DataItems.Count == 0 || ActualWidth == 0 || ActualHeight == 0)
            {
                Children.Clear();
                return;
            }

            const double padding = 40;
            const double bottomPadding = 60; // 增加底部间距，为X轴标签留出空间
            double chartWidth = ActualWidth - 2 * padding;
            double chartHeight = ActualHeight - padding - bottomPadding;

            if (chartWidth <= 0 || chartHeight <= 0) return;

            double maxTotal = DataItems.Max(item => item.Total);
            // 确保最大值不为0，避免除零错误
            if (maxTotal == 0) maxTotal = 100;

            // 检查是否需要完全重绘
            bool dimensionsChanged = Math.Abs(_lastChartWidth - chartWidth) > 0.1 ||
                                    Math.Abs(_lastChartHeight - chartHeight) > 0.1;
            bool scaleChanged = Math.Abs(_lastMaxTotal - maxTotal) > 0.1;

            if (_needsFullRedraw || dimensionsChanged || scaleChanged)
            {
                _lastChartWidth = chartWidth;
                _lastChartHeight = chartHeight;
                _lastMaxTotal = maxTotal;

                // 完全重绘
                FullRedraw(padding, bottomPadding, chartWidth, chartHeight, maxTotal);
                _needsFullRedraw = false;
            }

            // 更新柱子动画
            UpdateBarsOnly();
        }

        private void FullRedraw(double padding, double bottomPadding, double chartWidth, double chartHeight, double maxTotal)
        {
            Children.Clear();
            _gridLines.Clear();
            _axisLabels.Clear();
            _barPart1Elements.Clear();
            _barPart2Elements.Clear();
            _valueLabels.Clear();
            _faultLabels.Clear(); // 清空faultLabels
            _xAxisLabels.Clear();

            double scaleY = chartHeight / maxTotal;

            // 创建网格线和Y轴标签（静态元素）
            for (int i = 0; i <= 4; i++)
            {
                double value = i * maxTotal / 4;
                double y = padding + chartHeight - value * scaleY;

                var gridLine = new Line
                {
                    X1 = padding,
                    Y1 = y,
                    X2 = padding + chartWidth,
                    Y2 = y,
                    Stroke = new SolidColorBrush(Color.FromRgb(243, 244, 246)),
                    StrokeThickness = 1
                };
                _gridLines.Add(gridLine);
                Children.Add(gridLine);

                var label = new TextBlock
                {
                    Text = value.ToString("F0"),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 10,
                    Foreground = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };
                SetLeft(label, padding - 30);
                SetTop(label, y - 8);
                _axisLabels.Add(label);
                Children.Add(label);
            }

            // 创建坐标轴（静态元素）
            _xAxis = new Line
            {
                X1 = padding,
                Y1 = padding + chartHeight,
                X2 = padding + chartWidth,
                Y2 = padding + chartHeight,
                Stroke = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                StrokeThickness = 2
            };
            Children.Add(_xAxis);

            _yAxis = new Line
            {
                X1 = padding,
                Y1 = padding,
                X2 = padding,
                Y2 = padding + chartHeight,
                Stroke = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                StrokeThickness = 2
            };
            Children.Add(_yAxis);

            // 预创建柱子元素
            double barWidth = Math.Min(50, chartWidth / DataItems.Count * 0.6); // 限制最大宽度
            double barSpacing = chartWidth / DataItems.Count;

            var color1 = Color.FromRgb(16, 185, 129);   // 绿色
            var color2 = Color.FromRgb(59, 130, 246);   // 蓝色

            int? currentYear = null;
            bool useColor1 = true;

            //Debug.WriteLine($"[AnimatedBarChart] 创建 {DataItems.Count} 个柱子，宽度={barWidth:F1}，间距={barSpacing:F1}");

            for (int i = 0; i < DataItems.Count; i++)
            {
                var item = DataItems[i];
                double x = padding + i * barSpacing + (barSpacing - barWidth) / 2;

                // 确定颜色
                if (!string.IsNullOrEmpty(item.Label) &&
                    DateTime.TryParseExact(item.Label, "yyyy-MM",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime itemDate))
                {
                    if (currentYear != itemDate.Year)
                    {
                        if (currentYear != null)
                        {
                            useColor1 = !useColor1;
                        }
                        currentYear = itemDate.Year;
                    }
                }

                // 创建第一部分柱子（初始高度为0）
                var part1Rect = new Rectangle
                {
                    Width = barWidth,
                    Height = 0,
                    Fill = new SolidColorBrush(useColor1 ? color1 : color2),
                    StrokeThickness = 0,
                    RadiusX = 2,
                    RadiusY = 2
                };
                SetLeft(part1Rect, x);
                SetTop(part1Rect, padding + chartHeight);
                _barPart1Elements.Add(part1Rect);
                Children.Add(part1Rect);

                // 创建第二部分柱子（初始高度为0）
                var part2Rect = new Rectangle
                {
                    Width = barWidth,
                    Height = 0,
                    Fill = new SolidColorBrush(Color.FromRgb(99, 102, 241)),
                    StrokeThickness = 0,
                    RadiusX = 2,
                    RadiusY = 2
                };
                SetLeft(part2Rect, x);
                SetTop(part2Rect, padding + chartHeight);
                _barPart2Elements.Add(part2Rect);
                Children.Add(part2Rect);

                // 创建数值标签（初始隐藏）
                // Part1 的标签
                var valueLabel = new TextBlock
                {
                    Text = item.Part1 > 0 ? $"{item.Part1:F0}" : "", // 0值不显示
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Colors.White), // 改为白色在柱体内更清晰
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    Visibility = Visibility.Hidden
                };

                // Part2 的标签
                var faultLabel = new TextBlock
                {
                    Text = item.Part2 > 0 ? $"{item.Part2:F0}" : "", // 0值不显示
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Colors.White), // 改为白色在柱体内更清晰
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    Visibility = Visibility.Hidden
                };

                // 测量文本大小以便准确定位
                valueLabel.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                double valueLabelWidth = valueLabel.DesiredSize.Width;

                faultLabel.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                double faultLabelWidth = faultLabel.DesiredSize.Width;

                SetLeft(valueLabel, x + barWidth / 2 - valueLabelWidth / 2);
                SetLeft(faultLabel, x + barWidth / 2 - faultLabelWidth / 2);

                _valueLabels.Add(valueLabel);
                _faultLabels.Add(faultLabel);
                Children.Add(valueLabel);
                Children.Add(faultLabel);

                // 创建X轴标签（显示月份）
                if (i % Math.Max(1, DataItems.Count / 12) == 0) // 控制显示密度
                {
                    var xLabel = new TextBlock
                    {
                        Text = item.Label,
                        FontFamily = new FontFamily("Segoe UI"),
                        FontSize = 9,
                        Foreground = new SolidColorBrush(Color.FromRgb(107, 114, 128)),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = TextAlignment.Center
                    };

                    xLabel.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    double xLabelWidth = xLabel.DesiredSize.Width;

                    SetLeft(xLabel, x + barWidth / 2 - xLabelWidth / 2);
                    SetTop(xLabel, padding + chartHeight + 5);
                    _xAxisLabels.Add(xLabel);
                    Children.Add(xLabel);
                }

                //Debug.WriteLine($"[AnimatedBarChart] 柱子[{i}] {item.Label}: Part1={item.Part1}, Part2={item.Part2}, Total={item.Total}");
            }
        }

        private void UpdateBarsOnly()
        {
            if (DataItems == null || DataItems.Count == 0 ||
                _barPart1Elements.Count != DataItems.Count) return;

            const double padding = 40;
            const double bottomPadding = 60;
            double chartHeight = ActualHeight - padding - bottomPadding;
            if (chartHeight <= 0) return;

            double maxTotal = DataItems.Max(item => item.Total);
            if (maxTotal == 0) maxTotal = 100;
            double scaleY = chartHeight / maxTotal;

            // 修改动画参数，确保所有柱子都能显示
            // 使用更合理的动画时间分配
            double animationWindow = 0.7; // 70%的时间用于动画展开
            double itemDelay = DataItems.Count > 1 ? animationWindow / DataItems.Count : 0;
            double itemDuration = 0.3; // 每个柱子的动画持续时间

            //Debug.WriteLine($"[UpdateBarsOnly] Progress={AnimationProgress:F3}, Items={DataItems.Count}");

            for (int i = 0; i < DataItems.Count; i++)
            {
                var item = DataItems[i];
                double baseY = padding + chartHeight;

                // 计算当前柱子的动画进度
                double itemStartTime = i * itemDelay;
                double itemEndTime = Math.Min(1.0, itemStartTime + itemDuration);
                double barProgress = 0;

                if (AnimationProgress >= itemEndTime)
                {
                    barProgress = 1.0;
                }
                else if (AnimationProgress > itemStartTime)
                {
                    barProgress = (AnimationProgress - itemStartTime) / itemDuration;
                    // 使用缓动函数使动画更平滑
                    barProgress = EaseOutCubic(barProgress);
                }

                double part1Height = item.Part1 * scaleY * barProgress;
                double part2Height = item.Part2 * scaleY * barProgress;
                double totalHeight = part1Height + part2Height;

                // 更新第一部分柱子
                var part1Rect = _barPart1Elements[i];
                part1Rect.Height = part1Height;
                SetTop(part1Rect, baseY - part1Height);

                // 更新第二部分柱子
                var part2Rect = _barPart2Elements[i];
                part2Rect.Height = part2Height;
                SetTop(part2Rect, baseY - totalHeight);

                // 更新数值标签位置和可见性
                var valueLabel = _valueLabels[i];
                var faultLabel = _faultLabels[i]; // 修正：从正确的集合获取

                // Part1 标签位置 - 在Part1柱体的中央
                if (item.Part1 > 0 && part1Height > 15) // 只有柱体高度足够时才显示
                {
                    double valueLabelY = baseY - part1Height / 2 - valueLabel.DesiredSize.Height / 2;
                    SetTop(valueLabel, valueLabelY);

                    bool shouldShowPart1Label = barProgress > 0.5 && part1Height > 15;
                    valueLabel.Visibility = shouldShowPart1Label ? Visibility.Visible : Visibility.Hidden;

                    if (shouldShowPart1Label)
                    {
                        double labelOpacity = Math.Min(1.0, (barProgress - 0.5) * 2);
                        valueLabel.Opacity = labelOpacity;
                    }
                }
                else
                {
                    valueLabel.Visibility = Visibility.Hidden;
                }

                // Part2 标签位置 - 在Part2柱体的中央
                if (item.Part2 > 0 && part2Height > 15) // 只有柱体高度足够时才显示
                {
                    double faultLabelY = baseY - totalHeight + part2Height / 2 - faultLabel.DesiredSize.Height / 2;
                    SetTop(faultLabel, faultLabelY);

                    bool shouldShowPart2Label = barProgress > 0.5 && part2Height > 15;
                    faultLabel.Visibility = shouldShowPart2Label ? Visibility.Visible : Visibility.Hidden;

                    if (shouldShowPart2Label)
                    {
                        double labelOpacity = Math.Min(1.0, (barProgress - 0.5) * 2);
                        faultLabel.Opacity = labelOpacity;
                    }
                }
                else
                {
                    faultLabel.Visibility = Visibility.Hidden;
                }

                // 调试输出关键数据
                //if (i == DataItems.Count - 1 || (item.Label != null && item.Label.StartsWith("2025")))
                //{
                //    Debug.WriteLine($"  柱子[{i}] {item.Label}: Progress={barProgress:F3}, Part1Height={part1Height:F1}, Part2Height={part2Height:F1}");
                //}
            }

            // 确保在动画完成时所有符合条件的元素都显示
            if (AnimationProgress >= 0.99)
            {
                for (int i = 0; i < DataItems.Count; i++)
                {
                    var item = DataItems[i];
                    double part1Height = item.Part1 * scaleY;
                    double part2Height = item.Part2 * scaleY;

                    if (item.Part1 > 0 && part1Height > 15)
                    {
                        _valueLabels[i].Visibility = Visibility.Visible;
                        _valueLabels[i].Opacity = 1.0;
                    }

                    if (item.Part2 > 0 && part2Height > 15)
                    {
                        _faultLabels[i].Visibility = Visibility.Visible;
                        _faultLabels[i].Opacity = 1.0;
                    }
                }
                Debug.WriteLine("[UpdateBarsOnly] 动画完成，显示所有符合条件的标签");
            }
        }

        // 缓动函数，使动画更自然
        private double EaseOutCubic(double t)
        {
            return 1 - Math.Pow(1 - t, 3);
        }
    }
    #endregion
}