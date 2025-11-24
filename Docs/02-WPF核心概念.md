# 第2章：WPF 核心概念

> **本章目标**：理解 WPF 的核心机制和设计思想
> **前置知识**：C# 基础（第 1 章）
> **学习时间**：5-7 天

---

## 📚 本章内容

1. [什么是 WPF](#什么是-wpf)
2. [XAML 基础](#xaml-基础)
3. [数据绑定机制](#数据绑定机制)
4. [命令系统](#命令系统)
5. [布局系统](#布局系统)
6. [样式和模板](#样式和模板)
7. [资源系统](#资源系统)
8. [依赖属性（进阶）](#依赖属性进阶)
9. [路由事件（进阶）](#路由事件进阶)

---

## 什么是 WPF

### WPF 简介

**WPF (Windows Presentation Foundation)** 是微软的桌面应用程序开发框架。

**核心特点**：
- **基于矢量图形**：使用 DirectX 渲染，缩放不失真
- **XAML 声明式 UI**：界面和逻辑分离
- **强大的数据绑定**：自动同步 UI 和数据
- **样式和模板**：高度可定制化
- **MVVM 支持**：天生支持 MVVM 架构模式

### WPF vs WinForms

| 特性 | WPF | WinForms |
|------|-----|----------|
| UI 定义 | XAML（声明式） | C# 代码（命令式） |
| 渲染引擎 | DirectX（矢量） | GDI+（像素） |
| 数据绑定 | 强大且灵活 | 基础功能 |
| 样式定制 | 非常灵活 | 有限 |
| 学习曲线 | 较陡 | 较平缓 |
| 现代化程度 | 高 | 低 |

**示例对比**：

```csharp
// WinForms（代码创建 UI）
Button button = new Button();
button.Text = "点击我";
button.Width = 100;
button.Click += (s, e) => MessageBox.Show("Hello");
this.Controls.Add(button);
```

```xml
<!-- WPF（XAML 声明 UI） -->
<Button Content="点击我"
        Width="100"
        Command="{Binding ClickCommand}" />
```

---

## XAML 基础

### 什么是 XAML？

**XAML (eXtensible Application Markup Language)** 是基于 XML 的标记语言，用于声明式创建 UI。

**本质**：XAML 是创建对象的语法糖

```xml
<!-- XAML -->
<Button Content="点击我" Width="100" Height="30" />

<!-- 等价的 C# 代码 -->
Button button = new Button
{
    Content = "点击我",
    Width = 100,
    Height = 30
};
```

### 基本语法

#### 1. 元素对应类

```xml
<Window>         <!-- System.Windows.Window 类 -->
    <Button />   <!-- System.Windows.Controls.Button 类 -->
</Window>
```

#### 2. 属性设置（两种方式）

```xml
<!-- 方式 1：Attribute 语法（简洁） -->
<Button Content="点击我" Width="100" />

<!-- 方式 2：Property Element 语法（复杂属性） -->
<Button Width="100">
    <Button.Content>
        <StackPanel>
            <TextBlock Text="第一行" />
            <TextBlock Text="第二行" />
        </StackPanel>
    </Button.Content>
</Button>
```

**何时使用 Property Element？**
- 属性值是复杂对象（如 StackPanel）
- 需要设置多行内容

#### 3. 命名空间

```xml
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="clr-namespace:MyApp.ViewModels">

    <!-- xmlns：默认命名空间（WPF 控件） -->
    <Button />

    <!-- x：XAML 语言特性 -->
    <TextBlock x:Name="myTextBlock" />

    <!-- local：自定义命名空间 -->
    <local:MyCustomControl />
</Window>
```

**命名空间解析**：

```xml
<!-- 引用当前程序集的命名空间 -->
xmlns:vm="clr-namespace:DataVisualizationPlatform.ViewModels"

<!-- 引用外部程序集 -->
xmlns:toolkit="clr-namespace:Microsoft.Toolkit.Wpf.UI;assembly=Microsoft.Toolkit.Wpf.UI"
```

#### 4. 附加属性（Attached Property）

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>

    <!-- Grid.Row 是附加属性（TextBlock 本身没有 Row 属性） -->
    <TextBlock Grid.Row="0" Text="标题" />
    <ListBox Grid.Row="1" />
</Grid>
```

**附加属性的作用**：
- 允许父元素为子元素附加额外信息
- 常见：`Grid.Row`, `Canvas.Left`, `DockPanel.Dock`

#### 5. 标记扩展（Markup Extension）

```xml
<!-- {Binding} - 数据绑定 -->
<TextBlock Text="{Binding Username}" />

<!-- {StaticResource} - 静态资源引用 -->
<Button Style="{StaticResource PrimaryButtonStyle}" />

<!-- {x:Static} - 静态成员引用 -->
<TextBlock Text="{x:Static local:Constants.AppTitle}" />

<!-- {x:Type} - 类型引用 -->
<DataTemplate DataType="{x:Type local:Person}">
    <!-- ... -->
</DataTemplate>
```

**标记扩展语法**：`{ExtensionName 参数}`

---

## 数据绑定机制

### 什么是数据绑定？

**数据绑定**：自动同步 UI 和数据源，无需手动更新。

```csharp
// ❌ 手动更新 UI（WinForms 风格）
textBox.Text = user.Name;
user.Name = textBox.Text;  // 需要在事件中写

// ✅ 数据绑定（WPF 风格）
// XAML: <TextBox Text="{Binding Name}" />
// 自动同步！
```

### DataContext

**DataContext**：数据上下文，指定数据源。

```xml
<Window>
    <Window.DataContext>
        <local:LoginViewModel />
    </Window.DataContext>

    <!-- 绑定到 DataContext.Username -->
    <TextBox Text="{Binding Username}" />
</Window>
```

**DataContext 继承**：

```xml
<Window>
    <Window.DataContext>
        <local:MainViewModel />
    </Window.DataContext>

    <Grid>
        <!-- Grid 继承 Window 的 DataContext -->
        <TextBlock Text="{Binding Title}" />

        <StackPanel>
            <!-- StackPanel 也继承 -->
            <TextBlock Text="{Binding Subtitle}" />
        </StackPanel>
    </Grid>
</Window>
```

### 绑定模式（Mode）

```xml
<!-- OneWay：单向（ViewModel → View） -->
<TextBlock Text="{Binding Status, Mode=OneWay}" />
<!-- 用途：只读显示 -->

<!-- TwoWay：双向（ViewModel ↔ View） -->
<TextBox Text="{Binding Username, Mode=TwoWay}" />
<!-- 用途：可编辑输入 -->

<!-- OneTime：一次性（初始化时绑定一次） -->
<TextBlock Text="{Binding AppVersion, Mode=OneTime}" />
<!-- 用途：静态配置 -->

<!-- OneWayToSource：反向（View → ViewModel） -->
<Slider Value="{Binding Volume, Mode=OneWayToSource}" />
<!-- 用途：特殊场景 -->
```

**默认模式**：
- `TextBox.Text` → `TwoWay`
- `TextBlock.Text` → `OneWay`
- 大多数控件 → `OneWay`

### UpdateSourceTrigger

**控制何时更新数据源**：

```xml
<!-- PropertyChanged：每次输入都更新（实时） -->
<TextBox Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}" />
<!-- 用途：实时搜索 -->

<!-- LostFocus：失去焦点时更新（默认） -->
<TextBox Text="{Binding Username, UpdateSourceTrigger=LostFocus}" />
<!-- 用途：减少更新频率 -->

<!-- Explicit：手动调用 UpdateSource() 才更新 -->
<TextBox x:Name="txtManual" Text="{Binding Name, UpdateSourceTrigger=Explicit}" />
<!-- 需要代码：txtManual.GetBindingExpression(TextBox.TextProperty).UpdateSource(); -->
```

**示例**：实时搜索 vs 按钮触发

```xml
<!-- 实时搜索 -->
<TextBox Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}" />
<!-- 用户每输入一个字符就搜索 -->

<!-- 按钮触发搜索 -->
<TextBox x:Name="searchBox" />
<Button Content="搜索" Command="{Binding SearchCommand}"
        CommandParameter="{Binding ElementName=searchBox, Path=Text}" />
<!-- 用户点击按钮才搜索 -->
```

### 绑定路径

```xml
<!-- 简单属性 -->
<TextBlock Text="{Binding Username}" />

<!-- 嵌套属性 -->
<TextBlock Text="{Binding User.Address.City}" />

<!-- 索引器 -->
<TextBlock Text="{Binding Items[0].Name}" />

<!-- 空路径（绑定整个 DataContext） -->
<ContentControl Content="{Binding}" />
```

### 相对绑定（RelativeSource）

```xml
<!-- 绑定到自身 -->
<TextBlock Text="{Binding RelativeSource={RelativeSource Self}, Path=Name}" />

<!-- 绑定到父元素 -->
<TextBlock Text="{Binding RelativeSource={RelativeSource AncestorType=Window}, Path=Title}" />

<!-- 绑定到模板中的控件 -->
<ControlTemplate TargetType="Button">
    <Border Background="{TemplateBinding Background}">
        <!-- TemplateBinding 是 RelativeSource 的简写 -->
    </Border>
</ControlTemplate>
```

**实际应用**：在 ItemTemplate 中访问父容器的 DataContext

```xml
<ListBox ItemsSource="{Binding Items}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <!-- DataContext 是单个 Item -->
            <Button Content="{Binding Name}"
                    Command="{Binding DataContext.DeleteCommand,
                              RelativeSource={RelativeSource AncestorType=ListBox}}"
                    CommandParameter="{Binding}" />
            <!-- ↑ 访问 ListBox 的 DataContext（ViewModel） -->
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

### 值转换器（Value Converter）

**用途**：在绑定时转换值

```csharp
// 定义转换器
public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isVisible)
        {
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }
        return false;
    }
}
```

```xml
<!-- 注册转换器 -->
<Window.Resources>
    <local:BoolToVisibilityConverter x:Key="BoolToVisConverter" />
</Window.Resources>

<!-- 使用转换器 -->
<TextBlock Text="加载中..."
           Visibility="{Binding IsLoading, Converter={StaticResource BoolToVisConverter}}" />
```

**常见转换器**：
- `BoolToVisibilityConverter`：bool → Visibility
- `StringFormatConverter`：格式化字符串
- `ColorToBrushConverter`：Color → Brush

---

## 命令系统

### 为什么需要命令？

**传统事件处理（不推荐）**：

```xml
<Button Click="Button_Click" />
```

```csharp
// Code-Behind
private void Button_Click(object sender, RoutedEventArgs e)
{
    // 业务逻辑在 View 中！
    // ❌ 难以测试
    // ❌ View 和逻辑耦合
}
```

**命令模式（推荐）**：

```xml
<Button Command="{Binding LoginCommand}" />
```

```csharp
// ViewModel
public ICommand LoginCommand { get; }

public LoginViewModel()
{
    LoginCommand = new RelayCommand(Login, CanLogin);
}

private void Login()
{
    // 业务逻辑在 ViewModel 中
    // ✅ 易于测试
    // ✅ View 和逻辑分离
}

private bool CanLogin()
{
    return !string.IsNullOrEmpty(Username);
}
```

### ICommand 接口

```csharp
public interface ICommand
{
    // 执行命令
    void Execute(object? parameter);

    // 判断命令是否可执行
    bool CanExecute(object? parameter);

    // CanExecute 状态变化时触发
    event EventHandler? CanExecuteChanged;
}
```

### RelayCommand 实现

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

    // 手动刷新 CanExecute
    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
```

### 命令参数（CommandParameter）

```xml
<Button Content="删除"
        Command="{Binding DeleteCommand}"
        CommandParameter="{Binding}" />
<!-- 将当前 DataContext 作为参数传递 -->
```

```csharp
// 接受参数的命令
public class RelayCommand<T> : ICommand
{
    private readonly Action<T> _execute;
    private readonly Func<T, bool>? _canExecute;

    public void Execute(object? parameter)
    {
        _execute((T)parameter);
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute == null || _canExecute((T)parameter);
    }
}

// 使用
DeleteCommand = new RelayCommand<Person>(person =>
{
    Persons.Remove(person);
});
```

### CommunityToolkit.Mvvm 的命令

```csharp
using CommunityToolkit.Mvvm.Input;

public partial class LoginViewModel : ObservableObject
{
    [RelayCommand]
    private void Login()
    {
        // 自动生成 LoginCommand 属性
    }

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        // 自动生成 LoginCommand (AsyncRelayCommand)
    }

    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(Username);
    }
}
```

---

## 布局系统

### 常用布局容器

#### 1. Grid（最常用）

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>    <!-- 自动高度 -->
        <RowDefinition Height="*"/>       <!-- 占满剩余空间 -->
        <RowDefinition Height="100"/>     <!-- 固定 100 像素 -->
        <RowDefinition Height="2*"/>      <!-- 2 倍权重 -->
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="200"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Row="0" Grid.Column="0" Text="标题" />
    <ListBox Grid.Row="1" Grid.Column="0" Grid.ColumnSpan="2" />
    <!-- ColumnSpan：跨越 2 列 -->
</Grid>
```

**Grid 的优势**：
- 最灵活的布局
- 支持复杂的行列组合
- 自适应窗口大小

#### 2. StackPanel（堆栈）

```xml
<!-- 垂直堆叠（默认） -->
<StackPanel Orientation="Vertical">
    <Button Content="按钮1" />
    <Button Content="按钮2" />
    <Button Content="按钮3" />
</StackPanel>

<!-- 水平堆叠 -->
<StackPanel Orientation="Horizontal">
    <TextBlock Text="姓名：" />
    <TextBox Width="200" />
</StackPanel>
```

**特点**：
- 按顺序排列子元素
- 不会自动换行
- 垂直时，子元素宽度自动拉伸

#### 3. DockPanel（停靠）

```xml
<DockPanel LastChildFill="True">
    <Menu DockPanel.Dock="Top" />
    <StatusBar DockPanel.Dock="Bottom" />
    <TreeView DockPanel.Dock="Left" Width="200" />
    <TextBox />  <!-- 最后一个元素填充剩余空间 -->
</DockPanel>
```

**Dock 值**：`Top`, `Bottom`, `Left`, `Right`

#### 4. WrapPanel（自动换行）

```xml
<WrapPanel Orientation="Horizontal">
    <Button Content="按钮1" Width="100" />
    <Button Content="按钮2" Width="100" />
    <Button Content="按钮3" Width="100" />
    <!-- 宽度不够时自动换行 -->
</WrapPanel>
```

#### 5. Canvas（绝对定位）

```xml
<Canvas>
    <Button Canvas.Left="50" Canvas.Top="100" Content="按钮" />
    <Ellipse Canvas.Left="200" Canvas.Top="50" Width="100" Height="100" Fill="Red" />
</Canvas>
```

**用途**：
- 绘图应用
- 需要精确定位的场景

### 布局属性

```xml
<Button Content="按钮"
        Margin="10"              <!-- 外边距 -->
        Padding="5"              <!-- 内边距 -->
        HorizontalAlignment="Left"    <!-- 水平对齐 -->
        VerticalAlignment="Top"       <!-- 垂直对齐 -->
        Width="100"              <!-- 宽度 -->
        Height="30"              <!-- 高度 -->
        MinWidth="80"            <!-- 最小宽度 -->
        MaxWidth="200" />        <!-- 最大宽度 -->
```

**Alignment 值**：
- `Left`, `Center`, `Right`, `Stretch`（水平）
- `Top`, `Center`, `Bottom`, `Stretch`（垂直）

**Margin 语法**：

```xml
<Button Margin="10" />           <!-- 上下左右都是 10 -->
<Button Margin="10,5" />         <!-- 左右 10，上下 5 -->
<Button Margin="10,5,10,5" />    <!-- 左、上、右、下 -->
```

---

## 样式和模板

### 样式（Style）

**样式**：复用一组属性设置

```xml
<!-- 定义样式 -->
<Window.Resources>
    <Style x:Key="PrimaryButtonStyle" TargetType="Button">
        <Setter Property="Background" Value="#2196F3" />
        <Setter Property="Foreground" Value="White" />
        <Setter Property="FontSize" Value="14" />
        <Setter Property="Padding" Value="15,5" />
        <Setter Property="Margin" Value="5" />
    </Style>
</Window.Resources>

<!-- 使用样式 -->
<Button Content="登录" Style="{StaticResource PrimaryButtonStyle}" />
<Button Content="注册" Style="{StaticResource PrimaryButtonStyle}" />
```

**样式继承（BasedOn）**：

```xml
<Style x:Key="BaseButtonStyle" TargetType="Button">
    <Setter Property="FontSize" Value="14" />
    <Setter Property="Padding" Value="10,5" />
</Style>

<Style x:Key="PrimaryButtonStyle" TargetType="Button" BasedOn="{StaticResource BaseButtonStyle}">
    <Setter Property="Background" Value="#2196F3" />
    <Setter Property="Foreground" Value="White" />
</Style>
```

**隐式样式（自动应用）**：

```xml
<!-- 没有 x:Key，自动应用到所有 Button -->
<Style TargetType="Button">
    <Setter Property="FontSize" Value="14" />
</Style>

<Button Content="按钮1" />  <!-- 自动应用样式 -->
<Button Content="按钮2" />  <!-- 自动应用样式 -->
```

### 触发器（Trigger）

```xml
<Style TargetType="Button">
    <Setter Property="Background" Value="White" />

    <!-- 鼠标悬停时改变背景色 -->
    <Style.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Setter Property="Background" Value="#E3F2FD" />
        </Trigger>

        <!-- 禁用时改变前景色 -->
        <Trigger Property="IsEnabled" Value="False">
            <Setter Property="Foreground" Value="Gray" />
        </Trigger>
    </Style.Triggers>
</Style>
```

**DataTrigger（基于数据触发）**：

```xml
<Style TargetType="TextBlock">
    <Style.Triggers>
        <DataTrigger Binding="{Binding Status}" Value="Online">
            <Setter Property="Foreground" Value="Green" />
        </DataTrigger>
        <DataTrigger Binding="{Binding Status}" Value="Offline">
            <Setter Property="Foreground" Value="Red" />
        </DataTrigger>
    </Style.Triggers>
</Style>
```

### 控件模板（ControlTemplate）

**ControlTemplate**：重新定义控件的视觉结构

```xml
<Style TargetType="Button">
    <Setter Property="Template">
        <Setter.Value>
            <ControlTemplate TargetType="Button">
                <Border Background="{TemplateBinding Background}"
                        CornerRadius="5"
                        Padding="{TemplateBinding Padding}">
                    <ContentPresenter HorizontalAlignment="Center"
                                    VerticalAlignment="Center" />
                </Border>
            </ControlTemplate>
        </Setter.Value>
    </Setter>
</Style>
```

**TemplateBinding**：绑定到模板化控件的属性

```xml
<Border Background="{TemplateBinding Background}"
        BorderBrush="{TemplateBinding BorderBrush}"
        BorderThickness="{TemplateBinding BorderThickness}">
    <!-- ... -->
</Border>
```

### 数据模板（DataTemplate）

**DataTemplate**：定义数据的显示方式

```xml
<ListBox ItemsSource="{Binding Persons}">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="{Binding Name}" FontWeight="Bold" />
                <TextBlock Text=", " />
                <TextBlock Text="{Binding Age}" />
                <TextBlock Text="岁" />
            </StackPanel>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

**ItemContainerStyle vs ItemTemplate**：

```xml
<ListBox ItemsSource="{Binding Items}">
    <!-- ItemContainerStyle：ListBoxItem 的样式 -->
    <ListBox.ItemContainerStyle>
        <Style TargetType="ListBoxItem">
            <Setter Property="Padding" Value="10" />
            <Setter Property="Margin" Value="5" />
        </Style>
    </ListBox.ItemContainerStyle>

    <!-- ItemTemplate：数据的显示模板 -->
    <ListBox.ItemTemplate>
        <DataTemplate>
            <TextBlock Text="{Binding Name}" />
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

---

## 资源系统

### 资源定义

```xml
<!-- 窗口级资源 -->
<Window.Resources>
    <SolidColorBrush x:Key="PrimaryBrush" Color="#2196F3" />
    <sys:Double x:Key="StandardFontSize">14</sys:Double>
</Window.Resources>

<!-- 应用程序级资源（App.xaml） -->
<Application.Resources>
    <Style x:Key="GlobalButtonStyle" TargetType="Button">
        <!-- ... -->
    </Style>
</Application.Resources>
```

### StaticResource vs DynamicResource

```xml
<!-- StaticResource：编译时解析（性能好） -->
<Button Background="{StaticResource PrimaryBrush}" />

<!-- DynamicResource：运行时解析（可动态更改） -->
<Button Background="{DynamicResource ThemeBrush}" />
```

```csharp
// 运行时更改资源
this.Resources["ThemeBrush"] = new SolidColorBrush(Colors.Red);
// DynamicResource 会自动更新，StaticResource 不会
```

### 资源字典（ResourceDictionary）

```xml
<!-- Styles/ButtonStyles.xaml -->
<ResourceDictionary xmlns="...">
    <Style x:Key="PrimaryButtonStyle" TargetType="Button">
        <!-- ... -->
    </Style>
</ResourceDictionary>
```

```xml
<!-- 合并资源字典 -->
<Window.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Styles/ButtonStyles.xaml" />
            <ResourceDictionary Source="Styles/TextBoxStyles.xaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Window.Resources>
```

---

## 依赖属性（进阶）

### 什么是依赖属性？

**依赖属性 (Dependency Property)**：WPF 增强的属性系统，支持：
- 数据绑定
- 样式设置
- 动画
- 属性值继承
- 默认值和验证

### 普通属性 vs 依赖属性

```csharp
// 普通属性
public class MyControl : Control
{
    public string Title { get; set; }  // 不支持绑定、样式等
}

// 依赖属性
public class MyControl : Control
{
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(MyControl),
            new PropertyMetadata("默认标题"));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
}
```

### 依赖属性的优势

```xml
<!-- 支持数据绑定 -->
<local:MyControl Title="{Binding PageTitle}" />

<!-- 支持样式设置 -->
<Style TargetType="local:MyControl">
    <Setter Property="Title" Value="标题" />
</Style>

<!-- 支持动画 -->
<DoubleAnimation Storyboard.TargetProperty="Width" To="200" />
```

---

## 路由事件（进阶）

### 什么是路由事件？

**路由事件**：可以在元素树中传播的事件。

**三种路由策略**：

1. **Bubbling（冒泡）**：从触发元素向上传播到根元素
2. **Tunneling（隧道）**：从根元素向下传播到触发元素
3. **Direct（直接）**：只在触发元素上触发

### 冒泡事件示例

```xml
<Window PreviewMouseDown="Window_PreviewMouseDown"  <!-- 隧道 -->
        MouseDown="Window_MouseDown">               <!-- 冒泡 -->
    <Grid PreviewMouseDown="Grid_PreviewMouseDown"
          MouseDown="Grid_MouseDown">
        <Button PreviewMouseDown="Button_PreviewMouseDown"
                MouseDown="Button_MouseDown"
                Content="点击我" />
    </Grid>
</Window>
```

**点击 Button 时的执行顺序**：

```
1. Window_PreviewMouseDown    (隧道，向下)
2. Grid_PreviewMouseDown      (隧道，向下)
3. Button_PreviewMouseDown    (隧道，到达目标)
4. Button_MouseDown           (冒泡，从目标开始)
5. Grid_MouseDown             (冒泡，向上)
6. Window_MouseDown           (冒泡，向上)
```

**事件命名约定**：
- `Preview` 前缀 → 隧道事件
- 无 `Preview` → 冒泡事件

### 停止事件传播

```csharp
private void Button_MouseDown(object sender, MouseButtonEventArgs e)
{
    e.Handled = true;  // 停止继续传播
}
```

---

## 本章总结

### 核心知识点 ✅

- [ ] WPF 的核心特点和优势
- [ ] XAML 基本语法（元素、属性、命名空间、附加属性、标记扩展）
- [ ] 数据绑定机制（DataContext、Mode、UpdateSourceTrigger、RelativeSource）
- [ ] 值转换器（IValueConverter）
- [ ] 命令系统（ICommand、RelayCommand、CommandParameter）
- [ ] 布局系统（Grid、StackPanel、DockPanel 等）
- [ ] 样式和模板（Style、ControlTemplate、DataTemplate）
- [ ] 资源系统（StaticResource、DynamicResource、ResourceDictionary）
- [ ] 依赖属性和路由事件（进阶概念）

### 实践建议

1. **创建简单项目**：用 Grid + StackPanel 创建一个登录界面
2. **练习数据绑定**：绑定 TextBox 到 ViewModel 属性
3. **使用命令**：用 RelayCommand 替代 Click 事件
4. **定义样式**：创建可复用的按钮样式
5. **转换器练习**：实现 BoolToVisibilityConverter

### 下一步

- 第 3 章：MVVM 架构详解
- 第 4 章：依赖注入与服务

---

**继续学习！** 🚀
