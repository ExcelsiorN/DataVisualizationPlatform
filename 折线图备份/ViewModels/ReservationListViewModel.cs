using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace DataVisualizationPlatform.ViewModels
{
    public class ReservationListViewModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();
        public ObservableCollection<ReservationListModel> ReservationList { get; } = new();
        public ICommand FlipCardCommand { get; }

        public ReservationListViewModel()
        {
            FlipCardCommand = new RelayCommand<ReservationListModel>(FlipCard);
            WeakReferenceMessenger.Default.Register<ChangePageMessage>(this, (r, msg) =>
            {
                if (msg.Parameter != null)
                {
                    // 将 Parameter 转为 string
                    string targetEquipment = msg.Parameter.ToString();
                    LoadEquipment(targetEquipment);
                }
            });
        }

        private async void FlipCard(ReservationListModel reservationlist)
        {
            if (reservationlist != null)
            {
                if (!reservationlist.IsFlipped)
                {
                    // 翻转到背面：先开始动画，延时后切换内容
                    reservationlist.IsFlipped = true;
                    await Task.Delay(500); // 等待动画到中间点
                    reservationlist.IsContentFlipped = true;
                }
                else
                {
                    // 翻转到正面：先开始动画，延时后切换内容
                    reservationlist.IsFlipped = false;
                    await Task.Delay(500); // 等待动画到中间点
                    reservationlist.IsContentFlipped = false;
                }
            }
        }

        private void LoadEquipment(string targetEquipment = null)
        {
            try
            {
                var reservationlist = JsonConvert.DeserializeObject<List<ReservationListModel>>(_jsonData._ReservationList);
                ReservationList.Clear();

                if (reservationlist != null)
                {
                    foreach (var item in reservationlist)
                    {
                        // 如果传入参数不为空，只显示匹配的条目
                        if (string.IsNullOrEmpty(targetEquipment) || item.Res_Equipment == targetEquipment)
                        {
                            ReservationList.Add(item);
                        }
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
