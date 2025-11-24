using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DataVisualizationPlatform.Tools
{
    /// <summary>
    /// 用于将Json.cs中的verbatim字符串转换为标准JSON文件
    /// </summary>
    public class JsonConverter
    {
        public static void ConvertJsonCsToFiles()
        {
            try
            {
                // 查找Json.cs文件
                string jsonCsPath = FindJsonCsPath();
                if (string.IsNullOrEmpty(jsonCsPath) || !File.Exists(jsonCsPath))
                {
                    Console.WriteLine("错误：找不到Json.cs文件");
                    return;
                }

                Console.WriteLine($"找到Json.cs文件：{jsonCsPath}");

                // 读取Json.cs文件内容
                string content = File.ReadAllText(jsonCsPath, Encoding.UTF8);

                // 提取并转换四个JSON字段
                ExtractAndSave(content, "_EquipmentInfo", "EquipmentInfo.json");
                ExtractAndSave(content, "_ReservationList", "ReservationList.json");
                ExtractAndSave(content, "_FaultReport", "FaultReport.json");
                ExtractAndSave(content, "_TimeSet", "TimeSet.json");

                Console.WriteLine("\n转换完成！");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"转换失败：{ex.Message}");
            }
        }

        private static void ExtractAndSave(string fileContent, string fieldName, string outputFileName)
        {
            try
            {
                Console.WriteLine($"\n正在处理 {fieldName}...");

                // 使用正则表达式提取字段内容
                string pattern = $@"public readonly string {fieldName} = @""([\s\S]*?)"";";
                var match = Regex.Match(fileContent, pattern, RegexOptions.Multiline);

                if (!match.Success || match.Groups.Count <= 1)
                {
                    Console.WriteLine($"  警告：未找到 {fieldName} 字段");
                    return;
                }

                // 提取匹配的内容
                string jsonContent = match.Groups[1].Value;

                // 转换格式
                jsonContent = ConvertVerbatimToJson(jsonContent);

                // 保存到文件
                string dataFolder = GetDataFolderPath();
                string outputPath = Path.Combine(dataFolder, outputFileName);

                File.WriteAllText(outputPath, jsonContent, Encoding.UTF8);

                Console.WriteLine($"  ✓ 已保存到：{outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ 处理 {fieldName} 失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 将C# verbatim字符串转换为标准JSON格式
        /// </summary>
        private static string ConvertVerbatimToJson(string verbatimString)
        {
            // 1. 将双引号 "" 替换为单引号 "
            string result = verbatimString.Replace("\"\"", "\"");

            // 2. 移除每行开头的8个空格
            result = Regex.Replace(result, @"^        ", "", RegexOptions.Multiline);

            // 3. 去除首尾空白
            result = result.Trim();

            return result;
        }

        private static string FindJsonCsPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 向上查找项目根目录
            DirectoryInfo? directory = new DirectoryInfo(baseDirectory);
            while (directory != null && directory.Name != "DataVisualizationPlatform")
            {
                directory = directory.Parent;
            }

            if (directory != null)
            {
                string jsonPath = Path.Combine(directory.FullName, "Services", "Json.cs");
                if (File.Exists(jsonPath))
                    return jsonPath;
            }

            // 备用搜索路径
            string[] possiblePaths = new[]
            {
                Path.Combine(baseDirectory, "..", "..", "..", "Services", "Json.cs"),
                Path.Combine(baseDirectory, "Services", "Json.cs"),
            };

            foreach (var path in possiblePaths)
            {
                string fullPath = Path.GetFullPath(path);
                if (File.Exists(fullPath))
                    return fullPath;
            }

            return string.Empty;
        }

        private static string GetDataFolderPath()
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 向上查找项目根目录
            DirectoryInfo? directory = new DirectoryInfo(baseDirectory);
            while (directory != null && directory.Name != "DataVisualizationPlatform")
            {
                directory = directory.Parent;
            }

            string dataFolder;
            if (directory != null)
            {
                dataFolder = Path.Combine(directory.FullName, "Data");
            }
            else
            {
                dataFolder = Path.Combine(baseDirectory, "..", "..", "..", "Data");
                dataFolder = Path.GetFullPath(dataFolder);
            }

            // 确保文件夹存在
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            return dataFolder;
        }

        // 测试方法
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine("=== JSON转换工具 ===");
        //    Console.WriteLine("将Json.cs中的数据转换为独立的JSON文件\n");

        //    ConvertJsonCsToFiles();

        //    Console.WriteLine("\n按任意键退出...");
        //    Console.ReadKey();
        //}
    }
}
