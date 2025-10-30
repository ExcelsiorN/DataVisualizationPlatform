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
        private string _searchText = string.Empty;

        public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();
        public ObservableCollection<EquipmentInfoModel> FilteredEquipmentList { get; } = new();

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SearchCommand { get; }

        public EditViewModel()
        {
            LoadEquipmentData();

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

            // 自动保存新添加的设备
            SaveEquipmentData(null);
        }

        private void DeleteEquipment(object? parameter)
        {
            if (SelectedEquipment == null)
                return;

            var result = MessageBox.Show(
                $"确定要删除设备 '{SelectedEquipment.Equ_Name}' ({SelectedEquipment.Equ_Id}) 吗？",
                "确认删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                EquipmentList.Remove(SelectedEquipment);
                FilteredEquipmentList.Remove(SelectedEquipment);
                SelectedEquipment = null;

                // 自动保存删除后的设备列表
                SaveEquipmentData(null);
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
                // 序列化设备数据为 JSON
                var jsonString = JsonConvert.SerializeObject(EquipmentList, Formatting.Indented);

                // 读取 Json.cs 文件
                string jsonFilePath = FindJsonFilePath();
                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    MessageBox.Show("无法找到 Json.cs 文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                // 查找并替换 _EquipmentInfo 的内容
                int startIndex = fileContent.IndexOf("public readonly string _EquipmentInfo = @\"");
                if (startIndex == -1)
                {
                    MessageBox.Show("无法在 Json.cs 中找到 _EquipmentInfo 字段！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 找到起始位置（@" 之后）
                int contentStart = fileContent.IndexOf("@\"", startIndex) + 2;

                // 找到结束位置（下一个 ";）
                int contentEnd = fileContent.IndexOf("\";", contentStart);
                if (contentEnd == -1)
                {
                    MessageBox.Show("无法解析 Json.cs 文件格式！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // 格式化 JSON 字符串，添加适当的缩进和转义
                string formattedJson = FormatJsonForCSharp(jsonString);

                // 替换内容
                string newContent = fileContent.Substring(0, contentStart) +
                                  formattedJson +
                                  fileContent.Substring(contentEnd);

                // 写回文件
                File.WriteAllText(jsonFilePath, newContent, Encoding.UTF8);

                // 发送数据更新消息，通知其他页面重新加载数据
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
            // 将 JSON 字符串格式化为适合 C# 字符串的格式
            var lines = jsonString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var formattedLines = lines.Select(line =>
            {
                // 添加缩进
                string indentedLine = "        " + line;
                // 转义引号
                indentedLine = indentedLine.Replace("\"", "\"\"");
                return indentedLine;
            });

            return string.Join("\r\n", formattedLines);
        }

        private string FindJsonFilePath()
        {
            // 从当前项目目录查找 Json.cs 文件
            string currentDir = AppDomain.CurrentDomain.BaseDirectory;

            // 向上查找项目根目录
            DirectoryInfo? directory = new DirectoryInfo(currentDir);
            while (directory != null && directory.Name != "DataVisualizationPlatform")
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                // 查找 Services/Json.cs
                string jsonPath = Path.Combine(directory.FullName, "Services", "Json.cs");
                if (File.Exists(jsonPath))
                    return jsonPath;
            }

            // 如果找不到，尝试从解决方案目录查找
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
