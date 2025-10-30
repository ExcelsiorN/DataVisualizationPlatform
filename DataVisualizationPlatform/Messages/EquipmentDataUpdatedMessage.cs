namespace DataVisualizationPlatform.Messages
{
    /// <summary>
    /// 设备数据更新消息
    /// 当设备数据被保存到文件后发送此消息，通知其他页面重新加载数据
    /// </summary>
    public class EquipmentDataUpdatedMessage
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
