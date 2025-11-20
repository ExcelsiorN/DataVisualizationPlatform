using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace DataVisualizationPlatform.ViewModels
{
    /// <summary>
    /// 时段编辑ViewModel
    /// </summary>
    public partial class TimeSlotEditViewModel : ViewModelBase
    {
        private readonly Json _jsonData = new Json();

        [ObservableProperty]
        private TimeSetConfigModel? _selectedConfig;

        [ObservableProperty]
        private string? _selectedTimeSlot;

        [ObservableProperty]
        private string? _startHour;

        [ObservableProperty]
        private string? _startMinute;

        [ObservableProperty]
        private string? _endHour;

        [ObservableProperty]
        private string? _endMinute;

        [ObservableProperty]
        private bool _isEditingExistingSlot = false;

        public ObservableCollection<TimeSetConfigModel> TimeSetConfigs { get; } = new();

        // 小时选项：0-23
        public ObservableCollection<string> HourOptions { get; } = new();

        // 分钟选项：0, 10, 20, 30, 40, 50
        public ObservableCollection<string> MinuteOptions { get; } = new();

        public TimeSlotEditViewModel()
        {
            InitializeTimeOptions();
            LoadTimeSetData();
        }

        /// <summary>
        /// 初始化时间选项
        /// </summary>
        private void InitializeTimeOptions()
        {
            // 初始化小时选项 0-23
            for (int i = 0; i <= 23; i++)
            {
                HourOptions.Add(i.ToString());
            }

            // 初始化分钟选项：00, 10, 20, 30, 40, 50
            for (int i = 0; i <= 50; i += 10)
            {
                MinuteOptions.Add(i.ToString("D2"));
            }
        }

        partial void OnSelectedTimeSlotChanged(string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                // 解析时段并填充到输入框
                ParseTimeSlot(value);
                IsEditingExistingSlot = true;
            }
            else
            {
                IsEditingExistingSlot = false;
            }
        }

        /// <summary>
        /// 解析时段字符串并填充到输入框
        /// </summary>
        private void ParseTimeSlot(string timeSlot)
        {
            try
            {
                var parts = timeSlot.Split('-');
                if (parts.Length == 2)
                {
                    var startParts = parts[0].Trim().Split(':');
                    var endParts = parts[1].Trim().Split(':');

                    if (startParts.Length == 2 && endParts.Length == 2)
                    {
                        StartHour = int.Parse(startParts[0]).ToString();
                        StartMinute = int.Parse(startParts[1]).ToString();
                        EndHour = int.Parse(endParts[0]).ToString();
                        EndMinute = int.Parse(endParts[1]).ToString();
                    }
                }
            }
            catch
            {
                // 解析失败，保持当前值
            }
        }

        /// <summary>
        /// 加载时段配置数据
        /// </summary>
        private void LoadTimeSetData()
        {
            try
            {
                var timeSetJson = _jsonData.GetTimeSetJson();
                var timeSetData = JsonConvert.DeserializeObject<ObservableCollection<TimeSetConfigModel>>(timeSetJson);

                TimeSetConfigs.Clear();

                if (timeSetData != null)
                {
                    foreach (var item in timeSetData)
                    {
                        TimeSetConfigs.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载时段配置数据失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 添加新的预约时段配置
        /// </summary>
        [RelayCommand]
        private void AddConfig()
        {
            var newConfig = new TimeSetConfigModel
            {
                Set_Id = $"预约时段配置{TimeSetConfigs.Count + 1}",
                TimeSlots = new ObservableCollection<string>()
            };

            TimeSetConfigs.Add(newConfig);
            SelectedConfig = newConfig;
        }

        /// <summary>
        /// 删除选中的预约时段配置
        /// </summary>
        [RelayCommand]
        private void DeleteConfig()
        {
            if (SelectedConfig == null)
            {
                MessageBox.Show("请先选择要删除的预约时段配置", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"确定要删除配置 '{SelectedConfig.Set_Id}' 吗？",
                "确认删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TimeSetConfigs.Remove(SelectedConfig);
                SelectedConfig = TimeSetConfigs.FirstOrDefault();
                MessageBox.Show("配置已删除！请点击顶部'保存数据'按钮保存所有修改。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 添加或更新时段
        /// </summary>
        [RelayCommand]
        private void AddOrUpdateTimeSlot()
        {
            if (SelectedConfig == null)
            {
                MessageBox.Show("请先选择一个预约时段配置", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 验证输入
            if (!ValidateTimeInputs())
            {
                return;
            }

            // 构建时段字符串，确保格式为 HH:mm
            var sh = int.Parse(StartHour!);
            var sm = int.Parse(StartMinute!);
            var eh = int.Parse(EndHour!);
            var em = int.Parse(EndMinute!);

            var newTimeSlot = $"{sh}:{sm:D2}-{eh}:{em:D2}";
            var startTime = new TimeSpan(sh, sm, 0);
            var endTime = new TimeSpan(eh, em, 0);

            if (IsEditingExistingSlot && !string.IsNullOrEmpty(SelectedTimeSlot))
            {
                // 更新现有时段 - 检查与其他时段的冲突（排除当前编辑的时段）
                if (CheckTimeSlotConflict(startTime, endTime, SelectedTimeSlot))
                {
                    return;
                }

                var index = SelectedConfig.TimeSlots.IndexOf(SelectedTimeSlot);
                if (index >= 0)
                {
                    SelectedConfig.TimeSlots[index] = newTimeSlot;
                    MessageBox.Show("时段已更新！请点击顶部'保存数据'按钮保存所有修改。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // 添加新时段
                // 检查是否已存在
                if (SelectedConfig.TimeSlots.Contains(newTimeSlot))
                {
                    MessageBox.Show("该时段已存在", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // 检查时段冲突
                if (CheckTimeSlotConflict(startTime, endTime))
                {
                    return;
                }

                SelectedConfig.TimeSlots.Add(newTimeSlot);
                MessageBox.Show("时段已添加！请点击顶部'保存数据'按钮保存所有修改。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            // 清空输入框
            ClearInputs();
        }

        /// <summary>
        /// 验证时间输入
        /// </summary>
        private bool ValidateTimeInputs()
        {
            // 验证是否为空
            if (string.IsNullOrWhiteSpace(StartHour) || string.IsNullOrWhiteSpace(StartMinute) ||
                string.IsNullOrWhiteSpace(EndHour) || string.IsNullOrWhiteSpace(EndMinute))
            {
                MessageBox.Show("请选择完整的时间信息", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // 验证是否为数字
            if (!int.TryParse(StartHour, out int sh) || !int.TryParse(StartMinute, out int sm) ||
                !int.TryParse(EndHour, out int eh) || !int.TryParse(EndMinute, out int em))
            {
                MessageBox.Show("请选择有效的时间", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // 验证开始时间小于结束时间
            var startTime = new TimeSpan(sh, sm, 0);
            var endTime = new TimeSpan(eh, em, 0);

            if (startTime >= endTime)
            {
                MessageBox.Show("开始时间必须小于结束时间", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // 验证时段最小时长（至少30分钟）
            var duration = endTime - startTime;
            if (duration.TotalMinutes < 30)
            {
                MessageBox.Show("时段时长不能少于30分钟", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查时段是否与已有时段冲突
        /// </summary>
        private bool CheckTimeSlotConflict(TimeSpan newStartTime, TimeSpan newEndTime, string? excludeTimeSlot = null)
        {
            if (SelectedConfig == null)
                return false;

            foreach (var existingSlot in SelectedConfig.TimeSlots)
            {
                // 如果是编辑模式，跳过当前正在编辑的时段
                if (excludeTimeSlot != null && existingSlot == excludeTimeSlot)
                    continue;

                // 解析已有时段
                var parts = existingSlot.Split('-');
                if (parts.Length != 2)
                    continue;

                var startParts = parts[0].Trim().Split(':');
                var endParts = parts[1].Trim().Split(':');

                if (startParts.Length != 2 || endParts.Length != 2)
                    continue;

                if (!int.TryParse(startParts[0], out int existingStartHour) ||
                    !int.TryParse(startParts[1], out int existingStartMinute) ||
                    !int.TryParse(endParts[0], out int existingEndHour) ||
                    !int.TryParse(endParts[1], out int existingEndMinute))
                    continue;

                var existingStartTime = new TimeSpan(existingStartHour, existingStartMinute, 0);
                var existingEndTime = new TimeSpan(existingEndHour, existingEndMinute, 0);

                // 检查时间段是否重叠
                // 重叠的条件：新时段的开始时间在已有时段内，或新时段的结束时间在已有时段内，或新时段完全包含已有时段
                if ((newStartTime >= existingStartTime && newStartTime < existingEndTime) ||
                    (newEndTime > existingStartTime && newEndTime <= existingEndTime) ||
                    (newStartTime <= existingStartTime && newEndTime >= existingEndTime))
                {
                    MessageBox.Show($"时段冲突！新时段与已有时段 '{existingSlot}' 存在重叠", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 清空输入框
        /// </summary>
        [RelayCommand]
        private void ClearInputs()
        {
            StartHour = null;
            StartMinute = null;
            EndHour = null;
            EndMinute = null;
            SelectedTimeSlot = null;
            IsEditingExistingSlot = false;
        }

        /// <summary>
        /// 删除选中的时段
        /// </summary>
        [RelayCommand]
        private void DeleteTimeSlot()
        {
            if (SelectedConfig == null)
            {
                MessageBox.Show("请先选择一个预约时段配置", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(SelectedTimeSlot))
            {
                MessageBox.Show("请先选择要删除的时段", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show(
                $"确定要删除时段 '{SelectedTimeSlot}' 吗？",
                "确认删除",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SelectedConfig.TimeSlots.Remove(SelectedTimeSlot);
                MessageBox.Show("时段已删除！请点击顶部'保存数据'按钮保存所有修改。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearInputs();
            }
        }

        /// <summary>
        /// 保存所有修改
        /// </summary>
        [RelayCommand]
        private void Save()
        {
            try
            {
                // 序列化为JSON
                var json = JsonConvert.SerializeObject(TimeSetConfigs, Formatting.Indented);

                // 保存到Json.cs文件
                SaveToJsonFile(json);

                MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// 保存JSON数据到Json.cs文件
        /// </summary>
        private void SaveToJsonFile(string jsonData)
        {
            try
            {
                // 获取Json.cs文件路径
                var projectPath = Directory.GetCurrentDirectory();
                var jsonFilePath = Path.Combine(projectPath, "..", "..", "..", "Services", "Json.cs");
                jsonFilePath = Path.GetFullPath(jsonFilePath);

                if (!File.Exists(jsonFilePath))
                {
                    throw new FileNotFoundException("未找到Json.cs文件", jsonFilePath);
                }

                // 读取文件内容
                var fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                // 转义JSON字符串中的双引号（在逐字字符串中，双引号需要转义为 ""）
                var escapedJson = jsonData.Replace("\"", "\"\"");

                // 格式化JSON字符串，添加缩进
                var lines = escapedJson.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var formattedJson = string.Join("\r\n            ", lines);

                // 构建新的_TimeSet字段值（使用逐字字符串格式）
                var newTimeSetValue = $"@\"{formattedJson}\"";

                // 找到_TimeSet字段的开始位置
                var startIndex = fileContent.IndexOf("public readonly string _TimeSet = @\"");
                if (startIndex == -1)
                {
                    throw new InvalidOperationException("未找到_TimeSet字段定义");
                }

                // 找到该字段的结束位置（找到下一个 ";）
                var searchStart = startIndex + "public readonly string _TimeSet = ".Length;
                var endIndex = fileContent.IndexOf("\";", searchStart);
                if (endIndex == -1)
                {
                    throw new InvalidOperationException("_TimeSet字段定义不完整");
                }

                // 找到完整的_TimeSet定义（包含分号）
                var currentDefinition = fileContent.Substring(startIndex, endIndex - startIndex + 2);

                // 替换为新的定义
                fileContent = fileContent.Replace(currentDefinition, $"public readonly string _TimeSet = {newTimeSetValue};");

                // 写回文件
                File.WriteAllText(jsonFilePath, fileContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"写入Json.cs文件失败: {ex.Message}", ex);
            }
        }
    }
}
