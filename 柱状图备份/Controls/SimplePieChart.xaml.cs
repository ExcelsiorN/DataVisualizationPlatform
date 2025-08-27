using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DataVisualizationPlatform.Controls
{
    public partial class SimplePieChart : UserControl
    {
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(SimplePieChart),
                new PropertyMetadata(0.0, OnPercentageChanged));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(SimplePieChart),
                new PropertyMetadata(Brushes.LimeGreen));

        public double Percentage
        {
            get { return (double)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public SimplePieChart()
        {
            InitializeComponent();
            Loaded += (s, e) => UpdatePieChart();
        }

        private static void OnPercentageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SimplePieChart pieChart)
            {
                pieChart.UpdatePieChart();
            }
        }

        private void UpdatePieChart()
        {
            if (PieCanvas == null || PercentageText == null) return;

            // 更新文字
            PercentageText.Text = $"{Percentage:F0}%";

            // 清除之前的内容
            PieCanvas.Children.Clear();

            double percentage = Math.Max(0, Math.Min(100, Percentage));

            if (percentage <= 0) return;

            double radius = 40;
            Point center = new Point(50, 50);

            if (percentage >= 100)
            {
                // 完整圆形
                Ellipse fullCircle = new Ellipse
                {
                    Width = radius * 2,
                    Height = radius * 2,
                    Fill = Fill,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(fullCircle, center.X - radius);
                Canvas.SetTop(fullCircle, center.Y - radius);
                PieCanvas.Children.Add(fullCircle);
            }
            else
            {
                // 创建扇形
                double angle = (percentage / 100.0) * 360.0;
                double startAngle = -90; // 从12点方向开始
                double endAngle = startAngle + angle;

                // 转换为弧度
                double startRad = startAngle * Math.PI / 180.0;
                double endRad = endAngle * Math.PI / 180.0;

                // 计算起点和终点
                Point startPoint = new Point(
                    center.X + radius * Math.Cos(startRad),
                    center.Y + radius * Math.Sin(startRad)
                );

                Point endPoint = new Point(
                    center.X + radius * Math.Cos(endRad),
                    center.Y + radius * Math.Sin(endRad)
                );

                // 创建路径
                PathGeometry pathGeometry = new PathGeometry();
                PathFigure pathFigure = new PathFigure
                {
                    StartPoint = center,
                    IsClosed = true
                };

                // 线段到起点
                pathFigure.Segments.Add(new LineSegment(startPoint, false));

                // 弧线到终点
                pathFigure.Segments.Add(new ArcSegment(
                    endPoint,
                    new Size(radius, radius),
                    0,
                    angle > 180, // isLargeArc
                    SweepDirection.Clockwise,
                    true
                ));

                pathGeometry.Figures.Add(pathFigure);

                Path piePath = new Path
                {
                    Data = pathGeometry,
                    Fill = Fill,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                };

                PieCanvas.Children.Add(piePath);
            }
        }
    }
}
