using DataVisualizationPlatform.ViewModels;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataVisualizationPlatform.Views
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : Page
    {
        public Edit(EditViewModel viewModel)
        {
            InitializeComponent();

            // 订阅数据上下文变化事件（在设置 DataContext 之前）
            DataContextChanged += Edit_DataContextChanged;

            // 订阅部署地址控件的事件
            LongitudeTextBox.TextChanged += DeploymentAddress_Changed;
            LatitudeTextBox.TextChanged += DeploymentAddress_Changed;
            LongitudeDirectionToggle.Checked += DeploymentAddress_Changed;
            LongitudeDirectionToggle.Unchecked += DeploymentAddress_Changed;
            LatitudeDirectionToggle.Checked += DeploymentAddress_Changed;
            LatitudeDirectionToggle.Unchecked += DeploymentAddress_Changed;

            // 设置 DataContext（这会触发 DataContextChanged 事件）
            DataContext = viewModel;

            // 订阅 ViewModel 的属性变化事件
            if (viewModel != null)
            {
                viewModel.PropertyChanged += ViewModel_PropertyChanged;

                // 如果已经有正在编辑的设备，初始化部署地址
                if (viewModel.EditingEquipment != null)
                {
                    _isUpdatingAddress = true;
                    ParseDeploymentAddress(viewModel.EditingEquipment.Equ_DeploymentAddress);
                    _isUpdatingAddress = false;
                }
            }
        }

        private void Edit_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            // 取消订阅旧的 ViewModel
            if (e.OldValue is EditViewModel oldViewModel)
            {
                oldViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            // 订阅新的 ViewModel
            if (e.NewValue is EditViewModel newViewModel)
            {
                newViewModel.PropertyChanged += ViewModel_PropertyChanged;

                // 初始化部署地址
                if (newViewModel.EditingEquipment != null)
                {
                    _isUpdatingAddress = true;
                    ParseDeploymentAddress(newViewModel.EditingEquipment.Equ_DeploymentAddress);
                    _isUpdatingAddress = false;
                }
            }
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EditViewModel.EditingEquipment))
            {
                if (DataContext is EditViewModel viewModel && viewModel.EditingEquipment != null)
                {
                    _isUpdatingAddress = true;
                    ParseDeploymentAddress(viewModel.EditingEquipment.Equ_DeploymentAddress);
                    _isUpdatingAddress = false;
                }
            }
        }

        // 整数验证 - 只允许输入整数
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // 正整数验证 - 只允许输入正整数（不包括0）
        private void PositiveIntegerValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // 只允许输入数字
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // 防止粘贴非正整数内容
        private void PositiveIntegerPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                // 检查是否为纯数字且不为空
                if (!Regex.IsMatch(text, "^[0-9]+$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        // 验证正整数输入框失去焦点时的值
        private void PositiveIntegerLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string text = textBox.Text.Trim();

                // 如果为空，设置为0
                if (string.IsNullOrWhiteSpace(text))
                {
                    textBox.Text = "0";
                    return;
                }

                // 如果能转换为整数，检查是否为正整数
                if (int.TryParse(text, out int value))
                {
                    // 如果是负数或0，重置为0
                    if (value < 0)
                    {
                        textBox.Text = "0";
                    }
                }
                else
                {
                    // 无法转换为整数，重置为0
                    textBox.Text = "0";
                }

                // 验证已用固定时长不能大于固定时长
                ValidateUsedDuration();
            }
        }

        // 验证已用固定时长不能大于固定时长
        private void ValidateUsedDuration()
        {
            if (DataContext is EditViewModel viewModel && viewModel.EditingEquipment != null)
            {
                // 从字符串中提取数值
                string fixedDurationStr = viewModel.EditingEquipment.Equ_FixedDurationThisYear?.Replace("小时", "").Trim() ?? "0";
                string usedDurationStr = viewModel.EditingEquipment.Equ_UsedFixedDurationThisYear?.Replace("小时", "").Trim() ?? "0";

                if (int.TryParse(fixedDurationStr, out int fixedDuration) &&
                    int.TryParse(usedDurationStr, out int usedDuration))
                {
                    if (usedDuration > fixedDuration)
                    {
                        MessageBox.Show(
                            $"已用固定时长（{usedDuration}小时）不能大于固定时长（{fixedDuration}小时）！\n已自动调整为固定时长。",
                            "验证错误",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        // 自动调整为固定时长
                        viewModel.EditingEquipment.Equ_UsedFixedDurationThisYear = fixedDuration.ToString();
                    }
                }
            }
        }

        // 小数验证 - 允许输入小数（包括负数）
        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            // 允许数字、小数点、负号
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);

            // 防止多个小数点
            if (e.Text == "." && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }

            // 防止多个负号，负号只能在开头
            if (e.Text == "-" && (textBox.Text.Contains("-") || textBox.SelectionStart != 0))
            {
                e.Handled = true;
            }
        }

        // 解析部署地址
        private void ParseDeploymentAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                LongitudeTextBox.Text = "";
                LatitudeTextBox.Text = "";
                LongitudeDirectionToggle.IsChecked = false; // 东
                LatitudeDirectionToggle.IsChecked = false;  // 北
                return;
            }

            // 移除所有空格
            address = address.Replace(" ", "");

            // 尝试解析格式: "经度,纬度" 或 "经度, 纬度"
            var parts = address.Split(',');
            if (parts.Length == 2)
            {
                if (double.TryParse(parts[0], out double longitude))
                {
                    LongitudeTextBox.Text = System.Math.Abs(longitude).ToString();
                    LongitudeDirectionToggle.IsChecked = longitude < 0; // 西经为负
                }

                if (double.TryParse(parts[1], out double latitude))
                {
                    LatitudeTextBox.Text = System.Math.Abs(latitude).ToString();
                    LatitudeDirectionToggle.IsChecked = latitude < 0; // 南纬为负
                }
            }
        }

        // 部署地址改变时，更新到模型
        private bool _isUpdatingAddress = false;
        private void DeploymentAddress_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_isUpdatingAddress) return;

            if (DataContext is EditViewModel viewModel && viewModel.EditingEquipment != null)
            {
                // 构建新的部署地址字符串
                string newAddress = BuildDeploymentAddress();
                viewModel.EditingEquipment.Equ_DeploymentAddress = newAddress;
            }
        }

        // 构建部署地址字符串
        private string BuildDeploymentAddress()
        {
            if (string.IsNullOrWhiteSpace(LongitudeTextBox.Text) ||
                string.IsNullOrWhiteSpace(LatitudeTextBox.Text))
            {
                return "";
            }

            if (!double.TryParse(LongitudeTextBox.Text, out double longitude) ||
                !double.TryParse(LatitudeTextBox.Text, out double latitude))
            {
                return "";
            }

            // 根据方向调整符号
            if (LongitudeDirectionToggle.IsChecked == true) // 西经
            {
                longitude = -System.Math.Abs(longitude);
            }
            else // 东经
            {
                longitude = System.Math.Abs(longitude);
            }

            if (LatitudeDirectionToggle.IsChecked == true) // 南纬
            {
                latitude = -System.Math.Abs(latitude);
            }
            else // 北纬
            {
                latitude = System.Math.Abs(latitude);
            }

            return $"{longitude}, {latitude}";
        }

    }
}
