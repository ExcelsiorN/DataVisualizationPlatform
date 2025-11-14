# WPF MVVM 初学者教学文档
## 基于 DataVisualizationPlatform 项目的实战指南

---

## 📚 目录
- [第一章：WPF基础](#第一章wpf基础)
- [第二章：MVVM模式详解](#第二章mvvm模式详解)
- [第三章：数据绑定](#第三章数据绑定)
- [第四章：命令系统](#第四章命令系统)
- [第五章：依赖注入](#第五章依赖注入)
- [第六章：导航系统](#第六章导航系统)
- [第七章：实战案例](#第七章实战案例)
- [第八章：C#面试必备](#第八章c面试必备)
- [第九章：学习路径](#第九章学习路径)

---

## 前言

### 这份文档适合谁？
- ✅ 有C#基础，想学习WPF的开发者
- ✅ 了解WPF但不熟悉MVVM模式的开发者
- ✅ 想通过实际项目学习现代WPF开发的学习者
- ✅ 需要参考企业级WPF项目架构的开发者

### 你将学到什么？
- 🎯 WPF的核心概念和XAML语法
- 🎯 MVVM模式的完整实现
- 🎯 数据绑定和命令系统
- 🎯 依赖注入在WPF中的应用
- 🎯 现代WPF项目的最佳实践

### 学习方法建议
1. **按顺序学习** - 每章都建立在前一章的基础上
2. **实际编码** - 跟着示例代码实践
3. **查看源码** - 对照本项目的实际代码
4. **动手修改** - 尝试改动代码观察效果

---

## 第一章：WPF基础

### 1.1 什么是WPF？

**WPF (Windows Presentation Foundation)** 是微软推出的Windows桌面应用程序开发框架。

#### 核心特点
- 🎨 **基于DirectX** - 硬件加速渲染，性能优秀
- 📐 **声明式UI** - 使用XAML定义界面
- 🔗 **数据绑定** - UI自动同步数据变化
- 🎭 **样式和模板** - 强大的UI定制能力
- 📱 **分辨率独立** - 自适应不同DPI

#### WPF vs WinForms
```
WinForms:  代码创建UI，像素定位
WPF:       XAML声明UI，自适应布局
```

---

### 1.2 XAML基础

**XAML (eXtensible Application Markup Language)** 是用于定义WPF界面的标记语言。

#### 基本语法

```xaml
<!-- 1. 元素（Element） -->
<Button Content="点击我" />

<!-- 2. 属性（Property） -->
<Button Width="100" Height="30" />

<!-- 3. 嵌套（Nesting） -->
<StackPanel>
    <TextBlock Text="标题" />
    <Button Content="按钮" />
</StackPanel>

<!-- 4. 命名空间（Namespace） -->
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
</Window>

<!-- 5. x:Name - 给元素命名，可在CodeBehind中访问 -->
<Button x:Name="MyButton" Content="按钮" />

<!-- 6. 附加属性（Attached Property） -->
<Button Grid.Row="0" Grid.Column="1" />

<!-- 7. 标记扩展（Markup Extension） -->
<TextBlock Text="{Binding Title}" />
```

#### 本项目示例

**文件**: `Views/Login.xaml`
```xaml
<Page x:Class="DataVisualizationPlatform.Views.Login"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Grid>
        <!-- 用户名输入框 -->
        <TextBox Text="{Binding Username, UpdateSourceTrigger=PropertyChanged}" />

        <!-- 登录按钮 -->
        <Button Content="登录"
                Command="{Binding LoginCommand}" />
    </Grid>
</Page>
```

**解释**:
- `x:Class` - 关联的C#类
- `Text="{Binding Username}"` - 绑定到ViewModel的Username属性
- `Command="{Binding LoginCommand}"` - 绑定到ViewModel的登录命令

---

### 1.3 布局系统

WPF使用容器来管理子元素的位置和大小。

#### 常用布局容器

##### Grid - 表格布局
```xaml
<Grid>
    <!-- 定义行和列 -->
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />    <!-- 自适应高度 -->
        <RowDefinition Height="*" />       <!-- 占据剩余空间 -->
        <RowDefinition Height="100" />     <!-- 固定高度 -->
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="200" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <!-- 放置元素 -->
    <TextBlock Grid.Row="0" Grid.Column="0" Text="标题" />
    <TextBox Grid.Row="0" Grid.Column="1" />
    <Button Grid.Row="1" Grid.ColumnSpan="2" Content="提交" />
</Grid>
```

##### StackPanel - 堆叠布局
```xaml
<!-- 垂直堆叠 -->
<StackPanel Orientation="Vertical">
    <TextBlock Text="第一行" />
    <TextBlock Text="第二行" />
    <TextBlock Text="第三行" />
</StackPanel>

<!-- 水平堆叠 -->
<StackPanel Orientation="Horizontal">
    <Button Content="按钮1" />
    <Button Content="按钮2" />
</StackPanel>
```

##### WrapPanel - 自动换行
```xaml
<WrapPanel>
    <Button Content="按钮1" Width="100" />
    <Button Content="按钮2" Width="100" />
    <Button Content="按钮3" Width="100" />
    <!-- 宽度不够时自动换行 -->
</WrapPanel>
```

##### DockPanel - 停靠布局
```xaml
<DockPanel LastChildFill="True">
    <Menu DockPanel.Dock="Top" />           <!-- 顶部 -->
    <StatusBar DockPanel.Dock="Bottom" />   <!-- 底部 -->
    <TreeView DockPanel.Dock="Left" Width="200" /> <!-- 左侧 -->
    <ContentControl />                      <!-- 填充剩余空间 -->
</DockPanel>
```

#### 本项目示例

**文件**: `MainWindow.xaml`
```xaml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="200"/>      <!-- 导航栏 -->
        <ColumnDefinition Width="*"/>        <!-- 内容区域 -->
    </Grid.ColumnDefinitions>

    <!-- 左侧导航 -->
    <StackPanel Grid.Column="0" Background="#2C3E50">
        <RadioButton Content="主页A" Command="{Binding NavigateCommand}" />
        <RadioButton Content="主页B" Command="{Binding NavigateCommand}" />
        <RadioButton Content="数据" Command="{Binding NavigateCommand}" />
    </StackPanel>

    <!-- 右侧内容 -->
    <Frame Grid.Column="1"
           x:Name="MainContentFrame"
           NavigationUIVisibility="Hidden" />
</Grid>
```

---

## 第二章：MVVM模式详解

### 2.1 什么是MVVM？

**MVVM (Model-View-ViewModel)** 是一种软件架构模式，专为WPF等支持数据绑定的框架设计。

#### 三层结构

```
┌──────────┐
│   View   │ ← XAML界面，纯展示
└─────┬────┘
      │ 数据绑定
      ↓
┌──────────┐
│ViewModel │ ← 业务逻辑，命令处理
└─────┬────┘
      │ 调用
      ↓
┌──────────┐
│  Model   │ ← 数据模型，数据访问
└──────────┘
```

#### 职责划分

| 层级 | 职责 | 包含内容 | 不应包含 |
|------|------|----------|----------|
| **View** | UI展示 | XAML, 样式, 动画 | 业务逻辑 |
| **ViewModel** | 业务逻辑 | 属性, 命令, 数据处理 | UI元素引用 |
| **Model** | 数据定义 | 数据结构, 验证规则 | UI逻辑 |

---

### 2.2 为什么使用MVVM？

#### 传统方式的问题
```csharp
// ❌ 传统CodeBehind方式
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // UI逻辑和业务逻辑混在一起
        var username = UsernameTextBox.Text;
        if (string.IsNullOrEmpty(username))
        {
            MessageBox.Show("请输入用户名");
            return;
        }

        // 难以测试
        // 难以重用
        // 紧耦合
    }
}
```

#### MVVM的优势
```csharp
// ✅ MVVM方式
public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username;

    [RelayCommand]
    private void Login()
    {
        // 纯逻辑，不涉及UI
        if (string.IsNullOrEmpty(Username))
        {
            ErrorMessage = "请输入用户名";
            return;
        }

        // 易于测试
        // 可重用
        // 松耦合
    }
}
```

**好处**:
1. ✅ **可测试性** - ViewModel可以独立测试
2. ✅ **可维护性** - 职责清晰，代码组织良好
3. ✅ **可重用性** - ViewModel可以被多个View使用
4. ✅ **团队协作** - 设计师改XAML，开发者改ViewModel

---

### 2.3 MVVM实现要点

#### 要点1：INotifyPropertyChanged

**作用**: 当属性值改变时，自动通知UI更新。

**传统实现**:
```csharp
public class PersonViewModel : INotifyPropertyChanged
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));  // 通知UI
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

**现代实现（使用CommunityToolkit.Mvvm）**:
```csharp
public partial class PersonViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    // 源生成器自动生成:
    // - public string Name { get; set; }
    // - PropertyChanged通知
    // - partial void OnNameChanging(string value)
    // - partial void OnNameChanged(string value)
}
```

**本项目示例**:
```csharp
// ViewModels/LoginViewModel.cs
public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // 源生成器自动生成Username和ErrorMessage属性
}
```

---

#### 要点2：命令（Command）

**作用**: 将UI操作绑定到ViewModel的方法。

**传统实现**:
```csharp
public class MyCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool> _canExecute;

    public MyCommand(Action execute, Func<bool> canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute();
    }

    public void Execute(object parameter)
    {
        _execute();
    }

    public event EventHandler CanExecuteChanged;
}

// 使用
public ICommand LoginCommand { get; }

public LoginViewModel()
{
    LoginCommand = new MyCommand(Login, CanLogin);
}

private void Login() { /* ... */ }
private bool CanLogin() => !string.IsNullOrEmpty(Username);
```

**现代实现（使用CommunityToolkit.Mvvm）**:
```csharp
public partial class LoginViewModel : ViewModelBase
{
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private void Login()
    {
        // 登录逻辑
    }

    private bool CanLogin() => !string.IsNullOrEmpty(Username);

    // 源生成器自动生成:
    // - public IRelayCommand LoginCommand { get; }
    // - 自动调用CanLogin检查
}
```

**本项目示例**:
```csharp
// ViewModels/MainWindowViewModel.cs
public partial class MainWindowViewModel : ViewModelBase
{
    [RelayCommand]
    private void Navigate(string? target)
    {
        if (string.IsNullOrEmpty(target)) return;
        _navigationService.NavigateTo(target);
    }

    // XAML中使用:
    // <Button Command="{Binding NavigateCommand}"
    //         CommandParameter="HomePageA" />
}
```

---

#### 要点3：ViewModelBase基类

**作用**: 提供通用功能，减少重复代码。

**本项目实现**:
```csharp
// ViewModels/ViewModelBase.cs
public abstract partial class ViewModelBase : ObservableObject
{
    /// <summary>
    /// 是否正在加载
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    /// <summary>
    /// 是否未在加载
    /// </summary>
    public bool IsNotBusy => !IsBusy;

    /// <summary>
    /// 当ViewModel加载时调用
    /// </summary>
    public virtual void OnLoaded() { }

    /// <summary>
    /// 当ViewModel卸载时调用
    /// </summary>
    public virtual void OnUnloaded() { }
}
```

**使用**:
```csharp
public partial class HomePageAViewModel : ViewModelBase
{
    public override void OnLoaded()
    {
        base.OnLoaded();
        LoadData();        // 页面加载时获取数据
        StartAnimation();  // 启动动画
    }

    public override void OnUnloaded()
    {
        base.OnUnloaded();
        StopAnimation();   // 页面卸载时停止动画
    }
}
```

---

## 第三章：数据绑定

### 3.1 什么是数据绑定？

**数据绑定** 是WPF的核心功能，它建立了UI和数据之间的连接，数据变化时UI自动更新。

#### 绑定流程
```
ViewModel属性改变
   ↓
触发PropertyChanged事件
   ↓
WPF绑定系统监听到事件
   ↓
自动更新UI元素
```

---

### 3.2 绑定模式

#### OneWay - 单向绑定
数据源 → UI

```xaml
<!-- ViewModel的Title改变时，TextBlock自动更新 -->
<TextBlock Text="{Binding Title, Mode=OneWay}" />
```

#### TwoWay - 双向绑定
数据源 ↔ UI

```xaml
<!-- 用户输入时，Username属性自动更新 -->
<!-- Username改变时，TextBox也更新 -->
<TextBox Text="{Binding Username, Mode=TwoWay}" />
```

#### OneTime - 一次性绑定
数据源 → UI (仅初始化时)

```xaml
<!-- 只在初始化时绑定一次 -->
<TextBlock Text="{Binding AppVersion, Mode=OneTime}" />
```

#### OneWayToSource - 反向单向
UI → 数据源

```xaml
<!-- UI改变时更新数据源，但数据源改变不影响UI -->
<Slider Value="{Binding Volume, Mode=OneWayToSource}" />
```

---

### 3.3 绑定路径

#### 简单属性绑定
```xaml
<TextBlock Text="{Binding UserName}" />
```

#### 嵌套属性绑定
```xaml
<!-- 绑定到User.Address.City -->
<TextBlock Text="{Binding User.Address.City}" />
```

#### 集合绑定
```xaml
<ListBox ItemsSource="{Binding Users}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Name}" />
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

#### 索引绑定
```xaml
<TextBlock Text="{Binding Users[0].Name}" />
```

---

### 3.4 本项目绑定示例

#### 示例1：登录表单

**ViewModel**:
```csharp
// ViewModels/LoginViewModel.cs
public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
}
```

**XAML**:
```xaml
<!-- Views/Login.xaml -->
<StackPanel>
    <!-- 用户名输入，双向绑定 -->
    <TextBox Text="{Binding Username, Mode=TwoWay,
                    UpdateSourceTrigger=PropertyChanged}" />

    <!-- 密码输入 -->
    <PasswordBox />

    <!-- 错误消息，单向绑定 -->
    <TextBlock Text="{Binding ErrorMessage}"
               Foreground="Red"
               Visibility="{Binding HasError,
                           Converter={StaticResource BoolToVisibilityConverter}}" />

    <!-- 登录按钮 -->
    <Button Content="登录" Command="{Binding LoginCommand}" />
</StackPanel>
```

**关键点**:
- `UpdateSourceTrigger=PropertyChanged` - 每次输入都更新源
- `HasError` - 计算属性，无需通知
- 使用转换器将bool转为Visibility

---

#### 示例2：列表绑定

**ViewModel**:
```csharp
// ViewModels/ReservationListViewModel.cs
public class ReservationListViewModel : INotifyPropertyChanged
{
    public ObservableCollection<ReservationListModel> ReservationList { get; } = new();

    public void OnNavigatedTo(object? parameter)
    {
        ReservationList.Clear();

        // 加载数据
        var data = LoadData();
        foreach (var item in data)
        {
            ReservationList.Add(item);
        }
    }
}
```

**XAML**:
```xaml
<!-- Views/ReservationList.xaml -->
<ItemsControl ItemsSource="{Binding ReservationList}">
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <Border Background="White" Margin="10">
                <StackPanel>
                    <TextBlock Text="{Binding Res_Equipment}"
                               FontWeight="Bold" />
                    <TextBlock Text="{Binding Res_Date}" />
                    <TextBlock Text="{Binding Res_Status}" />
                </StackPanel>
            </Border>
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```

**关键点**:
- 使用`ObservableCollection` - 集合变化时自动通知UI
- `ItemTemplate` - 定义每项的显示模板
- `DataContext`自动设置为集合中的每一项

---

### 3.5 值转换器（Converter）

**作用**: 在绑定时转换数据类型。

#### 实现转换器
```csharp
// Converters/BooleanToVisibilityConverter.cs
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType,
                         object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType,
                             object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }
        return false;
    }
}
```

#### 注册转换器
```xaml
<Window.Resources>
    <local:BooleanToVisibilityConverter x:Key="BoolToVisConverter" />
</Window.Resources>
```

#### 使用转换器
```xaml
<TextBlock Text="错误消息"
           Visibility="{Binding HasError,
                       Converter={StaticResource BoolToVisConverter}}" />
```

#### 本项目的其他转换器

**StatusToBrushConverter** - 根据状态返回颜色
```csharp
public object Convert(object value, ...)
{
    return value?.ToString() switch
    {
        "已完成" => new SolidColorBrush(Colors.Green),
        "进行中" => new SolidColorBrush(Colors.Blue),
        "已取消" => new SolidColorBrush(Colors.Red),
        _ => new SolidColorBrush(Colors.Gray)
    };
}
```

---

## 第四章：命令系统

### 4.1 ICommand接口

WPF的命令系统基于`ICommand`接口：

```csharp
public interface ICommand
{
    // 判断命令是否可以执行
    bool CanExecute(object? parameter);

    // 执行命令
    void Execute(object? parameter);

    // 命令可执行状态改变时触发
    event EventHandler? CanExecuteChanged;
}
```

---

### 4.2 RelayCommand实现

#### 基础实现
```csharp
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute();
    }

    public void Execute(object? parameter)
    {
        _execute();
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
}
```

#### 带参数的RelayCommand
```csharp
public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Func<T?, bool>? _canExecute;

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute((T?)parameter);
    }

    public void Execute(object? parameter)
    {
        _execute((T?)parameter);
    }
}
```

---

### 4.3 使用CommunityToolkit.Mvvm

#### 无参数命令
```csharp
public partial class ExampleViewModel : ViewModelBase
{
    [RelayCommand]
    private void Save()
    {
        // 保存逻辑
    }

    // 自动生成: public IRelayCommand SaveCommand { get; }
}
```

**XAML使用**:
```xaml
<Button Content="保存" Command="{Binding SaveCommand}" />
```

---

#### 带参数命令
```csharp
public partial class ExampleViewModel : ViewModelBase
{
    [RelayCommand]
    private void Delete(int id)
    {
        // 删除ID为id的项
    }
}
```

**XAML使用**:
```xaml
<Button Content="删除"
        Command="{Binding DeleteCommand}"
        CommandParameter="{Binding ItemId}" />
```

---

#### 带CanExecute的命令
```csharp
public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        IsBusy = true;
        await Task.Delay(1000);  // 模拟登录
        IsBusy = false;
    }

    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(Username) && !IsBusy;
    }
}
```

**自动通知CanExecute变化**:
```csharp
// 当Username改变时，自动刷新LoginCommand的可执行状态
partial void OnUsernameChanged(string value)
{
    LoginCommand.NotifyCanExecuteChanged();
}
```

---

### 4.4 本项目命令示例

#### 示例1：导航命令

**ViewModel**:
```csharp
// ViewModels/MainWindowViewModel.cs
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    [RelayCommand]
    private void Navigate(string? target)
    {
        if (string.IsNullOrEmpty(target)) return;
        _navigationService.NavigateTo(target);
    }
}
```

**XAML**:
```xaml
<!-- MainWindow.xaml -->
<RadioButton Content="主页A"
             Command="{Binding NavigateCommand}"
             CommandParameter="HomePageA" />

<RadioButton Content="主页B"
             Command="{Binding NavigateCommand}"
             CommandParameter="HomePageB" />
```

---

#### 示例2：翻页命令

**ViewModel**:
```csharp
// ViewModels/MainWindowViewModel.cs
[RelayCommand]
private void PreviousPage()
{
    var currentPageType = _navigationService.CurrentPage?.GetType().Name;
    var previousPage = currentPageType switch
    {
        "HomePageA" => "HomePageC",
        "HomePageB" => "HomePageA",
        "HomePageC" => "HomePageB",
        _ => "HomePageB"
    };
    _navigationService.NavigateTo(previousPage);
}

[RelayCommand]
private void NextPage()
{
    var currentPageType = _navigationService.CurrentPage?.GetType().Name;
    var nextPage = currentPageType switch
    {
        "HomePageA" => "HomePageB",
        "HomePageB" => "HomePageC",
        "HomePageC" => "HomePageA",
        _ => "HomePageB"
    };
    _navigationService.NavigateTo(nextPage);
}
```

**XAML**:
```xaml
<Button Command="{Binding PreviousPageCommand}" Content="◀" />
<Button Command="{Binding NextPageCommand}" Content="▶" />
```

---

#### 示例3：异步命令

**ViewModel**:
```csharp
// ViewModels/LoginViewModel.cs
[RelayCommand(CanExecute = nameof(CanLogin))]
private async Task LoginAsync()
{
    ErrorMessage = string.Empty;

    // 验证
    if (Username.Trim() != TEST_USERNAME || Password != TEST_PASSWORD)
    {
        ErrorMessage = "用户名或密码错误";
        return;
    }

    // 显示加载状态
    IsBusy = true;

    try
    {
        // 模拟异步操作
        await Task.Delay(1500);

        // 登录成功
        var mainWindow = App.GetService<MainWindow>();
        mainWindow.Show();
    }
    finally
    {
        IsBusy = false;
    }
}

private bool CanLogin() => !IsBusy;
```

**XAML**:
```xaml
<Button Content="{Binding LoginButtonText}"
        Command="{Binding LoginCommand}"
        IsEnabled="{Binding IsNotBusy}" />
```

---

## 第五章：依赖注入

### 5.1 什么是依赖注入？

**依赖注入 (Dependency Injection, DI)** 是一种设计模式，用于实现控制反转(IoC)。

#### 传统方式的问题
```csharp
// ❌ 硬编码依赖，紧耦合
public class MainWindowViewModel
{
    private readonly NavigationService _navigationService;

    public MainWindowViewModel()
    {
        // 直接创建依赖对象
        _navigationService = new NavigationService();
        // 问题：
        // 1. 难以测试（无法Mock）
        // 2. 难以替换实现
        // 3. 违反单一职责原则
    }
}
```

#### DI方式
```csharp
// ✅ 通过构造函数注入，松耦合
public class MainWindowViewModel
{
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService)
    {
        // 依赖由外部注入
        _navigationService = navigationService;
        // 好处：
        // 1. 易于测试（可以注入Mock）
        // 2. 易于替换实现
        // 3. 符合依赖倒置原则
    }
}
```

---

### 5.2 服务生命周期

#### Singleton - 单例
整个应用程序只有一个实例。

**适用场景**:
- 导航服务
- 配置服务
- 日志服务

```csharp
services.AddSingleton<INavigationService, NavigationService>();
```

#### Transient - 瞬态
每次请求都创建新实例。

**适用场景**:
- ViewModel
- View
- 一次性服务

```csharp
services.AddTransient<MainWindowViewModel>();
services.AddTransient<MainWindow>();
```

#### Scoped - 作用域
在同一作用域内是同一实例（WPF中较少使用）。

---

### 5.3 本项目DI配置

#### App.xaml.cs配置
```csharp
public partial class App : Application
{
    private IServiceProvider? _serviceProvider;

    public App()
    {
        // 1. 初始化全局异常处理
        GlobalExceptionHandler.Initialize();

        // 2. 创建服务集合
        var services = new ServiceCollection();

        // 3. 配置服务
        ConfigureServices(services);

        // 4. 构建服务提供者
        _serviceProvider = services.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // 注册导航服务（单例）
        services.AddSingleton<INavigationService, NavigationService>();

        // 注册ViewModels（瞬态）
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<HomePageAViewModel>();
        services.AddTransient<HomePageBViewModel>();
        services.AddTransient<HomePageCViewModel>();
        services.AddTransient<DataViewModel>();
        services.AddTransient<EquipmentInfoViewModel>();
        services.AddTransient<FaultReportViewModel>();
        services.AddTransient<ReservationListViewModel>();

        // 注册Views（瞬态）
        services.AddTransient<MainWindow>();
        services.AddTransient<Login>();
        services.AddTransient<HomePageA>();
        services.AddTransient<HomePageB>();
        services.AddTransient<HomePageC>();
        services.AddTransient<Data>();
        services.AddTransient<EquipmentInfo>();
        services.AddTransient<FaultReport>();
        services.AddTransient<ReservationList>();
        services.AddTransient<Edit>();
    }

    // 启动应用
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 获取登录窗口
        var loginWindow = _serviceProvider!.GetRequiredService<Login>();
        loginWindow.Show();
    }

    // 静态方法，供其他地方获取服务
    public static T GetService<T>() where T : notnull
    {
        return ((App)Current)._serviceProvider!.GetRequiredService<T>();
    }
}
```

---

### 5.4 使用DI

#### 在ViewModel中注入服务
```csharp
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    // 通过构造函数注入
    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void Navigate(string? target)
    {
        // 使用注入的服务
        _navigationService.NavigateTo(target);
    }
}
```

#### 在View中注入ViewModel
```csharp
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel,
                      INavigationService navigationService)
    {
        InitializeComponent();

        // 设置DataContext
        DataContext = viewModel;

        // 也可以注入其他服务
        _navigationService = navigationService;
    }
}
```

#### 手动获取服务
```csharp
// 在任何地方获取服务
var mainWindow = App.GetService<MainWindow>();
mainWindow.Show();
```

---

## 第六章：导航系统

### 6.1 导航服务设计

#### 接口定义
```csharp
// Services/Navigation/INavigationService.cs
public interface INavigationService
{
    /// <summary>
    /// 导航到指定页面
    /// </summary>
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
```

---

#### 服务实现
```csharp
// Services/Navigation/NavigationService.cs
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

    private void RegisterPages()
    {
        // 注册页面映射
        _pageTypes.Add("HomePageA", typeof(Views.HomePageA));
        _pageTypes.Add("HomePageB", typeof(Views.HomePageB));
        _pageTypes.Add("HomePageC", typeof(Views.HomePageC));
        // ...
    }

    public void NavigateTo(string pageKey, object? parameter = null)
    {
        if (!_pageTypes.TryGetValue(pageKey, out var pageType))
        {
            throw new ArgumentException($"页面 '{pageKey}' 未注册");
        }

        // 使用DI容器创建页面
        var page = (Page)_serviceProvider.GetRequiredService(pageType);

        // 如果DataContext实现了INavigationAware，传递参数
        if (page.DataContext is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(parameter);
        }

        // 保存历史
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
```

---

### 6.2 INavigationAware接口

**作用**: ViewModel实现此接口可以接收导航参数。

```csharp
public interface INavigationAware
{
    void OnNavigatedTo(object? parameter);
    void OnNavigatedFrom();
}
```

#### 实现示例
```csharp
public class DataViewModel : INotifyPropertyChanged, INavigationAware
{
    public void OnNavigatedTo(object? parameter)
    {
        // 接收参数并加载数据
        if (parameter != null)
        {
            string targetYear = parameter.ToString();
            LoadData(targetYear);
        }
    }

    public void OnNavigatedFrom()
    {
        // 页面离开时的清理
        StopAnimations();
    }
}
```

---

### 6.3 完整导航流程

```
1. 用户点击按钮
   ↓
2. 触发NavigateCommand
   ↓
3. 调用NavigationService.NavigateTo("PageKey", parameter)
   ↓
4. 查找页面类型
   ↓
5. DI容器创建Page和ViewModel
   ↓
6. 设置DataContext
   ↓
7. 检测是否实现INavigationAware
   ↓
8. 调用OnNavigatedTo(parameter)
   ↓
9. 触发CurrentPageChanged事件
   ↓
10. MainWindow更新Frame内容
   ↓
11. 页面显示
```

---

### 6.4 使用示例

#### 简单导航
```csharp
// ViewModel
[RelayCommand]
private void NavigateToHome()
{
    _navigationService.NavigateTo("HomePageB");
}

// XAML
<Button Content="主页" Command="{Binding NavigateToHomeCommand}" />
```

#### 带参数导航
```csharp
// ViewModel
[RelayCommand]
private void OpenDataPage(string year)
{
    _navigationService.NavigateTo("Data", year);
}

// XAML
<Button Content="2024年数据"
        Command="{Binding OpenDataPageCommand}"
        CommandParameter="2024" />
```

#### 接收参数
```csharp
// DataViewModel.cs
public void OnNavigatedTo(object? parameter)
{
    if (parameter is string year)
    {
        LoadDataByYear(year);
    }
    else if (parameter is ValueTuple<int, int> tuple)
    {
        var (month, targetYear) = tuple;
        LoadDataByMonth(month, targetYear);
    }
}
```

---

## 第七章：实战案例

### 7.1 案例1：创建一个简单的待办事项页面

#### 第1步：创建Model
```csharp
// Models/TodoItem.cs
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
```

#### 第2步：创建ViewModel
```csharp
// ViewModels/TodoViewModel.cs
public partial class TodoViewModel : ViewModelBase
{
    // 待办事项列表
    public ObservableCollection<TodoItem> TodoItems { get; } = new();

    // 新待办事项标题
    [ObservableProperty]
    private string _newTodoTitle = string.Empty;

    // 添加待办事项
    [RelayCommand(CanExecute = nameof(CanAddTodo))]
    private void AddTodo()
    {
        var newItem = new TodoItem
        {
            Id = TodoItems.Count + 1,
            Title = NewTodoTitle,
            IsCompleted = false
        };

        TodoItems.Add(newItem);
        NewTodoTitle = string.Empty;  // 清空输入框
    }

    private bool CanAddTodo()
    {
        return !string.IsNullOrWhiteSpace(NewTodoTitle);
    }

    // 删除待办事项
    [RelayCommand]
    private void DeleteTodo(TodoItem item)
    {
        TodoItems.Remove(item);
    }

    // 切换完成状态
    [RelayCommand]
    private void ToggleComplete(TodoItem item)
    {
        item.IsCompleted = !item.IsCompleted;
    }

    // 属性变化时刷新命令状态
    partial void OnNewTodoTitleChanged(string value)
    {
        AddTodoCommand.NotifyCanExecuteChanged();
    }
}
```

#### 第3步：创建View
```xaml
<!-- Views/TodoPage.xaml -->
<Page x:Class="YourApp.Views.TodoPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>

        <!-- 输入区域 -->
        <StackPanel Grid.Row="0" Orientation="Horizontal" Margin="0,0,0,20">
            <TextBox Text="{Binding NewTodoTitle, UpdateSourceTrigger=PropertyChanged}"
                     Width="300"
                     Margin="0,0,10,0" />
            <Button Content="添加"
                    Command="{Binding AddTodoCommand}"
                    Width="80" />
        </StackPanel>

        <!-- 列表区域 -->
        <ListBox Grid.Row="1"
                 ItemsSource="{Binding TodoItems}">
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <Grid Margin="5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="Auto" />
                            <ColumnDefinition Width="*" />
                            <ColumnDefinition Width="Auto" />
                        </Grid.ColumnDefinitions>

                        <!-- 完成状态复选框 -->
                        <CheckBox Grid.Column="0"
                                  IsChecked="{Binding IsCompleted}"
                                  Command="{Binding DataContext.ToggleCompleteCommand,
                                           RelativeSource={RelativeSource AncestorType=ListBox}}"
                                  CommandParameter="{Binding}"
                                  Margin="0,0,10,0" />

                        <!-- 标题 -->
                        <TextBlock Grid.Column="1"
                                   Text="{Binding Title}"
                                   VerticalAlignment="Center">
                            <TextBlock.Style>
                                <Style TargetType="TextBlock">
                                    <Style.Triggers>
                                        <DataTrigger Binding="{Binding IsCompleted}" Value="True">
                                            <Setter Property="TextDecorations" Value="Strikethrough" />
                                            <Setter Property="Foreground" Value="Gray" />
                                        </DataTrigger>
                                    </Style.Triggers>
                                </Style>
                            </TextBlock.Style>
                        </TextBlock>

                        <!-- 删除按钮 -->
                        <Button Grid.Column="2"
                                Content="删除"
                                Command="{Binding DataContext.DeleteTodoCommand,
                                         RelativeSource={RelativeSource AncestorType=ListBox}}"
                                CommandParameter="{Binding}" />
                    </Grid>
                </DataTemplate>
            </ListBox.ItemTemplate>
        </ListBox>
    </Grid>
</Page>
```

#### 第4步：注册到DI容器
```csharp
// App.xaml.cs
services.AddTransient<TodoViewModel>();
services.AddTransient<TodoPage>();
```

#### 第5步：注册到导航
```csharp
// NavigationService.cs
_pageTypes.Add("Todo", typeof(Views.TodoPage));
```

#### 使用
```csharp
_navigationService.NavigateTo("Todo");
```

---

### 7.2 案例2：实现数据筛选功能

#### 场景
在FaultReport页面中，根据状态、设备、年份和月份筛选故障报告。

#### ViewModel实现
```csharp
public class FaultReportViewModel : INotifyPropertyChanged
{
    // 原始数据
    private List<FaultReportModel> _allReports = new();

    // 显示的数据
    public ObservableCollection<FaultReportModel> DisplayedReports { get; } = new();

    // 筛选条件
    [ObservableProperty]
    private string _selectedStatus = "全部";

    [ObservableProperty]
    private string _selectedDevice = "全部";

    [ObservableProperty]
    private string _selectedYear = DateTime.Now.Year.ToString();

    // 属性变化时重新筛选
    partial void OnSelectedStatusChanged(string value) => ApplyFilters();
    partial void OnSelectedDeviceChanged(string value) => ApplyFilters();
    partial void OnSelectedYearChanged(string value) => ApplyFilters();

    // 应用筛选
    private void ApplyFilters()
    {
        DisplayedReports.Clear();

        var filtered = _allReports.AsEnumerable();

        // 按状态筛选
        if (SelectedStatus != "全部")
        {
            filtered = filtered.Where(r => r.Status == SelectedStatus);
        }

        // 按设备筛选
        if (SelectedDevice != "全部")
        {
            filtered = filtered.Where(r => r.Equipment == SelectedDevice);
        }

        // 按年份筛选
        if (int.TryParse(SelectedYear, out int year))
        {
            filtered = filtered.Where(r =>
                DateTime.TryParse(r.Date, out var date) &&
                date.Year == year);
        }

        // 添加到显示列表
        foreach (var item in filtered)
        {
            DisplayedReports.Add(item);
        }
    }

    // 重置筛选
    [RelayCommand]
    private void ResetFilters()
    {
        SelectedStatus = "全部";
        SelectedDevice = "全部";
        SelectedYear = DateTime.Now.Year.ToString();
    }
}
```

#### XAML实现
```xaml
<StackPanel Orientation="Horizontal" Margin="0,0,0,20">
    <!-- 状态筛选 -->
    <ComboBox SelectedItem="{Binding SelectedStatus}"
              Width="100" Margin="0,0,10,0">
        <ComboBoxItem>全部</ComboBoxItem>
        <ComboBoxItem>待处理</ComboBoxItem>
        <ComboBoxItem>处理中</ComboBoxItem>
        <ComboBoxItem>已完成</ComboBoxItem>
    </ComboBox>

    <!-- 设备筛选 -->
    <ComboBox ItemsSource="{Binding Devices}"
              SelectedItem="{Binding SelectedDevice}"
              Width="150" Margin="0,0,10,0" />

    <!-- 重置按钮 -->
    <Button Content="重置"
            Command="{Binding ResetFiltersCommand}" />
</StackPanel>

<!-- 结果列表 -->
<ItemsControl ItemsSource="{Binding DisplayedReports}">
    <!-- ItemTemplate... -->
</ItemsControl>
```

---

### 7.3 案例3：数据编辑系统与跨页面数据同步

#### 场景说明
在DataVisualizationPlatform项目中，我们需要实现一个设备信息编辑页面，支持增删改查操作，并且数据修改后能够实时同步到其他页面，无需重启应用。

这个案例展示了：
- ✅ 完整的CRUD操作实现
- ✅ 文件I/O操作（读写源代码文件）
- ✅ 跨页面消息通信
- ✅ 单例模式的数据服务
- ✅ 动态数据加载

---

#### 第1步：创建消息类

**作用**：使用消息传递实现跨页面通信。

```csharp
// Messages/EquipmentDataUpdatedMessage.cs
namespace DataVisualizationPlatform.Messages
{
    /// <summary>
    /// 设备数据更新消息
    /// 当设备数据被修改后，发送此消息通知其他页面刷新
    /// </summary>
    public class EquipmentDataUpdatedMessage
    {
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
```

**关键点**：
- 消息类可以携带数据，也可以只作为通知
- 使用`WeakReferenceMessenger`避免内存泄漏

---

#### 第2步：创建数据服务

**作用**：提供动态数据加载功能，从源代码文件读取最新数据。

```csharp
// Services/JsonDataService.cs
public class JsonDataService
{
    private static JsonDataService? _instance;
    private static readonly object _lock = new object();

    // 单例模式实现
    public static JsonDataService Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new JsonDataService();
                    }
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 从Json.cs文件中动态读取设备信息
    /// </summary>
    public string GetEquipmentInfoJson()
    {
        try
        {
            string jsonFilePath = FindJsonFilePath();
            if (string.IsNullOrEmpty(jsonFilePath))
            {
                return new Json()._EquipmentInfo;
            }

            // 读取文件内容
            string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

            // 使用正则表达式提取_EquipmentInfo字段内容
            var match = Regex.Match(fileContent,
                @"public readonly string _EquipmentInfo = @""([\s\S]*?)"";",
                RegexOptions.Multiline);

            if (match.Success && match.Groups.Count > 1)
            {
                string content = match.Groups[1].Value;
                // 反转义双引号
                content = content.Replace("\"\"", "\"");
                // 移除多余的缩进
                content = Regex.Replace(content, @"^        ", "", RegexOptions.Multiline);
                return content;
            }

            return new Json()._EquipmentInfo;
        }
        catch
        {
            return new Json()._EquipmentInfo;
        }
    }

    private string FindJsonFilePath()
    {
        string currentDir = AppDomain.CurrentDomain.BaseDirectory;

        // 向上查找项目根目录
        DirectoryInfo? directory = new DirectoryInfo(currentDir);
        while (directory != null && directory.Name != "DataVisualizationPlatform")
        {
            directory = directory.Parent;
        }

        if (directory != null)
        {
            string jsonPath = Path.Combine(directory.FullName, "Services", "Json.cs");
            if (File.Exists(jsonPath))
                return jsonPath;
        }

        return string.Empty;
    }
}
```

**关键点**：
- **单例模式**：使用双重检查锁定确保线程安全
- **动态读取**：每次调用都从文件读取最新数据
- **正则表达式**：精确提取源代码中的JSON字符串
- **异常处理**：读取失败时返回默认数据

---

#### 第3步：实现EditViewModel

**完整的CRUD功能实现**：

```csharp
// ViewModels/EditViewModel.cs
public class EditViewModel : INotifyPropertyChanged
{
    private EquipmentInfoModel? _selectedEquipment;
    private string _searchText = string.Empty;

    public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();
    public ObservableCollection<EquipmentInfoModel> FilteredEquipmentList { get; } = new();

    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand SearchCommand { get; }

    public EditViewModel()
    {
        LoadEquipmentData();

        AddCommand = new RelayCommand<object>(AddEquipment);
        DeleteCommand = new RelayCommand<object>(DeleteEquipment, CanDeleteEquipment);
        SaveCommand = new RelayCommand<object>(SaveEquipmentData);
        SearchCommand = new RelayCommand<object>(SearchEquipment);
    }

    // 选中的设备
    public EquipmentInfoModel? SelectedEquipment
    {
        get => _selectedEquipment;
        set
        {
            if (_selectedEquipment != value)
            {
                _selectedEquipment = value;
                OnPropertyChanged();
            }
        }
    }

    // 搜索文本
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                SearchEquipment(null);  // 自动搜索
            }
        }
    }

    // 加载设备数据
    private void LoadEquipmentData()
    {
        try
        {
            // 使用JsonDataService获取最新数据
            var equipmentJson = JsonDataService.Instance.GetEquipmentInfoJson();
            var equipmentData = JsonConvert.DeserializeObject<ObservableCollection<EquipmentInfoModel>>(equipmentJson);

            EquipmentList.Clear();
            FilteredEquipmentList.Clear();

            if (equipmentData != null)
            {
                foreach (var item in equipmentData)
                {
                    EquipmentList.Add(item);
                    FilteredEquipmentList.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载设备数据失败: {ex.Message}", "错误",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // 添加设备
    private void AddEquipment(object? parameter)
    {
        var newEquipment = new EquipmentInfoModel
        {
            Equ_Id = $"fntp-{EquipmentList.Count}",
            Equ_Name = "新设备",
            Equ_OnlineStatus = "离线",
            Equ_AvailableBookingPeriod = "预约时段配置1",
            Equ_TotalOperationTime = "0年0月0天",
            Equ_FixedDurationThisYear = "0.0小时",
            Equ_UsedFixedDurationThisYear = "0.0小时",
            Equ_UsageRateThisYear = "0.0%",
            Equ_DeploymentAddress = "0.0, 0.0"
        };

        EquipmentList.Add(newEquipment);
        FilteredEquipmentList.Add(newEquipment);
        SelectedEquipment = newEquipment;

        // 自动保存
        SaveEquipmentData(null);
    }

    // 删除设备
    private void DeleteEquipment(object? parameter)
    {
        if (SelectedEquipment == null) return;

        var result = MessageBox.Show(
            $"确定要删除设备 '{SelectedEquipment.Equ_Name}' ({SelectedEquipment.Equ_Id}) 吗？",
            "确认删除",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            EquipmentList.Remove(SelectedEquipment);
            FilteredEquipmentList.Remove(SelectedEquipment);
            SelectedEquipment = null;

            // 自动保存
            SaveEquipmentData(null);
        }
    }

    private bool CanDeleteEquipment(object? parameter)
    {
        return SelectedEquipment != null;
    }

    // 保存设备数据到Json.cs文件
    private void SaveEquipmentData(object? parameter)
    {
        try
        {
            // 1. 序列化数据为JSON
            var jsonString = JsonConvert.SerializeObject(EquipmentList, Formatting.Indented);

            // 2. 读取Json.cs文件
            string jsonFilePath = FindJsonFilePath();
            string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

            // 3. 查找_EquipmentInfo字段的位置
            int startIndex = fileContent.IndexOf("public readonly string _EquipmentInfo = @\"");
            int contentStart = fileContent.IndexOf("@\"", startIndex) + 2;
            int contentEnd = fileContent.IndexOf("\";", contentStart);

            // 4. 格式化JSON字符串（添加缩进和转义）
            string formattedJson = FormatJsonForCSharp(jsonString);

            // 5. 替换内容
            string newContent = fileContent.Substring(0, contentStart) +
                              formattedJson +
                              fileContent.Substring(contentEnd);

            // 6. 写回文件
            File.WriteAllText(jsonFilePath, newContent, Encoding.UTF8);

            // 7. 发送更新消息，通知其他页面刷新
            WeakReferenceMessenger.Default.Send(new EquipmentDataUpdatedMessage());

            MessageBox.Show("设备数据保存成功！", "成功",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存设备数据失败: {ex.Message}", "错误",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    // 将JSON格式化为C#字符串格式
    private string FormatJsonForCSharp(string jsonString)
    {
        var lines = jsonString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var formattedLines = lines.Select(line =>
        {
            string indentedLine = "        " + line;  // 添加缩进
            indentedLine = indentedLine.Replace("\"", "\"\"");  // 转义引号
            return indentedLine;
        });

        return string.Join("\r\n", formattedLines);
    }

    // 搜索设备
    private void SearchEquipment(object? parameter)
    {
        FilteredEquipmentList.Clear();

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            foreach (var item in EquipmentList)
            {
                FilteredEquipmentList.Add(item);
            }
        }
        else
        {
            var filtered = EquipmentList.Where(e =>
                e.Equ_Id.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                e.Equ_Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                e.Equ_OnlineStatus.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
            );

            foreach (var item in filtered)
            {
                FilteredEquipmentList.Add(item);
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

**关键点**：
- **自动保存**：增加和删除操作后自动保存
- **文件I/O**：直接修改源代码文件`Json.cs`
- **消息广播**：保存成功后发送消息通知其他页面
- **搜索功能**：支持按ID、名称、状态搜索

---

#### 第4步：订阅消息更新

**在其他ViewModel中订阅数据更新消息**：

```csharp
// ViewModels/HomePageBViewModel.cs
public class HomePageBViewModel : INotifyPropertyChanged
{
    public HomePageBViewModel()
    {
        // 订阅设备数据更新消息
        WeakReferenceMessenger.Default.Register<EquipmentDataUpdatedMessage>(
            this, (recipient, message) =>
            {
                // 收到消息后重新加载数据
                LoadData();
                CalculateEquipmentStatistics();
            });
    }

    private void LoadData()
    {
        // 使用JsonDataService获取最新数据
        var equipmentJson = JsonDataService.Instance.GetEquipmentInfoJson();
        var equipment = JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(equipmentJson);

        EquipmentList.Clear();
        if (equipment != null)
        {
            foreach (var item in equipment)
            {
                EquipmentList.Add(item);
            }
        }
    }
}
```

```csharp
// ViewModels/EquipmentInfoViewModel.cs
public class EquipmentInfoViewModel : INotifyPropertyChanged
{
    public EquipmentInfoViewModel()
    {
        // 同样订阅更新消息
        WeakReferenceMessenger.Default.Register<EquipmentDataUpdatedMessage>(
            this, (recipient, message) =>
            {
                LoadEquipment();
            });
    }

    private void LoadEquipment()
    {
        var equipmentJson = JsonDataService.Instance.GetEquipmentInfoJson();
        var equipment = JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(equipmentJson);

        EquipmentList.Clear();
        if (equipment != null)
        {
            foreach (var item in equipment)
            {
                EquipmentList.Add(item);
            }
        }
    }
}
```

**关键点**：
- **弱引用**：`WeakReferenceMessenger`自动管理订阅生命周期
- **Lambda表达式**：简洁的消息处理
- **动态加载**：从文件读取最新数据，而不是使用缓存

---

#### 第5步：完整的数据流程

```
用户操作流程：
1. 用户在Edit页面添加/修改/删除设备
   ↓
2. EditViewModel.SaveEquipmentData()
   ↓
3. 序列化数据为JSON字符串
   ↓
4. 使用正则表达式替换Json.cs文件中的内容
   ↓
5. File.WriteAllText()写入文件
   ↓
6. 发送EquipmentDataUpdatedMessage消息
   ↓
7. HomePageBViewModel和EquipmentInfoViewModel收到消息
   ↓
8. 调用JsonDataService.GetEquipmentInfoJson()
   ↓
9. 从文件读取最新JSON数据
   ↓
10. 反序列化并更新ObservableCollection
   ↓
11. UI自动更新显示最新数据
```

---

#### 核心技术总结

##### 1. 单例模式
```csharp
// 双重检查锁定，确保线程安全
public static JsonDataService Instance
{
    get
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new JsonDataService();
                }
            }
        }
        return _instance;
    }
}
```

##### 2. 消息传递模式
```csharp
// 发送消息
WeakReferenceMessenger.Default.Send(new EquipmentDataUpdatedMessage());

// 订阅消息
WeakReferenceMessenger.Default.Register<EquipmentDataUpdatedMessage>(
    this, (recipient, message) =>
    {
        // 处理消息
    });
```

##### 3. 文件I/O操作
```csharp
// 读取文件
string content = File.ReadAllText(path, Encoding.UTF8);

// 正则表达式提取
var match = Regex.Match(content, pattern, RegexOptions.Multiline);

// 写入文件
File.WriteAllText(path, newContent, Encoding.UTF8);
```

##### 4. 属性变化通知
```csharp
// Model需要完整的属性实现
private string _equ_OnlineStatus = string.Empty;
public string Equ_OnlineStatus
{
    get => _equ_OnlineStatus;
    set
    {
        if (_equ_OnlineStatus != value)
        {
            _equ_OnlineStatus = value;
            OnPropertyChanged(nameof(Equ_OnlineStatus));
        }
    }
}
```

---

#### 学到的经验

##### 经验1：数据同步的重要性
在多页面应用中，数据修改后必须通知所有相关页面更新，否则会出现数据不一致。

##### 经验2：单例模式的应用
对于全局共享的服务（如数据服务），使用单例模式可以：
- 节省内存
- 确保数据一致性
- 简化访问方式

##### 经验3：消息传递vs直接调用
消息传递的优势：
- ✅ 解耦：发送者和接收者互不依赖
- ✅ 灵活：可以有多个订阅者
- ✅ 安全：使用弱引用避免内存泄漏

##### 经验4：Model必须实现INotifyPropertyChanged
如果Model的属性使用自动属性，UI不会自动更新。必须：
```csharp
// ❌ 错误：使用自动属性
public string Equ_Name { get; set; }

// ✅ 正确：完整实现
private string _equ_Name = string.Empty;
public string Equ_Name
{
    get => _equ_Name;
    set
    {
        if (_equ_Name != value)
        {
            _equ_Name = value;
            OnPropertyChanged(nameof(Equ_Name));
        }
    }
}
```

##### 经验5：ComboBox绑定陷阱
```csharp
// ❌ 错误：绑定到SelectedItem会显示"ComboBoxItem:在线"
<ComboBox SelectedItem="{Binding Equ_OnlineStatus}">
    <ComboBoxItem Content="在线"/>
</ComboBox>

// ✅ 正确：使用SelectedValue和SelectedValuePath
<ComboBox SelectedValue="{Binding Equ_OnlineStatus, UpdateSourceTrigger=PropertyChanged}"
          SelectedValuePath="Content">
    <ComboBoxItem Content="在线"/>
    <ComboBoxItem Content="离线"/>
</ComboBox>
```

---

### 7.4 案例4：输入验证与自定义控件样式

#### 场景说明
在设备编辑页面中，我们需要实现以下功能：
1. **整数验证** - 固定时长字段只能输入整数
2. **小数验证** - 部署地址的经纬度可以输入小数（包括负数）
3. **ToggleButton样式** - 可点击切换东/西、南/北方向，并有颜色指示
4. **双向数据转换** - 部署地址字符串与经纬度组件互相转换
5. **只读字段** - 设备ID和使用率不可编辑

这个案例展示了：
- ✅ 输入验证的实现方法
- ✅ ToggleButton的高级样式定制
- ✅ 复杂数据的双向转换
- ✅ 事件订阅顺序的重要性
- ✅ 循环更新的防止

---

#### 第1步：实现输入验证

**作用**：限制用户只能输入特定格式的数据。

##### 整数验证
```csharp
// Views/Edit.xaml.cs
// 整数验证 - 只允许输入整数
private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
{
    Regex regex = new Regex("[^0-9]+");
    e.Handled = regex.IsMatch(e.Text);
}
```

**XAML使用**：
```xaml
<TextBox Text="{Binding Equ_FixedDurationThisYear, UpdateSourceTrigger=PropertyChanged}"
         PreviewTextInput="NumberValidationTextBox" />
```

**关键点**：
- `PreviewTextInput` - 在输入生效前验证
- `e.Handled = true` - 阻止不符合条件的输入
- `[^0-9]+` - 正则表达式，匹配非数字字符

---

##### 小数验证（含负数）
```csharp
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
```

**XAML使用**：
```xaml
<TextBox x:Name="LongitudeTextBox"
         PreviewTextInput="DecimalValidationTextBox" />
```

**关键点**：
- 多重验证：正则表达式 + 额外逻辑
- `textBox.SelectionStart` - 获取光标位置
- 防止多个小数点和负号

---

#### 第2步：ToggleButton样式与颜色切换

**问题**：直接在ToggleButton上设置Background属性会导致Trigger无法改变颜色。

##### 错误示例
```xaml
<!-- ❌ 错误：Background在控件上直接设置，Trigger无法覆盖 -->
<ToggleButton Background="#2196F3">
    <ToggleButton.Style>
        <Style TargetType="ToggleButton">
            <Style.Triggers>
                <Trigger Property="IsChecked" Value="True">
                    <!-- 这个Setter不会生效！ -->
                    <Setter Property="Background" Value="#FF9800"/>
                </Trigger>
            </Style.Triggers>
        </Style>
    </ToggleButton.Style>
</ToggleButton>
```

##### 正确实现
```xaml
<!-- ✅ 正确：Background在Style的Setter中设置 -->
<ToggleButton x:Name="LongitudeDirectionToggle" Width="60" Height="36">
    <ToggleButton.Style>
        <Style TargetType="ToggleButton">
            <!-- 在Setter中设置默认背景 -->
            <Setter Property="Background" Value="#2196F3"/>
            <Setter Property="Content" Value="东"/>
            <Setter Property="Foreground" Value="White"/>
            <Setter Property="FontSize" Value="14"/>
            <Setter Property="Cursor" Value="Hand"/>
            <Setter Property="Margin" Value="5,0,0,0"/>

            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="ToggleButton">
                        <Border Background="{TemplateBinding Background}"
                                CornerRadius="4"
                                BorderBrush="#E0E0E0"
                                BorderThickness="1">
                            <ContentPresenter HorizontalAlignment="Center"
                                            VerticalAlignment="Center"
                                            TextElement.Foreground="{TemplateBinding Foreground}"/>
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>

            <!-- Trigger可以正常覆盖Background -->
            <Style.Triggers>
                <Trigger Property="IsChecked" Value="True">
                    <Setter Property="Content" Value="西"/>
                    <Setter Property="Background" Value="#FF9800"/>
                </Trigger>
            </Style.Triggers>
        </Style>
    </ToggleButton.Style>
</ToggleButton>
```

**关键点**：
1. **TemplateBinding** - 在ControlTemplate中使用`{TemplateBinding Background}`绑定背景
2. **Setter优先级** - Style中的Setter优先级低于Trigger
3. **直接属性优先级最高** - 控件上直接设置的属性会覆盖Style和Trigger

**CSS优先级类比**：
```
直接属性 > Style.Triggers > Style.Setters
类似于：
inline style > #id > .class
```

---

#### 第3步：部署地址的双向转换

**需求**：部署地址存储格式为 `"经度, 纬度"`（如 `"120.5, 30.2"`），但UI上需要分别显示为两个输入框，且有方向切换按钮。

##### 解析部署地址（字符串 → UI组件）
```csharp
// Views/Edit.xaml.cs
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
```

**关键点**：
- 负数表示西经/南纬
- 正数表示东经/北纬
- 显示时总是显示绝对值

---

##### 构建部署地址（UI组件 → 字符串）
```csharp
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
```

**关键点**：
- 西经/南纬：取绝对值后加负号
- 东经/北纬：取绝对值（确保是正数）
- 格式化为 `"经度, 纬度"`

---

##### UI组件变化时自动更新模型
```csharp
private bool _isUpdatingAddress = false;

private void DeploymentAddress_Changed(object sender, System.Windows.RoutedEventArgs e)
{
    // 防止循环更新
    if (_isUpdatingAddress) return;

    if (DataContext is EditViewModel viewModel && viewModel.SelectedEquipment != null)
    {
        // 构建新的部署地址字符串
        string newAddress = BuildDeploymentAddress();
        viewModel.SelectedEquipment.Equ_DeploymentAddress = newAddress;
    }
}
```

**XAML订阅事件**：
```csharp
// 在构造函数中订阅
LongitudeTextBox.TextChanged += DeploymentAddress_Changed;
LatitudeTextBox.TextChanged += DeploymentAddress_Changed;
LongitudeDirectionToggle.Checked += DeploymentAddress_Changed;
LongitudeDirectionToggle.Unchecked += DeploymentAddress_Changed;
LatitudeDirectionToggle.Checked += DeploymentAddress_Changed;
LatitudeDirectionToggle.Unchecked += DeploymentAddress_Changed;
```

---

#### 第4步：事件订阅顺序与DataContext生命周期

**问题**：如果在设置DataContext之前没有订阅DataContextChanged事件，初始数据不会正确显示。

##### 错误示例
```csharp
// ❌ 错误：先设置DataContext，再订阅事件
public Edit(EditViewModel viewModel)
{
    InitializeComponent();

    // DataContext设置后，DataContextChanged事件已经错过了
    DataContext = viewModel;

    // 这个订阅太晚了！
    DataContextChanged += Edit_DataContextChanged;
}
```

##### 正确实现
```csharp
// ✅ 正确：事件订阅顺序
public Edit(EditViewModel viewModel)
{
    InitializeComponent();

    // 1. 订阅数据上下文变化事件（在设置 DataContext 之前）
    DataContextChanged += Edit_DataContextChanged;

    // 2. 订阅部署地址控件的事件
    LongitudeTextBox.TextChanged += DeploymentAddress_Changed;
    LatitudeTextBox.TextChanged += DeploymentAddress_Changed;
    LongitudeDirectionToggle.Checked += DeploymentAddress_Changed;
    LongitudeDirectionToggle.Unchecked += DeploymentAddress_Changed;
    LatitudeDirectionToggle.Checked += DeploymentAddress_Changed;
    LatitudeDirectionToggle.Unchecked += DeploymentAddress_Changed;

    // 3. 设置 DataContext（这会触发 DataContextChanged 事件）
    DataContext = viewModel;

    // 4. 订阅 ViewModel 的属性变化事件
    if (viewModel != null)
    {
        viewModel.PropertyChanged += ViewModel_PropertyChanged;

        // 如果已经有选中的设备，初始化部署地址
        if (viewModel.SelectedEquipment != null)
        {
            _isUpdatingAddress = true;
            ParseDeploymentAddress(viewModel.SelectedEquipment.Equ_DeploymentAddress);
            _isUpdatingAddress = false;
        }
    }
}
```

**事件处理实现**：
```csharp
private void Edit_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
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
        if (newViewModel.SelectedEquipment != null)
        {
            _isUpdatingAddress = true;
            ParseDeploymentAddress(newViewModel.SelectedEquipment.Equ_DeploymentAddress);
            _isUpdatingAddress = false;
        }
    }
}

private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
{
    if (e.PropertyName == nameof(EditViewModel.SelectedEquipment))
    {
        if (DataContext is EditViewModel viewModel && viewModel.SelectedEquipment != null)
        {
            _isUpdatingAddress = true;
            ParseDeploymentAddress(viewModel.SelectedEquipment.Equ_DeploymentAddress);
            _isUpdatingAddress = false;
        }
    }
}
```

**关键点**：
1. **订阅在设置之前** - DataContextChanged必须在设置DataContext之前订阅
2. **取消旧订阅** - 避免内存泄漏
3. **使用标志位** - `_isUpdatingAddress`防止循环更新

---

#### 第5步：防止循环更新

**问题**：UI更新导致模型变化，模型变化又导致UI更新，形成死循环。

**解决方案：使用标志位**
```csharp
private bool _isUpdatingAddress = false;

// UI组件变化时
private void DeploymentAddress_Changed(object sender, RoutedEventArgs e)
{
    if (_isUpdatingAddress) return;  // 防止循环

    // 更新模型
    if (DataContext is EditViewModel viewModel && viewModel.SelectedEquipment != null)
    {
        string newAddress = BuildDeploymentAddress();
        viewModel.SelectedEquipment.Equ_DeploymentAddress = newAddress;
    }
}

// 模型变化时
private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
{
    if (e.PropertyName == nameof(EditViewModel.SelectedEquipment))
    {
        if (DataContext is EditViewModel viewModel && viewModel.SelectedEquipment != null)
        {
            _isUpdatingAddress = true;  // 设置标志
            ParseDeploymentAddress(viewModel.SelectedEquipment.Equ_DeploymentAddress);
            _isUpdatingAddress = false; // 重置标志
        }
    }
}
```

**循环示意图**：
```
没有标志位时：
UI改变 → 更新模型 → 触发PropertyChanged → 更新UI → UI改变 → ...（死循环）

有标志位时：
UI改变 → 更新模型 ✓
模型改变 → 检查标志 → 更新UI → UI改变 → 检查标志 → 跳过 ✓
```

---

#### 第6步：只读字段的实现

**XAML实现**：
```xaml
<!-- 设备ID - 只读，灰色背景 -->
<TextBox Text="{Binding SelectedEquipment.Equ_Id, UpdateSourceTrigger=PropertyChanged}"
         Style="{StaticResource TextBoxStyle}"
         IsReadOnly="True"
         Background="#F5F5F5"
         Foreground="#757575"/>

<!-- 使用率 - 只读，灰色背景 -->
<TextBox Text="{Binding SelectedEquipment.Equ_UsageRateThisYear, UpdateSourceTrigger=PropertyChanged}"
         Style="{StaticResource TextBoxStyle}"
         IsReadOnly="True"
         Background="#F5F5F5"
         Foreground="#757575"/>
```

**关键点**：
- `IsReadOnly="True"` - 只读模式
- `Background="#F5F5F5"` - 浅灰色背景，视觉上表示不可编辑
- `Foreground="#757575"` - 深灰色文字

---

#### 第7步：带单位的输入框

**需求**：用户只能输入数字，单位"小时"固定显示。

**XAML实现**：
```xaml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="Auto"/>
    </Grid.ColumnDefinitions>

    <!-- 输入框 -->
    <TextBox Grid.Column="0"
             Text="{Binding SelectedEquipment.Equ_FixedDurationThisYear, UpdateSourceTrigger=PropertyChanged}"
             Style="{StaticResource TextBoxStyle}"
             PreviewTextInput="NumberValidationTextBox"
             Margin="0,0,5,0"/>

    <!-- 单位标签 -->
    <TextBlock Grid.Column="1"
               Text="小时"
               VerticalAlignment="Center"
               FontSize="14"
               Foreground="#757575"
               Margin="5,0,0,0"/>
</Grid>
```

**效果**：
```
┌──────────┬──────┐
│ 100      │ 小时 │
└──────────┴──────┘
  输入框     标签
```

---

#### 完整数据流程

```
用户操作流程：

1. 用户点击不同设备
   ↓
2. EditViewModel.SelectedEquipment改变
   ↓
3. 触发PropertyChanged事件
   ↓
4. ViewModel_PropertyChanged被调用
   ↓
5. 设置_isUpdatingAddress = true
   ↓
6. ParseDeploymentAddress()解析地址字符串
   ↓
7. 更新LongitudeTextBox, LatitudeTextBox, ToggleButton
   ↓
8. TextBox.TextChanged和ToggleButton.Checked事件触发
   ↓
9. DeploymentAddress_Changed检测到_isUpdatingAddress=true，跳过
   ↓
10. 设置_isUpdatingAddress = false
   ↓
11. UI显示完成

用户修改经纬度：

1. 用户在TextBox中输入 "120.5"
   ↓
2. PreviewTextInput事件，DecimalValidationTextBox验证
   ↓
3. 验证通过，输入生效
   ↓
4. TextChanged事件触发
   ↓
5. DeploymentAddress_Changed被调用
   ↓
6. _isUpdatingAddress = false，继续执行
   ↓
7. BuildDeploymentAddress()构建地址字符串
   ↓
8. 更新SelectedEquipment.Equ_DeploymentAddress
   ↓
9. 模型已更新，但_isUpdatingAddress=false所以不会反向更新UI
```

---

#### 核心技术总结

##### 1. PreviewTextInput验证
```csharp
// 优势：在输入生效前拦截
private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
{
    Regex regex = new Regex("[^0-9]+");
    e.Handled = regex.IsMatch(e.Text);  // 阻止不合法输入
}

// 对比：TextChanged在输入生效后才触发，需要手动回退
```

##### 2. ToggleButton样式优先级
```
优先级从高到低：
1. 控件直接设置的属性（如 <ToggleButton Background="Red"/>）
2. Style.Triggers
3. Style.Setters
4. 默认样式

正确做法：将Background放在Style.Setters中，让Trigger可以覆盖
```

##### 3. 双向转换模式
```csharp
// 模式1：使用IValueConverter（适合简单转换）
public class StringToComponentsConverter : IValueConverter
{
    public object Convert(object value, ...) { }
    public object ConvertBack(object value, ...) { }
}

// 模式2：在CodeBehind中手动转换（适合复杂逻辑）
// 本项目使用的方式，因为涉及多个控件的协同
private void ParseDeploymentAddress(string address) { }
private string BuildDeploymentAddress() { }
```

##### 4. 事件生命周期管理
```csharp
// 重要原则：
// 1. 先订阅，后设置DataContext
// 2. 取消订阅旧ViewModel，避免内存泄漏
// 3. 使用标志位防止循环更新

public Edit(EditViewModel viewModel)
{
    InitializeComponent();
    DataContextChanged += Edit_DataContextChanged;  // 先订阅
    // 订阅其他事件...
    DataContext = viewModel;  // 后设置
}
```

---

#### 学到的经验

##### 经验1：输入验证要在输入前拦截
使用`PreviewTextInput`而不是`TextChanged`，可以避免用户看到不合法的输入闪烁。

##### 经验2：ToggleButton样式不生效的常见原因
直接在控件上设置的属性优先级最高，会覆盖Style中的所有设置，包括Trigger。

##### 经验3：复杂数据转换不一定要用Converter
当转换涉及多个UI元素时，在CodeBehind中手动转换可能更清晰。

##### 经验4：防止循环更新的标准模式
```csharp
private bool _isUpdating = false;

private void UpdateFromUI()
{
    if (_isUpdating) return;
    // 更新模型...
}

private void UpdateFromModel()
{
    _isUpdating = true;
    // 更新UI...
    _isUpdating = false;
}
```

##### 经验5：事件订阅顺序很重要
DataContextChanged事件必须在设置DataContext之前订阅，否则会错过初始化时的事件。

---

## 第八章：C#面试必备

### 8.1 面向对象编程（OOP）

#### 8.1.1 四大特性

##### 1. 封装（Encapsulation）

**概念**：将数据和操作数据的方法封装在一起，隐藏内部实现细节。

**本项目示例**：
```csharp
// Models/EquipmentInfoModel.cs
public class EquipmentInfoModel : INotifyPropertyChanged
{
    // 私有字段 - 隐藏实现
    private string _equ_Name = string.Empty;

    // 公共属性 - 提供访问接口
    public string Equ_Name
    {
        get => _equ_Name;
        set
        {
            if (_equ_Name != value)
            {
                _equ_Name = value;
                OnPropertyChanged(nameof(Equ_Name));  // 封装了通知逻辑
            }
        }
    }

    // 私有方法 - 内部实现
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // 公共事件 - 对外接口
    public event PropertyChangedEventHandler? PropertyChanged;
}
```

**面试问题**：
- Q: 为什么要使用属性而不是公共字段？
- A:
  1. 可以在get/set中添加验证逻辑
  2. 可以控制读写权限（只读/只写）
  3. 可以触发事件通知
  4. 符合面向对象封装原则

---

##### 2. 继承（Inheritance）

**概念**：子类继承父类的属性和方法，实现代码复用。

**本项目示例**：
```csharp
// ViewModels/ViewModelBase.cs - 基类
public abstract partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;

    public virtual void OnLoaded() { }
    public virtual void OnUnloaded() { }
}

// ViewModels/LoginViewModel.cs - 子类
public partial class LoginViewModel : ViewModelBase
{
    // 继承了IsBusy, IsNotBusy等属性

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        IsBusy = true;  // 使用继承的属性
        try
        {
            await Task.Delay(1500);
            // 登录逻辑...
        }
        finally
        {
            IsBusy = false;
        }
    }

    // 重写基类方法
    public override void OnLoaded()
    {
        base.OnLoaded();  // 调用父类实现
        // 子类特有逻辑...
    }
}
```

**继承层次**：
```
Object (所有类的基类)
  ↓
ObservableObject (CommunityToolkit.Mvvm提供)
  ↓
ViewModelBase (项目基类)
  ↓
LoginViewModel, HomePageAViewModel, DataViewModel... (具体ViewModel)
```

**面试问题**：
- Q: C#支持多重继承吗？
- A: 不支持类的多重继承，但支持接口的多重继承。一个类只能继承一个基类，但可以实现多个接口。

```csharp
// ❌ 错误：不能继承多个类
public class MyClass : ClassA, ClassB { }

// ✅ 正确：可以实现多个接口
public class MyClass : INotifyPropertyChanged, IDisposable { }

// ✅ 正确：继承一个类，实现多个接口
public class MyClass : ViewModelBase, INavigationAware, IDisposable { }
```

---

##### 3. 多态（Polymorphism）

**概念**：同一操作作用于不同对象，产生不同的执行结果。

**方法重写（Override）**：
```csharp
// 基类
public abstract class ViewModelBase
{
    // virtual关键字表示可被重写
    public virtual void OnLoaded()
    {
        Console.WriteLine("ViewModelBase.OnLoaded");
    }
}

// 子类1
public class HomePageAViewModel : ViewModelBase
{
    // override关键字重写父类方法
    public override void OnLoaded()
    {
        base.OnLoaded();  // 可选：调用父类实现
        Console.WriteLine("HomePageAViewModel.OnLoaded");
        StartAnimation();
    }
}

// 子类2
public class DataViewModel : ViewModelBase
{
    public override void OnLoaded()
    {
        base.OnLoaded();
        Console.WriteLine("DataViewModel.OnLoaded");
        LoadData();
    }
}

// 使用多态
ViewModelBase vm1 = new HomePageAViewModel();
ViewModelBase vm2 = new DataViewModel();

vm1.OnLoaded();  // 输出: ViewModelBase.OnLoaded
                 //       HomePageAViewModel.OnLoaded

vm2.OnLoaded();  // 输出: ViewModelBase.OnLoaded
                 //       DataViewModel.OnLoaded
```

**接口实现**：
```csharp
// 定义接口
public interface INavigationAware
{
    void OnNavigatedTo(object? parameter);
    void OnNavigatedFrom();
}

// 不同类实现同一接口，提供不同实现
public class DataViewModel : INavigationAware
{
    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is string year)
            LoadDataByYear(year);
    }

    public void OnNavigatedFrom()
    {
        StopAnimations();
    }
}

public class EditViewModel : INavigationAware
{
    public void OnNavigatedTo(object? parameter)
    {
        LoadEquipmentData();
    }

    public void OnNavigatedFrom()
    {
        SaveChanges();
    }
}
```

**面试问题**：
- Q: virtual、override、abstract的区别？
- A:
  - `virtual`: 可被重写的方法，必须有实现
  - `override`: 重写父类的virtual或abstract方法
  - `abstract`: 抽象方法，没有实现，子类必须重写

```csharp
public abstract class Animal
{
    // 抽象方法 - 必须被重写
    public abstract void MakeSound();

    // 虚方法 - 可被重写
    public virtual void Sleep()
    {
        Console.WriteLine("Sleeping...");
    }
}

public class Dog : Animal
{
    // 必须重写抽象方法
    public override void MakeSound()
    {
        Console.WriteLine("Woof!");
    }

    // 可选重写虚方法
    public override void Sleep()
    {
        Console.WriteLine("Dog sleeping...");
    }
}
```

---

##### 4. 抽象（Abstraction）

**概念**：隐藏复杂的实现细节，只暴露必要的接口。

**抽象类示例**：
```csharp
// ViewModelBase.cs - 抽象基类
public abstract partial class ViewModelBase : ObservableObject
{
    // 具体实现
    [ObservableProperty]
    private bool _isBusy;

    // 抽象接口（虚方法）
    public virtual void OnLoaded() { }
    public virtual void OnUnloaded() { }
}
```

**接口示例**：
```csharp
// Services/Navigation/INavigationService.cs
public interface INavigationService
{
    void NavigateTo(string pageKey, object? parameter = null);
    bool GoBack();
    void ClearHistory();
    Page? CurrentPage { get; }
    event EventHandler<Page?>? CurrentPageChanged;
}

// 具体实现由NavigationService类提供
// 使用者只需要知道接口，不需要知道具体实现
```

**面试问题**：
- Q: 抽象类和接口的区别？
- A:

| 特性 | 抽象类 | 接口 |
|------|--------|------|
| 继承 | 单继承 | 多继承 |
| 成员 | 可以有字段、属性、方法 | 只能有方法、属性、事件、索引器 |
| 实现 | 可以有具体实现 | 不能有实现（C# 8.0+支持默认实现） |
| 访问修饰符 | 可以有任意修饰符 | 成员默认public |
| 构造函数 | 可以有 | 不能有 |
| 使用场景 | "是一个"关系 | "能做什么"关系 |

**本项目示例**：
```csharp
// 抽象类：表示"是一个"关系
// LoginViewModel "是一个" ViewModelBase
public class LoginViewModel : ViewModelBase { }

// 接口：表示"能做什么"
// DataViewModel "能做" 导航感知的事情
public class DataViewModel : INavigationAware { }
```

---

### 8.2 C#核心语法

#### 8.2.1 属性（Property）

**自动属性**：
```csharp
// 简单的自动属性
public string Name { get; set; }

// 只读自动属性
public string Id { get; }

// 带初始值的自动属性
public DateTime CreatedDate { get; set; } = DateTime.Now;

// 不同访问级别
public string Name { get; private set; }  // 外部只读，内部可写
```

**完整属性（带通知）**：
```csharp
// 本项目的Model必须用完整属性
private string _equ_Name = string.Empty;
public string Equ_Name
{
    get => _equ_Name;
    set
    {
        if (_equ_Name != value)
        {
            _equ_Name = value;
            OnPropertyChanged(nameof(Equ_Name));
        }
    }
}
```

**使用Source Generator的属性**：
```csharp
// 使用CommunityToolkit.Mvvm
public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _username = string.Empty;

    // 自动生成:
    // public string Username { get; set; }
    // 包含PropertyChanged通知
}
```

**面试问题**：
- Q: 属性和字段的区别？
- A:
  - 字段是变量，属性是方法（get/set访问器）
  - 属性可以添加验证逻辑
  - 属性可以是只读或只写
  - 属性支持数据绑定

---

#### 8.2.2 委托和事件（Delegate & Event）

**委托定义**：
```csharp
// 委托是类型安全的函数指针
public delegate void MyDelegate(string message);

// 使用委托
public class Example
{
    public void Method1(string msg)
    {
        Console.WriteLine($"Method1: {msg}");
    }

    public void Method2(string msg)
    {
        Console.WriteLine($"Method2: {msg}");
    }

    public void Test()
    {
        MyDelegate del = Method1;
        del += Method2;  // 多播委托

        del("Hello");
        // 输出:
        // Method1: Hello
        // Method2: Hello
    }
}
```

**内置委托**：
```csharp
// Action - 无返回值
Action action = () => Console.WriteLine("Action");
Action<string> actionWithParam = (s) => Console.WriteLine(s);
Action<int, string> actionWith2Params = (i, s) => Console.WriteLine($"{i}: {s}");

// Func - 有返回值（最后一个类型参数是返回类型）
Func<int> func = () => 42;
Func<string, int> funcWithParam = (s) => s.Length;
Func<int, int, int> add = (a, b) => a + b;

// Predicate - 返回bool
Predicate<int> isPositive = (n) => n > 0;
```

**本项目中的委托使用**：
```csharp
// RelayCommand内部使用委托
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public void Execute(object? parameter)
    {
        _execute();  // 调用委托
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute();  // 调用委托
    }
}
```

**事件（Event）**：
```csharp
// 定义事件
public class Publisher
{
    // 事件定义
    public event EventHandler<string>? MessageReceived;

    public void SendMessage(string message)
    {
        // 触发事件
        MessageReceived?.Invoke(this, message);
    }
}

// 订阅事件
public class Subscriber
{
    public void Subscribe(Publisher publisher)
    {
        publisher.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, string message)
    {
        Console.WriteLine($"Received: {message}");
    }
}
```

**本项目中的事件**：
```csharp
// INotifyPropertyChanged接口
public event PropertyChangedEventHandler? PropertyChanged;

protected void OnPropertyChanged(string propertyName)
{
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}

// NavigationService中的事件
public event EventHandler<Page?>? CurrentPageChanged;

private void NotifyCurrentPageChanged()
{
    CurrentPageChanged?.Invoke(this, CurrentPage);
}
```

**面试问题**：
- Q: 委托和事件的区别？
- A:
  - 事件是基于委托的封装
  - 事件只能在类内部触发（Invoke），外部只能订阅（+=）和取消订阅（-=）
  - 委托可以在外部调用
  - 事件更安全，防止外部随意触发

```csharp
public class Example
{
    // 委托字段 - 外部可以调用
    public Action MyDelegate;

    // 事件 - 外部只能订阅
    public event Action MyEvent;

    public void Test()
    {
        MyDelegate?.Invoke();  // OK
        MyEvent?.Invoke();     // OK
    }
}

// 使用
var example = new Example();
example.MyDelegate += () => { };
example.MyDelegate?.Invoke();  // OK - 外部可以调用委托

example.MyEvent += () => { };
example.MyEvent?.Invoke();     // ❌ 编译错误 - 外部不能触发事件
```

---

#### 8.2.3 Lambda表达式

**基本语法**：
```csharp
// 无参数
() => Console.WriteLine("Hello")

// 一个参数（可省略括号）
x => x * 2
(x) => x * 2

// 多个参数
(x, y) => x + y

// 代码块
(x, y) =>
{
    int sum = x + y;
    return sum;
}
```

**本项目中的Lambda使用**：

**1. LINQ查询**：
```csharp
// FaultReportViewModel.cs
private void ApplyFilters()
{
    var filtered = _allReports.AsEnumerable();

    // Lambda表达式用于Where条件
    if (SelectedStatus != "全部")
    {
        filtered = filtered.Where(r => r.Status == SelectedStatus);
    }

    if (SelectedDevice != "全部")
    {
        filtered = filtered.Where(r => r.Equipment == SelectedDevice);
    }

    // Lambda表达式用于筛选
    filtered = filtered.Where(r =>
        DateTime.TryParse(r.Date, out var date) &&
        date.Year == year);
}
```

**2. 事件订阅**：
```csharp
// HomePageBViewModel.cs
WeakReferenceMessenger.Default.Register<EquipmentDataUpdatedMessage>(
    this, (recipient, message) =>
    {
        // Lambda表达式作为事件处理器
        LoadData();
        CalculateEquipmentStatistics();
    });
```

**3. 命令定义**：
```csharp
// 使用Lambda定义命令
public ICommand SaveCommand { get; }

public ViewModel()
{
    SaveCommand = new RelayCommand(() =>
    {
        // Lambda表达式作为命令实现
        SaveData();
    }, () => CanSave());  // Lambda表达式作为CanExecute
}
```

**4. 集合操作**：
```csharp
// 使用Lambda进行集合操作
var onlineDevices = EquipmentList.Where(e => e.Equ_OnlineStatus == "在线").ToList();

var deviceNames = EquipmentList.Select(e => e.Equ_Name).ToList();

var sortedList = EquipmentList.OrderBy(e => e.Equ_Id).ToList();

var firstDevice = EquipmentList.FirstOrDefault(e => e.Equ_Id == "fntp-0");

var hasOffline = EquipmentList.Any(e => e.Equ_OnlineStatus == "离线");

var allOnline = EquipmentList.All(e => e.Equ_OnlineStatus == "在线");
```

---

#### 8.2.4 LINQ（Language Integrated Query）

**查询语法 vs 方法语法**：
```csharp
var devices = new List<EquipmentInfoModel>();

// 查询语法
var result1 = from d in devices
              where d.Equ_OnlineStatus == "在线"
              orderby d.Equ_Name
              select d.Equ_Name;

// 方法语法（本项目主要使用）
var result2 = devices
    .Where(d => d.Equ_OnlineStatus == "在线")
    .OrderBy(d => d.Equ_Name)
    .Select(d => d.Equ_Name);
```

**常用LINQ操作**：

**1. Where - 筛选**：
```csharp
// EditViewModel.cs
private void SearchEquipment(object? parameter)
{
    var filtered = EquipmentList.Where(e =>
        e.Equ_Id.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
        e.Equ_Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
        e.Equ_OnlineStatus.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
    );

    FilteredEquipmentList.Clear();
    foreach (var item in filtered)
    {
        FilteredEquipmentList.Add(item);
    }
}
```

**2. Select - 投影**：
```csharp
// 提取设备名称列表
var deviceNames = EquipmentList.Select(e => e.Equ_Name).ToList();

// 创建匿名类型
var summary = EquipmentList.Select(e => new
{
    Id = e.Equ_Id,
    Name = e.Equ_Name,
    Status = e.Equ_OnlineStatus
}).ToList();
```

**3. OrderBy/OrderByDescending - 排序**：
```csharp
// 按ID排序
var sortedById = EquipmentList.OrderBy(e => e.Equ_Id).ToList();

// 降序排序
var sortedByNameDesc = EquipmentList.OrderByDescending(e => e.Equ_Name).ToList();

// 多重排序
var sorted = EquipmentList
    .OrderBy(e => e.Equ_OnlineStatus)
    .ThenBy(e => e.Equ_Name)
    .ToList();
```

**4. First/FirstOrDefault**：
```csharp
// First - 找不到会抛异常
var first = EquipmentList.First(e => e.Equ_OnlineStatus == "在线");

// FirstOrDefault - 找不到返回null
var firstOrNull = EquipmentList.FirstOrDefault(e => e.Equ_Id == "fntp-999");
if (firstOrNull != null)
{
    // 找到了
}
```

**5. Any/All/Count**：
```csharp
// Any - 是否存在满足条件的元素
bool hasOnline = EquipmentList.Any(e => e.Equ_OnlineStatus == "在线");

// All - 是否所有元素都满足条件
bool allOnline = EquipmentList.All(e => e.Equ_OnlineStatus == "在线");

// Count - 统计数量
int onlineCount = EquipmentList.Count(e => e.Equ_OnlineStatus == "在线");
```

**6. GroupBy - 分组**：
```csharp
// 按在线状态分组
var grouped = EquipmentList.GroupBy(e => e.Equ_OnlineStatus);

foreach (var group in grouped)
{
    Console.WriteLine($"状态: {group.Key}, 数量: {group.Count()}");
    foreach (var device in group)
    {
        Console.WriteLine($"  - {device.Equ_Name}");
    }
}
```

**7. Skip/Take - 分页**：
```csharp
int pageSize = 10;
int pageIndex = 0;

// 分页查询
var page = EquipmentList
    .Skip(pageIndex * pageSize)
    .Take(pageSize)
    .ToList();
```

**面试问题**：
- Q: LINQ的延迟执行是什么意思？
- A: LINQ查询在定义时不会立即执行，只有在枚举结果时才执行（如调用ToList()、foreach等）

```csharp
// 定义查询 - 不执行
var query = EquipmentList.Where(e => e.Equ_OnlineStatus == "在线");

// 修改数据
EquipmentList.Add(new EquipmentInfoModel { Equ_OnlineStatus = "在线" });

// 现在执行查询 - 会包含刚添加的数据
var result = query.ToList();
```

---

#### 8.2.5 异步编程（Async/Await）

**基本概念**：
```csharp
// async关键字标记异步方法
public async Task<string> GetDataAsync()
{
    // await等待异步操作完成
    var result = await SomeAsyncOperation();
    return result;
}
```

**本项目中的异步使用**：

**1. 异步命令**：
```csharp
// LoginViewModel.cs
[RelayCommand(CanExecute = nameof(CanLogin))]
private async Task LoginAsync()
{
    ErrorMessage = string.Empty;

    // 验证
    if (Username.Trim() != TEST_USERNAME || Password != TEST_PASSWORD)
    {
        ErrorMessage = "用户名或密码错误";
        return;
    }

    IsBusy = true;

    try
    {
        // 异步操作 - 不会阻塞UI线程
        await Task.Delay(1500);

        // 登录成功后的操作
        var mainWindow = App.GetService<MainWindow>();
        mainWindow.Show();

        // 关闭登录窗口
        Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w is Login)
            ?.Close();
    }
    finally
    {
        IsBusy = false;
    }
}

private bool CanLogin() => !IsBusy;
```

**2. 异步方法签名**：
```csharp
// 无返回值
public async Task DoSomethingAsync()
{
    await Task.Delay(1000);
}

// 有返回值
public async Task<int> GetCountAsync()
{
    await Task.Delay(1000);
    return 42;
}

// 事件处理器可以是async void
private async void Button_Click(object sender, RoutedEventArgs e)
{
    await DoSomethingAsync();
}
```

**3. 并行异步操作**：
```csharp
// 顺序执行
var result1 = await Operation1Async();
var result2 = await Operation2Async();  // 等待Operation1完成后才执行

// 并行执行
var task1 = Operation1Async();
var task2 = Operation2Async();
await Task.WhenAll(task1, task2);  // 同时执行，等待都完成

// 获取结果
var result1 = await task1;
var result2 = await task2;
```

**4. 异步LINQ**：
```csharp
// 假设有异步数据加载
public async Task<List<EquipmentInfoModel>> LoadEquipmentAsync()
{
    await Task.Delay(1000);  // 模拟网络请求

    var json = await File.ReadAllTextAsync("data.json");
    var data = JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(json);

    return data ?? new List<EquipmentInfoModel>();
}

// 使用
public async Task InitializeAsync()
{
    IsBusy = true;

    var equipment = await LoadEquipmentAsync();

    EquipmentList.Clear();
    foreach (var item in equipment)
    {
        EquipmentList.Add(item);
    }

    IsBusy = false;
}
```

**面试问题**：

- Q: async/await的工作原理？
- A: async/await是语法糖，编译器会将异步方法转换为状态机。await会暂停方法执行，将控制权返回给调用者，当异步操作完成后继续执行。

- Q: Task和Thread的区别？
- A:
  - Thread是操作系统级别的线程，开销大
  - Task是更高层的抽象，由线程池管理，开销小
  - Task支持async/await，Thread不支持
  - Task可以返回值，Thread不能直接返回值

- Q: 什么时候用Task.Run？
- A:
  ```csharp
  // CPU密集型操作，使用Task.Run放到后台线程
  var result = await Task.Run(() =>
  {
      // 复杂计算
      return ComputeComplexCalculation();
  });

  // I/O操作，直接使用异步API，不需要Task.Run
  var data = await File.ReadAllTextAsync("file.txt");  // 已经是异步的
  ```

---

### 8.3 高级特性

#### 8.3.1 泛型（Generics）

**泛型类**：
```csharp
// 定义泛型类
public class Repository<T> where T : class
{
    private List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T? GetById(int id)
    {
        // 实现查找逻辑
        return default;
    }

    public List<T> GetAll()
    {
        return _items;
    }
}

// 使用
var equipmentRepo = new Repository<EquipmentInfoModel>();
equipmentRepo.Add(new EquipmentInfoModel());
```

**泛型方法**：
```csharp
// App.xaml.cs中的泛型方法
public static T GetService<T>() where T : notnull
{
    return ((App)Current)._serviceProvider!.GetRequiredService<T>();
}

// 使用
var mainWindow = App.GetService<MainWindow>();
var viewModel = App.GetService<LoginViewModel>();
```

**泛型约束**：
```csharp
// where T : class         - T必须是引用类型
// where T : struct        - T必须是值类型
// where T : new()         - T必须有无参构造函数
// where T : BaseClass     - T必须继承自BaseClass
// where T : IInterface    - T必须实现IInterface
// where T : notnull       - T不能为null（C# 8.0+）

public class Example<T> where T : ViewModelBase, INavigationAware, new()
{
    public T CreateInstance()
    {
        return new T();  // 因为有new()约束，可以创建实例
    }
}
```

**本项目中的泛型使用**：

**1. ObservableCollection\<T\>**：
```csharp
public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();
public ObservableCollection<FaultReportModel> FaultReports { get; } = new();
```

**2. RelayCommand\<T\>**：
```csharp
// 带参数的命令
public class RelayCommand<T> : ICommand
{
    private readonly Action<T?> _execute;
    private readonly Func<T?, bool>? _canExecute;

    public void Execute(object? parameter)
    {
        _execute((T?)parameter);
    }
}

// 使用
[RelayCommand]
private void Delete(int id)
{
    // DeleteCommand是RelayCommand<int>类型
}
```

**3. 依赖注入容器**：
```csharp
// 注册服务
services.AddSingleton<INavigationService, NavigationService>();
services.AddTransient<MainWindowViewModel>();

// 获取服务 - 使用泛型方法
var service = serviceProvider.GetRequiredService<INavigationService>();
```

---

#### 8.3.2 扩展方法（Extension Methods）

**定义扩展方法**：
```csharp
// 必须是静态类
public static class StringExtensions
{
    // 必须是静态方法，第一个参数用this修饰
    public static bool IsNullOrEmpty(this string? str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static string Truncate(this string str, int maxLength)
    {
        if (str.Length <= maxLength)
            return str;

        return str.Substring(0, maxLength) + "...";
    }
}

// 使用
string text = "Hello World";
bool isEmpty = text.IsNullOrEmpty();  // 像调用实例方法一样
string short = text.Truncate(5);  // "Hello..."
```

**LINQ就是扩展方法**：
```csharp
// LINQ的Where、Select等都是扩展方法
public static class Enumerable
{
    public static IEnumerable<T> Where<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate)
    {
        foreach (var item in source)
        {
            if (predicate(item))
                yield return item;
        }
    }
}
```

---

#### 8.3.3 可空引用类型（Nullable Reference Types）

**C# 8.0+特性**：
```csharp
// 启用可空引用类型（项目文件中配置）
<Nullable>enable</Nullable>

// 不可为null的引用类型
string name = "John";
name = null;  // ⚠️ 警告：不能将null赋值给非空类型

// 可为null的引用类型
string? nullableName = null;  // ✅ OK
nullableName = "John";  // ✅ OK

// 使用前检查
if (nullableName != null)
{
    Console.WriteLine(nullableName.Length);  // ✅ 安全
}

// 或使用null条件运算符
Console.WriteLine(nullableName?.Length);  // 如果为null返回null

// 或使用null合并运算符
string displayName = nullableName ?? "Unknown";
```

**本项目中的使用**：
```csharp
// ViewModels/EditViewModel.cs
private EquipmentInfoModel? _selectedEquipment;  // 可为null

public EquipmentInfoModel? SelectedEquipment
{
    get => _selectedEquipment;
    set
    {
        if (_selectedEquipment != value)
        {
            _selectedEquipment = value;
            OnPropertyChanged();
        }
    }
}

// 使用时检查null
if (SelectedEquipment != null)
{
    string name = SelectedEquipment.Equ_Name;
}

// 或使用null条件运算符
string? name = SelectedEquipment?.Equ_Name;
```

---

#### 8.3.4 模式匹配（Pattern Matching）

**类型模式**：
```csharp
object obj = "Hello";

// 传统方式
if (obj is string)
{
    string str = (string)obj;
    Console.WriteLine(str.Length);
}

// 模式匹配
if (obj is string str)
{
    Console.WriteLine(str.Length);  // str已经是string类型
}
```

**Switch表达式（C# 8.0+）**：
```csharp
// 传统switch语句
string GetStatusColor(string status)
{
    switch (status)
    {
        case "在线":
            return "Green";
        case "离线":
            return "Red";
        case "维护中":
            return "Orange";
        default:
            return "Gray";
    }
}

// Switch表达式
string GetStatusColor(string status) => status switch
{
    "在线" => "Green",
    "离线" => "Red",
    "维护中" => "Orange",
    _ => "Gray"  // _ 表示默认情况
};
```

**本项目中的使用**：
```csharp
// MainWindowViewModel.cs
[RelayCommand]
private void PreviousPage()
{
    var currentPageType = _navigationService.CurrentPage?.GetType().Name;
    var previousPage = currentPageType switch
    {
        "HomePageA" => "HomePageC",
        "HomePageB" => "HomePageA",
        "HomePageC" => "HomePageB",
        _ => "HomePageB"
    };
    _navigationService.NavigateTo(previousPage);
}

// DataViewModel.cs - 参数模式匹配
public void OnNavigatedTo(object? parameter)
{
    if (parameter is string year)
    {
        LoadDataByYear(year);
    }
    else if (parameter is ValueTuple<int, int> tuple)
    {
        var (month, targetYear) = tuple;
        LoadDataByMonth(month, targetYear);
    }
}
```

---

### 8.4 常见面试题

#### 8.4.1 值类型 vs 引用类型

**值类型**：
- 存储在栈上
- 包含实际数据
- 赋值时复制值
- 类型：int, double, bool, struct, enum

**引用类型**：
- 引用存储在栈上，数据存储在堆上
- 包含对数据的引用
- 赋值时复制引用
- 类型：class, interface, delegate, string, array

**示例**：
```csharp
// 值类型
int a = 10;
int b = a;  // 复制值
b = 20;
Console.WriteLine(a);  // 输出: 10 (a不受影响)

// 引用类型
var equipment1 = new EquipmentInfoModel { Equ_Name = "设备1" };
var equipment2 = equipment1;  // 复制引用，指向同一对象
equipment2.Equ_Name = "设备2";
Console.WriteLine(equipment1.Equ_Name);  // 输出: 设备2 (equipment1也改变了)
```

**面试问题**：
- Q: string是值类型还是引用类型？
- A: string是引用类型，但表现得像值类型（因为不可变性）

```csharp
string str1 = "Hello";
string str2 = str1;
str2 = "World";  // 创建新string对象，str2指向新对象
Console.WriteLine(str1);  // 输出: Hello (str1不变)
```

---

#### 8.4.2 装箱和拆箱

**装箱（Boxing）**：值类型 → 引用类型
```csharp
int num = 123;
object obj = num;  // 装箱：在堆上创建int的副本
```

**拆箱（Unboxing）**：引用类型 → 值类型
```csharp
object obj = 123;  // 已装箱
int num = (int)obj;  // 拆箱：必须显式转换
```

**性能影响**：
```csharp
// ❌ 性能差：频繁装箱
ArrayList list = new ArrayList();
for (int i = 0; i < 1000; i++)
{
    list.Add(i);  // 每次都装箱
}

// ✅ 性能好：使用泛型，避免装箱
List<int> list = new List<int>();
for (int i = 0; i < 1000; i++)
{
    list.Add(i);  // 不装箱
}
```

---

#### 8.4.3 ==和Equals的区别

**值类型**：
```csharp
int a = 5;
int b = 5;
Console.WriteLine(a == b);  // true - 比较值
Console.WriteLine(a.Equals(b));  // true - 比较值
```

**引用类型（默认行为）**：
```csharp
var obj1 = new EquipmentInfoModel { Equ_Id = "fntp-0" };
var obj2 = new EquipmentInfoModel { Equ_Id = "fntp-0" };

Console.WriteLine(obj1 == obj2);  // false - 比较引用
Console.WriteLine(obj1.Equals(obj2));  // false - 默认比较引用

var obj3 = obj1;
Console.WriteLine(obj1 == obj3);  // true - 相同引用
```

**string的特殊行为**：
```csharp
string str1 = "Hello";
string str2 = "Hello";
Console.WriteLine(str1 == str2);  // true - string重写了==运算符
Console.WriteLine(str1.Equals(str2));  // true
```

**重写Equals**：
```csharp
public class EquipmentInfoModel
{
    public string Equ_Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is EquipmentInfoModel other)
        {
            return Equ_Id == other.Equ_Id;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Equ_Id.GetHashCode();
    }
}
```

---

#### 8.4.4 访问修饰符

| 修饰符 | 类内部 | 派生类 | 同程序集 | 其他程序集 |
|--------|--------|--------|----------|------------|
| public | ✅ | ✅ | ✅ | ✅ |
| private | ✅ | ❌ | ❌ | ❌ |
| protected | ✅ | ✅ | ❌ | ❌ |
| internal | ✅ | ✅ | ✅ | ❌ |
| protected internal | ✅ | ✅ | ✅ | 派生类可访问 |
| private protected | ✅ | 同程序集的派生类 | ❌ | ❌ |

**示例**：
```csharp
public class ViewModelBase
{
    public bool IsBusy { get; set; }  // 所有地方可访问

    private bool _isLoading;  // 只有ViewModelBase内部可访问

    protected void OnLoaded() { }  // ViewModelBase和派生类可访问

    internal void InternalMethod() { }  // 同程序集内可访问

    protected internal void ProtectedInternalMethod() { }  // 同程序集或派生类
}
```

---

#### 8.4.5 ref、out、in参数

**ref - 引用传递**：
```csharp
void Increment(ref int num)
{
    num++;  // 修改原始值
}

int value = 5;
Increment(ref value);
Console.WriteLine(value);  // 输出: 6

// 使用ref时，变量必须先初始化
int num;  // ❌ 错误
Increment(ref num);

int num = 0;  // ✅ 正确
Increment(ref num);
```

**out - 输出参数**：
```csharp
bool TryParse(string input, out int result)
{
    return int.TryParse(input, out result);
}

// 使用out时，变量不需要初始化
int number;  // ✅ OK
if (TryParse("123", out number))
{
    Console.WriteLine(number);  // 输出: 123
}

// C# 7.0+可以内联声明
if (TryParse("123", out int num))
{
    Console.WriteLine(num);
}
```

**本项目中的使用**：
```csharp
// Edit.xaml.cs
private void ParseDeploymentAddress(string address)
{
    var parts = address.Split(',');
    if (parts.Length == 2)
    {
        // 使用TryParse（内部使用out参数）
        if (double.TryParse(parts[0], out double longitude))
        {
            LongitudeTextBox.Text = System.Math.Abs(longitude).ToString();
        }

        if (double.TryParse(parts[1], out double latitude))
        {
            LatitudeTextBox.Text = System.Math.Abs(latitude).ToString();
        }
    }
}
```

**in - 只读引用**：
```csharp
// 传递大型结构体时，使用in避免复制，同时防止修改
void ProcessData(in LargeStruct data)
{
    // 可以读取data，但不能修改
    var value = data.Value;
    // data.Value = 10;  // ❌ 编译错误
}
```

---

#### 8.4.6 StringBuilder vs String

**String是不可变的**：
```csharp
string str = "Hello";
str += " World";  // 创建新string对象，原对象被丢弃

// 性能问题
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString();  // 每次都创建新string，性能差
}
```

**StringBuilder是可变的**：
```csharp
var sb = new StringBuilder("Hello");
sb.Append(" World");  // 修改同一对象

// 性能好
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);  // 不创建新对象，性能好
}
string result = sb.ToString();
```

**选择建议**：
- 少量字符串连接（<5次）：使用String
- 大量字符串连接：使用StringBuilder
- 字符串插值：使用 $"{}"

---

#### 8.4.7 using语句和IDisposable

**IDisposable接口**：
```csharp
public interface IDisposable
{
    void Dispose();
}
```

**实现IDisposable**：
```csharp
public class FileHandler : IDisposable
{
    private FileStream? _fileStream;
    private bool _disposed = false;

    public FileHandler(string path)
    {
        _fileStream = File.OpenRead(path);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            // 释放托管资源
            _fileStream?.Dispose();
        }

        // 释放非托管资源

        _disposed = true;
    }

    ~FileHandler()
    {
        Dispose(false);
    }
}
```

**using语句**：
```csharp
// 传统using
using (var handler = new FileHandler("file.txt"))
{
    // 使用handler
}  // 自动调用Dispose

// C# 8.0+ using声明
using var handler = new FileHandler("file.txt");
// 使用handler
// 作用域结束时自动调用Dispose
```

**本项目中的使用**：
```csharp
// 虽然项目中没有直接实现IDisposable，但使用了很多实现了IDisposable的类
// 例如文件操作
var content = File.ReadAllText(path);  // 内部使用using管理FileStream
```

---

### 8.5 面试准备建议

#### 8.5.1 如何介绍这个项目

**项目简介模板**：
```
这是一个基于WPF和MVVM模式的数据可视化平台，主要功能包括：
1. 设备信息管理（CRUD操作）
2. 故障报告系统
3. 预约列表管理
4. 数据可视化展示

技术栈：
- 框架：WPF (.NET 8.0)
- 架构：MVVM
- 依赖注入：Microsoft.Extensions.DependencyInjection
- MVVM工具包：CommunityToolkit.Mvvm
- 数据序列化：Newtonsoft.Json
- 图表库：LiveCharts

项目亮点：
1. 完整的MVVM架构实现
2. 依赖注入和服务导向设计
3. 实时数据同步（消息传递机制）
4. 输入验证和数据转换
5. 动态数据加载和文件I/O操作
```

#### 8.5.2 常见项目相关问题

**Q1: 为什么选择MVVM？**
```
A: MVVM模式将业务逻辑与UI分离，带来以下好处：
1. 可测试性：ViewModel可以独立测试，不需要UI
2. 可维护性：职责清晰，易于维护和修改
3. 可重用性：ViewModel可以被多个View使用
4. 团队协作：设计师和开发者可以并行工作

在本项目中，所有业务逻辑都在ViewModel中实现，View仅负责展示，
通过数据绑定和命令实现交互，完全解耦。
```

**Q2: 如何实现跨页面数据同步？**
```
A: 项目使用了三种方式：
1. 单例数据服务：JsonDataService使用单例模式，确保数据一致性
2. 消息传递：使用WeakReferenceMessenger发送EquipmentDataUpdatedMessage，
   通知其他页面刷新数据
3. 动态文件读取：每次加载数据都从源文件读取最新内容

具体流程：
Edit页面保存数据 → 发送消息 → HomePageB/EquipmentInfo收到消息 →
调用JsonDataService获取最新数据 → UI自动更新
```

**Q3: 依赖注入是如何实现的？**
```
A: 项目使用Microsoft.Extensions.DependencyInjection：
1. 在App.xaml.cs的ConfigureServices方法中注册服务
2. NavigationService注册为Singleton（全局单例）
3. ViewModels和Views注册为Transient（每次创建新实例）
4. 通过构造函数注入依赖

优势：
- 松耦合：类之间不直接依赖，而是依赖接口
- 易测试：可以注入Mock对象进行单元测试
- 易维护：修改实现类不影响使用者
```

**Q4: 如何处理异步操作？**
```
A: 项目使用async/await模式：
1. 登录操作使用异步命令（Task类型）
2. 使用IsBusy属性显示加载状态
3. try-finally确保IsBusy正确重置
4. CanExecute配合IsBusy防止重复提交

示例：LoginViewModel的LoginAsync方法
- 设置IsBusy=true禁用按钮
- await异步操作，不阻塞UI
- finally中重置IsBusy
```

---

#### 8.5.3 编码题准备

**题目1：实现一个ObservableObject基类**
```csharp
public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

// 使用
public class PersonViewModel : ObservableObject
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}
```

**题目2：实现一个RelayCommand**
```csharp
public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute();
    }

    public void Execute(object? parameter)
    {
        _execute();
    }
}
```

**题目3：实现LINQ的Where方法**
```csharp
public static class MyLinq
{
    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        foreach (var item in source)
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}

// 使用
var numbers = new[] { 1, 2, 3, 4, 5 };
var evenNumbers = numbers.MyWhere(n => n % 2 == 0);
```

---

#### 8.5.4 面试准备检查清单

**基础知识**：
- ✅ 值类型 vs 引用类型
- ✅ 装箱和拆箱
- ✅ == vs Equals
- ✅ string vs StringBuilder
- ✅ ref, out, in参数
- ✅ 访问修饰符

**面向对象**：
- ✅ 四大特性（封装、继承、多态、抽象）
- ✅ 抽象类 vs 接口
- ✅ virtual, override, abstract
- ✅ 继承链和多态调用

**高级特性**：
- ✅ 委托和事件
- ✅ Lambda表达式
- ✅ LINQ查询
- ✅ 异步编程（async/await）
- ✅ 泛型和泛型约束
- ✅ 扩展方法
- ✅ 可空引用类型
- ✅ 模式匹配

**项目经验**：
- ✅ MVVM模式的理解和实现
- ✅ 依赖注入的原理和使用
- ✅ 数据绑定机制
- ✅ 命令系统
- ✅ 导航系统
- ✅ 消息传递
- ✅ 文件I/O操作

**实践能力**：
- ✅ 能手写ObservableObject
- ✅ 能手写RelayCommand
- ✅ 能实现简单的LINQ方法
- ✅ 能解释异步代码的执行流程
- ✅ 能分析代码的性能问题

---

## 第九章：学习路径

### 9.1 初学者学习路线

#### 阶段1：WPF基础（1-2周）
1. **XAML语法** ✅
   - 元素和属性
   - 布局容器
   - 资源和样式

2. **基础控件** ✅
   - Button, TextBox, TextBlock
   - ListBox, ComboBox
   - Grid, StackPanel

3. **事件处理** ✅
   - Click事件
   - Loaded/Unloaded
   - 键盘和鼠标事件

**练习项目**: 简单的计算器应用

---

#### 阶段2：数据绑定（1-2周）
1. **绑定基础** ✅
   - OneWay, TwoWay
   - UpdateSourceTrigger
   - DataContext

2. **集合绑定** ✅
   - ObservableCollection
   - ItemsControl
   - DataTemplate

3. **值转换器** ✅
   - IValueConverter
   - 常用转换器

**练习项目**: 学生信息管理系统

---

#### 阶段3：MVVM模式（2-3周）
1. **MVVM理论** ✅
   - Model-View-ViewModel
   - 职责分离
   - 为什么使用MVVM

2. **INotifyPropertyChanged** ✅
   - 手动实现
   - 使用CommunityToolkit.Mvvm

3. **命令系统** ✅
   - ICommand接口
   - RelayCommand
   - [RelayCommand]特性

**练习项目**: Todo待办事项应用

---

#### 阶段4：高级特性（2-3周）
1. **依赖注入** ✅
   - DI容器配置
   - 服务生命周期
   - 构造函数注入

2. **导航系统** ✅
   - 页面导航
   - 参数传递
   - 导航历史

3. **消息传递** ✅
   - WeakReferenceMessenger
   - 跨ViewModel通信

**练习项目**: 本项目的简化版

---

#### 阶段5：实战项目（4-6周）
1. **分析需求** ✅
2. **设计架构** ✅
3. **实现功能** ✅
4. **测试调试** ✅
5. **优化性能** ✅

**项目**: DataVisualizationPlatform

---

### 9.2 推荐学习资源

#### 官方文档
- [WPF官方文档](https://docs.microsoft.com/wpf/)
- [MVVM Toolkit](https://learn.microsoft.com/windows/communitytoolkit/mvvm/)
- [.NET文档](https://docs.microsoft.com/dotnet/)

#### 书籍推荐
- 《Pro WPF in C# 2012》
- 《WPF编程宝典》
- 《深入浅出WPF》

#### 视频教程
- Microsoft Learn
- Pluralsight WPF课程
- YouTube WPF教程

#### 开源项目
- **本项目** - DataVisualizationPlatform
- [WPF Samples](https://github.com/microsoft/WPF-Samples)
- [ModernWPF](https://github.com/Kinnara/ModernWpf)

---

### 9.3 常见学习误区

#### 误区1：过早关注UI美化
❌ **错误**: 一开始就花大量时间调整样式
✅ **正确**: 先掌握MVVM核心概念，功能实现后再美化

#### 误区2：不理解数据绑定就使用
❌ **错误**: 复制粘贴绑定代码，不理解原理
✅ **正确**: 理解绑定机制，知道何时更新

#### 误区3：在CodeBehind写业务逻辑
❌ **错误**: 在`.xaml.cs`中写大量逻辑
✅ **正确**: 逻辑放在ViewModel，CodeBehind只处理UI相关

#### 误区4：忽视MVVM的意义
❌ **错误**: 为了用MVVM而用，不理解好处
✅ **正确**: 理解MVVM解决的问题和带来的价值

#### 误区5：不使用依赖注入
❌ **错误**: 到处`new`对象
✅ **正确**: 使用DI容器管理对象生命周期

---

### 9.4 实践建议

#### 建议1：从小项目开始
不要一开始就尝试复杂项目，从简单的CRUD应用开始。

#### 建议2：多看源码
阅读优秀的开源WPF项目，学习最佳实践。

#### 建议3：写代码注释
养成写注释的习惯，帮助理解和记忆。

#### 建议4：使用版本控制
用Git管理代码，方便回退和协作。

#### 建议5：重构代码
定期重构，改进代码质量。

---

### 9.5 进阶方向

#### 方向1：性能优化
- 虚拟化
- 异步编程
- 内存管理

#### 方向2：高级UI
- 自定义控件
- 附加行为
- 动画效果

#### 方向3：架构设计
- 插件架构
- 微服务集成
- 响应式编程

#### 方向4：跨平台
- .NET MAUI
- Avalonia
- Uno Platform

---

## 附录

### A. 快速参考

#### XAML常用命名空间
```xaml
xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
xmlns:local="clr-namespace:YourNamespace"
```

#### 常用特性
```csharp
[ObservableProperty]          // 自动生成属性
[RelayCommand]                // 自动生成命令
[NotifyPropertyChangedFor]    // 通知其他属性
[NotifyCanExecuteChangedFor]  // 通知命令状态
```

#### 常用转换器
- BooleanToVisibilityConverter
- NullToVisibilityConverter
- InverseBooleanConverter
- StringFormatConverter

---

### B. 调试技巧

#### 绑定调试
```xaml
<!-- 设置追踪级别 -->
<TextBlock Text="{Binding Title,
           PresentationTraceSources.TraceLevel=High}" />
```

#### 输出调试信息
```csharp
System.Diagnostics.Debug.WriteLine($"当前值: {value}");
```

#### 使用断点
- 在属性Set方法设置断点
- 在命令方法设置断点
- 在OnNavigatedTo设置断点

---

### C. 常见问题速查

| 问题 | 可能原因 | 解决方案 |
|------|----------|----------|
| 绑定不生效 | DataContext未设置 | 检查DataContext |
| 命令无法执行 | CanExecute返回false | 检查CanExecute逻辑 |
| 属性不更新 | 未通知PropertyChanged | 使用[ObservableProperty] |
| 页面无内容 | 未实现INavigationAware | 实现OnNavigatedTo |
| DI注入失败 | 服务未注册 | 在App.xaml.cs注册 |

---

## 结语

### 学习建议
1. **打好基础** - 不要跳过基础知识
2. **多写代码** - 实践是最好的学习方式
3. **理解原理** - 知其然也要知其所以然
4. **保持耐心** - WPF有一定学习曲线
5. **持续学习** - 技术不断更新，保持学习

### 下一步行动
1. ✅ 完整阅读本文档
2. ✅ 运行DataVisualizationPlatform项目
3. ✅ 尝试修改代码观察效果
4. ✅ 完成每个阶段的练习项目
5. ✅ 开始自己的WPF项目

### 获取帮助
- 查阅官方文档
- 搜索Stack Overflow
- 加入WPF社区
- 阅读优秀开源项目

---

**祝你学习愉快！**

**文档版本**: v2.0
**最后更新**: 2025-11-12
**适用项目**: DataVisualizationPlatform 2.0.0

**更新记录**:
- v2.0 (2025-11-12): 新增第八章"C#面试必备"，涵盖面向对象、核心语法、高级特性、常见面试题和面试准备建议，帮助准备C#/.NET开发岗位面试
- v1.2 (2025-11-12): 新增案例4 - 输入验证与自定义控件样式
- v1.1 (2025-10-30): 新增案例3 - 数据编辑系统与跨页面数据同步
- v1.0 (2025-10-29): 初始版本
