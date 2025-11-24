# 第3章：MVVM 架构详解

> **本章目标**：深入理解 MVVM 设计模式及其在 WPF 中的应用
> **前置知识**：C# 基础、WPF 核心概念
> **学习时间**：4-6 天

---

## 📚 本章内容

1. [什么是 MVVM](#什么是-mvvm)
2. [MVVM 三层架构](#mvvm-三层架构)
3. [INotifyPropertyChanged 详解](#inotifypropertychanged-详解)
4. [CommunityToolkit.Mvvm](#communitytoolkitmvvm)
5. [命令模式深入](#命令模式深入)
6. [消息传递机制](#消息传递机制)
7. [MVVM vs MVC vs MVP](#mvvm-vs-mvc-vs-mvp)
8. [MVVM 最佳实践](#mvvm-最佳实践)
9. [项目中的 MVVM 应用](#项目中的-mvvm-应用)

---

## 什么是 MVVM

### MVVM 定义

**MVVM (Model-View-ViewModel)** 是一种软件架构模式，专门为 WPF/Silverlight 等支持数据绑定的UI框架设计。

**核心思想**：分离界面和业务逻辑，通过数据绑定自动同步。

### 为什么需要 MVVM？

**问题**：传统的 Code-Behind 开发

```xml
<!-- View -->
<TextBox x:Name="txtUsername" />
<Button Click="LoginButton_Click" />
```

```csharp
// Code-Behind（View 的代码）
private void LoginButton_Click(object sender, RoutedEventArgs e)
{
    string username = txtUsername.Text;  // 直接访问 UI 元素
    if (string.IsNullOrEmpty(username))
    {
        MessageBox.Show("请输入用户名");
        return;
    }

    // 业务逻辑混在 View 中
    var user = UserService.Login(username);
    if (user != null)
    {
        NavigateToMainPage();
    }
}
```

**问题**：
- ❌ View 和业务逻辑耦合
- ❌ 难以单元测试（需要创建 UI）
- ❌ 代码复用困难
- ❌ 团队协作困难（设计师和开发者冲突）

**解决方案：MVVM**

```xml
<!-- View -->
<TextBox Text="{Binding Username, UpdateSourceTrigger=PropertyChanged}" />
<Button Command="{Binding LoginCommand}" />
```

```csharp
// ViewModel（不依赖 UI）
public class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username = string.Empty;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        var user = await _userService.LoginAsync(Username);
        if (user != null)
        {
            _navigationService.NavigateTo("Main");
        }
    }

    private bool CanLogin() => !string.IsNullOrEmpty(Username);
}
```

**优势**：
- ✅ View 和逻辑完全分离
- ✅ 易于单元测试（无需 UI）
- ✅ 代码复用性强
- ✅ 支持设计时数据（设计师友好）

---

## MVVM 三层架构

### 架构图

```
┌─────────────┐
│    View     │ XAML 界面（只负责显示）
│  (XAML)     │
└──────┬──────┘
       │ 数据绑定
       │ 命令绑定
┌──────▼──────┐
│  ViewModel  │ 视图逻辑（负责 UI 状态和交互）
│   (C#)      │
└──────┬──────┘
       │ 调用
       │ 通知
┌──────▼──────┐
│    Model    │ 业务逻辑和数据（纯业务，无 UI）
│   (C#)      │
└─────────────┘
```

### View（视图）

**职责**：
- 定义 UI 结构（XAML）
- 显示数据
- 接收用户输入
- **不包含**业务逻辑

**示例**：

```xml
<Page x:Class="DataVisualizationPlatform.Views.Login">
    <Page.DataContext>
        <vm:LoginViewModel />
    </Page.DataContext>

    <Grid>
        <TextBox Text="{Binding Username}" />
        <PasswordBox x:Name="passwordBox" />
        <Button Content="登录" Command="{Binding LoginCommand}" />
    </Grid>
</Page>
```

**Code-Behind 应该有什么？**

```csharp
// Login.xaml.cs
public partial class Login : Page
{
    public Login()
    {
        InitializeComponent();  // 只有初始化代码
    }

    // ✅ 可以：纯 UI 操作（焦点、动画等）
    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ((TextBox)sender).SelectAll();
    }

    // ❌ 不应该：业务逻辑
    // private void Button_Click(object sender, RoutedEventArgs e)
    // {
    //     var result = UserService.Login(...);  // 业务逻辑
    // }
}
```

---

### ViewModel（视图模型）

**职责**：
- 管理 View 的状态（属性）
- 处理 View 的交互（命令）
- 调用 Model 执行业务逻辑
- 将 Model 数据转换为 View 需要的格式
- **不引用** View（解耦）

**示例**：

```csharp
public class LoginViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;

    // 属性：View 绑定的数据
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    // 命令：View 触发的操作
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {
            // 调用 Model 层的服务
            var user = await _userService.LoginAsync(Username);

            if (user != null)
            {
                // 导航到主页
                _navigationService.NavigateTo("Main");
            }
            else
            {
                ErrorMessage = "用户名或密码错误";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"登录失败：{ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanLogin()
    {
        return !string.IsNullOrEmpty(Username) && !IsBusy;
    }

    // 依赖注入
    public LoginViewModel(IUserService userService, INavigationService navigationService)
    {
        _userService = userService;
        _navigationService = navigationService;
    }
}
```

**ViewModel 的关键点**：
- ✅ 使用属性而非字段（支持绑定）
- ✅ 实现 INotifyPropertyChanged（属性变化通知）
- ✅ 使用命令而非事件（可测试）
- ✅ 依赖注入服务（可测试、可替换）
- ❌ 不引用 UI 元素（`Button`, `TextBox` 等）
- ❌ 不使用 `MessageBox`（改用服务或消息）

---

### Model（模型）

**职责**：
- 业务逻辑
- 数据访问
- 数据验证
- **不关心** UI

**示例**：

```csharp
// Model 层：数据模型
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }

    // 业务逻辑
    public bool IsActive()
    {
        return (DateTime.Now - CreatedAt).TotalDays < 365;
    }
}

// Model 层：服务
public interface IUserService
{
    Task<User?> LoginAsync(string username, string password);
    Task<bool> RegisterAsync(User user);
    Task<List<User>> GetAllUsersAsync();
}

public class UserService : IUserService
{
    private readonly IRepository<User> _repository;

    public async Task<User?> LoginAsync(string username, string password)
    {
        // 数据访问逻辑
        var user = await _repository.FindAsync(u =>
            u.Username == username && u.Password == HashPassword(password));

        return user;
    }

    private string HashPassword(string password)
    {
        // 密码哈希逻辑
        return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));
    }
}
```

**Model 的关键点**：
- ✅ 纯业务逻辑，无 UI 依赖
- ✅ 可独立测试
- ✅ 可在多个 ViewModel 中复用
- ❌ 不实现 INotifyPropertyChanged（除非需要）
- ❌ 不调用导航或显示对话框

---

## INotifyPropertyChanged 详解

### 为什么需要属性通知？

**问题**：普通属性不会通知 UI 更新

```csharp
public class MyViewModel
{
    public string Name { get; set; }
}
```

```xml
<TextBlock Text="{Binding Name}" />
```

```csharp
// 修改属性
viewModel.Name = "新名称";
// ❌ UI 不会更新！（因为没有通知）
```

**解决方案**：实现 INotifyPropertyChanged

```csharp
public class MyViewModel : INotifyPropertyChanged
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
                OnPropertyChanged(nameof(Name));  // ✅ 通知 UI
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
```

```csharp
// 修改属性
viewModel.Name = "新名称";
// ✅ UI 自动更新！
```

### 手动实现模式

```csharp
public class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
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
public class MyViewModel : ViewModelBase
{
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}
```

**[CallerMemberName] 的作用**：

```csharp
// 自动获取调用方法的名称
protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
{
    // propertyName 自动填充为 "Name"
}

public string Name
{
    set
    {
        SetProperty(ref _name, value);
        // 等同于：SetProperty(ref _name, value, "Name");
    }
}
```

---

## CommunityToolkit.Mvvm

### 什么是 CommunityToolkit.Mvvm？

**CommunityToolkit.Mvvm** (以前叫 MVVM Toolkit) 是微软官方的 MVVM 工具库，使用**源生成器**自动生成样板代码。

**优势**：
- ✅ 大幅减少代码量
- ✅ 编译时生成，无运行时开销
- ✅ 强类型，无魔法字符串
- ✅ 支持最新 C# 特性

### ObservableObject 基类

```csharp
using CommunityToolkit.Mvvm.ComponentModel;

public partial class MyViewModel : ObservableObject
{
    // partial 关键字必须有！（源生成器需要）
}
```

### [ObservableProperty] 特性

```csharp
public partial class LoginViewModel : ObservableObject
{
    // 字段必须是 private，以下划线开头
    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private bool _isBusy;
}

// 源生成器自动生成（你看不到，但存在）：
/*
public string Username
{
    get => _username;
    set => SetProperty(ref _username, value);
}

public string Password
{
    get => _password;
    set => SetProperty(ref _password, value);
}

public bool IsBusy
{
    get => _isBusy;
    set => SetProperty(ref _isBusy, value);
}
*/
```

**命名规则**：
- 字段：`_username`（小写，下划线开头）
- 生成的属性：`Username`（去掉下划线，首字母大写）

### [NotifyPropertyChangedFor] 特性

**场景**：一个属性变化影响另一个属性

```csharp
public partial class UserViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]  // FirstName 变化时通知 FullName
    private string _firstName = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]  // LastName 变化时通知 FullName
    private string _lastName = string.Empty;

    // 计算属性
    public string FullName => $"{FirstName} {LastName}";
}
```

```xml
<TextBox Text="{Binding FirstName}" />
<TextBox Text="{Binding LastName}" />
<TextBlock Text="{Binding FullName}" />
<!-- FirstName 或 LastName 变化时，FullName 自动更新 -->
```

### [NotifyCanExecuteChangedFor] 特性

**场景**：属性变化影响命令的可执行状态

```csharp
public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]  // Username 变化时刷新命令
    private string _username = string.Empty;

    [RelayCommand(CanExecute = nameof(CanLogin))]
    private void Login()
    {
        // 登录逻辑
    }

    private bool CanLogin() => !string.IsNullOrEmpty(Username);
}
```

**效果**：
- 用户输入用户名 → `Username` 变化
- 自动调用 `CanLogin()` 检查
- 按钮自动启用/禁用

---

## 命令模式深入

### [RelayCommand] 特性

```csharp
public partial class MyViewModel : ObservableObject
{
    // 无参数命令
    [RelayCommand]
    private void Save()
    {
        // 保存逻辑
    }
    // 生成：public IRelayCommand SaveCommand { get; }

    // 带参数命令
    [RelayCommand]
    private void Delete(Person person)
    {
        Persons.Remove(person);
    }
    // 生成：public IRelayCommand<Person> DeleteCommand { get; }

    // 异步命令
    [RelayCommand]
    private async Task LoadDataAsync()
    {
        await Task.Delay(1000);
    }
    // 生成：public IAsyncRelayCommand LoadDataCommand { get; }

    // 带 CanExecute
    [RelayCommand(CanExecute = nameof(CanSave))]
    private void SaveWithValidation()
    {
        // 保存逻辑
    }

    private bool CanSave()
    {
        return IsValid && !IsBusy;
    }
}
```

### 异步命令的特殊处理

```csharp
[RelayCommand]
private async Task LoginAsync()
{
    IsBusy = true;
    try
    {
        await _userService.LoginAsync(Username);
    }
    finally
    {
        IsBusy = false;
    }
}
```

**AsyncRelayCommand 的优势**：
- 自动管理 `IsRunning` 状态
- 防止重复执行（执行中不能再次触发）
- 支持取消操作（CancellationToken）

```csharp
[RelayCommand(IncludeCancelCommand = true)]
private async Task LoadDataAsync(CancellationToken token)
{
    for (int i = 0; i < 100; i++)
    {
        token.ThrowIfCancellationRequested();
        await Task.Delay(100, token);
    }
}
// 生成：LoadDataCommand 和 LoadDataCancelCommand
```

```xml
<Button Content="加载" Command="{Binding LoadDataCommand}" />
<Button Content="取消" Command="{Binding LoadDataCancelCommand}" />
```

---

## 消息传递机制

### 为什么需要消息传递？

**问题**：ViewModel 之间如何通信？

```csharp
// ❌ 错误：直接引用其他 ViewModel
public class ViewModelA
{
    private ViewModelB _viewModelB = new ViewModelB();

    public void DoSomething()
    {
        _viewModelB.UpdateData();  // 强耦合
    }
}
```

**解决方案**：使用消息传递（发布-订阅模式）

```csharp
// ViewModel A 发送消息
WeakReferenceMessenger.Default.Send(new DataUpdatedMessage { NewData = data });

// ViewModel B 接收消息
WeakReferenceMessenger.Default.Register<DataUpdatedMessage>(this, (recipient, message) =>
{
    // 更新 UI
    LoadData();
});
```

### WeakReferenceMessenger

```csharp
// 定义消息
public record DataUpdatedMessage
{
    public string NewData { get; init; }
}

// 发送端
public class EditorViewModel : ObservableObject
{
    private void Save()
    {
        // 保存数据...

        // 发送消息通知其他 ViewModel
        WeakReferenceMessenger.Default.Send(new DataUpdatedMessage { NewData = "已保存" });
    }
}

// 接收端
public class ListViewModel : ObservableRecipient  // 继承 ObservableRecipient
{
    public ListViewModel()
    {
        // 方式 1：手动注册
        WeakReferenceMessenger.Default.Register<DataUpdatedMessage>(this, (r, m) =>
        {
            LoadData();  // 重新加载数据
        });

        // 方式 2：使用 Receive 方法（需要继承 ObservableRecipient）
        IsActive = true;  // 激活自动注册
    }

    // 继承 ObservableRecipient 后，重写 Receive 方法
    protected override void Receive(DataUpdatedMessage message)
    {
        LoadData();
    }
}
```

**为什么叫 WeakReference（弱引用）？**
- 普通事件：订阅者会强引用发布者，导致内存泄漏
- WeakReferenceMessenger：使用弱引用，订阅者可以被 GC 回收

### 取消注册

```csharp
// 手动取消注册
WeakReferenceMessenger.Default.Unregister<DataUpdatedMessage>(this);

// 取消所有注册
WeakReferenceMessenger.Default.UnregisterAll(this);

// 使用 ObservableRecipient 时
protected override void OnDeactivated()
{
    base.OnDeactivated();
    // IsActive = false 时自动取消注册
}
```

---

## MVVM vs MVC vs MVP

### 对比表

| 特性 | MVVM | MVC | MVP |
|------|------|-----|-----|
| View 和逻辑 | 数据绑定自动同步 | 手动更新 View | Presenter 手动更新 View |
| 测试难度 | 容易（ViewModel 独立） | 中等 | 容易 |
| 代码量 | 少（自动绑定） | 多 | 中等 |
| 适用框架 | WPF, Xamarin, UWP | Web (ASP.NET MVC) | WinForms, Android |
| View 引用 | View 不知道 ViewModel | View 知道 Controller | View 知道 Presenter |

### 架构图对比

**MVVM**：
```
View ←→ ViewModel ←→ Model
     (双向绑定)    (调用)
```

**MVC**：
```
View → Controller → Model
  ↑                   ↓
  └───────────────────┘
       (通知更新)
```

**MVP**：
```
View ←→ Presenter ←→ Model
   (接口)        (调用)
```

---

## MVVM 最佳实践

### 1. ViewModel 不应该引用 View

```csharp
// ❌ 错误
public class MyViewModel
{
    private MainWindow _window;

    public void ShowDialog()
    {
        _window.ShowDialog();  // 引用 View
    }
}

// ✅ 正确：使用服务
public class MyViewModel
{
    private readonly IDialogService _dialogService;

    public void ShowDialog()
    {
        _dialogService.ShowMessage("提示", "操作成功");
    }
}
```

### 2. View 不应该有业务逻辑

```csharp
// ❌ 错误：Code-Behind 有业务逻辑
private void Button_Click(object sender, RoutedEventArgs e)
{
    if (string.IsNullOrEmpty(txtUsername.Text))
    {
        MessageBox.Show("请输入用户名");
        return;
    }

    var user = UserService.Login(txtUsername.Text);  // 业务逻辑
}

// ✅ 正确：Code-Behind 只有 UI 操作
private void TextBox_GotFocus(object sender, RoutedEventArgs e)
{
    ((TextBox)sender).SelectAll();  // 纯 UI 操作
}
```

### 3. 使用依赖注入

```csharp
// ❌ 错误：在 ViewModel 中 new 服务
public class MyViewModel
{
    private UserService _userService = new UserService();
}

// ✅ 正确：依赖注入
public class MyViewModel
{
    private readonly IUserService _userService;

    public MyViewModel(IUserService userService)
    {
        _userService = userService;
    }
}
```

### 4. 使用 ObservableCollection

```csharp
// ❌ 错误：使用 List
public List<Person> Persons { get; set; } = new();

Persons.Add(new Person());  // UI 不会更新

// ✅ 正确：使用 ObservableCollection
public ObservableCollection<Person> Persons { get; } = new();

Persons.Add(new Person());  // UI 自动更新
```

### 5. 命令而非事件

```xml
<!-- ❌ 错误：使用事件 -->
<Button Click="Button_Click" />

<!-- ✅ 正确：使用命令 -->
<Button Command="{Binding SaveCommand}" />
```

---

## 项目中的 MVVM 应用

### 项目结构

```
DataVisualizationPlatform/
├── Models/              ← Model 层
│   ├── EquipmentInfoModel.cs
│   ├── ReservationModel.cs
│   └── UserModel.cs
├── ViewModels/          ← ViewModel 层
│   ├── LoginViewModel.cs
│   ├── MainWindowViewModel.cs
│   ├── DataViewModel.cs
│   └── EditViewModel.cs
├── Views/               ← View 层
│   ├── Login.xaml
│   ├── Data.xaml
│   └── Edit.xaml
├── Services/            ← Model 层（业务逻辑）
│   ├── Navigation/
│   │   ├── INavigationService.cs
│   │   └── NavigationService.cs
│   └── Json.cs
└── App.xaml.cs          ← DI 配置
```

### 示例：LoginViewModel

```csharp
public partial class LoginViewModel : ObservableObject
{
    // Model 层服务（依赖注入）
    private readonly INavigationService _navigationService;

    // View 绑定的属性
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _username = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    // View 绑定的命令
    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {
        IsBusy = true;
        try
        {
            await Task.Delay(1500);  // 模拟网络请求（实际应调用 UserService）

            // 调用导航服务（不直接操作 Window）
            var mainWindow = App.GetService<MainWindow>();
            mainWindow?.Show();

            // 关闭登录窗口（通过消息或服务）
        }
        finally
        {
            IsBusy = false;
        }
    }

    private bool CanLogin() => !string.IsNullOrEmpty(Username) && !IsBusy;

    // 依赖注入
    public LoginViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }
}
```

---

## 本章总结

### 核心知识点 ✅

- [ ] MVVM 的三层架构和各层职责
- [ ] View 不应有业务逻辑，ViewModel 不应引用 View
- [ ] INotifyPropertyChanged 的作用和实现
- [ ] CommunityToolkit.Mvvm 的核心特性
- [ ] `[ObservableProperty]` 和 `[RelayCommand]` 的使用
- [ ] `[NotifyPropertyChangedFor]` 和 `[NotifyCanExecuteChangedFor]`
- [ ] WeakReferenceMessenger 消息传递
- [ ] MVVM vs MVC vs MVP 的区别
- [ ] MVVM 最佳实践

### 面试要点

**Q1：什么是 MVVM？为什么要用 MVVM？**

A：MVVM 是 Model-View-ViewModel 的缩写，是一种软件架构模式。它通过数据绑定实现 View 和 ViewModel 的自动同步，使界面和业务逻辑分离。优势包括：可测试性强、代码复用、团队协作友好。

**Q2：MVVM 和 MVC 有什么区别？**

A：主要区别在于 View 和逻辑的交互方式。MVVM 使用双向数据绑定自动同步，MVC 需要手动更新 View。MVVM 更适合 WPF 等支持数据绑定的框架，MVC 更适合 Web 应用。

**Q3：ViewModel 可以引用 View 吗？**

A：不可以。ViewModel 应该完全独立于 View，这样才能进行单元测试。如果需要显示对话框或导航，应该通过服务（IDialogService, INavigationService）实现。

**Q4：ObservableCollection 和 List 有什么区别？**

A：ObservableCollection 实现了 INotifyCollectionChanged 接口，当添加/删除元素时会自动通知 UI 更新。List 修改后 UI 不会更新。

---

**下一章预告**：第 4 章 - 依赖注入与服务

**继续学习！** 🚀
