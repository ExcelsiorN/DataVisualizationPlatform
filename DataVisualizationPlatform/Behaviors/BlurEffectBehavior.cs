using DataVisualizationPlatform.Behaviors;
using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace DataVisualizationPlatform.Behaviors
{
    public class BlurEffectBehavior : Behavior<FrameworkElement>
    {
        private readonly object _blurLock = new();
        private bool _isBlurActive;
        private FrameworkElement? _targetElement;

        public static readonly DependencyProperty IsBlurEnabledProperty =
            DependencyProperty.Register(
                nameof(IsBlurEnabled),
                typeof(bool),
                typeof(BlurEffectBehavior),
                new PropertyMetadata(false, OnIsBlurEnabledChanged));

        public static readonly DependencyProperty UseMouseOverProperty =
            DependencyProperty.Register(
                nameof(UseMouseOver),
                typeof(bool),
                typeof(BlurEffectBehavior),
                new PropertyMetadata(false));

        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register(
                nameof(TargetElement),
                typeof(FrameworkElement),
                typeof(BlurEffectBehavior),
                new PropertyMetadata(null));

        public static readonly DependencyProperty BlurRadiusProperty =
            DependencyProperty.Register(
                nameof(BlurRadius),
                typeof(double),
                typeof(BlurEffectBehavior),
                new PropertyMetadata(10.0));

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(
                nameof(AnimationDuration),
                typeof(TimeSpan),
                typeof(BlurEffectBehavior),
                new PropertyMetadata(TimeSpan.FromSeconds(0.4)));

        public bool IsBlurEnabled
        {
            get => (bool)GetValue(IsBlurEnabledProperty);
            set => SetValue(IsBlurEnabledProperty, value);
        }

        public bool UseMouseOver
        {
            get => (bool)GetValue(UseMouseOverProperty);
            set => SetValue(UseMouseOverProperty, value);
        }

        public FrameworkElement? TargetElement
        {
            get => (FrameworkElement?)GetValue(TargetElementProperty);
            set => SetValue(TargetElementProperty, value);
        }

        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        public TimeSpan AnimationDuration
        {
            get => (TimeSpan)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (UseMouseOver)
            {
                AssociatedObject.MouseEnter += OnMouseEnter;
                AssociatedObject.MouseLeave += OnMouseLeave;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (UseMouseOver)
            {
                AssociatedObject.MouseEnter -= OnMouseEnter;
                AssociatedObject.MouseLeave -= OnMouseLeave;
            }
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            ApplyBlur();
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            RemoveBlur();
        }

        private static void OnIsBlurEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BlurEffectBehavior behavior && !behavior.UseMouseOver)
            {
                if ((bool)e.NewValue)
                {
                    behavior.ApplyBlur();
                }
                else
                {
                    behavior.RemoveBlur();
                }
            }
        }

        private void ApplyBlur()
        {
            lock (_blurLock)
            {
                if (_isBlurActive) return;

                _isBlurActive = true;

                // 确定要模糊的目标元素
                var target = UseMouseOver ? (TargetElement ?? FindTargetElement()) : AssociatedObject;
                if (target == null) return;

                _targetElement = target;

                if (!(target.Effect is BlurEffect))
                {
                    var blur = new BlurEffect { Radius = 0 };
                    target.Effect = blur;

                    var animation = new DoubleAnimation
                    {
                        From = 0,
                        To = BlurRadius,
                        Duration = AnimationDuration,
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
                    };

                    blur.BeginAnimation(BlurEffect.RadiusProperty, animation);
                }
            }
        }

        private void RemoveBlur()
        {
            lock (_blurLock)
            {
                if (!_isBlurActive) return;

                _isBlurActive = false;

                var target = _targetElement ?? (UseMouseOver ? (TargetElement ?? FindTargetElement()) : AssociatedObject);
                if (target == null) return;

                if (target.Effect is BlurEffect blur)
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
                        target.Effect = null;
                    };

                    blur.BeginAnimation(BlurEffect.RadiusProperty, animation);
                }
            }
        }

        /// <summary>
        /// 查找目标元素（默认查找父容器中的 Display Grid）
        /// </summary>
        private FrameworkElement? FindTargetElement()
        {
            var parent = AssociatedObject.Parent as FrameworkElement;
            while (parent != null)
            {
                if (parent is System.Windows.Controls.Grid grid && grid.Name == "Display")
                {
                    return grid;
                }
                parent = parent.Parent as FrameworkElement;
            }
            return null;
        }
    }
}
