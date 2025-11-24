# 附录B：知识点检查清单

> **用途**：系统化检验自己的学习成果，为面试做准备
> **使用方法**：逐条自测，标记✅已掌握 / ⚠️需强化 / ❌未掌握

---

## 📋 如何使用本清单

### 自测标准
- **✅ 已掌握**：能独立写出代码，能向他人清晰讲解原理
- **⚠️ 需强化**：理解概念，但编码时需要参考资料
- **❌ 未掌握**：概念模糊，无法独立实现

### 面试准备建议
- **必须掌握**：标注 ⭐⭐⭐ 的知识点（面试高频）
- **加分项**：标注 ⭐⭐ 的知识点（体现深度）
- **进阶内容**：标注 ⭐ 的知识点（高级岗位要求）

---

## 一、C# 语言基础（面向对象）

### 1.1 类型系统 ⭐⭐⭐

- [ ] **值类型 vs 引用类型**
  - [ ] 能准确说出两者的内存分配位置（栈/堆）
  - [ ] 能举例说明 struct 和 class 的区别
  - [ ] 能解释传参时的行为差异（值传递/引用传递）
  - 自测题：`int[] array` 是值类型还是引用类型？为什么？

- [ ] **装箱 (Boxing) 和拆箱 (Unboxing)** ⭐⭐
  - [ ] 能解释什么情况下会发生装箱
  - [ ] 能说明装箱的性能代价
  - [ ] 能改写代码避免不必要的装箱
  - 自测题：`object obj = 10;` 这行代码发生了什么？

- [ ] **可空类型 (Nullable Types)** ⭐⭐⭐
  - [ ] 理解 `int?` 的本质（`Nullable<int>`）
  - [ ] 掌握空值合并运算符 `??` 和 `??=`
  - [ ] 掌握空值条件运算符 `?.` 和 `?[]`
  - 自测题：`int? a = null; int b = a ?? 10;` b 的值是多少？

- [ ] **ref / out / in 参数修饰符** ⭐⭐
  - [ ] 能说明三者的使用场景和区别
  - [ ] 知道 ref 要求参数必须初始化
  - [ ] 知道 out 可以不初始化传入，但必须在方法内赋值
  - 自测题：什么时候使用 out 而不是返回值？

### 1.2 面向对象特性 ⭐⭐⭐

- [ ] **封装 (Encapsulation)**
  - [ ] 理解属性 (Property) vs 字段 (Field) 的区别
  - [ ] 能正确使用访问修饰符（public/private/protected/internal）
  - [ ] 理解自动属性 `{ get; set; }` 的本质
  - 自测题：为什么推荐使用属性而不是公开字段？

- [ ] **继承 (Inheritance)** ⭐⭐⭐
  - [ ] 理解 base 关键字调用基类成员
  - [ ] 理解 virtual、override、abstract 的区别
  - [ ] 理解密封类 sealed 的作用
  - 自测题：抽象类 vs 接口，何时使用哪个？

- [ ] **多态 (Polymorphism)** ⭐⭐
  - [ ] 理解方法重载 (Overload) vs 方法重写 (Override)
  - [ ] 理解运行时多态的实现原理
  - [ ] 能举例说明里氏替换原则
  - 自测题：`List<object>` 能否添加任意类型对象？为什么？

- [ ] **接口 (Interface)** ⭐⭐⭐
  - [ ] 理解接口定义契约的作用
  - [ ] 知道一个类可以实现多个接口
  - [ ] 理解显式接口实现的场景
  - 自测题：为什么 INavigationService 要定义为接口？

### 1.3 泛型 ⭐⭐⭐

- [ ] **泛型类和泛型方法**
  - [ ] 能编写和使用泛型类 `MyClass<T>`
  - [ ] 能编写和使用泛型方法 `void Method<T>(T value)`
  - [ ] 理解泛型的类型安全优势
  - 自测题：`List<int>` 和 `ArrayList` 的区别？

- [ ] **泛型约束 (Constraints)** ⭐⭐
  - [ ] 掌握 `where T : class` (引用类型约束)
  - [ ] 掌握 `where T : struct` (值类型约束)
  - [ ] 掌握 `where T : new()` (无参构造约束)
  - [ ] 掌握 `where T : BaseClass` (基类约束)
  - 自测题：`where T : IComparable<T>` 是什么意思？

- [ ] **协变和逆变** ⭐
  - [ ] 理解 `out` (协变) 的含义
  - [ ] 理解 `in` (逆变) 的含义
  - [ ] 知道 `IEnumerable<out T>` 的协变特性
  - 自测题：为什么 `List<string>` 不能赋值给 `List<object>`？

### 1.4 委托和事件 ⭐⭐⭐

- [ ] **委托 (Delegate)** ⭐⭐⭐
  - [ ] 理解委托是类型安全的函数指针
  - [ ] 能定义和使用自定义委托
  - [ ] 理解多播委托 (Multicast Delegate)
  - [ ] 掌握内置委托 `Action<T>` 和 `Func<T, TResult>`
  - 自测题：`Action` vs `Func`，何时用哪个？

- [ ] **事件 (Event)** ⭐⭐⭐
  - [ ] 理解事件是对委托的封装
  - [ ] 能定义和发布事件（`event EventHandler`）
  - [ ] 能订阅和取消订阅事件（`+=` / `-=`）
  - [ ] 理解事件只能在定义类内部触发
  - 自测题：为什么不直接用 public 委托而要用 event？

- [ ] **Lambda 表达式** ⭐⭐⭐
  - [ ] 能编写单行 Lambda `x => x * 2`
  - [ ] 能编写多行 Lambda `x => { return x * 2; }`
  - [ ] 理解闭包 (Closure) 和变量捕获
  - 自测题：Lambda 表达式捕获的是变量本身还是值？

### 1.5 异步编程 ⭐⭐⭐

- [ ] **async / await** ⭐⭐⭐
  - [ ] 理解异步方法的命名约定（`...Async`）
  - [ ] 理解 `Task` 和 `Task<T>` 的区别
  - [ ] 能正确使用 `await` 等待异步操作
  - [ ] 理解 `async void` 只能用于事件处理程序
  - 自测题：为什么 `LoginAsync` 返回 `Task` 而不是 `void`？

- [ ] **Task 和并行** ⭐⭐
  - [ ] 能使用 `Task.Run` 在后台线程执行代码
  - [ ] 能使用 `Task.WhenAll` 并行等待多个任务
  - [ ] 能使用 `Task.WhenAny` 等待任意一个任务完成
  - [ ] 理解 `ConfigureAwait(false)` 的作用
  - 自测题：WPF 中为什么 UI 更新必须在 UI 线程？

- [ ] **异常处理** ⭐⭐
  - [ ] 能在 async 方法中正确使用 try-catch
  - [ ] 理解异步方法中异常传播机制
  - [ ] 能处理 `Task` 的异常（`task.Exception`）
  - 自测题：不 await 的 Task 出现异常会怎样？

### 1.6 LINQ ⭐⭐

- [ ] **查询语法 vs 方法语法**
  - [ ] 能使用方法语法：`Where`, `Select`, `OrderBy`
  - [ ] 能使用查询语法：`from ... where ... select`
  - [ ] 理解两者的等价性和互换
  - 自测题：写出查询所有偶数并排序的 LINQ 表达式

- [ ] **常用 LINQ 方法** ⭐⭐
  - [ ] `First` / `FirstOrDefault` / `Single` / `SingleOrDefault`
  - [ ] `Any` / `All` / `Count` / `Sum` / `Average`
  - [ ] `GroupBy` / `Join` / `Distinct` / `Skip` / `Take`
  - 自测题：`First()` 和 `FirstOrDefault()` 的区别？

- [ ] **延迟执行 (Deferred Execution)** ⭐
  - [ ] 理解 LINQ 查询何时真正执行
  - [ ] 理解 `ToList()` / `ToArray()` 的立即执行
  - [ ] 知道多次枚举的性能影响
  - 自测题：以下代码执行几次查询？
    ```csharp
    var query = list.Where(x => x > 10);
    var count = query.Count();
    var first = query.First();
    ```

### 1.7 集合类型 ⭐⭐

- [ ] **常用集合** ⭐⭐⭐
  - [ ] `List<T>` - 动态数组
  - [ ] `Dictionary<TKey, TValue>` - 键值对集合
  - [ ] `HashSet<T>` - 无重复元素集合
  - [ ] `ObservableCollection<T>` - WPF 数据绑定专用
  - 自测题：为什么 WPF 绑定要用 ObservableCollection？

- [ ] **接口理解** ⭐⭐
  - [ ] `IEnumerable<T>` - 可枚举
  - [ ] `ICollection<T>` - 可计数集合
  - [ ] `IList<T>` - 可索引列表
  - [ ] `INotifyCollectionChanged` - 集合变化通知
  - 自测题：`IEnumerable` 有 `Count` 属性吗？

---

## 二、WPF 核心概念

### 2.1 XAML 基础 ⭐⭐⭐

- [ ] **XAML 语法**
  - [ ] 理解 XAML 是对象实例化的声明式语法
  - [ ] 理解属性 (Attribute) 语法：`<Button Content="Click"/>`
  - [ ] 理解属性元素 (Property Element) 语法
  - [ ] 理解附加属性 (Attached Property)：`Grid.Row="0"`
  - 自测题：以下两种写法等价吗？
    ```xml
    <Button Content="Click"/>
    <Button><Button.Content>Click</Button.Content></Button>
    ```

- [ ] **命名空间** ⭐⭐
  - [ ] 理解默认命名空间（WPF 控件）
  - [ ] 理解 `xmlns:x` 的作用（XAML 语言特性）
  - [ ] 能添加自定义命名空间引用本地类
  - 自测题：`x:Name` 和 `Name` 的区别？

- [ ] **标记扩展 (Markup Extensions)** ⭐⭐⭐
  - [ ] `{Binding}` - 数据绑定
  - [ ] `{StaticResource}` - 静态资源引用
  - [ ] `{DynamicResource}` - 动态资源引用
  - [ ] `{x:Static}` - 静态成员引用
  - 自测题：StaticResource vs DynamicResource，何时用哪个？

### 2.2 数据绑定 ⭐⭐⭐

- [ ] **绑定基础** ⭐⭐⭐
  - [ ] 理解 DataContext 的作用和继承机制
  - [ ] 理解绑定路径 (Path)：`{Binding Username}`
  - [ ] 理解绑定模式：OneWay / TwoWay / OneTime / OneWayToSource
  - [ ] 理解 UpdateSourceTrigger：PropertyChanged / LostFocus / Explicit
  - 自测题：TextBox 默认的 UpdateSourceTrigger 是什么？

- [ ] **绑定源** ⭐⭐
  - [ ] 绑定到 DataContext（默认）
  - [ ] 绑定到自身：`{Binding RelativeSource={RelativeSource Self}}`
  - [ ] 绑定到祖先元素：`RelativeSource AncestorType`
  - [ ] 绑定到静态资源：`{Binding Source={StaticResource ...}}`
  - 自测题：如何绑定到父容器的 DataContext？

- [ ] **值转换器 (Value Converter)** ⭐⭐
  - [ ] 能实现 `IValueConverter` 接口
  - [ ] 理解 `Convert` 和 `ConvertBack` 方法
  - [ ] 能在 XAML 中注册和使用转换器
  - [ ] 知道常见转换器：BoolToVisibility、StringFormat
  - 自测题：如何将布尔值转换为 Visibility？

- [ ] **数据验证** ⭐
  - [ ] 理解 `IDataErrorInfo` 接口
  - [ ] 理解 `ValidationRule` 的使用
  - [ ] 能显示验证错误信息
  - 自测题：如何在 UI 上显示验证错误？

### 2.3 命令 (Commands) ⭐⭐⭐

- [ ] **ICommand 接口** ⭐⭐⭐
  - [ ] 理解 `Execute(object parameter)` 方法
  - [ ] 理解 `CanExecute(object parameter)` 方法
  - [ ] 理解 `CanExecuteChanged` 事件
  - [ ] 能实现自定义 Command 类
  - 自测题：CanExecute 返回 false 时 Button 会怎样？

- [ ] **RelayCommand / DelegateCommand** ⭐⭐⭐
  - [ ] 理解 RelayCommand 的实现原理
  - [ ] 能使用 CommunityToolkit.Mvvm 的 `[RelayCommand]`
  - [ ] 理解命令参数 (CommandParameter)
  - [ ] 能绑定异步命令 (AsyncRelayCommand)
  - 自测题：`[RelayCommand]` 生成的命令属性名称规则？

- [ ] **内置命令** ⭐
  - [ ] 知道 ApplicationCommands（Copy、Paste、Close）
  - [ ] 知道 NavigationCommands（BrowseBack、BrowseForward）
  - [ ] 理解命令路由 (Command Routing)
  - 自测题：RoutedCommand vs ICommand 的区别？

### 2.4 样式和模板 ⭐⭐

- [ ] **样式 (Style)** ⭐⭐
  - [ ] 能定义和应用样式
  - [ ] 理解 TargetType 的作用
  - [ ] 理解样式继承 `BasedOn`
  - [ ] 理解隐式样式（无 x:Key）
  - 自测题：如何让所有 Button 应用同一个样式？

- [ ] **控件模板 (ControlTemplate)** ⭐⭐
  - [ ] 理解 ControlTemplate 重定义控件外观
  - [ ] 理解 `{TemplateBinding}` 的作用
  - [ ] 知道 ContentPresenter 和 ItemsPresenter
  - [ ] 能使用 VisualStateManager 定义状态
  - 自测题：ControlTemplate vs DataTemplate 的区别？

- [ ] **数据模板 (DataTemplate)** ⭐⭐⭐
  - [ ] 理解 DataTemplate 定义数据显示方式
  - [ ] 能为 ListBox.ItemTemplate 定义模板
  - [ ] 能使用 DataTemplateSelector 根据数据选择模板
  - [ ] 理解 HierarchicalDataTemplate（树形结构）
  - 自测题：ItemTemplate vs ItemContainerStyle 的区别？

- [ ] **资源字典 (ResourceDictionary)** ⭐⭐
  - [ ] 能定义应用程序级资源
  - [ ] 能定义窗口/页面级资源
  - [ ] 能将资源抽离到单独的 .xaml 文件
  - [ ] 能合并资源字典 (MergedDictionaries)
  - 自测题：资源查找的顺序是什么？

### 2.5 布局系统 ⭐⭐⭐

- [ ] **常用布局容器** ⭐⭐⭐
  - [ ] `Grid` - 网格布局（最常用）
  - [ ] `StackPanel` - 堆栈布局（水平/垂直）
  - [ ] `DockPanel` - 停靠布局
  - [ ] `WrapPanel` - 自动换行布局
  - [ ] `Canvas` - 绝对定位布局
  - 自测题：Grid 的 RowDefinition="Auto" 是什么意思？

- [ ] **布局属性** ⭐⭐
  - [ ] Margin - 外边距
  - [ ] Padding - 内边距
  - [ ] HorizontalAlignment / VerticalAlignment
  - [ ] Width/Height vs MinWidth/MaxWidth
  - 自测题：HorizontalAlignment="Stretch" 的作用？

### 2.6 控件 ⭐⭐

- [ ] **基本控件** ⭐⭐⭐
  - [ ] TextBlock（只读文本） vs TextBox（可编辑）
  - [ ] Button、CheckBox、RadioButton
  - [ ] ComboBox、ListBox、ListView
  - [ ] PasswordBox（密码框）
  - 自测题：为什么 PasswordBox.Password 不能绑定？

- [ ] **容器控件** ⭐⭐
  - [ ] Border、GroupBox、Expander
  - [ ] ScrollViewer、TabControl
  - [ ] Frame（页面导航容器）
  - 自测题：Frame vs ContentControl 的区别？

- [ ] **高级控件** ⭐
  - [ ] DataGrid - 数据表格
  - [ ] TreeView - 树形视图
  - [ ] Menu、ContextMenu
  - [ ] Popup、Tooltip
  - 自测题：DataGrid 的双向绑定如何实现？

---

## 三、MVVM 架构模式

### 3.1 MVVM 概念 ⭐⭐⭐

- [ ] **三层架构** ⭐⭐⭐
  - [ ] Model - 数据模型和业务逻辑
  - [ ] View - 用户界面（XAML）
  - [ ] ViewModel - 视图逻辑和数据绑定桥梁
  - [ ] 理解三者的职责分离
  - 自测题：为什么 ViewModel 不应该引用 View？

- [ ] **数据绑定的核心作用** ⭐⭐⭐
  - [ ] 理解数据绑定实现 View 和 ViewModel 解耦
  - [ ] 理解命令绑定替代事件处理
  - [ ] 理解 ViewModel 可测试性优势
  - 自测题：MVVM vs MVC 的主要区别？

### 3.2 INotifyPropertyChanged ⭐⭐⭐

- [ ] **接口实现** ⭐⭐⭐
  - [ ] 理解 `PropertyChanged` 事件的作用
  - [ ] 能手动实现 INotifyPropertyChanged
  - [ ] 理解属性 setter 中触发通知的时机
  - [ ] 能使用 CallerMemberName 简化代码
  - 自测题：为什么要判断新旧值是否相等才触发通知？

- [ ] **MVVM 工具库** ⭐⭐⭐
  - [ ] 能使用 CommunityToolkit.Mvvm
  - [ ] 理解 `ObservableObject` 基类
  - [ ] 理解 `[ObservableProperty]` 源生成器
  - [ ] 理解生成的属性命名规则（去掉下划线，首字母大写）
  - 自测题：`[ObservableProperty] private string _username;` 生成什么？

- [ ] **集合通知** ⭐⭐
  - [ ] 理解为什么要用 `ObservableCollection<T>`
  - [ ] 理解 INotifyCollectionChanged 接口
  - [ ] 知道 List 修改不会通知 UI
  - 自测题：如何让 ListBox 实时显示集合变化？

### 3.3 命令模式 ⭐⭐⭐

- [ ] **RelayCommand** ⭐⭐⭐
  - [ ] 能使用 `[RelayCommand]` 特性生成命令
  - [ ] 理解 CanExecute 方法命名规则（`Can{MethodName}`）
  - [ ] 能使用 `NotifyCanExecuteChangedFor` 刷新命令状态
  - [ ] 理解命令参数的传递和使用
  - 自测题：如何动态启用/禁用按钮？

- [ ] **异步命令** ⭐⭐⭐
  - [ ] 能使用 AsyncRelayCommand
  - [ ] 理解异步命令的执行状态管理
  - [ ] 能在执行期间禁用按钮（IsBusy 模式）
  - [ ] 能处理异步命令中的异常
  - 自测题：为什么异步命令方法要返回 Task？

### 3.4 消息传递 ⭐⭐

- [ ] **WeakReferenceMessenger** ⭐⭐
  - [ ] 理解消息传递解耦组件的作用
  - [ ] 能发送消息：`messenger.Send(new MyMessage())`
  - [ ] 能接收消息：`messenger.Register<MyMessage>(this, handler)`
  - [ ] 理解消息的作用域（默认 / 指定通道）
  - [ ] 记得在 ViewModel 销毁时取消注册
  - 自测题：为什么叫 Weak Reference（弱引用）？

- [ ] **消息定义** ⭐⭐
  - [ ] 能定义自定义消息类型
  - [ ] 能使用 ValueChangedMessage<T> 传递值
  - [ ] 理解消息继承自 `record` 的优势
  - 自测题：为什么消息类通常定义为 record？

---

## 四、依赖注入 (DI)

### 4.1 DI 基础概念 ⭐⭐⭐

- [ ] **控制反转 (IoC)** ⭐⭐⭐
  - [ ] 理解 IoC 容器管理对象创建
  - [ ] 理解依赖由容器注入而非手动 new
  - [ ] 理解 DI 是实现 IoC 的一种方式
  - 自测题：为什么要用 DI 而不是直接 new 对象？

- [ ] **构造函数注入** ⭐⭐⭐
  - [ ] 能通过构造函数声明依赖
  - [ ] 理解容器自动解析和注入依赖
  - [ ] 理解依赖链的自动构建
  - 自测题：如果 A 依赖 B，B 依赖 C，如何注册？

### 4.2 服务生命周期 ⭐⭐⭐

- [ ] **Singleton** ⭐⭐⭐
  - [ ] 理解全局唯一实例
  - [ ] 理解应用程序生命周期内只创建一次
  - [ ] 知道何时使用：共享状态、服务类
  - 自测题：NavigationService 为什么是 Singleton？

- [ ] **Transient** ⭐⭐⭐
  - [ ] 理解每次请求都创建新实例
  - [ ] 知道何时使用：无状态、ViewModel
  - [ ] 理解与 Singleton 的内存差异
  - 自测题：ViewModel 为什么是 Transient？

- [ ] **Scoped** ⭐
  - [ ] 理解作用域内单例
  - [ ] 知道在 WPF 中较少使用（主要用于 Web）
  - 自测题：Scoped 和 Singleton 的区别？

### 4.3 Microsoft.Extensions.DependencyInjection ⭐⭐⭐

- [ ] **服务注册** ⭐⭐⭐
  - [ ] 能使用 `AddSingleton<TService, TImplementation>()`
  - [ ] 能使用 `AddTransient<TService>()`
  - [ ] 理解接口注册 vs 具体类注册
  - 自测题：以下代码有什么问题？
    ```csharp
    services.AddSingleton<INavigationService>();
    ```

- [ ] **服务解析** ⭐⭐⭐
  - [ ] 能使用 `serviceProvider.GetService<T>()`
  - [ ] 能使用 `GetRequiredService<T>()`（找不到会抛异常）
  - [ ] 理解在 App.xaml.cs 中配置容器
  - 自测题：GetService vs GetRequiredService 的区别？

---

## 五、项目架构与实现

### 5.1 项目结构 ⭐⭐⭐

- [ ] **分层结构** ⭐⭐⭐
  - [ ] Views 文件夹 - 存放 XAML 视图
  - [ ] ViewModels 文件夹 - 存放视图模型
  - [ ] Models 文件夹 - 存放数据模型
  - [ ] Services 文件夹 - 存放服务类
  - [ ] Resources 文件夹 - 存放资源（样式、图片）
  - 自测题：为什么要分文件夹而不是全放在一起？

- [ ] **命名约定** ⭐⭐
  - [ ] View 名称：Login.xaml
  - [ ] ViewModel 名称：LoginViewModel.cs
  - [ ] Service 接口：INavigationService.cs
  - [ ] Service 实现：NavigationService.cs
  - 自测题：为什么服务要定义接口？

### 5.2 导航系统 ⭐⭐⭐

- [ ] **INavigationService 接口** ⭐⭐⭐
  - [ ] 理解 `NavigateTo(string pageKey)` 方法
  - [ ] 理解 `GoBack()` 方法和历史栈
  - [ ] 理解 `CurrentPage` 属性
  - [ ] 理解 `CurrentPageChanged` 事件
  - 自测题：为什么导航键用 string 而不是 Type？

- [ ] **页面注册机制** ⭐⭐⭐
  - [ ] 理解 `_pageTypes` 字典存储映射
  - [ ] 理解页面通过 DI 容器创建
  - [ ] 理解注册时同时注册到 DI 和导航字典
  - 自测题：页面实例由谁创建？

- [ ] **导航历史** ⭐⭐
  - [ ] 理解 `Stack<Page>` 存储历史
  - [ ] 理解前进时压栈，后退时出栈
  - [ ] 理解 `ClearHistory()` 的作用
  - 自测题：连续导航 A→B→C，后退一次到哪里？

### 5.3 数据持久化 ⭐⭐

- [ ] **JSON 序列化** ⭐⭐⭐
  - [ ] 能使用 `JsonConvert.SerializeObject(obj)`
  - [ ] 能使用 `JsonConvert.DeserializeObject<T>(json)`
  - [ ] 理解对象序列化为 JSON 字符串
  - [ ] 能处理序列化异常
  - 自测题：什么类型的对象不能序列化？

- [ ] **文件 I/O** ⭐⭐
  - [ ] 能使用 `File.ReadAllText(path)` 读取文件
  - [ ] 能使用 `File.WriteAllText(path, content)` 写入文件
  - [ ] 理解异步文件操作（`ReadAllTextAsync`）
  - [ ] 能处理文件不存在的情况
  - 自测题：为什么要用异步 I/O？

### 5.4 数据可视化 ⭐⭐

- [ ] **自定义控件** ⭐⭐
  - [ ] 理解 SimplePieChart 的实现原理
  - [ ] 理解 AnimatedLineChart 的绘制逻辑
  - [ ] 能绑定数据到图表控件
  - 自测题：如何让图表实时更新数据？

- [ ] **GMap.NET 地图** ⭐
  - [ ] 能添加 GMapControl 到界面
  - [ ] 能设置地图中心和缩放级别
  - [ ] 能添加标记点 (Marker)
  - 自测题：如何切换地图提供商（Google/Bing）？

---

## 六、面试准备（本项目）

### 6.1 技术栈介绍 ⭐⭐⭐

- [ ] **能清晰描述项目使用的技术栈**
  - [ ] .NET 8.0 + WPF
  - [ ] MVVM 架构模式
  - [ ] CommunityToolkit.Mvvm（MVVM 工具库）
  - [ ] Microsoft.Extensions.DependencyInjection（依赖注入）
  - [ ] GMap.NET（地图控件）
  - [ ] Newtonsoft.Json（JSON 序列化）
  - [ ] MahApps.Metro（现代化 UI）

- [ ] **能说明为什么选择这些技术**
  - [ ] 为什么用 MVVM？（可测试性、分离关注点）
  - [ ] 为什么用 DI？（解耦、可维护性）
  - [ ] 为什么用 CommunityToolkit.Mvvm？（减少样板代码、源生成器）

### 6.2 核心功能讲解 ⭐⭐⭐

- [ ] **登录系统** ⭐⭐⭐
  - [ ] 能讲解登录流程（UI → ViewModel → 验证 → 跳转）
  - [ ] 能说明数据绑定的实现（Username/Password）
  - [ ] 能说明异步命令的使用（LoginCommand）
  - [ ] 能说明 IsBusy 状态管理
  - 模拟问题：登录按钮如何在输入为空时禁用？

- [ ] **导航系统** ⭐⭐⭐
  - [ ] 能讲解导航服务的设计思路
  - [ ] 能说明页面注册机制
  - [ ] 能说明历史栈的实现
  - [ ] 能说明与 DI 的集成
  - 模拟问题：如何实现页面间传参？

- [ ] **数据展示** ⭐⭐
  - [ ] 能讲解 CRUD 操作的实现
  - [ ] 能说明 ObservableCollection 的使用
  - [ ] 能说明数据绑定到 ListBox
  - [ ] 能说明数据持久化（JSON 文件）
  - 模拟问题：如何让列表实时更新？

- [ ] **数据可视化** ⭐⭐
  - [ ] 能讲解图表控件的使用
  - [ ] 能说明数据如何绑定到图表
  - [ ] 能说明动画效果的实现思路
  - 模拟问题：如何绘制折线图？

### 6.3 遇到的问题和解决方案 ⭐⭐⭐

- [ ] **问题 1：PasswordBox 不能绑定** ⭐⭐⭐
  - [ ] 原因：PasswordBox.Password 不是依赖属性
  - [ ] 解决：使用附加属性或直接在 Code-Behind 处理
  - [ ] 反思：安全性考虑（内存中不存储密码）

- [ ] **问题 2：导航后 ViewModel 状态保留** ⭐⭐
  - [ ] 原因：ViewModel 注册为 Singleton
  - [ ] 解决：改为 Transient 每次创建新实例
  - [ ] 反思：生命周期选择的重要性

- [ ] **问题 3：集合修改不更新 UI** ⭐⭐
  - [ ] 原因：使用了 `List<T>` 而非 `ObservableCollection<T>`
  - [ ] 解决：替换为 ObservableCollection
  - [ ] 反思：WPF 数据绑定的通知机制

### 6.4 项目亮点 ⭐⭐⭐

- [ ] **能总结项目的技术亮点**
  - [ ] 严格遵循 MVVM 模式，View 和 ViewModel 完全解耦
  - [ ] 使用依赖注入，提高代码可测试性和可维护性
  - [ ] 使用源生成器减少样板代码（CommunityToolkit.Mvvm）
  - [ ] 实现了可复用的导航服务
  - [ ] 集成第三方控件（地图、图表）
  - [ ] 数据持久化（JSON 序列化）

- [ ] **能说明自己的贡献**
  - [ ] 我独立完成了 XX 模块的开发
  - [ ] 我优化了 XX 功能的性能
  - [ ] 我解决了 XX 技术难题

### 6.5 30 秒项目介绍 ⭐⭐⭐

- [ ] **能在 30 秒内介绍项目**
  > "我开发了一个基于 WPF 的数据可视化平台，主要用于设备管理和数据分析。
  > 项目采用 MVVM 架构，使用 .NET 8 和依赖注入，实现了登录、导航、数据
  > CRUD、图表展示等功能。我负责核心业务模块的开发，包括导航系统的设计
  > 和实现。项目严格遵循设计模式，代码结构清晰，可维护性强。"

---

## 七、进阶知识点

### 7.1 设计模式 ⭐⭐

- [ ] **单例模式 (Singleton)** ⭐⭐
  - [ ] 理解单例的实现方式
  - [ ] 理解 DI 容器中的 Singleton 生命周期
  - 自测题：手写一个线程安全的单例

- [ ] **工厂模式 (Factory)** ⭐
  - [ ] 理解 DI 容器本质是工厂
  - [ ] 理解 `serviceProvider.GetService<T>()` 的工厂行为
  - 自测题：工厂模式的优点？

- [ ] **观察者模式 (Observer)** ⭐⭐⭐
  - [ ] 理解 INotifyPropertyChanged 是观察者模式
  - [ ] 理解 Event 是观察者模式
  - [ ] 理解 WeakReferenceMessenger 是观察者模式
  - 自测题：观察者模式解决什么问题？

- [ ] **命令模式 (Command)** ⭐⭐⭐
  - [ ] 理解 ICommand 是命令模式
  - [ ] 理解命令封装操作和参数
  - [ ] 理解命令的撤销/重做扩展
  - 自测题：命令模式的优点？

### 7.2 高级 WPF ⭐

- [ ] **依赖属性 (Dependency Property)** ⭐
  - [ ] 理解 DP 支持样式、数据绑定、动画
  - [ ] 理解 DP 的元数据和回调
  - [ ] 能注册自定义依赖属性
  - 自测题：普通属性 vs 依赖属性的区别？

- [ ] **附加属性 (Attached Property)** ⭐
  - [ ] 理解附加属性为其他类添加属性
  - [ ] 理解 `Grid.Row` 是附加属性
  - [ ] 能定义自定义附加属性
  - 自测题：何时使用附加属性？

- [ ] **路由事件 (Routed Event)** ⭐
  - [ ] 理解冒泡 (Bubbling) 事件
  - [ ] 理解隧道 (Tunneling) 事件（Preview 前缀）
  - [ ] 理解事件处理顺序
  - 自测题：PreviewMouseDown vs MouseDown 的执行顺序？

### 7.3 性能优化 ⭐

- [ ] **虚拟化 (Virtualization)** ⭐
  - [ ] 理解 ListBox 的 UI 虚拟化
  - [ ] 知道 `VirtualizingStackPanel` 的作用
  - [ ] 知道大数据量时启用虚拟化
  - 自测题：1 万条数据的 ListBox 如何优化？

- [ ] **冻结 (Freezable)** ⭐
  - [ ] 理解 Brush、Pen 等继承自 Freezable
  - [ ] 理解 Freeze() 提高性能
  - [ ] 知道冻结后不可修改
  - 自测题：为什么要冻结资源？

- [ ] **绑定性能** ⭐
  - [ ] 避免复杂的绑定路径
  - [ ] 使用 OneWay 代替 TwoWay（如果不需要反向）
  - [ ] 避免绑定到索引器（`list[0]`）
  - 自测题：如何诊断绑定性能问题？

---

## 八、面试高频问题速查

### C# 基础 ⭐⭐⭐

- [ ] 值类型 vs 引用类型
- [ ] 装箱和拆箱
- [ ] ref vs out vs in
- [ ] struct vs class
- [ ] interface vs abstract class
- [ ] 多态的实现原理
- [ ] 委托和事件的区别
- [ ] Lambda 表达式和闭包
- [ ] async/await 原理
- [ ] LINQ 延迟执行

### WPF 基础 ⭐⭐⭐

- [ ] XAML 是什么
- [ ] 数据绑定的原理
- [ ] INotifyPropertyChanged 的作用
- [ ] Command 和 Event 的区别
- [ ] DataTemplate vs ControlTemplate
- [ ] StaticResource vs DynamicResource
- [ ] Dependency Property 的作用

### MVVM 架构 ⭐⭐⭐

- [ ] MVVM 的三层职责
- [ ] MVVM vs MVC vs MVP
- [ ] ViewModel 如何不引用 View
- [ ] ObservableCollection 的作用
- [ ] 消息传递的应用场景

### 依赖注入 ⭐⭐⭐

- [ ] 什么是依赖注入
- [ ] DI 的优点
- [ ] Singleton vs Transient vs Scoped
- [ ] 构造函数注入 vs 属性注入

---

## 🎯 面试准备检查表

### 基础必备（必须全部掌握）
- [ ] 能独立运行项目并演示功能
- [ ] 能讲解项目的技术栈和架构
- [ ] 能讲解 MVVM 模式的实现
- [ ] 能讲解至少 3 个核心功能的代码逻辑
- [ ] 能回答 50+ C# 基础面试题
- [ ] 能回答 30+ WPF 相关面试题

### 进阶加分（展现深度）
- [ ] 能讲解依赖注入的实现和优势
- [ ] 能讲解导航系统的设计思路
- [ ] 能讲解遇到的技术难题和解决方案
- [ ] 能现场编写简单的 MVVM 代码
- [ ] 能优化现有代码的性能或结构

### 实战演练（模拟面试）
- [ ] 30 秒项目介绍练习 5 遍以上
- [ ] 模拟讲解核心功能 3 次以上
- [ ] 白板编码练习（ViewModel、Command）
- [ ] 准备 3 个"你遇到的最大挑战"的故事
- [ ] 准备 5 个技术亮点的详细说明

---

## 📊 学习进度追踪

### 自我评估（每周更新）

**第 1 周**：C# 基础 ___/50 ✅  WPF 基础 ___/40 ✅

**第 2 周**：MVVM 模式 ___/30 ✅  依赖注入 ___/20 ✅

**第 3 周**：项目实战 ___/40 ✅  面试准备 ___/30 ✅

**总体掌握度**：______%

### 薄弱环节记录
1. ___________________________
2. ___________________________
3. ___________________________

### 下一步计划
- [ ] ___________________________
- [ ] ___________________________
- [ ] ___________________________

---

## 💡 使用建议

1. **每天检查 5-10 个知识点**，不要贪多
2. **每个知识点自测后标记状态**（✅ ⚠️ ❌）
3. **重点强化 ⚠️ 和 ❌ 的知识点**
4. **面试前 3 天复习所有 ⭐⭐⭐ 知识点**
5. **面试前 1 天模拟完整项目讲解 3 遍**

祝你面试成功！🎉
