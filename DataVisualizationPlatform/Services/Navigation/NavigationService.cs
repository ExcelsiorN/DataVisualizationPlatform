using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace DataVisualizationPlatform.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _pageTypes = new();
        private readonly Stack<Page> _navigationHistory = new();
        private Page? _currentPage;

        public Page? CurrentPage
        {
            get => _currentPage;
            private set
            {
                _currentPage = value;
                CurrentPageChanged?.Invoke(this, value);
            }
        }

        public event EventHandler<Page?>? CurrentPageChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RegisterPages();
        }

        /// <summary>
        /// 注册所有页面类型
        /// </summary>
        private void RegisterPages()
        {
            // 注册页面类型映射
            _pageTypes.Add("HomePageA", typeof(Views.HomePageA));
            _pageTypes.Add("HomePageB", typeof(Views.HomePageB));
            _pageTypes.Add("HomePageC", typeof(Views.HomePageC));
            _pageTypes.Add("MainMenu", typeof(Views.HomePageB)); // MainMenu映射到HomePageB（主菜单页）
            _pageTypes.Add("EquipmentInfo", typeof(Views.EquipmentInfo));
            _pageTypes.Add("ReservationList", typeof(Views.ReservationList));
            _pageTypes.Add("FaultReport", typeof(Views.FaultReport));
            _pageTypes.Add("Data", typeof(Views.Data));
            _pageTypes.Add("Edit", typeof(Views.Edit));
            _pageTypes.Add("FaultEdit", typeof(Views.FaultEdit));
        }

        public void NavigateTo(string pageKey, object? parameter = null)
        {
            if (!_pageTypes.TryGetValue(pageKey, out var pageType))
            {
                throw new ArgumentException($"页面 '{pageKey}' 未注册", nameof(pageKey));
            }

            // 使用DI容器创建页面实例
            var page = (Page)_serviceProvider.GetRequiredService(pageType);

            // 如果页面的DataContext实现了INavigationAware，传递参数
            if (page.DataContext is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(parameter);
            }

            // 保存当前页面到历史
            if (CurrentPage != null)
            {
                _navigationHistory.Push(CurrentPage);
            }

            CurrentPage = page;
        }

        public bool GoBack()
        {
            if (_navigationHistory.Count == 0)
                return false;

            CurrentPage = _navigationHistory.Pop();
            return true;
        }

        public void ClearHistory()
        {
            _navigationHistory.Clear();
        }
    }

    /// <summary>
    /// 导航感知接口，ViewModel可实现此接口接收导航参数
    /// </summary>
    public interface INavigationAware
    {
        void OnNavigatedTo(object? parameter);
        void OnNavigatedFrom();
    }
}
