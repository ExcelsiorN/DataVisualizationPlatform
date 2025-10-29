using System.ComponentModel;

namespace DataVisualizationPlatform.Models
{
    public class BarDataItem : INotifyPropertyChanged
    {
        public string Label { get; set; } = string.Empty;    // 年月标识 
        public double Part1 { get; set; }     // 月预约单数量
        public double Part2 { get; set; }     // 月故障数量
        public double Total => Part1 + Part2; // 总计
        public String Fal_Data { get; set; } = string.Empty; // 故障数据
        public String Fal_Eqid { get; set; } = string.Empty; // 设备ID
        public int Fal_Id { get; set; }          // 故障ID
        public String Fal_Type { get; set; } = string.Empty; // 故障类型
        public String Fal_Info { get; set; } = string.Empty; // 故障描述
        public String Fal_Detail { get; set; } = string.Empty; // 故障详情



        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
