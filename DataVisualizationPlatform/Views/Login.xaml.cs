using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DataVisualizationPlatform.ViewModels;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            // 窗口加载后聚焦到用户名输入框
            Loaded += (s, e) => UsernameTextBox.Focus();
        }

        /// <summary>
        /// 处理密码框的密码变化事件
        /// 因为 PasswordBox.Password 不是依赖属性,无法直接绑定
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TestClick(object sender, RoutedEventArgs e)
        {
            // 打开主窗口
            var mainWindow = new MainWindow();
            mainWindow.Show();

            // 关闭当前窗口（假设这是 LoginWindow）
            this.Close();
        }


        /// <summary>
        /// 允许拖动窗口
        /// </summary>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}