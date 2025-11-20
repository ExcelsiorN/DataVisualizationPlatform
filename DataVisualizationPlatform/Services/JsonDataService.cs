using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataVisualizationPlatform.Services
{
    /// <summary>
    /// JSON 数据服务
    /// 提供从 Json.cs 文件动态读取最新数据的功能
    /// </summary>
    public class JsonDataService
    {
        private static JsonDataService? _instance;
        private static readonly object _lock = new object();

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
        /// 从 Json.cs 文件中读取最新的设备信息
        /// </summary>
        public string GetEquipmentInfoJson()
        {
            try
            {
                string jsonFilePath = FindJsonFilePath();
                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    // 如果找不到文件，返回默认数据
                    return new Json()._EquipmentInfo;
                }

                string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                // 使用正则表达式提取 _EquipmentInfo 的内容
                var match = Regex.Match(fileContent, @"public readonly string _EquipmentInfo = @""([\s\S]*?)"";", RegexOptions.Multiline);

                if (match.Success && match.Groups.Count > 1)
                {
                    // 提取匹配的内容并反转义
                    string content = match.Groups[1].Value;
                    // 将双引号转义还原
                    content = content.Replace("\"\"", "\"");
                    // 移除多余的缩进空格
                    content = Regex.Replace(content, @"^        ", "", RegexOptions.Multiline);
                    return content;
                }

                // 如果无法解析，返回默认数据
                return new Json()._EquipmentInfo;
            }
            catch
            {
                // 发生错误时返回默认数据
                return new Json()._EquipmentInfo;
            }
        }

        /// <summary>
        /// 从 Json.cs 文件中读取最新的故障报告数据
        /// </summary>
        public string GetFaultReportJson()
        {
            try
            {
                string jsonFilePath = FindJsonFilePath();
                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    // 如果找不到文件，返回默认数据
                    return new Json()._FaultReport;
                }

                string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

                // 使用正则表达式提取 _FaultReport 的内容
                var match = Regex.Match(fileContent, @"public readonly string _FaultReport = @""([\s\S]*?)"";", RegexOptions.Multiline);

                if (match.Success && match.Groups.Count > 1)
                {
                    // 提取匹配的内容并反转义
                    string content = match.Groups[1].Value;
                    // 将双引号转义还原
                    content = content.Replace("\"\"", "\"");
                    // 移除多余的缩进空格
                    content = Regex.Replace(content, @"^        ", "", RegexOptions.Multiline);
                    return content;
                }

                // 如果无法解析，返回默认数据
                return new Json()._FaultReport;
            }
            catch
            {
                // 发生错误时返回默认数据
                return new Json()._FaultReport;
            }
        }

        /// <summary>
        /// 查找 Json.cs 文件路径
        /// </summary>
        private string FindJsonFilePath()
        {
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
    }
}
