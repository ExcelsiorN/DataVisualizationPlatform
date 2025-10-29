using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Services.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace DataVisualizationPlatform.ViewModels
{
    public class ReservationListViewModel : INotifyPropertyChanged, INavigationAware
    {
        private readonly Json _jsonData = new Json();
        public ObservableCollection<ReservationListModel> ReservationList { get; } = new();
        public ICommand FlipCardCommand { get; }

        public ReservationListViewModel()
        {
            FlipCardCommand = new RelayCommand<ReservationListModel>(FlipCard);
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

        private void LoadEquipmentByDate(int targetMonth, int targetYear)
        {
            try
            {
                var reservationlist = JsonConvert.DeserializeObject<List<ReservationListModel>>(_jsonData._ReservationList);
                ReservationList.Clear();

                int matchCount = 0; // 计数器

                if (reservationlist != null)
                {
                    foreach (var item in reservationlist)
                    {
                        if (DateTime.TryParse(item.Res_Date, out DateTime resDate))
                        {
                            if (resDate.Month == targetMonth && resDate.Year == targetYear)
                            {
                                ReservationList.Add(item);
                                matchCount++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"加载设备信息失败: {ex.Message}");
            }
        }

        #region INavigationAware
        public void OnNavigatedTo(object? parameter)
        {
            // 清空现有数据
            ReservationList.Clear();

            // 根据参数类型加载数据
            if (parameter is string targetEquipment)
            {
                // 传入设备名
                LoadEquipment(targetEquipment);
            }
            else if (parameter is ValueTuple<int, int> tuple)
            {
                // 传入 (月份, 年份)
                var (month, targetYear) = tuple;
                LoadEquipmentByDate(month, targetYear);
            }
            else
            {
                // 如果没有参数，加载当前月份的数据
                var now = DateTime.Now;
                LoadEquipmentByDate(now.Month, now.Year);
            }
        }

        public void OnNavigatedFrom()
        {
            // 页面离开时的清理工作（如果需要）
        }
        #endregion

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
