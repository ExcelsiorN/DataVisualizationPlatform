using CommunityToolkit.Mvvm.Input;
using DataVisualizationPlatform.Controls;
using DataVisualizationPlatform.Models;
using DataVisualizationPlatform.Services;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DataVisualizationPlatform.ViewModels
{
    public class HomePageCViewModel : INotifyPropertyChanged, IDisposable
    {
        #region 常量定义
        private const double DEFAULT_LATITUDE = 39.9042;  // 北京纬度
        private const double DEFAULT_LONGITUDE = 116.4074; // 北京经度
        private const double DEFAULT_ZOOM = 12;
        private const double FOCUSED_ZOOM = 16;
        private const double MARKER_WIDTH = 30;
        private const double MARKER_HEIGHT = 40;
        private const double MARKER_ICON_SIZE = 12;
        private const double MARKER_ICON_TOP_MARGIN = 8;
        private const double MARKER_OFFSET_X = -15;
        private const double MARKER_OFFSET_Y = -40;
        #endregion

        #region 私有字段
        private PointLatLng _position;
        private double _zoom;
        private GMapProvider _mapProvider;
        private readonly Json _jsonData = new Json();
        private GMapControl _mapControl;
        private CancellationTokenSource _loadCancellationTokenSource;
        private bool _isLoading;
        private readonly Dictionary<string, GMapMarker> _markerCache = new();
        #endregion

        #region 公共属性
        public ObservableCollection<EquipmentInfoModel> EquipmentList { get; } = new();

        public PointLatLng Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        public double Zoom
        {
            get => _zoom;
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    OnPropertyChanged(nameof(Zoom));
                }
            }
        }

        public GMapProvider MapProvider
        {
            get => _mapProvider;
            set
            {
                if (_mapProvider != value)
                {
                    _mapProvider = value;
                    OnPropertyChanged(nameof(MapProvider));
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        #endregion

        #region 命令
        public ICommand LocateCommand { get; }
        #endregion

        #region 事件
        public event EventHandler<string> ErrorOccurred;
        public event EventHandler<string> InfoMessageRequested;
        public event EventHandler<EquipmentInfoModel> EquipmentSelected;
        #endregion

        #region 构造函数
        public HomePageCViewModel()
        {
            InitializeMap();
            LocateCommand = new AsyncRelayCommand<string>(NavigateToEquipmentAsync);
            _ = LoadEquipmentAsync();
        }
        #endregion

        #region 初始化
        private void InitializeMap()
        {
            _ = TianDiTuMapProvider.Instance;
            _ = TianDiTuAnnotationProvider.Instance;
            _ = TianDiTuSatelliteProvider.Instance;

            foreach (var p in GMapProviders.List)
            {
                Debug.WriteLine($"📦 Provider 已注册: {p.Name}");
            }

            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // 使用卫星天地图
            MapProvider = TianDiTuSatelliteProvider.Instance;

            // 使用普通天地图
            // MapProvider = TianDiTuMapProvider.Instance;

            // 添加标注层
            var overlayProvider = TianDiTuAnnotationProvider.Instance;

            Position = new PointLatLng(DEFAULT_LATITUDE, DEFAULT_LONGITUDE);
            Zoom = DEFAULT_ZOOM;
            Debug.WriteLine($"✅ MapProvider 设置为: {MapProvider?.Name}");
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 设置地图控件引用，用于添加标记
        /// </summary>
        public void SetMapControl(GMapControl mapControl)
        {
            _mapControl = mapControl;
            UpdateMapMarkers();
        }
        #endregion

        #region 设备加载
        private async Task LoadEquipmentAsync()
        {
            // 取消之前的加载操作
            _loadCancellationTokenSource?.Cancel();
            _loadCancellationTokenSource?.Dispose();
            _loadCancellationTokenSource = new CancellationTokenSource();

            var cancellationToken = _loadCancellationTokenSource.Token;

            try
            {
                IsLoading = true;

                if (_jsonData?._EquipmentInfo == null)
                {
                    LogDebug("设备信息JSON数据为空");
                    return;
                }

                // 异步反序列化
                var equipment = await Task.Run(() =>
                    JsonConvert.DeserializeObject<List<EquipmentInfoModel>>(_jsonData._EquipmentInfo),
                    cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    return;

                // 更新UI集合
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    EquipmentList.Clear();
                    if (equipment != null)
                    {
                        foreach (var item in equipment.Where(e => e != null))
                        {
                            EquipmentList.Add(item);
                        }
                    }
                }, System.Windows.Threading.DispatcherPriority.Normal, cancellationToken);

                // 更新地图标记
                if (_mapControl != null)
                {
                    UpdateMapMarkers();
                }
            }
            catch (OperationCanceledException)
            {
                LogDebug("设备加载已取消");
            }
            catch (JsonException ex)
            {
                LogError("JSON反序列化失败", ex);
                RaiseError($"加载设备信息失败: 数据格式错误");
            }
            catch (Exception ex)
            {
                LogError("加载设备信息失败", ex);
                RaiseError($"加载设备信息失败: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion

        #region 地图标记管理
        /// <summary>
        /// 更新地图上的所有标记（增量更新）
        /// </summary>
        private void UpdateMapMarkers()
        {
            if (_mapControl == null || EquipmentList == null)
                return;

            try
            {
                // 获取当前设备ID集合
                var currentEquipmentIds = new HashSet<string>(
                    EquipmentList.Where(e => e?.Equ_Id != null).Select(e => e.Equ_Id));

                // 移除不再存在的标记
                var markersToRemove = _markerCache
                    .Where(kvp => !currentEquipmentIds.Contains(kvp.Key))
                    .ToList();

                foreach (var kvp in markersToRemove)
                {
                    _mapControl.Markers.Remove(kvp.Value);
                    CleanupMarker(kvp.Value);
                    _markerCache.Remove(kvp.Key);
                }

                // 添加或更新标记
                foreach (var equipment in EquipmentList)
                {
                    if (equipment == null || string.IsNullOrEmpty(equipment.Equ_Id))
                        continue;

                    if (string.IsNullOrEmpty(equipment.Equ_DeploymentAddress))
                        continue;

                    var coordinates = ParseCoordinates(equipment.Equ_DeploymentAddress);
                    if (!coordinates.HasValue)
                        continue;

                    // 如果标记已存在，更新位置；否则创建新标记
                    if (_markerCache.TryGetValue(equipment.Equ_Id, out var existingMarker))
                    {
                        existingMarker.Position = coordinates.Value;
                    }
                    else
                    {
                        var marker = CreateEquipmentMarker(coordinates.Value, equipment);
                        if (marker != null)
                        {
                            _mapControl.Markers.Add(marker);
                            _markerCache[equipment.Equ_Id] = marker;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("更新地图标记失败", ex);
            }
        }

        /// <summary>
        /// 创建设备标记
        /// </summary>
        private GMapMarker CreateEquipmentMarker(PointLatLng position, EquipmentInfoModel equipment)
        {
            var marker = new GMapMarker(position);

            // 创建标记的可视化元素
            var markerControl = CreateMarkerVisual();

            // 添加工具提示
            markerControl.ToolTip = FormatEquipmentTooltip(equipment);

            // 添加点击事件 - 使用弱事件模式避免内存泄漏
            var equipmentId = equipment.Equ_Id;
            markerControl.MouseLeftButtonUp += (sender, e) => OnMarkerClicked(equipmentId);

            marker.Shape = markerControl;
            marker.Offset = new Point(MARKER_OFFSET_X, MARKER_OFFSET_Y);
            marker.ZIndex = int.MaxValue;

            return marker;
        }

        /// <summary>
        /// 创建标记的视觉元素
        /// </summary>
        private Grid CreateMarkerVisual()
        {
            var markerControl = new Grid
            {
                Width = MARKER_WIDTH,
                Height = MARKER_HEIGHT
            };

            // 大头钉形状
            var pinPath = new Path
            {
                Width = MARKER_WIDTH,
                Height = MARKER_HEIGHT,
                Fill = new SolidColorBrush(Colors.Red),
                Stroke = new SolidColorBrush(Colors.DarkRed),
                StrokeThickness = 2,
                Data = System.Windows.Media.Geometry.Parse("M15,2 C10.029,2 6,6.029 6,11 C6,18.5 15,30 15,30 S24,18.5 24,11 C24,6.029 19.971,2 15,2 Z M15,7 C17.761,7 20,9.239 20,12 C20,14.761 17.761,17 15,17 C12.239,17 10,14.761 10,12 C10,9.239 12.239,7 15,7 Z"),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            // 设备图标
            var deviceIcon = new Ellipse
            {
                Width = MARKER_ICON_SIZE,
                Height = MARKER_ICON_SIZE,
                Fill = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, MARKER_ICON_TOP_MARGIN, 0, 0)
            };

            markerControl.Children.Add(pinPath);
            markerControl.Children.Add(deviceIcon);

            return markerControl;
        }

        /// <summary>
        /// 标记点击事件处理
        /// </summary>
        private void OnMarkerClicked(string equipmentId)
        {
            var equipment = EquipmentList.FirstOrDefault(e => e?.Equ_Id == equipmentId);
            if (equipment == null)
                return;

            var coordinates = ParseCoordinates(equipment.Equ_DeploymentAddress);
            if (coordinates.HasValue)
            {
                Position = coordinates.Value;
                Zoom = FOCUSED_ZOOM;
            }

            // 通过事件通知 View 显示详情
            EquipmentSelected?.Invoke(this, equipment);

            // 或者通过消息请求
            RaiseInfoMessage(FormatEquipmentDetails(equipment));
        }

        /// <summary>
        /// 清理标记资源
        /// </summary>
        private void CleanupMarker(GMapMarker marker)
        {
            if (marker?.Shape is Grid grid)
            {
                grid.MouseLeftButtonUp -= (sender, e) => OnMarkerClicked(null);
                grid.Children.Clear();
            }
        }
        #endregion

        #region 导航命令
        /// <summary>
        /// 导航到设备位置
        /// </summary>
        private async Task NavigateToEquipmentAsync(string deploymentAddress)
        {
            if (string.IsNullOrWhiteSpace(deploymentAddress))
            {
                RaiseError("设备位置信息无效");
                return;
            }

            try
            {
                await Task.Run(() =>
                {
                    var coordinates = ParseCoordinates(deploymentAddress);
                    if (coordinates.HasValue)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Position = coordinates.Value;
                            Zoom = FOCUSED_ZOOM;
                        });
                    }
                    else
                    {
                        RaiseError($"无法解析设备位置信息: {deploymentAddress}");
                    }
                });
            }
            catch (Exception ex)
            {
                LogError("导航到设备位置失败", ex);
                RaiseError($"跳转到设备位置时发生错误: {ex.Message}");
            }
        }
        #endregion

        #region 坐标解析
        /// <summary>
        /// 解析坐标字符串
        /// </summary>
        /// <param name="coordinateString">坐标字符串（格式：纬度,经度）</param>
        /// <returns>解析后的坐标点，如果解析失败则返回null</returns>
        private PointLatLng? ParseCoordinates(string coordinateString)
        {
            if (string.IsNullOrWhiteSpace(coordinateString))
                return null;

            try
            {
                // 移除可能的引号和多余的空格
                coordinateString = coordinateString.Trim().Trim('"', '\'');

                // 按逗号分割
                var parts = coordinateString.Split(',');

                if (parts.Length != 2)
                    return null;

                // 解析纬度和经度
                if (double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude) &&
                    double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude))
                {
                    // 验证坐标范围
                    if (IsValidCoordinate(latitude, longitude))
                    {
                        return new PointLatLng(latitude, longitude);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("解析坐标失败", ex);
            }

            return null;
        }

        /// <summary>
        /// 验证坐标是否在有效范围内
        /// </summary>
        private bool IsValidCoordinate(double latitude, double longitude)
        {
            return latitude >= -90 && latitude <= 90 && longitude >= -180 && longitude <= 180;
        }
        #endregion

        #region 格式化方法
        /// <summary>
        /// 格式化设备工具提示信息
        /// </summary>
        private string FormatEquipmentTooltip(EquipmentInfoModel equipment)
        {
            return $"{equipment?.Equ_Name ?? "未知设备"}\n" +
                   $"ID: {equipment?.Equ_Id ?? "N/A"}\n" +
                   $"地址: {equipment?.Equ_DeploymentAddress ?? "未知位置"}";
        }

        /// <summary>
        /// 格式化设备详细信息
        /// </summary>
        private string FormatEquipmentDetails(EquipmentInfoModel equipment)
        {
            return $"设备信息：\n" +
                   $"名称: {equipment?.Equ_Name ?? "未知设备"}\n" +
                   $"ID: {equipment?.Equ_Id ?? "N/A"}\n" +
                   $"位置: {equipment?.Equ_DeploymentAddress ?? "未知位置"}";
        }
        #endregion

        #region 事件触发
        private void RaiseError(string message)
        {
            ErrorOccurred?.Invoke(this, message);
        }

        private void RaiseInfoMessage(string message)
        {
            InfoMessageRequested?.Invoke(this, message);
        }
        #endregion

        #region 日志
        private void LogDebug(string message)
        {
            System.Diagnostics.Debug.WriteLine($"[HomePageCViewModel] {message}");
        }

        private void LogError(string message, Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[HomePageCViewModel] ERROR: {message}\n{ex}");
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region IDisposable
        private bool _disposed = false;

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
                // 取消所有正在进行的操作
                _loadCancellationTokenSource?.Cancel();
                _loadCancellationTokenSource?.Dispose();

                // 清理所有标记
                foreach (var marker in _markerCache.Values)
                {
                    CleanupMarker(marker);
                }
                _markerCache.Clear();

                // 清理地图控件引用
                if (_mapControl != null)
                {
                    _mapControl.Markers.Clear();
                    _mapControl = null;
                }

                // 清理集合
                EquipmentList.Clear();
            }

            _disposed = true;
        }

        ~HomePageCViewModel()
        {
            Dispose(false);
        }
        #endregion
    }
}