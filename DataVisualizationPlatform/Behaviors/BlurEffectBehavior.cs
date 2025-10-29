using DataVisualizationPlatform.Behaviors;
using Microsoft.Xaml.Behaviors;                                                                                      
 using System;                                                                                                        
 using System.Windows;                                                                                                
 using System.Windows.Media;                                                                                          
 using System.Windows.Media.Animation;                                                                                
 using System.Windows.Media.Effects;                                                                                  
                                                                                                                      
 namespace DataVisualizationPlatform.Behaviors                                                                        
 {                                                                                                                                                                                                                    
     public class BlurEffectBehavior : Behavior<FrameworkElement>                                                     
     {                                                                                                                
         private readonly object _blurLock = new();                                                                   
         private bool _isBlurActive;                                                                                  
                                                                                                                      
         public static readonly DependencyProperty IsBlurEnabledProperty =                                            
             DependencyProperty.Register(                                                                             
                 nameof(IsBlurEnabled),                                                                               
                 typeof(bool),                                                                                        
                 typeof(BlurEffectBehavior),                                                                          
                 new PropertyMetadata(false, OnIsBlurEnabledChanged));                                                
                                                                                                                      
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
             get => (bool) GetValue(IsBlurEnabledProperty);                                                            
             set => SetValue(IsBlurEnabledProperty, value);                                                           
         }                                                                                                            
                                                                                                                      
         public double BlurRadius                                                                                     
         {                                                                                                            
             get => (double) GetValue(BlurRadiusProperty);                                                             
             set => SetValue(BlurRadiusProperty, value);                                                              
         }                                                                                                            
                                                                                                                      
         public TimeSpan AnimationDuration                                                                            
         {                                                                                                            
             get => (TimeSpan)GetValue(AnimationDurationProperty);                                                    
             set => SetValue(AnimationDurationProperty, value);                                                       
         }                                                                                                            
                                                                                                                      
         private static void OnIsBlurEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)         
         {                                                                                                            
             if (d is BlurEffectBehavior behavior)                                                                    
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
                 if (_isBlurActive || AssociatedObject == null) return;                                               
                                                                                                                      
                 _isBlurActive = true;                                                                                
                                                                                                                      
                 if (!(AssociatedObject.Effect is BlurEffect))                                                        
                 {                                                                                                    
                     var blur = new BlurEffect { Radius = 0 };                                                        
                     AssociatedObject.Effect = blur;                                                                  
                                                                                                                      
                     var animation = new DoubleAnimation                                                              
                     {                                                                                                
                         From = 0,                                                                                    
                         To = BlurRadius,                                                                             
                         Duration = AnimationDuration,                                                                
                         EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }                       
                     }
            ;                                                                                               
                                                                                                                      
                     blur.BeginAnimation(BlurEffect.RadiusProperty, animation);                                       
                 }                                                                                                    
             }                                                                                                        
         }                                                                                                            
                                                                                                                      
         private void RemoveBlur()                                                                                    
         {                                                                                                            
             lock (_blurLock)                                                                                         
             {                                                                                                        
                 if (!_isBlurActive || AssociatedObject == null) return;                                              
                                                                                                                      
                 _isBlurActive = false;                                                                               
                                                                                                                      
                 if (AssociatedObject.Effect is BlurEffect blur)                                                      
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
                         AssociatedObject.Effect = null;                                                              
                     };                                                                                               
                                                                                                                      
                     blur.BeginAnimation(BlurEffect.RadiusProperty, animation);                                       
                 }                                                                                                    
             }                                                                                                        
         }                                                                                                            
     }                                                                                                                
 }