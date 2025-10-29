using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace DataVisualizationPlatform.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // 测试账户常量
        private const string TEST_USERNAME = "admin";
        private const string TEST_PASSWORD = "123456";

        private string _username;
        private string _password;
        private bool _rememberMe;
        private bool _isLoading;
        private string _errorMessage;

        public IRelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync(), () => !IsLoading);
        }

        #region 属性

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                ErrorMessage = string.Empty; // 清除错误信息
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                ErrorMessage = string.Empty; // 清除错误信息
                OnPropertyChanged();
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotLoading));
                OnPropertyChanged(nameof(LoginButtonText));
            }
        }

        public bool IsNotLoading => !IsLoading;

        public string LoginButtonText => IsLoading ? "登录中..." : "登录";

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasError));
            }
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        #endregion

        #region 登录方法

        private async Task LoginAsync()
        {
            // 清除错误信息
            ErrorMessage = string.Empty;

            // 简单验证
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

            IsLoading = true;

            try
            {
                // 模拟网络延迟
                await Task.Delay(800);

                // 验证测试账户
                if (Username.Trim() == TEST_USERNAME && Password == TEST_PASSWORD)
                {
                    // 登录成功，跳转到主窗口
                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    // 查找并关闭Login窗口
                    var loginWindow = Application.Current.Windows
                        .OfType<Login>()
                        .FirstOrDefault();
                    loginWindow?.Close();
                }
                else
                {
                    ErrorMessage = "用户名或密码错误";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"登录失败: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}