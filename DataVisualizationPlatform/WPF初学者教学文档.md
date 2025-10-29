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
- [第八章：学习路径](#第八章学习路径)

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

## 第八章：学习路径

### 8.1 初学者学习路线

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

### 8.2 推荐学习资源

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

### 8.3 常见学习误区

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

### 8.4 实践建议

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

### 8.5 进阶方向

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

**文档版本**: v1.0
**最后更新**: 2025-10-29
**适用项目**: DataVisualizationPlatform 1.0.0
