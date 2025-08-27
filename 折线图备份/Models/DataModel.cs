using DataVisualizationPlatform.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Globalization;

namespace DataVisualizationPlatform.Models
{
    public class DataModel : INotifyPropertyChanged
    {
        private readonly Json _jsonData = new Json();

        public class MonthlyChartData
        {
            public string Month { get; set; } = string.Empty;
            public int ReservationCount { get; set; }
            public int CancelledReservationCount { get; set; } // 新增：已取消预约数量
            public int FaultCount { get; set; }
            public List<FaultEquipmentInfo> FaultEquipments { get; set; } = new List<FaultEquipmentInfo>();
        }

        public class FaultEquipmentInfo
        {
            public string Equ_Id { get; set; } = string.Empty;      // 设备编号
            public string Equ_Name { get; set; } = string.Empty;    // 设备名称
        }

        public List<MonthlyChartData> GetMonthlyData()
        {
            string faultJson = _jsonData.GetFaultReportJson();
            string reservationJson = _jsonData.GetReservationListJson();
            string equipmentJson = _jsonData.GetEquipmentInfoJson();

            var faults = JsonConvert.DeserializeObject<JArray>(faultJson);
            var reservations = JsonConvert.DeserializeObject<JArray>(reservationJson);
            var equipment = JsonConvert.DeserializeObject<JArray>(equipmentJson);

            var allMonths = faults.Select(f => ParseMonth((string)f["Fal_Data"]))
                .Concat(reservations.Select(r => ParseMonth((string)r["Res_Date"])))
                .Where(m => !string.IsNullOrEmpty(m))
                .Distinct()
                .OrderBy(m => DateTime.ParseExact(m!, "yyyy-MM", CultureInfo.InvariantCulture))
                .ToList();

            var result = new List<MonthlyChartData>();
            foreach (var month in allMonths)
            {
                int faultCount = faults.Count(f => ParseMonth((string)f["Fal_Data"]) == month);
                int reservationCount = reservations.Count(r =>
                    ParseMonth((string)r["Res_Date"]) == month &&
                    (string)r["Res_Status"] == "未完成");

                int cancelledReservationCount = reservations.Count(r =>
                    ParseMonth((string)r["Res_Date"]) == month &&
                    (string)r["Res_Status"] == "已取消");

                // 当月故障设备信息
                var faultEquipments = faults
                    .Where(f => ParseMonth((string)f["Fal_Data"]) == month)
                    .Select(f =>
                    {
                        string eqId = (string)f["Fal_Id"];
                        string eqName = equipment.FirstOrDefault(e => (string)e["Equ_Id"] == eqId)?["Equ_Name"]?.ToString() ?? "";
                        return new FaultEquipmentInfo { Equ_Id = eqId, Equ_Name = eqName };
                    })
                    .ToList();

                result.Add(new MonthlyChartData
                {
                    Month = month!,
                    ReservationCount = reservationCount,
                    CancelledReservationCount = cancelledReservationCount,
                    FaultCount = faultCount,
                    FaultEquipments = faultEquipments
                });
            }

            return result;
        }



        private string? ParseMonth(string? dateStr)
        {
            if (DateTime.TryParse(dateStr, out var date))
            {
                return date.ToString("yyyy-MM");
            }
            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}