# 附录A - 代码片段速查

> **用途**：快速查找常用代码模板，复制粘贴即可使用
> **更新日期**：2025-11-20

---

## 📋 目录

1. [MVVM基础](#1-mvvm基础)
2. [数据绑定](#2-数据绑定)
3. [命令](#3-命令)
4. [导航](#4-导航)
5. [消息传递](#5-消息传递)
6. [依赖注入](#6-依赖注入)
7. [异步编程](#7-异步编程)
8. [文件I/O](#8-文件io)
9. [XAML常用片段](#9-xaml常用片段)
10. [调试技巧](#10-调试技巧)

---

## 1. MVVM基础

### 1.1 ViewModel基类

```csharp
using CommunityToolkit.Mvvm.ComponentModel;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;

    public virtual void OnLoaded() { }
    public virtual void OnUnloaded() { }
}
```

### 1.2 完整的ViewModel示例

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

public partial class SampleViewModel : ViewModelBase
{
    // ═══════ 属性 ═══════
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private int _age;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // 计算属性
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    // 集合
    public ObservableCollection<Item> Items { get; } = new();

    // ═══════ 构造函数 ═══════
    public SampleViewModel()
    {
        LoadData();
    }

    // ═══════ 命令 ═══════
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrEmpty(Name))
        {
            ErrorMessage = "名称不能为空";
            return;
        }

        IsBusy = true;
        try
        {
            await Task.Delay(1000);  // 模拟保存
            // 保存逻辑
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void Delete(Item item)
    {
        Items.Remove(item);
    }

    // ═══════ 方法 ═══════
    private void LoadData()
    {
        // 加载数据
    }

    // ═══════ 属性变化钩子 ═══════
    partial void OnNameChanged(string value)
    {
        ErrorMessage = string.Empty;  // 清除错误
    }
}
```

---

## 2. 数据绑定

### 2.1 基本绑定

```xml
<!-- 单向绑定（OneWay） -->
<TextBlock Text="{Binding Name}"/>

<!-- 双向绑定（TwoWay） -->
<TextBox Text="{Binding Name, Mode=TwoWay}"/>

<!-- 实时更新 -->
<TextBox Text="{Binding Name, UpdateSourceTrigger=PropertyChanged}"/>

<!-- 绑定到DataContext -->
<StackPanel DataContext="{Binding CurrentItem}">
    <TextBlock Text="{Binding Title}"/>
    <TextBlock Text="{Binding Description}"/>
</StackPanel>
```

### 2.2 集合绑定

```xml
<!-- ListBox绑定 -->
<ListBox ItemsSource="{Binding Items}"
         SelectedItem="{Binding SelectedItem}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <StackPanel>
                <TextBlock Text="{Binding Name}" FontWeight="Bold"/>
                <TextBlock Text="{Binding Description}"/>
            </StackPanel>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>

<!-- ComboBox绑定 -->
<ComboBox ItemsSource="{Binding Items}"
          SelectedItem="{Binding SelectedItem}"
          DisplayMemberPath="Name"
          SelectedValuePath="Id"/>
```

### 2.3 值转换器

```csharp
// 定义转换器
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (Visibility)value == Visibility.Visible;
    }
}
```

```xml
<!-- 使用转换器 -->
<Window.Resources>
    <local:BoolToVisibilityConverter x:Key="BoolToVisConverter"/>
</Window.Resources>

<Border Visibility="{Binding IsVisible, Converter={StaticResource BoolToVisConverter}}"/>
```

---

## 3. 命令

### 3.1 简单命令

```csharp
[RelayCommand]
private void DoSomething()
{
    // 执行操作
}

[RelayCommand]
private void DoSomethingWithParameter(string parameter)
{
    // 使用参数
}
```

```xml
<Button Content="Click Me" Command="{Binding DoSomethingCommand}"/>
<Button Content="With Param" Command="{Binding DoSomethingWithParameterCommand}"
        CommandParameter="Hello"/>
```

### 3.2 异步命令

```csharp
[RelayCommand]
private async Task LoadDataAsync()
{
    IsBusy = true;
    try
    {
        await Task.Delay(1000);
        // 加载数据
    }
    catch (Exception ex)
    {
        ErrorMessage = ex.Message;
    }
    finally
    {
        IsBusy = false;
    }
}
```

### 3.3 带CanExecute的命令

```csharp
[RelayCommand(CanExecute = nameof(CanSave))]
private async Task SaveAsync()
{
    // 保存逻辑
}

private bool CanSave()
{
    return !string.IsNullOrEmpty(Name) && !IsBusy;
}

// 当Name变化时，通知命令重新检查CanExecute
[ObservableProperty]
[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
private string _name = string.Empty;
```

---

## 4. 导航

### 4.1 导航服务接口

```csharp
public interface INavigationService
{
    void NavigateTo(string pageKey, object? parameter = null);
    bool GoBack();
    void ClearHistory();
    Page? CurrentPage { get; }
    event EventHandler<Page?>? CurrentPageChanged;
}
```

### 4.2 使用导航服务

```csharp
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private void Navigate(string pageKey)
    {
        _navigationService.NavigateTo(pageKey);
    }

    [RelayCommand]
    private void NavigateWithParameter()
    {
        var data = new { Id = 1, Name = "Test" };
        _navigationService.NavigateTo("DetailPage", data);
    }
}
```

### 4.3 接收导航参数

```csharp
public partial class DetailViewModel : ViewModelBase, INavigationAware
{
    public void OnNavigatedTo(object? parameter)
    {
        if (parameter is MyModel model)
        {
            LoadData(model.Id);
        }
    }

    public void OnNavigatedFrom()
    {
        // 离开页面时的清理
    }
}
```

---

## 5. 消息传递

### 5.1 定义消息

```csharp
// Messages/DataUpdatedMessage.cs
public class DataUpdatedMessage
{
    public int Id { get; set; }
    public string Action { get; set; }  // "Added", "Updated", "Deleted"

    public DataUpdatedMessage(int id, string action)
    {
        Id = id;
        Action = action;
    }
}
```

### 5.2 发送消息

```csharp
using CommunityToolkit.Mvvm.Messaging;

// 发送简单消息
WeakReferenceMessenger.Default.Send(new DataUpdatedMessage(1, "Updated"));

// 发送带Payload的消息
var message = new DataUpdatedMessage(itemId, "Deleted");
WeakReferenceMessenger.Default.Send(message);
```

### 5.3 接收消息

```csharp
public partial class ListViewModel : ViewModelBase
{
    public ListViewModel()
    {
        // 注册消息
        WeakReferenceMessenger.Default.Register<DataUpdatedMessage>(this, OnDataUpdated);
    }

    private void OnDataUpdated(object recipient, DataUpdatedMessage message)
    {
        switch (message.Action)
        {
            case "Added":
                LoadNewItem(message.Id);
                break;
            case "Updated":
                RefreshItem(message.Id);
                break;
            case "Deleted":
                RemoveItem(message.Id);
                break;
        }
    }

    public override void OnUnloaded()
    {
        // 取消注册
        WeakReferenceMessenger.Default.Unregister<DataUpdatedMessage>(this);
    }
}
```

---

## 6. 依赖注入

### 6.1 注册服务（App.xaml.cs）

```csharp
private void ConfigureServices(IServiceCollection services)
{
    // Singleton：整个应用只有一个实例
    services.AddSingleton<INavigationService, NavigationService>();
    services.AddSingleton<IDataService, DataService>();

    // Transient：每次请求都创建新实例
    services.AddTransient<MainWindowViewModel>();
    services.AddTransient<LoginViewModel>();

    // Views
    services.AddTransient<MainWindow>();
    services.AddTransient<LoginWindow>();
}
```

### 6.2 获取服务

```csharp
// 通过App静态方法
public static T GetService<T>() where T : notnull
{
    return ((App)Current)._serviceProvider!.GetRequiredService<T>();
}

// 使用
var mainWindow = App.GetService<MainWindow>();
```

### 6.3 构造函数注入

```csharp
public class MainWindowViewModel
{
    private readonly INavigationService _navigationService;
    private readonly IDataService _dataService;

    // DI容器自动注入
    public MainWindowViewModel(
        INavigationService navigationService,
        IDataService dataService)
    {
        _navigationService = navigationService;
        _dataService = dataService;
    }
}
```

---

## 7. 异步编程

### 7.1 基本异步方法

```csharp
public async Task LoadDataAsync()
{
    IsBusy = true;
    try
    {
        // 异步I/O操作
        var data = await _dataService.GetDataAsync();

        // 更新UI（自动在UI线程）
        Items.Clear();
        foreach (var item in data)
        {
            Items.Add(item);
        }
    }
    catch (Exception ex)
    {
        ErrorMessage = $"加载失败：{ex.Message}";
    }
    finally
    {
        IsBusy = false;
    }
}
```

### 7.2 并行执行

```csharp
public async Task LoadMultipleDataAsync()
{
    IsBusy = true;
    try
    {
        // 并行执行多个异步操作
        var task1 = _service1.GetDataAsync();
        var task2 = _service2.GetDataAsync();
        var task3 = _service3.GetDataAsync();

        await Task.WhenAll(task1, task2, task3);

        // 使用结果
        var data1 = task1.Result;
        var data2 = task2.Result;
        var data3 = task3.Result;
    }
    finally
    {
        IsBusy = false;
    }
}
```

### 7.3 超时处理

```csharp
public async Task LoadDataWithTimeoutAsync()
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

    try
    {
        var data = await _dataService.GetDataAsync(cts.Token);
        // 处理数据
    }
    catch (OperationCanceledException)
    {
        ErrorMessage = "请求超时";
    }
}
```

---

## 8. 文件I/O

### 8.1 保存JSON

```csharp
using Newtonsoft.Json;

public async Task SaveDataAsync(List<MyModel> data)
{
    try
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        await File.WriteAllTextAsync("data.json", json);
    }
    catch (Exception ex)
    {
        ErrorMessage = $"保存失败：{ex.Message}";
    }
}
```

### 8.2 读取JSON

```csharp
public async Task<List<MyModel>> LoadDataAsync()
{
    try
    {
        if (!File.Exists("data.json"))
            return new List<MyModel>();

        var json = await File.ReadAllTextAsync("data.json");
        return JsonConvert.DeserializeObject<List<MyModel>>(json)
               ?? new List<MyModel>();
    }
    catch (Exception ex)
    {
        ErrorMessage = $"读取失败：{ex.Message}";
        return new List<MyModel>();
    }
}
```

### 8.3 文件选择对话框

```csharp
using Microsoft.Win32;

[RelayCommand]
private void SelectFile()
{
    var dialog = new OpenFileDialog
    {
        Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
        Title = "选择文件"
    };

    if (dialog.ShowDialog() == true)
    {
        FilePath = dialog.FileName;
        LoadFile(FilePath);
    }
}

[RelayCommand]
private void SaveFile()
{
    var dialog = new SaveFileDialog
    {
        Filter = "JSON files (*.json)|*.json",
        DefaultExt = ".json",
        FileName = "data.json"
    };

    if (dialog.ShowDialog() == true)
    {
        SaveToFile(dialog.FileName);
    }
}
```

---

## 9. XAML常用片段

### 9.1 窗口基本结构

```xml
<Window x:Class="MyApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:vm="clr-namespace:MyApp.ViewModels"
        Title="My Application" Height="600" Width="800"
        WindowStartupLocation="CenterScreen">

    <Window.DataContext>
        <vm:MainWindowViewModel/>
    </Window.DataContext>

    <Window.Resources>
        <BooleanToVisibilityConverter x:Key="BoolToVisConverter"/>
    </Window.Resources>

    <Grid>
        <!-- Content -->
    </Grid>
</Window>
```

### 9.2 常用布局

```xml
<!-- Grid布局 -->
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0" Text="Header"/>
    <ScrollViewer Grid.Row="1">
        <!-- Content -->
    </ScrollViewer>
    <StackPanel Grid.Row="2" Orientation="Horizontal">
        <!-- Footer -->
    </StackPanel>
</Grid>

<!-- StackPanel -->
<StackPanel Orientation="Vertical" Spacing="10">
    <TextBlock Text="Item 1"/>
    <TextBlock Text="Item 2"/>
</StackPanel>
```

### 9.3 样式定义

```xml
<Window.Resources>
    <!-- 按钮样式 -->
    <Style x:Key="PrimaryButton" TargetType="Button">
        <Setter Property="Background" Value="#007ACC"/>
        <Setter Property="Foreground" Value="White"/>
        <Setter Property="Padding" Value="16,8"/>
        <Setter Property="BorderThickness" Value="0"/>
        <Setter Property="Cursor" Value="Hand"/>
        <Style.Triggers>
            <Trigger Property="IsMouseOver" Value="True">
                <Setter Property="Background" Value="#005A9E"/>
            </Trigger>
        </Style.Triggers>
    </Style>
</Window.Resources>

<Button Content="Click Me" Style="{StaticResource PrimaryButton}"/>
```

### 9.4 数据模板

```xml
<ListBox ItemsSource="{Binding Items}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <Border Padding="10" Margin="5" Background="LightGray">
                <StackPanel>
                    <TextBlock Text="{Binding Title}" FontWeight="Bold"/>
                    <TextBlock Text="{Binding Description}" TextWrapping="Wrap"/>
                    <TextBlock Text="{Binding Date, StringFormat='yyyy-MM-dd'}"/>
                </StackPanel>
            </Border>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

---

## 10. 调试技巧

### 10.1 绑定调试

```xml
<!-- 启用绑定调试信息 -->
<TextBlock Text="{Binding Name, PresentationTraceSources.TraceLevel=High}"/>
```

查看输出窗口的绑定错误信息。

### 10.2 代码调试输出

```csharp
using System.Diagnostics;

// 输出调试信息
Debug.WriteLine($"当前值：{value}");

// 条件断言
Debug.Assert(value > 0, "值必须大于0");

// 仅在Debug模式执行
[Conditional("DEBUG")]
private void DebugLog(string message)
{
    Debug.WriteLine($"[DEBUG] {message}");
}
```

### 10.3 性能测量

```csharp
using System.Diagnostics;

var stopwatch = Stopwatch.StartNew();

// 执行操作
LoadData();

stopwatch.Stop();
Debug.WriteLine($"加载耗时：{stopwatch.ElapsedMilliseconds}ms");
```

---

## 🔖 快速查找索引

| 需求 | 章节 |
|------|------|
| 创建ViewModel | [1.2](#12-完整的viewmodel示例) |
| 数据绑定 | [2.1](#21-基本绑定) |
| 列表绑定 | [2.2](#22-集合绑定) |
| 按钮命令 | [3.1](#31-简单命令) |
| 异步命令 | [3.2](#32-异步命令) |
| 页面导航 | [4.2](#42-使用导航服务) |
| 消息发送 | [5.2](#52-发送消息) |
| 消息接收 | [5.3](#53-接收消息) |
| 注册服务 | [6.1](#61-注册服务appxamlcs) |
| 保存JSON | [8.1](#81-保存json) |
| 读取JSON | [8.2](#82-读取json) |

---

[⬅️ 返回目录](../README.md)
