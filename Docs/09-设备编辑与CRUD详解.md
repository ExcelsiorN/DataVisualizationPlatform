# 第9章：设备编辑与CRUD详解（Edit）

> **本章目标**：掌握完整的 CRUD（增删改查）实现
> **涉及文件**：`Views/Edit.xaml`, `ViewModels/EditViewModel.cs`
> **核心知识**：数据绑定、命令实现、集合操作、文件读写、对象克隆

---

## 📚 功能概述

Edit 页面是一个完整的**设备管理系统**，实现了：

1. **C (Create)**：添加新设备
2. **R (Read)**：显示设备列表，支持搜索过滤
3. **U (Update)**：编辑设备信息
4. **D (Delete)**：删除设备

### 界面布局

```
┌──────────────────────────────────────────────────┐
│ 搜索设备：[________]  ➕添加 🗑️删除 💾保存      │
├──────────────────────────────────────────────────┤
│ 设备列表    │ 设备详情编辑表单                   │
│            │                                    │
│ ■ 设备A    │ 设备 ID：[fntp-1      ]           │
│   fntp-1   │ 设备名称：[设备A      ]           │
│   在线     │ 在线状态：[▼ 在线     ]           │
│            │ 部署地址：[0.0, 0.0   ]           │
│ □ 设备B    │ 运行时间：[0年0月0天  ]           │
│   fntp-2   │ ...                                │
│   离线     │                                    │
└────────────┴────────────────────────────────────┘
```

---

## ViewModel 核心逻辑

### 1. 数据结构

```csharp
public class EditViewModel : INotifyPropertyChanged
{
    // 数据源
    public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();
    public ObservableCollection<EquipmentInfoModel> FilteredEquipmentList { get; } = new();

    // 选中的设备（原始数据）
    private EquipmentInfoModel? _selectedEquipment;
    public EquipmentInfoModel? SelectedEquipment { get; set; }

    // 正在编辑的副本（UI绑定到此属性）
    private EquipmentInfoModel? _editingEquipment;
    public EquipmentInfoModel? EditingEquipment { get; set; }

    // 搜索文本
    private string _searchText = string.Empty;
    public string SearchText { get; set; }

    // 命令
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand SearchCommand { get; }
}
```

**为什么需要 SelectedEquipment 和 EditingEquipment？**

```csharp
// 问题：如果直接绑定 SelectedEquipment
<TextBox Text="{Binding SelectedEquipment.Equ_Name}" />

// 用户输入 → 立即修改原始数据 → 未保存就生效！

// 解决方案：使用副本模式
SelectedEquipment  → 原始数据（用户点击保存前不修改）
EditingEquipment   → 副本数据（UI 绑定，可随意修改）

// 用户点击保存 → 将 EditingEquipment 复制回 SelectedEquipment
```

---

### 2. 选中设备时创建副本

```csharp
public EquipmentInfoModel? SelectedEquipment
{
    get => _selectedEquipment;
    set
    {
        if (_selectedEquipment != value)
        {
            _selectedEquipment = value;
            OnPropertyChanged();

            // 关键：创建副本用于编辑
            if (_selectedEquipment != null)
            {
                EditingEquipment = _selectedEquipment.Clone();  // 克隆
            }
            else
            {
                EditingEquipment = null;
            }
        }
    }
}
```

**对象克隆实现**：

```csharp
// EquipmentInfoModel.cs
public class EquipmentInfoModel
{
    public string Equ_Id { get; set; }
    public string Equ_Name { get; set; }
    // ... 其他属性

    // 克隆方法（深拷贝）
    public EquipmentInfoModel Clone()
    {
        return new EquipmentInfoModel
        {
            Equ_Id = this.Equ_Id,
            Equ_Name = this.Equ_Name,
            Equ_OnlineStatus = this.Equ_OnlineStatus,
            // ... 复制所有属性
        };
    }

    // 从副本复制回原对象
    public void CopyFrom(EquipmentInfoModel source)
    {
        this.Equ_Id = source.Equ_Id;
        this.Equ_Name = source.Equ_Name;
        // ... 复制所有属性
    }
}
```

---

### 3. 增加（Create）

```csharp
private void AddEquipment(object? parameter)
{
    var newEquipment = new EquipmentInfoModel
    {
        Equ_Id = $"fntp-{EquipmentList.Count}",  // 生成 ID
        Equ_Name = "新设备",
        Equ_OnlineStatus = "离线",
        Equ_AvailableBookingPeriod = "预约时段配置1",
        Equ_TotalOperationTime = "0年0月0天",
        Equ_FixedDurationThisYear = "0.0小时",
        Equ_UsedFixedDurationThisYear = "0.0小时",
        Equ_UsageRateThisYear = "0.0%",
        Equ_DeploymentAddress = "0.0, 0.0"
    };

    EquipmentList.Add(newEquipment);              // 添加到完整列表
    FilteredEquipmentList.Add(newEquipment);      // 添加到筛选列表
    SelectedEquipment = newEquipment;             // 自动选中新设备
}
```

**注意事项**：
- 生成唯一 ID（这里简化为计数，实际应使用 GUID）
- 同时添加到两个集合（原始 + 筛选后）
- 自动选中新设备，方便用户立即编辑

---

### 4. 删除（Delete）

```csharp
private void DeleteEquipment(object? parameter)
{
    if (SelectedEquipment == null)
        return;

    // 弹出确认对话框
    var result = MessageBox.Show(
        $"确定要删除设备 '{SelectedEquipment.Equ_Name}' ({SelectedEquipment.Equ_Id}) 吗？\n\n注意：删除后需要点击'保存'按钮才会真正删除。",
        "确认删除",
        MessageBoxButton.YesNo,
        MessageBoxImage.Question);

    if (result == MessageBoxResult.Yes)
    {
        EquipmentList.Remove(SelectedEquipment);
        FilteredEquipmentList.Remove(SelectedEquipment);
        SelectedEquipment = null;  // 清空选中
    }
}

// CanExecute：只有选中设备时才能删除
private bool CanDeleteEquipment(object? parameter)
{
    return SelectedEquipment != null;
}
```

**CanExecute 的作用**：

```csharp
// 命令注册
DeleteCommand = new RelayCommand<object>(DeleteEquipment, CanDeleteEquipment);

// XAML 绑定
<Button Command="{Binding DeleteCommand}" />

// 当 CanDeleteEquipment 返回 false 时：
// - 按钮自动变为禁用状态（灰色）
// - 点击无效

// 何时刷新 CanExecute？
SelectedEquipment = newValue;  // 属性变化时自动刷新命令状态
```

---

### 5. 保存（Update）

```csharp
private void SaveEquipmentData(object? parameter)
{
    try
    {
        // 步骤 1：将编辑的副本应用回原始对象
        if (EditingEquipment != null && SelectedEquipment != null)
        {
            SelectedEquipment.CopyFrom(EditingEquipment);
        }

        // 步骤 2：序列化为 JSON
        var jsonString = JsonConvert.SerializeObject(EquipmentList, Formatting.Indented);

        // 步骤 3：找到 Json.cs 文件路径
        string jsonFilePath = FindJsonFilePath();
        if (string.IsNullOrEmpty(jsonFilePath))
        {
            MessageBox.Show("无法找到 Json.cs 文件！", "错误");
            return;
        }

        // 步骤 4：读取 Json.cs 文件内容
        string fileContent = File.ReadAllText(jsonFilePath, Encoding.UTF8);

        // 步骤 5：找到 _EquipmentInfo 字段的位置
        int startIndex = fileContent.IndexOf("public readonly string _EquipmentInfo = @\"");
        int contentStart = fileContent.IndexOf("@\"", startIndex) + 2;
        int contentEnd = fileContent.IndexOf("\";", contentStart);

        // 步骤 6：格式化 JSON（转义引号，添加缩进）
        string formattedJson = FormatJsonForCSharp(jsonString);

        // 步骤 7：替换内容
        string newContent = fileContent.Substring(0, contentStart) +
                          formattedJson +
                          fileContent.Substring(contentEnd);

        // 步骤 8：写回文件
        File.WriteAllText(jsonFilePath, newContent, Encoding.UTF8);

        // 步骤 9：发送消息通知其他页面数据已更新
        WeakReferenceMessenger.Default.Send(new EquipmentDataUpdatedMessage());

        MessageBox.Show("设备数据保存成功！", "成功");
    }
    catch (Exception ex)
    {
        MessageBox.Show($"保存设备数据失败: {ex.Message}", "错误");
    }
}
```

**JSON 格式化（关键）**：

```csharp
private string FormatJsonForCSharp(string jsonString)
{
    // 原始 JSON：
    // {
    //   "Equ_Id": "fntp-1",
    //   "Equ_Name": "设备A"
    // }

    var lines = jsonString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    var formattedLines = lines.Select(line =>
    {
        // 1. 添加 C# 字符串字面量的缩进
        string indentedLine = "        " + line;

        // 2. 转义引号（" → ""）
        indentedLine = indentedLine.Replace("\"", "\"\"");

        return indentedLine;
    });

    return string.Join("\r\n", formattedLines);

    // 结果（C# 字符串字面量格式）：
    //         {
    //           ""Equ_Id"": ""fntp-1"",
    //           ""Equ_Name"": ""设备A""
    //         }
}
```

**为什么要修改 Json.cs 文件？**

```csharp
// Json.cs 的结构：
public class Json
{
    public readonly string _EquipmentInfo = @"
        [
          {
            ""Equ_Id"": ""fntp-1"",
            ""Equ_Name"": ""设备A""
          }
        ]
        ";
}

// 保存逻辑：
// 1. 读取整个 Json.cs 文件
// 2. 找到 _EquipmentInfo 字段的 JSON 部分
// 3. 替换为新的 JSON 数据
// 4. 写回文件

// 优点：数据持久化到代码文件中
// 缺点：需要重新编译才能生效（生产环境应使用数据库）
```

---

### 6. 搜索（Read）

```csharp
public string SearchText
{
    get => _searchText;
    set
    {
        if (_searchText != value)
        {
            _searchText = value;
            OnPropertyChanged();
            SearchEquipment(null);  // 搜索文本变化时立即搜索
        }
    }
}

private void SearchEquipment(object? parameter)
{
    FilteredEquipmentList.Clear();

    if (string.IsNullOrWhiteSpace(SearchText))
    {
        // 搜索为空 → 显示全部
        foreach (var item in EquipmentList)
        {
            FilteredEquipmentList.Add(item);
        }
    }
    else
    {
        // 筛选包含搜索文本的设备
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
```

**实时搜索实现**：

```xml
<!-- UpdateSourceTrigger=PropertyChanged → 每次输入都触发 -->
<TextBox Text="{Binding SearchText, UpdateSourceTrigger=PropertyChanged}" />
```

```csharp
// 用户输入 "设备A"
// ↓ UpdateSourceTrigger=PropertyChanged
SearchText = "设";    // 触发搜索
SearchText = "设备";  // 触发搜索
SearchText = "设备A"; // 触发搜索

// 结果：实时显示匹配的设备
```

---

## XAML 界面要点

### 1. 主从布局

```xml
<Grid.ColumnDefinitions>
    <ColumnDefinition Width="300"/>   <!-- 左侧列表（固定宽度） -->
    <ColumnDefinition Width="*"/>     <!-- 右侧详情（自适应） -->
</Grid.ColumnDefinitions>

<!-- 左侧：设备列表 -->
<ListBox Grid.Column="0"
         ItemsSource="{Binding FilteredEquipmentList}"
         SelectedItem="{Binding SelectedEquipment}" />

<!-- 右侧：编辑表单 -->
<Grid Grid.Column="1">
    <TextBox Text="{Binding EditingEquipment.Equ_Name}" />
    <!-- 绑定到 EditingEquipment（副本） -->
</Grid>
```

### 2. 命令绑定

```xml
<Button Content="➕ 添加设备"
        Command="{Binding AddCommand}" />

<Button Content="🗑️ 删除设备"
        Command="{Binding DeleteCommand}" />
        <!-- 按钮禁用状态由 CanDeleteEquipment 控制 -->

<Button Content="💾 保存数据"
        Command="{Binding SaveCommand}" />
```

### 3. 双向绑定

```xml
<!-- Mode=TwoWay：UI → ViewModel + ViewModel → UI -->
<!-- UpdateSourceTrigger=PropertyChanged：每次输入都更新 -->
<TextBox Text="{Binding EditingEquipment.Equ_Name,
                Mode=TwoWay,
                UpdateSourceTrigger=PropertyChanged}" />
```

---

## 完整流程

### 添加设备流程

```
1. 用户点击"添加设备"按钮
   ↓
2. AddCommand 执行
   ↓
3. 创建新 EquipmentInfoModel 对象
   ↓
4. 添加到 EquipmentList 和 FilteredEquipmentList
   ↓
5. SelectedEquipment = 新设备
   ↓
6. 触发 SelectedEquipment setter
   ↓
7. EditingEquipment = 新设备.Clone()
   ↓
8. UI 自动显示编辑表单
   ↓
9. 用户修改设备信息
   ↓
10. 点击"保存数据"
   ↓
11. EditingEquipment 复制回 SelectedEquipment
   ↓
12. 序列化 EquipmentList → JSON
   ↓
13. 写入 Json.cs 文件
```

### 编辑设备流程

```
1. 用户在列表中点击设备
   ↓
2. ListBox.SelectedItem 变化
   ↓
3. SelectedEquipment = 点击的设备
   ↓
4. EditingEquipment = SelectedEquipment.Clone()
   ↓
5. UI 显示设备详情（绑定到 EditingEquipment）
   ↓
6. 用户修改信息
   ↓
7. UpdateSourceTrigger=PropertyChanged → 实时更新 EditingEquipment
   ↓
8. 用户点击"保存"
   ↓
9. SelectedEquipment.CopyFrom(EditingEquipment)
   ↓
10. 保存到文件
```

---

## 关键设计模式

### 1. 副本编辑模式

**问题**：用户修改数据后没保存就切换到其他设备

```csharp
// ❌ 直接绑定原对象
<TextBox Text="{Binding SelectedEquipment.Equ_Name}" />
// 问题：修改立即生效，无法撤销

// ✅ 使用副本
<TextBox Text="{Binding EditingEquipment.Equ_Name}" />
// 优点：
// - 修改只在副本上
// - 点击保存才应用到原对象
// - 可以实现"取消"功能
```

### 2. 主从列表模式

```
┌────────────┬──────────────┐
│ 主列表      │ 详情表单      │
│ (ListBox)  │ (Grid)       │
│            │              │
│ ■ 项目A    │ 名称：[___]  │
│ □ 项目B    │ 地址：[___]  │
│ □ 项目C    │ ...          │
└────────────┴──────────────┘
```

**实现要点**：
- 左侧列表绑定 `FilteredEquipmentList`
- 选中项绑定 `SelectedEquipment`
- 右侧表单绑定 `EditingEquipment`

### 3. 命令模式

```csharp
// 传统事件处理（Code-Behind）
private void AddButton_Click(object sender, RoutedEventArgs e)
{
    // ❌ 逻辑在 View 中，难以测试
}

// 命令模式（MVVM）
public ICommand AddCommand { get; }  // ViewModel
// ✅ 逻辑在 ViewModel 中，易于测试

// 测试代码：
var vm = new EditViewModel();
vm.AddCommand.Execute(null);
Assert.AreEqual(1, vm.EquipmentList.Count);
```

---

## 本章总结

### 核心知识点 ✅

- [ ] 完整的 CRUD 实现（增删改查）
- [ ] 对象克隆（副本编辑模式）
- [ ] 集合筛选（实时搜索）
- [ ] 命令的 CanExecute（动态启用/禁用）
- [ ] JSON 序列化和文件操作
- [ ] 消息传递（数据更新通知）
- [ ] 主从列表布局

### 面试要点

**Q1：如何防止用户未保存就修改数据？**

A：使用副本编辑模式。选中设备时创建克隆对象（EditingEquipment），UI 绑定到副本。用户点击保存时，才将副本的修改复制回原对象（SelectedEquipment）。

**Q2：搜索功能是如何实现的？**

A：维护两个集合：EquipmentList（全部数据）和 FilteredEquipmentList（筛选后）。UI 绑定 FilteredEquipmentList。搜索文本变化时，用 LINQ Where 筛选 EquipmentList，清空并重新填充 FilteredEquipmentList。

**Q3：CanExecute 有什么作用？**

A：CanExecute 控制命令是否可执行。例如删除命令只在选中设备时才可用：`CanDeleteEquipment() => SelectedEquipment != null`。WPF 会根据返回值自动启用/禁用绑定的按钮。

---

**下一章预告**：第 10 章 - 设备信息页面详解（列表展示和消息订阅）

**继续学习！** 🚀
