using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.Projections;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DataVisualizationPlatform.Controls
{
    public class TianDiTuMapProvider : GMapProvider
    {
        public static readonly TianDiTuMapProvider Instance;

        // 天地图API密钥（失效后需重新申请）
        private const string API_KEY = "34acc74a9c8c4e267970474c3187cb42";

        // 请求限流：最大并发请求数
        private static readonly SemaphoreSlim RequestThrottle = new SemaphoreSlim(10, 10);

        // 请求去重：记录正在请求的瓦片
        private static readonly ConcurrentDictionary<string, Task<PureImage>> PendingRequests =
            new ConcurrentDictionary<string, Task<PureImage>>();

        static TianDiTuMapProvider()
        {
            Instance = new TianDiTuMapProvider();
            GMapProviders.List.Add(Instance);
        }

        private TianDiTuMapProvider()
        {
            RefererUrl = "http://www.tianditu.gov.cn/";
            Copyright = "© 天地图";
            MaxZoom = 18;
            MinZoom = 1;
        }

        public override Guid Id
        {
            get { return new Guid("4F0C8A7B-9E3D-4A6F-8B2C-1D5E9F3A7B4C"); }
        }

        public override string Name
        {
            get { return "TianDiTu Vector"; }
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        // 返回包含标注层的数组
        public override GMapProvider[] Overlays
        {
            get { return new GMapProvider[] { this, TianDiTuAnnotationProvider.Instance }; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, string.Empty);
                string cacheKey = $"{Name}_{zoom}_{pos.X}_{pos.Y}";

                // 请求去重：如果这个瓦片正在请求中，等待已有的请求
                if (PendingRequests.TryGetValue(cacheKey, out var existingTask))
                {
                    return existingTask.Result;
                }

                // 创建新的请求任务
                var task = Task.Run(async () =>
                {
                    // 请求限流
                    if (!await RequestThrottle.WaitAsync(TimeSpan.FromSeconds(5)))
                    {
                        Debug.WriteLine($"⚠️ 请求超时被丢弃: {cacheKey}");
                        return null;
                    }

                    try
                    {
                        return GetTileImageUsingHttp(url);
                    }
                    finally
                    {
                        RequestThrottle.Release();
                    }
                });

                // 将任务加入待处理队列
                PendingRequests.TryAdd(cacheKey, task);

                // 等待结果（设置超时）
                if (task.Wait(TimeSpan.FromSeconds(10)))
                {
                    return task.Result;
                }
                else
                {
                    Debug.WriteLine($"⚠️ 瓦片加载超时: {cacheKey}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ 加载瓦片失败 [{zoom},{pos.X},{pos.Y}]: {ex.Message}");
                return null;
            }
            finally
            {
                // 清理已完成的请求
                string cacheKey = $"{Name}_{zoom}_{pos.X}_{pos.Y}";
                PendingRequests.TryRemove(cacheKey, out _);
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            int serverIndex = (int)(pos.X + pos.Y) % 8;
            return string.Format(
                "http://t{0}.tianditu.gov.cn/DataServer?T=vec_w&x={1}&y={2}&l={3}&tk={4}",
                serverIndex, pos.X, pos.Y, zoom, API_KEY);
        }
    }

    // 天地图矢量标注层（地名、道路名等）
    public class TianDiTuAnnotationProvider : GMapProvider
    {
        public static readonly TianDiTuAnnotationProvider Instance;
        private const string API_KEY = "34acc74a9c8c4e267970474c3187cb42";

        private static readonly SemaphoreSlim RequestThrottle = new SemaphoreSlim(10, 10);
        private static readonly ConcurrentDictionary<string, Task<PureImage>> PendingRequests =
            new ConcurrentDictionary<string, Task<PureImage>>();

        static TianDiTuAnnotationProvider()
        {
            Instance = new TianDiTuAnnotationProvider();
            GMapProviders.List.Add(Instance);
        }

        private TianDiTuAnnotationProvider()
        {
            RefererUrl = "http://www.tianditu.gov.cn/";
            Copyright = "© 天地图";
            MaxZoom = 18;
            MinZoom = 1;
        }

        public override Guid Id
        {
            get { return new Guid("5F1C9A8B-0E4D-5A7F-9B3C-2D6E0F4A8B5C"); }
        }

        public override string Name
        {
            get { return "TianDiTu Annotation"; }
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        public override GMapProvider[] Overlays
        {
            get { return null; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, string.Empty);
                string cacheKey = $"{Name}_{zoom}_{pos.X}_{pos.Y}";

                if (PendingRequests.TryGetValue(cacheKey, out var existingTask))
                {
                    return existingTask.Result;
                }

                var task = Task.Run(async () =>
                {
                    if (!await RequestThrottle.WaitAsync(TimeSpan.FromSeconds(5)))
                    {
                        return null;
                    }

                    try
                    {
                        return GetTileImageUsingHttp(url);
                    }
                    finally
                    {
                        RequestThrottle.Release();
                    }
                });

                PendingRequests.TryAdd(cacheKey, task);

                if (task.Wait(TimeSpan.FromSeconds(10)))
                {
                    return task.Result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ 标注层加载失败 [{zoom},{pos.X},{pos.Y}]: {ex.Message}");
                return null;
            }
            finally
            {
                string cacheKey = $"{Name}_{zoom}_{pos.X}_{pos.Y}";
                PendingRequests.TryRemove(cacheKey, out _);
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            int serverIndex = (int)(pos.X + pos.Y) % 8;
            return string.Format(
                "http://t{0}.tianditu.gov.cn/DataServer?T=cva_w&x={1}&y={2}&l={3}&tk={4}",
                serverIndex, pos.X, pos.Y, zoom, API_KEY);
        }
    }

    // 天地图影像地图（卫星图）
    public class TianDiTuSatelliteProvider : GMapProvider
    {
        public static readonly TianDiTuSatelliteProvider Instance;
        private const string API_KEY = "34acc74a9c8c4e267970474c3187cb42";

        private static readonly SemaphoreSlim RequestThrottle = new SemaphoreSlim(10, 10);
        private static readonly ConcurrentDictionary<string, Task<PureImage>> PendingRequests =
            new ConcurrentDictionary<string, Task<PureImage>>();

        static TianDiTuSatelliteProvider()
        {
            Instance = new TianDiTuSatelliteProvider();
            GMapProviders.List.Add(Instance);
        }

        private TianDiTuSatelliteProvider()
        {
            RefererUrl = "http://www.tianditu.gov.cn/";
            Copyright = "© 天地图";
            MaxZoom = 18;
            MinZoom = 1;
        }

        public override Guid Id
        {
            get { return new Guid("6F2C0A9B-1E5D-6A8F-0B4C-3D7E1F5A9B6C"); }
        }

        public override string Name
        {
            get { return "TianDiTu Satellite"; }
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        // ⭐ 卫星图也可以添加标注层
        public override GMapProvider[] Overlays
        {
            get { return new GMapProvider[] { this, TianDiTuAnnotationProvider.Instance }; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            try
            {
                string url = MakeTileImageUrl(pos, zoom, string.Empty);
                string cacheKey = $"{Name}_{zoom}_{pos.X}_{pos.Y}";

                if (PendingRequests.TryGetValue(cacheKey, out var existingTask))
                {
                    return existingTask.Result;
                }

                var task = Task.Run(async () =>
                {
                    if (!await RequestThrottle.WaitAsync(TimeSpan.FromSeconds(5)))
                    {
                        return null;
                    }

                    try
                    {
                        return GetTileImageUsingHttp(url);
                    }
                    finally
                    {
                        RequestThrottle.Release();
                    }
                });

                PendingRequests.TryAdd(cacheKey, task);

                if (task.Wait(TimeSpan.FromSeconds(10)))
                {
                    return task.Result;
                }
                else
                {
                    Debug.WriteLine($"⚠️ 卫星图加载超时: {cacheKey}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ 卫星图加载失败 [{zoom},{pos.X},{pos.Y}]: {ex.Message}");
                return null;
            }
            finally
            {
                string cacheKey = $"{Name}_{zoom}_{pos.X}_{pos.Y}";
                PendingRequests.TryRemove(cacheKey, out _);
            }
        }

        string MakeTileImageUrl(GPoint pos, int zoom, string language)
        {
            int serverIndex = (int)(pos.X + pos.Y) % 8;
            return string.Format(
                "http://t{0}.tianditu.gov.cn/DataServer?T=img_w&x={1}&y={2}&l={3}&tk={4}",
                serverIndex, pos.X, pos.Y, zoom, API_KEY);
        }
    }
}