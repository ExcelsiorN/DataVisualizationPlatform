using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Collections.ObjectModel;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;


namespace DataVisualizationPlatform.Controls
{
    public static class ChartDataService    
    {
        public static ObservableCollection<Models.BarDataItem> LoadBarData(Json jsonData)
        {
            // 加载预订数据
            var reservations = JsonConvert.DeserializeObject<List<HomePageModel>>(jsonData._ReservationList);
            var completedReservations = reservations
                .Where(r => r.Res_Status == "已完成")
                .Where(r => DateTime.TryParse(r.Res_Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .ToList();

            // 加载故障报告数据
            var faultReports = JsonConvert.DeserializeObject<List<BarDataItem>>(jsonData._FaultReport);

            // 提取所有有效日期
            var reservationDates = completedReservations
                .Select(r => DateTime.Parse(r.Res_Date, CultureInfo.InvariantCulture))
                .ToList();

            var faultDates = faultReports
                .Where(f => !string.IsNullOrEmpty(f.Fal_Data))
                .Select(f => DateTime.ParseExact(f.Fal_Data, "yyyy-MM", CultureInfo.InvariantCulture))
                .ToList();

            var allDates = reservationDates.Concat(faultDates).ToList();
            if (allDates.Count == 0)
                return new ObservableCollection<Models.BarDataItem>();

            // 找出最小和最大月份
            DateTime minDate = new DateTime(allDates.Min(d => d).Year, allDates.Min(d => d).Month, 1);
            DateTime maxDate = new DateTime(allDates.Max(d => d).Year, allDates.Max(d => d).Month, 1);

            var result = new List<Models.BarDataItem>();

            for (DateTime dt = minDate; dt <= maxDate; dt = dt.AddMonths(1))
            {
                string monthLabel = dt.ToString("yyyy-MM");

                int monthlyReservations = reservationDates.Count(d => d.Year == dt.Year && d.Month == dt.Month);
                int monthlyFaults = faultDates.Count(d => d.Year == dt.Year && d.Month == dt.Month);

                Debug.WriteLine($"[ChartDataService] {monthLabel}: 预约{monthlyReservations}单, 故障{monthlyFaults}个");

                result.Add(new Models.BarDataItem
                {
                    Label = monthLabel,
                    Part1 = monthlyReservations,
                    Part2 = monthlyFaults
                });
            }

            return new ObservableCollection<Models.BarDataItem>(result);
        }
    }
}