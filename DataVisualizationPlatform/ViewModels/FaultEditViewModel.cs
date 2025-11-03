using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Messages;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class FaultEditViewModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();
        private BarDataItem? _selectedFault;
        private string _searchText = string.Empty;
        private string _statusFilter = "全部状态";
        private string _selectedDevice = "全部设备";
        private string _selectedYear = "全部年份";
        private string _selectedMonth = "全部月份";

        public ObservableCollection<BarDataItem> FaultList { get; } = new();
        public ObservableCollection<BarDataItem> FilteredFaultList { get; } = new();
        public ObservableCollection<string> DeviceList { get; } = new();
        public ObservableCollection<string> YearList { get; } = new();
        public ObservableCollection<string> MonthList { get; } = new();

        public ICommand SaveCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public FaultEditViewModel()
        {
            LoadFaultData();

            SaveCommand = new RelayCommand<object>(SaveFaultData);
            ClearFilterCommand = new RelayCommand<object>(ClearFilters);

            // 监听故障数据更新
            WeakReferenceMessenger.Default.Register<FaultDataUpdatedMessage>(this, (recipient, message) =>
            {
                LoadFaultData();
            });
        }

        public BarDataItem? SelectedFault
        {
            get => _selectedFault;
            set
            {
                if (_selectedFault != value)
                {
                    _selectedFault = value;
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
                    ApplyFilters();
                }
            }
        }

        public string StatusFilter
        {
            get => _statusFilter;
            set
            {
                if (_statusFilter != value)
                {
                    _statusFilter = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public string SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (_selectedDevice != value)
                {
                    _selectedDevice = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (_selectedMonth != value)
                {
                    _selectedMonth = value;
                    OnPropertyChanged();
                    ApplyFilters();
                }
            }
        }

        private void LoadFaultData()
        {
            try
            {
                var faultData = JsonConvert.DeserializeObject<List<BarDataItem>>(_jsonData._FaultReport);

                FaultList.Clear();
                FilteredFaultList.Clear();

                if (faultData != null)
                {
                    // 按优先级排序：待处理 > 处理中 > 已处理
                    var sortedData = faultData.OrderBy(f => GetStatusPriority(f.Fal_Info))
                                              .ThenBy(f => f.Fal_Data);

                    foreach (var item in sortedData)
                    {
                        FaultList.Add(item);
                    }

                    // 初始化筛选列表
                    InitializeFilterLists();

                    ApplyFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载故障数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeFilterLists()
        {
            // 初始化设备列表
            DeviceList.Clear();
            DeviceList.Add("全部设备");
            var devices = FaultList.Select(f => f.Fal_Eqid).Distinct().OrderBy(d => d);
            foreach (var device in devices)
            {
                DeviceList.Add(device);
            }

            // 初始化年份列表
            YearList.Clear();
            YearList.Add("全部年份");
            var years = FaultList
                .Select(f => f.Fal_Data)
                .Where(d => !string.IsNullOrEmpty(d))
                .Select(d => d.Split('-')[0])
                .Distinct()
                .OrderByDescending(y => y);
            foreach (var year in years)
            {
                YearList.Add(year);
            }

            // 初始化月份列表
            MonthList.Clear();
            MonthList.Add("全部月份");
            for (int i = 1; i <= 12; i++)
            {
                MonthList.Add(i.ToString("00"));
            }
        }

        private int GetStatusPriority(string status)
        {
            return status switch
            {
                "待处理" => 1,
                "处理中" => 2,
                "已处理" => 3,
                _ => 4
            };
        }

        private void ApplyFilters()
        {
            FilteredFaultList.Clear();

            var filtered = FaultList.AsEnumerable();

            // 应用搜索过滤（搜索设备ID、故障类型、故障详情、批注信息）
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filtered = filtered.Where(f =>
                    f.Fal_Id.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    f.Fal_Eqid.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    f.Fal_Type.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    f.Fal_Detail.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    (!string.IsNullOrEmpty(f.Fal_Remark) && f.Fal_Remark.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                );
            }

            // 应用设备筛选
            if (SelectedDevice != "全部设备")
            {
                filtered = filtered.Where(f => f.Fal_Eqid == SelectedDevice);
            }

            // 应用年份筛选
            if (SelectedYear != "全部年份")
            {
                filtered = filtered.Where(f =>
                    !string.IsNullOrEmpty(f.Fal_Data) &&
                    f.Fal_Data.StartsWith(SelectedYear)
                );
            }

            // 应用月份筛选
            if (SelectedMonth != "全部月份")
            {
                filtered = filtered.Where(f =>
                {
                    if (string.IsNullOrEmpty(f.Fal_Data) || !f.Fal_Data.Contains('-'))
                        return false;
                    var parts = f.Fal_Data.Split('-');
                    return parts.Length >= 2 && parts[1] == SelectedMonth;
                });
            }

            // 应用状态过滤
            if (StatusFilter != "全部状态")
            {
                filtered = filtered.Where(f => f.Fal_Info == StatusFilter);
            }

            // 按优先级排序后添加到筛选列表
            var sortedFiltered = filtered.OrderBy(f => GetStatusPriority(f.Fal_Info))
                                         .ThenBy(f => f.Fal_Data);

            foreach (var item in sortedFiltered)
            {
                FilteredFaultList.Add(item);
            }
        }

        private void SaveFaultData(object? parameter)
        {
            try
            {
                // 序列化故障数据为 JSON
                var jsonString = JsonConvert.SerializeObject(FaultList, Formatting.Indented);

                // 读取 Json.cs 文件
                string jsonFilePath = FindJsonFilePath();
                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    MessageBox.Show("无法找到 Json.cs 文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                // 查找并替换 _FaultReport 的内容
                int startIndex = fileContent.IndexOf("public readonly string _FaultReport = @\"");
                if (startIndex == -1)
                {
                    MessageBox.Show("无法在 Json.cs 中找到 _FaultReport 字段！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
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
                WeakReferenceMessenger.Default.Send(new FaultDataUpdatedMessage());

                MessageBox.Show("故障数据保存成功！", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存故障数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void ClearFilters(object? parameter)
        {
            // 清空搜索文本
            SearchText = string.Empty;

            // 重置所有筛选条件为默认值
            SelectedDevice = "全部设备";
            SelectedYear = "全部年份";
            SelectedMonth = "全部月份";
            StatusFilter = "全部状态";

            // 筛选会自动通过属性变化触发
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
