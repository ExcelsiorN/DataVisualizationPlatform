using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataVisualizationPlatform.ViewModels
{
    public class EquipmentInfoViewModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();
        public IRelayCommand OpenReservationListCommand { get; }

        public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();

        // 添加导航命令
        public ICommand NavigateCommand { get; }

        public EquipmentInfoViewModel()
        {
            LoadEquipment();

            OpenReservationListCommand = new RelayCommand<object>((param) =>
            {
                WeakReferenceMessenger.Default.Send(new ChangePageMessage
                {
                    NewPage = new ReservationList(),
                    Parameter = param 
                });
                Console.WriteLine($"消息已发送，参数={param}");
            });

        }

        private void LoadEquipment()
        {
            try
            {
                var equipment = JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(_jsonData._EquipmentInfo);
                EquipmentList.Clear();

                if (equipment != null)
                {
                    foreach (var item in equipment)
                    {
                        EquipmentList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"加载设备信息失败: {ex.Message}");
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}