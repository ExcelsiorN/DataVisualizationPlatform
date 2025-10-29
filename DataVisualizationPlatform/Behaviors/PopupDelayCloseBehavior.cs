using Microsoft.Xaml.Behaviors;                                                                                      
 using System;                                                                                                        
 using System.Windows;                                                                                                
 using System.Windows.Controls.Primitives;                                                                            
 using System.Windows.Input;                                                                                          
 using System.Windows.Threading;                                                                                      
                                                                                                                      
 namespace DataVisualizationPlatform.Behaviors                                                                        
 {                                                                                                                    
     /// <summary>                                                                                                    
     /// Popup延迟关闭行为                                                                                            
     /// </summary>                                                                                                   
     public class PopupDelayCloseBehavior : Behavior<Popup>                                                           
     {                                                                                                                
         private DispatcherTimer? _closeTimer;                                                                        
                                                                                                                      
         public static readonly DependencyProperty CloseDelayProperty =                                               
             DependencyProperty.Register(                                                                             
                 nameof(CloseDelay),                                                                                  
                 typeof(TimeSpan),                                                                                    
                 typeof(PopupDelayCloseBehavior),                                                                     
                 new PropertyMetadata(TimeSpan.FromMilliseconds(300)));                                               
                                                                                                                      
         public TimeSpan CloseDelay                                                                                   
         {                                                                                                            
             get => (TimeSpan) GetValue(CloseDelayProperty);                                                           
             set => SetValue(CloseDelayProperty, value);                                                              
         }                                                                                                            
                                                                                                                      
         protected override void OnAttached()                                                                         
         {                                                                                                            
             base.OnAttached();                                                                                       
                                                                                                                      
             _closeTimer = new DispatcherTimer { Interval = CloseDelay
};                                             
             _closeTimer.Tick += OnCloseTimerTick;                                                                    
                                                                                                                      
             AssociatedObject.MouseEnter += OnPopupMouseEnter;                                                        
             AssociatedObject.MouseLeave += OnPopupMouseLeave;                                                        
         }                                                                                                            
                                                                                                                      
         protected override void OnDetaching()                                                                        
         {                                                                                                            
             base.OnDetaching();                                                                                      
                                                                                                                      
             if (_closeTimer != null)                                                                                 
             {                                                                                                        
                 _closeTimer.Stop();                                                                                  
                 _closeTimer.Tick -= OnCloseTimerTick;                                                                
             }                                                                                                        
                                                                                                                      
             AssociatedObject.MouseEnter -= OnPopupMouseEnter;                                                        
             AssociatedObject.MouseLeave -= OnPopupMouseLeave;                                                        
         }                                                                                                            
                                                                                                                      
         private void OnPopupMouseEnter(object sender, MouseEventArgs e)                                              
         {                                                                                                            
             _closeTimer?.Stop();                                                                                     
         }                                                                                                            
                                                                                                                      
         private void OnPopupMouseLeave(object sender, MouseEventArgs e)                                              
         {                                                                                                            
             _closeTimer?.Start();                                                                                    
         }                                                                                                            
                                                                                                                      
         private void OnCloseTimerTick(object? sender, EventArgs e)                                                   
         {                                                                                                            
             _closeTimer?.Stop();                                                                                     
             AssociatedObject.IsOpen = false;                                                                         
         }                                                                                                            
     }                                                                                                                
 }