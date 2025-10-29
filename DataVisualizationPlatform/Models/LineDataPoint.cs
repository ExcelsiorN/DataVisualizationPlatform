using Newtonsoft.Json.Linq;
using System.ComponentModel;                                                                                         
                                                                                                                      
 namespace DataVisualizationPlatform.Models                                                                           
 {                                                                                                                    
     /// <summary>                                                                                                    
     /// 折线图数据点模型                                                                                             
     /// </summary>                                                                                                   
     public class LineDataPoint : INotifyPropertyChanged                                                              
     {                                                                                                                
         private double _x;                                                                                           
         private double _y;                                                                                           
         private string _label = string.Empty;                                                                        
                                                                                                                      
         public double X                                                                                              
         {                                                                                                            
             get => _x;                                                                                               
             set                                                                                                      
             {                                                                                                        
                 if (_x != value)                                                                                     
                 {                                                                                                    
                     _x = value;                                                                                      
                     OnPropertyChanged(nameof(X));                                                                    
                 }                                                                                                    
             }                                                                                                        
         }                                                                                                            
                                                                                                                      
         public double Y                                                                                              
         {                                                                                                            
             get => _y;                                                                                               
             set                                                                                                      
             {                                                                                                        
                 if (_y != value)                                                                                     
                 {                                                                                                    
                     _y = value;                                                                                      
                     OnPropertyChanged(nameof(Y));                                                                    
                 }                                                                                                    
             }                                                                                                        
         }                                                                                                            
                                                                                                                      
         public string Label                                                                                          
         {                                                                                                            
             get => _label;                                                                                           
             set                                                                                                      
             {                                                                                                        
                 if (_label != value)                                                                                 
                 {                                                                                                    
                     _label = value;                                                                                  
                     OnPropertyChanged(nameof(Label));                                                                
                 }                                                                                                    
             }                                                                                                        
         }                                                                                                            
                                                                                                                      
         public event PropertyChangedEventHandler? PropertyChanged;                                                   
                                                                                                                      
         protected void OnPropertyChanged(string propertyName)                                                        
         {                                                                                                            
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));                               
         }                                                                                                            
     }                                                                                                                
 }