using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace DataVisualizationPlatform.ViewModels
{
    public class EditViewModel : INotifyPropertyChanged 
    {
        private readonly Json _jsonData = new Json();
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
                var timeSetJson = _jsonData.GetTimeSetJson();
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

        private void SaveEquipmentData(object? parameter)
        {
            try
            {
                // 在保存前，将正在编辑的副本应用回原始对象
                if (EditingEquipment != null && SelectedEquipment != null)
                {
                    SelectedEquipment.CopyFrom(EditingEquipment);
                }

                var jsonString = JsonConvert.SerializeObject(EquipmentList, Formatting.Indented);

                string jsonFilePath = FindJsonFilePath();
                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    MessageBox.Show("无法找到 Json.cs 文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                int startIndex = fileContent.IndexOf("public readonly string _EquipmentInfo = @\"");
                if (startIndex == -1)
                {
                    MessageBox.Show("无法在 Json.cs 中找到 _EquipmentInfo 字段！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                int contentStart = fileContent.IndexOf("@\"", startIndex) + 2;

                int contentEnd = fileContent.IndexOf("\";", contentStart);
                if (contentEnd == -1)
                {
                    MessageBox.Show("无法解析 Json.cs 文件格式！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string formattedJson = FormatJsonForCSharp(jsonString);

                string newContent = fileContent.Substring(0, contentStart) +
                                  formattedJson +
                                  fileContent.Substring(contentEnd);

                File.WriteAllText(jsonFilePath, newContent, Encoding.UTF8);

                WeakReferenceMessenger.Default.Send(new EquipmentDataUpdatedMessage());

                MessageBox.Show("设备数据保存成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存设备数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FormatJsonForCSharp(string jsonString)
        {

            var lines = jsonString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var formattedLines = lines.Select(line =>
            {
                string indentedLine = "        " + line;
                indentedLine = indentedLine.Replace("\"", "\"\"");
                return indentedLine;
            });

            return string.Join("\r\n", formattedLines);
        }

        private string FindJsonFilePath()
        {
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;

            // 向上查找项目根目录
            DirectoryInfo? directory = new DirectoryInfo(currentDir);
            while (directory != null && directory.Name != "DataVisualizationPlatform")
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                string jsonPath = Path.Combine(directory.FullName, "Services", "Json.cs");
                if (File.Exists(jsonPath))
                    return jsonPath;
            }

            string solutionDir = Path.Combine(currentDir, "..", "..", "..", "..");
            string[] possiblePaths = new[]
            {
                Path.Combine(solutionDir, "Services", "Json.cs"),
                Path.Combine(solutionDir, "DataVisualizationPlatform", "Services", "Json.cs"),
            };

            foreach (var path in possiblePaths)
            {
                string fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                    return fullPath;
            }

            return string.Empty;
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
