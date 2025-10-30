using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Views;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DataVisualizationPlatform.ViewModels
{
    /// <summary>
    /// 登录ViewModel
    /// </summary>
    public partial class LoginViewModel : ViewModelBase
    {
        // 测试账户常量
        private const string TEST_USERNAME = "admin";
        private const string TEST_PASSWORD = "123456";

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private bool _rememberMe;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        // 计算属性
        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
        public string LoginButtonText => IsBusy ? "登录中..." : "登录";

        public LoginViewModel()
        {
            // 订阅IsBusy属性变化，通知LoginButtonText更新
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IsBusy))
                {
                    OnPropertyChanged(nameof(LoginButtonText));
                }
            };
        }

        // 属性变化时清除错误信息
        partial void OnUsernameChanged(string value)
        {
            ErrorMessage = string.Empty;
        }

        partial void OnPasswordChanged(string value)
        {
            ErrorMessage = string.Empty;
        }

        partial void OnErrorMessageChanged(string value)
        {
            OnPropertyChanged(nameof(HasError));
        }

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            ErrorMessage = string.Empty;

            // 验证输入
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "请输入用户名";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "请输入密码";
                return;
            }

            // 验证账号密码
            if (Username.Trim() != TEST_USERNAME || Password != TEST_PASSWORD)
            {
                // 账号密码错误，直接显示错误，不显示遮罩
                ErrorMessage = "用户名或密码错误";
                return;
            }

            // 账号密码正确，显示遮罩并模拟加载过程
            IsBusy = true;

            try
            {
                // 模拟加载过程（例如：验证token、加载用户数据等）
                await Task.Delay(1500); // 显示1.5秒的加载动画

                // 登录成功，通过DI容器获取MainWindow
                var mainWindow = App.GetService<MainWindow>();
                mainWindow.Show();

                // 查找并关闭Login窗口
                var loginWindow = Application.Current.Windows
                    .OfType<Login>()
                    .FirstOrDefault();
                loginWindow?.Close();
            }
            catch (Exception ex)
            {
                // 改进的异常处理
                ErrorMessage = "登录失败，请稍后重试";
                System.Diagnostics.Debug.WriteLine($"登录异常: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanLogin() => !IsBusy;
    }
}
