using DataVisualizationPlatform.Commands;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DataVisualizationPlatform.ViewModels
{
    public class FaultReportViewModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();
        public ObservableCollection<FaultReportModel> EquipmentList { get; } = new();
        public ICommand FlipCardCommand { get; }

        public FaultReportViewModel()
        {
            FlipCardCommand = new RelayCommand<FaultReportModel>(FlipCard);
            LoadEquipment();
        }

        private async void FlipCard(FaultReportModel equipment)
        {
            if (equipment == null) return;

            if (!equipment.IsFlipped)
            {
                // 开始翻转到背面
                equipment.IsFlipped = true;

                // 等待翻转动画到中间点（压缩完成）
                await Task.Delay(250);

                // 切换内容到背面
                equipment.IsContentFlipped = true;

                // 开始放大动画
                equipment.IsExpanded = true;

                // 等待翻转恢复完成
                await Task.Delay(250);
            }
            else
            {
                // 开始翻回正面
                equipment.IsFlipped = false;

                // 等待翻转到中间点
                await Task.Delay(250);

                // 切换内容到正面
                equipment.IsContentFlipped = false;

                // 开始缩小
                equipment.IsExpanded = false;

                // 等待缩小完成
                await Task.Delay(300);
            }
        }

        private void LoadEquipment()
        {
            try
            {
                var equipment = JsonConvert.DeserializeObject<List<FaultReportModel>>(_jsonData._EquipmentInfo);
                EquipmentList.Clear();
                if (equipment != null)
                {
                    foreach (var item in equipment)
                        EquipmentList.Add(item);
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