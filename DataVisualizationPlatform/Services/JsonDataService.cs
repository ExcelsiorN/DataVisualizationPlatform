namespace DataVisualizationPlatform.Services
{
    /// <summary>
    /// JSON 数据服务
    /// 提供从独立JSON文件读取最新数据的功能
    /// </summary>
    public class JsonDataService
    {
        private static JsonDataService? _instance;
        private static readonly object _lock = new object();
        private readonly JsonDataManager _dataManager;

        private JsonDataService()
        {
            _dataManager = JsonDataManager.Instance;
        }

        public static JsonDataService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new JsonDataService();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 读取设备信息JSON
        /// </summary>
        public string GetEquipmentInfoJson()
        {
            return _dataManager.GetEquipmentInfoJson();
        }

        /// <summary>
        /// 读取预约列表JSON
        /// </summary>
        public string GetReservationListJson()
        {
            return _dataManager.GetReservationListJson();
        }

        /// <summary>
        /// 读取故障报告JSON
        /// </summary>
        public string GetFaultReportJson()
        {
            return _dataManager.GetFaultReportJson();
        }

        /// <summary>
        /// 读取时段配置JSON
        /// </summary>
        public string GetTimeSetJson()
        {
            return _dataManager.GetTimeSetJson();
        }

        /// <summary>
        /// 保存设备信息JSON
        /// </summary>
        public void SaveEquipmentInfoJson(string jsonContent)
        {
            _dataManager.SaveEquipmentInfoJson(jsonContent);
        }

        /// <summary>
        /// 保存预约列表JSON
        /// </summary>
        public void SaveReservationListJson(string jsonContent)
        {
            _dataManager.SaveReservationListJson(jsonContent);
        }

        /// <summary>
        /// 保存故障报告JSON
        /// </summary>
        public void SaveFaultReportJson(string jsonContent)
        {
            _dataManager.SaveFaultReportJson(jsonContent);
        }

        /// <summary>
        /// 保存时段配置JSON
        /// </summary>
        public void SaveTimeSetJson(string jsonContent)
        {
            _dataManager.SaveTimeSetJson(jsonContent);
        }
    }
}
