using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Views;
using Newtonsoft.Json;


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
                    PageKey = "ReservationList",
                    Parameter = param
                });
                Console.WriteLine($"消息已发送，参数={param}");
            });

            // 订阅设备数据更新消息
            WeakReferenceMessenger.Default.Register<EquipmentDataUpdatedMessage>(this, (recipient, message) =>
            {
                // 重新加载设备数据
                LoadEquipment();
            });
        }

        private void LoadEquipment()
        {
            try
            {
                // 使用 JsonDataService 获取最新的设备数据
                var equipmentJson = JsonDataService.Instance.GetEquipmentInfoJson();
                var equipment = JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(equipmentJson);
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