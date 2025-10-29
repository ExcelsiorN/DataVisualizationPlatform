using System;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Services.Navigation
{
    /// <summary>
    /// 导航服务接口，用于解耦ViewModel和View
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// 导航到指定页面
        /// </summary>
        /// <param name="pageKey">页面键名</param>
        /// <param name="parameter">导航参数</param>
        void NavigateTo(string pageKey, object? parameter = null);

        /// <summary>
        /// 返回上一页
        /// </summary>
        bool GoBack();

        /// <summary>
        /// 清空导航历史
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// 当前页面
        /// </summary>
        Page? CurrentPage { get; }

        /// <summary>
        /// 当前页面变化事件
        /// </summary>
        event EventHandler<Page?>? CurrentPageChanged;
    }
}
