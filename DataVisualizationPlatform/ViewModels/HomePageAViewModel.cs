using CommunityToolkit.Mvvm.ComponentModel;                                                                          
 using DataVisualizationPlatform.Models;                                                                              
 using DataVisualizationPlatform.Services;                                                                            
 using DataVisualizationPlatform.Services.Navigation;                                                                 
 using Newtonsoft.Json;                                                                                               
 using System;                                                                                                        
 using System.Collections.Generic;                                                                                    
 using System.Collections.ObjectModel;                                                                                
 using System.Diagnostics;                                                                                            
 using System.Globalization;                                                                                          
 using System.Linq;                                                                                                   
 using System.Windows.Threading;                                                                                      
                                                                                                                      
 namespace DataVisualizationPlatform.ViewModels                                                                       
 {                                                                                                                    
     /// <summary>                                                                                                    
     /// 首页A的ViewModel                                                                                             
     /// </summary>                                                                                                   
     public partial class HomePageAViewModel : ViewModelBase, INavigationAware                                        
     {                                                                                                                
         private readonly Json _jsonData = new Json();                                                                
         private readonly DispatcherTimer _animationTimer;                                                            
         private DateTime _animationStartTime;                                                                        
                                                                                                                      
         [ObservableProperty]                                                                                         
         private ObservableCollection<LineDataPoint> _lineData = new();                                               
                                                                                                                      
         [ObservableProperty]                                                                                         
         private double _animationProgress;                                                                           
                                                                                                                      
         public int AnimationDuration { get; } = 3000; // 3秒                                                         
                                                                                                                      
         public HomePageAViewModel()                                                                                  
         {                                                                                                            
             _animationTimer = new DispatcherTimer                                                                    
             {                                                                                                        
                 Interval = TimeSpan.FromMilliseconds(16) // ~60 FPS                                                  
             };                                                                                                       
             _animationTimer.Tick += AnimationTimer_Tick;                                                             
         }                                                                                                            
                                                                                                                      
         public override void OnLoaded()                                                                              
         {                                                                                                            
             base.OnLoaded();                                                                                         
             InitializeData();                                                                                        
             StartAnimation();                                                                                        
         }                                                                                                            
                                                                                                                      
         public override void OnUnloaded()                                                                            
         {                                                                                                            
             base.OnUnloaded();                                                                                       
             _animationTimer.Stop();                                                                                  
         }                                                                                                            
                                                                                                                      
         public void OnNavigatedTo(object? parameter)                                                                 
         {                                                                                                            
             // 可以根据导航参数初始化数据                                                                            
             InitializeData();                                                                                        
         }                                                                                                            
                                                                                                                      
         public void OnNavigatedFrom()                                                                                
         {                                                                                                            
             _animationTimer.Stop();                                                                                  
         }                                                                                                            
                                                                                                                      
         private void InitializeData()                                                                                
         {                                                                                                            
             LineData = LoadLineData();                                                                               
         }                                                                                                            
                                                                                                                      
         private ObservableCollection<LineDataPoint> LoadLineData()                                                   
         {                                                                                                            
             var reservations = JsonConvert.DeserializeObject<List<HomePageModel>>(_jsonData._ReservationList);       
             if (reservations == null) return new ObservableCollection<LineDataPoint>();                              
                                                                                                                      
             var completedDates = reservations                                                                        
                 .Where(r => r.Res_Status == "已完成")                                                                
                 .Select(r => DateTime.TryParse(r.Res_Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out    
 DateTime dt)                                                                                                         
                                 ? (DateTime?)dt                                                                      
                                 : null)                                                                              
                 .Where(dt => dt.HasValue)                                                                            
                 .Select(dt => dt!.Value)                                                                             
                 .ToList();                                                                                           
                                                                                                                      
             if (completedDates.Count == 0)                                                                           
                 return new ObservableCollection<LineDataPoint>();                                                    
                                                                                                                      
             var grouped = completedDates                                                                             
                 .GroupBy(d => d.Year * 100 + d.Month)                                                                
                 .ToDictionary(g => g.Key, g => g.Count());                                                           
                                                                                                                      
             DateTime minDate = new DateTime(completedDates.Min(d => d).Year, completedDates.Min(d => d).Month, 1);   
             DateTime maxDate = new DateTime(completedDates.Max(d => d).Year, completedDates.Max(d => d).Month, 1);   
                                                                                                                      
             var result = new ObservableCollection<LineDataPoint>();                                                  
                                                                                                                      
             for (DateTime dt = minDate; dt <= maxDate; dt = dt.AddMonths(1))                                         
             {                                                                                                        
                 int key = dt.Year * 100 + dt.Month;                                                                  
                 int count = grouped.ContainsKey(key) ? grouped[key] : 0;                                             
                                                                                                                      
                 Debug.WriteLine($"年月: {dt:yyyy-MM}, 订单数量: {count}");                                           
                                                                                                                      
                 result.Add(new LineDataPoint                                                                         
                 {                                                                                                    
                     X = key,                                                                                         
                     Y = count,                                                                                       
                     Label = dt.ToString("yyyy-MM")                                                                   
                 });                                                                                                  
             }                                                                                                        
                                                                                                                      
             return result;                                                                                           
         }                                                                                                            
                                                                                                                      
         private void StartAnimation()                                                                                
         {                                                                                                            
             AnimationProgress = 0;                                                                                   
             _animationStartTime = DateTime.Now;                                                                      
             _animationTimer.Start();                                                                                 
         }                                                                                                            
                                                                                                                      
         private void AnimationTimer_Tick(object? sender, EventArgs e)                                                
         {                                                                                                            
             var elapsed = DateTime.Now - _animationStartTime;                                                        
             var progress = Math.Min(elapsed.TotalMilliseconds / AnimationDuration, 1.0);                             
                                                                                                                      
             AnimationProgress = EaseInOut(progress);                                                                 
                                                                                                                      
             if (progress >= 1.0)                                                                                     
             {                                                                                                        
                 _animationTimer.Stop();                                                                              
             }                                                                                                        
         }                                                                                                            
                                                                                                                      
         private static double EaseInOut(double t)                                                                    
         {                                                                                                            
             return t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;                                                       
         }                                                                                                            
     }                                                                                                                
 }