using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Controls;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using DataVisualizationPlatform.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataVisualizationPlatform.ViewModels
{
    public class FaultReportViewModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();
        private List<BarDataItem> _allFaultReportData = new(); // 存储所有原始数据
        public ICommand RefreshCommand { get; }

        public event Action RequestScrollToTop;

        // 筛选后的数据列表
        public ObservableCollection<BarDataItem> FaultReportList { get; } = new();

        // 设备列表
        public ObservableCollection<string> Devices { get; } = new();

        // 年份列表
        public ObservableCollection<string> Years { get; } = new();

        // 月份列表
        public ObservableCollection<string> Months { get; } = new();

        // 状态列表
        public ObservableCollection<string> Statuses { get; } = new();


        private string _selectedDevice = "全部";
        public string SelectedDevice
        {
            get => _selectedDevice;
            set
            {
                if (SetProperty(ref _selectedDevice, value))
                {
                    ApplyFiltersAndSort();
                }
            }
        }

        private string _selectedFal_Info = "全部";
        public string SelectedFal_Info
        {
            get => _selectedFal_Info;
            set
            {
                if (SetProperty(ref _selectedFal_Info, value))
                {
                    ApplyFiltersAndSort();
                }
            }
        }

        private string _selectedYear = "全部";
        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (SetProperty(ref _selectedYear, value))
                {
                    // 当选择年份时，更新月份可见性
                    IsMonthVisible = value != "全部";
                    if (value == "全部")
                    {
                        SelectedMonth = "全部";
                    }
                    ApplyFiltersAndSort();
                }
            }
        }

        private string _selectedMonth = "全部";
        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                if (SetProperty(ref _selectedMonth, value))
                {
                    ApplyFiltersAndSort();
                }
            }
        }


        private bool _isMonthVisible = false;
        public bool IsMonthVisible
        {
            get => _isMonthVisible;
            set => SetProperty(ref _isMonthVisible, value);
        }

        public FaultReportViewModel()
        {
            InitializeFilters();

            LoadData();

            RefreshCommand = new RelayCommand(ExecuteRefresh);
        }

        #region Private Methods

        private void InitializeFilters()
        {
            // 初始化月份选择器（1-12月）
            Months.Clear(); // 先清空，避免重复添加
            Months.Add("全部");
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(i.ToString("00"));
                Debug.WriteLine($"Added month: {i.ToString("00")}");
            }

            Debug.WriteLine($"=== Months initialized, Count: {Months.Count} ===");
            foreach (var month in Months)
            {
                Debug.WriteLine($"Month: {month}");
            }
        }

        private void LoadData()
        {
            try
            {
                var faultReport = JsonConvert.DeserializeObject<List<BarDataItem>>(_jsonData._FaultReport);

                if (faultReport != null)
                {
                    _allFaultReportData = faultReport;
                    Debug.WriteLine("=== Sample Data Labels ===");
                    foreach (var item in _allFaultReportData.Take(5))
                    {
                        Debug.WriteLine($"Label: {item.Label}, Fal_Eqid: {item.Fal_Eqid}, Fal_Type: {item.Fal_Type}");
                    }
                    // 提取并填充设备列表
                    ExtractDevices();

                    // 提取并填充年份列表
                    ExtractYears();

                    // 提取并填充状态列表
                    ExtractFal_Info();

                    // 初始显示所有数据（排序后）
                    ApplyFiltersAndSort();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"加载设备信息失败: {ex.Message}");
            }
        }

        private void ExecuteRefresh()
        {
            SelectedDevice = "全部";
            SelectedFal_Info = "全部";
            SelectedYear = "全部";
            SelectedMonth = "全部";
        }


        private void ExtractDevices()
        {
            Devices.Clear();
            Devices.Add("全部");

            // 从数据中提取唯一的设备ID
            var uniqueDevices = _allFaultReportData
                .Where(item => !string.IsNullOrEmpty(item.Fal_Eqid))
                .Select(item => item.Fal_Eqid)
                .Distinct()
                .OrderBy(id => id);

            foreach (var device in uniqueDevices)
            {
                Devices.Add(device);
                Debug.WriteLine("Added device: " + device);
            }
        }

        private void ExtractFal_Info()
        {
            Statuses.Clear();
            Statuses.Add("全部");
            // 从数据中提取唯一的状态
            var uniqueStatuses = _allFaultReportData
                .Where(item => !string.IsNullOrEmpty(item.Fal_Info))
                .Select(item => item.Fal_Info)
                .Distinct()
                .OrderBy(status => status);
            foreach (var status in uniqueStatuses)
            {
                Statuses.Add(status);
                Debug.WriteLine("Added status: " + status);
            }
        }

        private void ExtractYears()
        {
            Years.Clear();
            Years.Add("全部");

            DateTime start = new DateTime(2023, 1, 1);
            DateTime end = DateTime.Now;

            for (DateTime date = start; date <= end; date = date.AddYears(1))
            {
                Years.Add(date.ToString("yyyy"));
            }
        }

        private void ApplyFiltersAndSort()
        {
            if (_allFaultReportData == null) return;

            // 应用筛选
            var filteredData = _allFaultReportData.AsEnumerable();

            // 按状态筛选
            if (SelectedFal_Info != "全部" && !string.IsNullOrEmpty(SelectedFal_Info))
            {
                filteredData = filteredData.Where(item => item.Fal_Info == SelectedFal_Info);
            }

            // 按设备筛选
            if (SelectedDevice != "全部" && !string.IsNullOrEmpty(SelectedDevice))
            {
                filteredData = filteredData.Where(item => item.Fal_Eqid == SelectedDevice);
            }

            // 按年份筛选
            if (SelectedYear != "全部" && !string.IsNullOrEmpty(SelectedYear))
            {
                filteredData = filteredData.Where(item =>
                    !string.IsNullOrEmpty(item.Fal_Data) &&
                    item.Fal_Data.StartsWith(SelectedYear));
            }

            // 按月份筛选（仅在选择了年份后）
            if (IsMonthVisible && SelectedMonth != "全部" && !string.IsNullOrEmpty(SelectedMonth))
            {
                string yearMonth = SelectedYear + SelectedMonth; // 组合成YYYYMM格式
                string yearMonthDash = SelectedYear + "-" + SelectedMonth; // 组合成YYYY-MM格式

                filteredData = filteredData.Where(item =>
                    !string.IsNullOrEmpty(item.Fal_Data) &&
                    (item.Fal_Data.StartsWith(yearMonth) || item.Fal_Data.StartsWith(yearMonthDash)));
            }

            // 排序：将"待处理"的故障靠前显示
            var sortedData = filteredData.OrderBy(item =>
            {
                // 首先按故障类型排序："待处理"排在前面
                if (item.Fal_Info == "待处理")
                    return 0;
                else if (item.Fal_Info == "处理中")
                    return 1;
                else if (item.Fal_Info == "已处理")
                    return 2;
                else
                    return 3;
            })
            .ThenBy(item => item.Label) // 其次按时间排序
            .ThenBy(item => item.Fal_Eqid); // 最后按设备ID排序

            // 更新显示列表
            FaultReportList.Clear();
            foreach (var item in sortedData)
            {
                FaultReportList.Add(item);
            }

            RequestScrollToTop?.Invoke();
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}