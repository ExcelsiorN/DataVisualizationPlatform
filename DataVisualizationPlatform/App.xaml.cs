using DataVisualizationPlatform;
using DataVisualizationPlatform.Services.ExceptionHandling;
using DataVisualizationPlatform.Services.Navigation;                                                                 
 using DataVisualizationPlatform.ViewModels;                                                                          
 using DataVisualizationPlatform.Views;                                                                               
 using Microsoft.Extensions.DependencyInjection;                                                                      
 using System;                                                                                                        
 using System.Windows;                                                                                                
                                                                                                                      
 namespace DataVisualizationPlatform                                                                                  
 {                                                                                                                    
     public partial class App : Application                                                                           
     {                                                                                                                
         private IServiceProvider? _serviceProvider;                                                                  
                                                                                                                      
         public App()                                                                                                 
         {                                                                                          
            GlobalExceptionHandler.Initialize();                                                                     
                                                                                                                      
            var services = new ServiceCollection();                                                                  
            ConfigureServices(services);                                                                             
            _serviceProvider = services.BuildServiceProvider();
        }                                                                                                            
                                                                                                                      
         private void ConfigureServices(IServiceCollection services)                                                  
         {                                                                                                            
             // 注册导航服务                                                                                          
             services.AddSingleton<INavigationService, NavigationService>();                                          
                                                                                                                      
             // 注册ViewModels（每次获取都创建新实例）                                                                
             services.AddTransient<MainWindowViewModel>();                                                            
             services.AddTransient<LoginViewModel>();                                                                 
             services.AddTransient<HomePageAViewModel>();                                                             
             services.AddTransient<HomePageBViewModel>();                                                             
             services.AddTransient<HomePageCViewModel>();                                                             
             services.AddTransient<DataViewModel>();                                                                  
             services.AddTransient<EquipmentInfoViewModel>();                                                         
             services.AddTransient<FaultReportViewModel>();                                                           
             services.AddTransient<ReservationListViewModel>();
            services.AddTransient<EditViewModel>();                                                       
                                                                                                                      
             // 注册Views                                                                                             
             services.AddTransient<MainWindow>();                                                                     
             services.AddTransient<Login>();                                                                          
             services.AddTransient<HomePageA>();                                                                      
             services.AddTransient<HomePageB>();                                                                      
             services.AddTransient<HomePageC>();                                                                      
             services.AddTransient<Data>();                                                                           
             services.AddTransient<EquipmentInfo>();                                                                  
             services.AddTransient<FaultReport>();                                                                    
             services.AddTransient<ReservationList>();                                                                
             services.AddTransient<Edit>();                                                                           
                                                                                                                                                                      
         }                                                                                                            
                                                                                                                      
         protected override void OnStartup(StartupEventArgs e)                                                        
         {                                                                                                            
             base.OnStartup(e);                                                                                       
                                                                                                                      
             // 显示登录窗口                                                                                          
             var loginWindow = _serviceProvider!.GetRequiredService<Login>();                                         
             loginWindow.Show();                                                                                      
         }                                                                                                            
                                                                                                                      
         /// <summary>                                                                                                
         /// 获取服务实例（供其他地方使用）                                                                           
         /// </summary>                                                                                               
         public static T GetService<T>() where T : notnull                                                            
         {                                                                                                            
             return ((App)Current)._serviceProvider!.GetRequiredService<T>();                                         
         }                                                                                                            
     }                                                                                                                
 }                                        