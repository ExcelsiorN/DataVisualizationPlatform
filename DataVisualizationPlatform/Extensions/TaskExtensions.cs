using System;                                                                                                        
 using System.Threading.Tasks;                                                                                        
 using System.Windows;                                                                                                
                                                                                                                      
 namespace DataVisualizationPlatform.Extensions                                                                       
 {                                                                                                                    
     /// <summary>                                                                                                    
     /// Task扩展方法，用于统一异常处理                                                                               
     /// </summary>                                                                                                   
     public static class TaskExtensions                                                                               
     {                                                                                                                
         /// <summary>                                                                                                
         /// 安全执行异步任务，自动处理异常                                                                           
         /// </summary>                                                                                               
         public static async Task SafeExecuteAsync(                                                                   
             this Task task,                                                                                          
             string? errorMessage = null,                                                                             
             Action<Exception>? onError = null)                                                                       
         {                                                                                                            
             try                                                                                                      
             {                                                                                                        
                 await task;                                                                                          
             }                                                                                                        
             catch (Exception ex)                                                                                     
             {                                                                                                        
                 System.Diagnostics.Debug.WriteLine($"异步任务异常: {ex}");                                           
                 onError?.Invoke(ex);                                                                                 
                                                                                                                      
                 if (!string.IsNullOrEmpty(errorMessage))                                                             
                 {                                                                                                    
                     MessageBox.Show(errorMessage, "错误", MessageBoxButton.OK, MessageBoxImage.Error);               
                 }                                                                                                    
             }                                                                                                        
         }                                                                                                            
                                                                                                                      
         /// <summary>                                                                                                
         /// 安全执行异步任务（带返回值），自动处理异常                                                               
         /// </summary>                                                                                               
         public static async Task<T?> SafeExecuteAsync<T>(                                                            
             this Task<T> task,                                                                                       
             string? errorMessage = null,                                                                             
             Action<Exception>? onError = null)                                                                       
         {                                                                                                            
             try                                                                                                      
             {                                                                                                        
                 return await task;                                                                                   
             }                                                                                                        
             catch (Exception ex)                                                                                     
             {                                                                                                        
                 System.Diagnostics.Debug.WriteLine($"异步任务异常: {ex}");                                           
                 onError?.Invoke(ex);                                                                                 
                                                                                                                      
                 if (!string.IsNullOrEmpty(errorMessage))                                                             
                 {                                                                                                    
                     MessageBox.Show(errorMessage, "错误", MessageBoxButton.OK, MessageBoxImage.Error);               
                 }                                                                                                    
                                                                                                                      
                 return default;                                                                                      
             }                                                                                                        
         }                                                                                                            
     }                                                                                                                
 }