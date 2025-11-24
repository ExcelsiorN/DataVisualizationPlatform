using System;
using System.IO;
using System.Text;

namespace DataVisualizationPlatform.Services
{
    /// <summary>
    /// JSON数据管理器 - 负责读取和写入独立的JSON文件
    /// </summary>
    public class JsonDataManager
    {
        private static JsonDataManager? _instance;
        private static readonly object _lock = new object();

        private readonly string _dataFolderPath;
        private readonly string _equipmentInfoPath;
        private readonly string _reservationListPath;
        private readonly string _faultReportPath;
        private readonly string _timeSetPath;

        private JsonDataManager()
        {
            // 获取数据文件夹路径
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 向上查找项目根目录
            DirectoryInfo? directory = new DirectoryInfo(baseDirectory);
            while (directory != null && directory.Name != "DataVisualizationPlatform")
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                _dataFolderPath = Path.Combine(directory.FullName, "Data");
            }
            else
            {
                // 如果找不到项目根目录，使用相对路径
                _dataFolderPath = Path.Combine(baseDirectory, "..", "..", "..", "Data");
                _dataFolderPath = Path.GetFullPath(_dataFolderPath);
            }

            // 确保Data文件夹存在
            if (!Directory.Exists(_dataFolderPath))
            {
                Directory.CreateDirectory(_dataFolderPath);
            }

            // 设置各个JSON文件的路径
            _equipmentInfoPath = Path.Combine(_dataFolderPath, "EquipmentInfo.json");
            _reservationListPath = Path.Combine(_dataFolderPath, "ReservationList.json");
            _faultReportPath = Path.Combine(_dataFolderPath, "FaultReport.json");
            _timeSetPath = Path.Combine(_dataFolderPath, "TimeSet.json");

            // 初始化文件（如果不存在则创建）
            InitializeFiles();
        }

        public static JsonDataManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new JsonDataManager();
                        }
                    }
                }
                return _instance;
            }
        }

        private void InitializeFiles()
        {
            // 如果文件不存在，创建空的JSON数组
            CreateFileIfNotExists(_equipmentInfoPath);
            CreateFileIfNotExists(_reservationListPath);
            CreateFileIfNotExists(_faultReportPath);
            CreateFileIfNotExists(_timeSetPath);
        }

        private void CreateFileIfNotExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "[]", Encoding.UTF8);
            }
        }

        /// <summary>
        /// 读取设备信息JSON
        /// </summary>
        public string GetEquipmentInfoJson()
        {
            try
            {
                if (File.Exists(_equipmentInfoPath))
                {
                    return File.ReadAllText(_equipmentInfoPath, Encoding.UTF8);
                }
                return "[]";
            }
            catch (Exception ex)
            {
                throw new Exception($"读取设备信息JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入设备信息JSON
        /// </summary>
        public void SaveEquipmentInfoJson(string jsonContent)
        {
            try
            {
                File.WriteAllText(_equipmentInfoPath, jsonContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存设备信息JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取预约列表JSON
        /// </summary>
        public string GetReservationListJson()
        {
            try
            {
                if (File.Exists(_reservationListPath))
                {
                    return File.ReadAllText(_reservationListPath, Encoding.UTF8);
                }
                return "[]";
            }
            catch (Exception ex)
            {
                throw new Exception($"读取预约列表JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入预约列表JSON
        /// </summary>
        public void SaveReservationListJson(string jsonContent)
        {
            try
            {
                File.WriteAllText(_reservationListPath, jsonContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存预约列表JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取故障报告JSON
        /// </summary>
        public string GetFaultReportJson()
        {
            try
            {
                if (File.Exists(_faultReportPath))
                {
                    return File.ReadAllText(_faultReportPath, Encoding.UTF8);
                }
                return "[]";
            }
            catch (Exception ex)
            {
                throw new Exception($"读取故障报告JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入故障报告JSON
        /// </summary>
        public void SaveFaultReportJson(string jsonContent)
        {
            try
            {
                File.WriteAllText(_faultReportPath, jsonContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存故障报告JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 读取时段配置JSON
        /// </summary>
        public string GetTimeSetJson()
        {
            try
            {
                if (File.Exists(_timeSetPath))
                {
                    return File.ReadAllText(_timeSetPath, Encoding.UTF8);
                }
                return "[]";
            }
            catch (Exception ex)
            {
                throw new Exception($"读取时段配置JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 写入时段配置JSON
        /// </summary>
        public void SaveTimeSetJson(string jsonContent)
        {
            try
            {
                File.WriteAllText(_timeSetPath, jsonContent, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存时段配置JSON失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取数据文件夹路径
        /// </summary>
        public string GetDataFolderPath() => _dataFolderPath;
    }
}
