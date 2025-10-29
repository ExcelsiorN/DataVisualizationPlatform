using System;
using System.Diagnostics;
using System.Windows;

namespace DataVisualizationPlatform.Services.ExceptionHandling
{
    /// <summary>
    /// 全局异常处理器
    /// </summary>
    public static class GlobalExceptionHandler
    {
        private static bool _isInitialized;

        /// <summary>
        /// 初始化全局异常处理
        /// </summary>
        public static void Initialize()
        {
            if (_isInitialized) return;

            // 捕获未处理的异常
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            // 捕获UI线程未处理的异常
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;

            // 捕获Task中未观察到的异常
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;

            _isInitialized = true;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                HandleException(exception, "AppDomain.UnhandledException");
            }
        }

        private static void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            HandleException(e.Exception, "Dispatcher.UnhandledException");
            e.Handled = true; // 标记为已处理，防止应用崩溃
        }

        private static void OnUnobservedTaskException(object? sender, System.Threading.Tasks.UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, "TaskScheduler.UnobservedTaskException");
            e.SetObserved(); // 标记为已观察
        }

        private static void HandleException(Exception exception, string source)
        {
            // 1. 记录到调试输出
            Debug.WriteLine($"[{source}] {exception}");

            // 2. TODO: 记录到日志文件
            // Logger.Error(exception, $"Exception from {source}");

            // 3. 显示用户友好的错误消息
            var message = GetUserFriendlyMessage(exception);
            MessageBox.Show(
                message,
                "发生错误",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private static string GetUserFriendlyMessage(Exception exception)
        {
            return exception switch
            {
                InvalidOperationException => "操作无效，请检查您的操作是否正确。",
                ArgumentException => "参数错误，请检查输入的数据。",
                System.IO.IOException => "文件操作失败，请检查文件是否存在或权限是否足够。",
                System.Net.Http.HttpRequestException => "网络请求失败，请检查网络连接。",
                UnauthorizedAccessException => "权限不足，请以管理员身份运行。",
                _ => $"发生未知错误，请联系技术支持。\n错误信息：{exception.Message}"
            };
        }
    }
}
