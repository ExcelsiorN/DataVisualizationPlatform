using Jamesnet.Wpf.Animation;
using Jamesnet.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NavigationBar
{
 
    public class MagicBar : ListBox
    {
        private ValueItem? _vi;
        private Storyboard? _sb;
        private Grid? _circle;

        static MagicBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MagicBar), new FrameworkPropertyMetadata(typeof(MagicBar)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _circle = (Grid)GetTemplateChild("PART_Circle");

            InitStoryBoard(_circle);

            // 设置初始位置到当前选中的索引
            if (SelectedIndex >= 0 && _circle != null)
            {
                Canvas.SetLeft(_circle, SelectedIndex * 90);
            }
        }

        private void InitStoryBoard(Grid circle)
        {
            _vi = new();
            _sb = new();

            _vi.Mode = EasingFunctionBaseMode.QuinticEaseInOut;
            _vi.Property = new PropertyPath(Canvas.LeftProperty);
            _vi.Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500));

            Storyboard.SetTarget(_vi, circle);
            Storyboard.SetTargetProperty(_vi, _vi.Property);

            _sb.Children.Add(_vi);
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (_vi != null && _sb != null && SelectedIndex >= 0)
            {
                _vi.To = SelectedIndex * 90;
                _sb.Begin();
            }
        }

        //private void Login_Loaded(object sender, RoutedEventArgs e)
        //{
        //    bar.SelectedIndex = 0;
        //}
    }
}
