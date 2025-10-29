using CommunityToolkit.Mvvm.ComponentModel;

namespace DataVisualizationPlatform.ViewModels
{
    /// <summary>
    /// ViewModel基类，所有ViewModel都应继承此类
    /// 使用CommunityToolkit.Mvvm的ObservableObject
    /// </summary>
    public abstract partial class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// 是否正在加载/处理
        /// 使用[ObservableProperty]自动生成IsBusy属性和OnIsBusyChanged partial方法
        /// </summary>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        /// <summary>
        /// 是否未在加载/处理
        /// </summary>
        public bool IsNotBusy => !IsBusy;

        /// <summary>
        /// 当ViewModel加载时调用
        /// </summary>
        public virtual void OnLoaded()
        {
        }

        /// <summary>
        /// 当ViewModel卸载时调用
        /// </summary>
        public virtual void OnUnloaded()
        {
        }
    }
}
