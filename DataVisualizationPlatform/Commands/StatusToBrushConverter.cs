using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace DataVisualizationPlatform.Commands;

public class StatusToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string status = value?.ToString()?.Trim();
        return status switch
        {
            "在线" => Brushes.LimeGreen,
            "已支付" => Brushes.LimeGreen,
            "已完成" => Brushes.LimeGreen,
            "离线" => Brushes.Gray,
            "待支付" => Brushes.Gray,
            "未完成" => Brushes.Gray,
            "已退款" => Brushes.Red,
            "已取消" => Brushes.Red,
            "待处理" => Brushes.Red,
            "处理中" => Brushes.Orange,
            "已处理" => Brushes.Green,
            _ => Brushes.LightGray,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}