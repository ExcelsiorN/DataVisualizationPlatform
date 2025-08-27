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
        private Point _startPoint;
        private bool _isDragging;

        public MainMenuViewModel ViewModel => DataContext as MainMenuViewModel;

        public MainMenu()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(this);
            _isDragging = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                Point current = e.GetPosition(this);
                double deltaX = current.X - _startPoint.X;
                if (Math.Abs(deltaX) > 30)
                {
                    if (deltaX < 0)
                    {
                        var sb = (Storyboard)Resources["SlideLeftStoryboard"];
                        EventHandler handler = null;
                        handler = (_, __) =>
                        {
                            ViewModel.MoveNext();
                            sb.Completed -= handler; // 正确解绑
                        };
                        sb.Completed += handler;
                        sb.Begin();
                    }
                    else
                    {
                        var sb = (Storyboard)Resources["SlideRightStoryboard"];
                        EventHandler handler = null;
                        handler = (_, __) =>
                        {
                            ViewModel.MovePrevious();
                            sb.Completed -= handler; // 正确解绑
                        };
                        sb.Completed += handler;
                        sb.Begin();
                    }

                    _isDragging = false;
                }
            }
        }


        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
        }
    }
}