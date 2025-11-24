using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace DataVisualizationPlatform.ViewModels
{
    public class EditViewModel : INotifyPropertyChanged
    {
        private EquipmentInfoModel? _selectedEquipment;
        private EquipmentInfoModel? _editingEquipment;
        private string _searchText = string.Empty;

        public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();
        public ObservableCollection<EquipmentInfoModel> FilteredEquipmentList { get; } = new();
        public ObservableCollection<string> TimeSetConfigOptions { get; } = new();

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SearchCommand { get; }

        public EditViewModel()
        {
            LoadEquipmentData();
            LoadTimeSetConfigs();

            AddCommand = new RelayCommand<object>(AddEquipment);
            DeleteCommand = new RelayCommand<object>(DeleteEquipment, CanDeleteEquipment);
            SaveCommand = new RelayCommand<object>(SaveEquipmentData);
            SearchCommand = new RelayCommand<object>(SearchEquipment);
        }

        public EquipmentInfoModel? SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                if (_selectedEquipment != value)
                {
                    _selectedEquipment = value;
                    OnPropertyChanged();

                    // 当选择设备时，创建副本用于编辑
                    if (_selectedEquipment != null)
                    {
                        EditingEquipment = _selectedEquipment.Clone();
                    }
                    else
                    {
                        EditingEquipment = null;
                    }
                }
            }
        }

        /// <summary>
        /// 当前正在编辑的设备副本（UI绑定到此属性）
        /// </summary>
        public EquipmentInfoModel? EditingEquipment
        {
            get => _editingEquipment;
            set
            {
                if (_editingEquipment != value)
                {
                    _editingEquipment = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    SearchEquipment(null);
                }
            }
        }

        private void LoadEquipmentData()
        {
            try
            {
                // 使用 JsonDataService 获取最新的设备数据
                var equipmentJson = JsonDataService.Instance.GetEquipmentInfoJson();
                var equipmentData = JsonConvert.DeserializeObject<ObservableCollection<EquipmentInfoModel>>(equipmentJson);

                EquipmentList.Clear();
                FilteredEquipmentList.Clear();

                if (equipmentData != null)
                {
                    foreach (var item in equipmentData)
                    {
                        EquipmentList.Add(item);
                        FilteredEquipmentList.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载设备数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTimeSetConfigs()
        {
            try
            {
                var timeSetJson = JsonDataService.Instance.GetTimeSetJson();
                var timeSetData = JsonConvert.DeserializeObject<ObservableCollection<TimeSetConfigModel>>(timeSetJson);

                TimeSetConfigOptions.Clear();

                if (timeSetData != null)
                {
                    foreach (var config in timeSetData)
                    {
                        TimeSetConfigOptions.Add(config.Set_Id);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载时段配置失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddEquipment(object? parameter)
        {
            var newEquipment = new EquipmentInfoModel
            {
                Equ_Id = $"fntp-{EquipmentList.Count}",
                Equ_Name = "新设备",
                Equ_OnlineStatus = "离线",
                Equ_AvailableBookingPeriod = "预约时段配置1",
                Equ_TotalOperationTime = "0年0月0天",
                Equ_FixedDurationThisYear = "0.0小时",
                Equ_UsedFixedDurationThisYear = "0.0小时",
                Equ_UsageRateThisYear = "0.0%",
                Equ_DeploymentAddress = "0.0, 0.0"
            };

            EquipmentList.Add(newEquipment);
            FilteredEquipmentList.Add(newEquipment);
            SelectedEquipment = newEquipment;
        }

        private void DeleteEquipment(object? parameter)
        {
            if (SelectedEquipment == null)
                return;

            var result = MessageBox.Show(
                $"确定要删除设备 '{SelectedEquipment.Equ_Name}' ({SelectedEquipment.Equ_Id}) 吗？\n\n注意：删除后需要点击'保存'按钮才会真正删除。",
                "确认删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                EquipmentList.Remove(SelectedEquipment);
                FilteredEquipmentList.Remove(SelectedEquipment);
                SelectedEquipment = null;
            }
        }

        private bool CanDeleteEquipment(object? parameter)
        {
            return SelectedEquipment != null;
        }

        private bool ValidateAllEquipmentDurations()
        {
            var invalidEquipments = new System.Collections.Generic.List<string>();

            foreach (var equipment in EquipmentList)
            {
                // 从字符串中提取数值
                string fixedDurationStr = equipment.Equ_FixedDurationThisYear?.Replace("小时", "").Trim() ?? "0";
                string usedDurationStr = equipment.Equ_UsedFixedDurationThisYear?.Replace("小时", "").Trim() ?? "0";

                if (int.TryParse(fixedDurationStr, out int fixedDuration) &&
                    int.TryParse(usedDurationStr, out int usedDuration))
                {
                    if (usedDuration > fixedDuration)
                    {
                        invalidEquipments.Add($"{equipment.Equ_Name}（{equipment.Equ_Id}）: 已用时长 {usedDuration}小时 > 固定时长 {fixedDuration}小时");
                    }
                }
            }

            if (invalidEquipments.Count > 0)
            {
                string message = "以下设备的已用固定时长大于固定时长，请修正后再保存：\n\n";
                message += string.Join("\n", invalidEquipments);

                MessageBox.Show(message, "数据验证失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void SaveEquipmentData(object? parameter)
        {
            try
            {
                // 在保存前验证所有设备的已用固定时长
                if (!ValidateAllEquipmentDurations())
                {
                    return;
                }

                // 在保存前，将正在编辑的副本应用回原始对象
                if (EditingEquipment != null && SelectedEquipment != null)
                {
                    SelectedEquipment.CopyFrom(EditingEquipment);
                }

                // 序列化为JSON字符串
                var jsonString = JsonConvert.SerializeObject(EquipmentList, Formatting.Indented);

                // 使用JsonDataService保存到独立的JSON文件
                JsonDataService.Instance.SaveEquipmentInfoJson(jsonString);

                // 发送数据更新消息
                WeakReferenceMessenger.Default.Send(new EquipmentDataUpdatedMessage());

                MessageBox.Show("设备数据保存成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存设备数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SearchEquipment(object? parameter)
        {
            FilteredEquipmentList.Clear();

            if (string.IsNullOrWhiteSpace(SearchText))
            {
                foreach (var item in EquipmentList)
                {
                    FilteredEquipmentList.Add(item);
                }
            }
            else
            {
                var filtered = EquipmentList.Where(e =>
                    e.Equ_Id.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.Equ_Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    e.Equ_OnlineStatus.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                );

                foreach (var item in filtered)
                {
                    FilteredEquipmentList.Add(item);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
