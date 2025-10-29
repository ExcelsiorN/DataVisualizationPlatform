namespace DataVisualizationPlatform.Messages
{
    /// <summary>
    /// 页面切换消息
    /// 用于通过WeakReferenceMessenger在不同ViewModel之间传递页面切换请求
    /// </summary>
    public class ChangePageMessage
    {
        /// <summary>
        /// 要导航到的页面键
        /// </summary>
        public string? PageKey { get; set; }

        /// <summary>
        /// 导航参数
        /// </summary>
        public object? Parameter { get; set; }
    }
}
