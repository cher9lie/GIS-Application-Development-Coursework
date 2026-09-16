using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
// 还可以加上这个，方便之后操作
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;

namespace GISDev5
{
    public partial class Form1 : Form
    {
        // 定义点集，用于存储消防车的移动路径
        private IPointCollection m_ptCollection;

        // 定义标记元素，用于在地图上画出消防车的图标
        private IMarkerElement m_markerElement;

        // 计数器，记录当前走到第几个点了
        private int m_step = 0;

        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // ========================================================
            // 方案 C：回归 MXD 加载 (环境修复后版本)
            // ========================================================

            // 1. 设置 MXD 文件的路径
            // 请确认这个文件是你刚刚新建的、能打开的那个
            string mxdPath = System.IO.Path.Combine(Application.StartupPath, "Data", "FireResponse.mxd");

            // 检查文件是否存在
            if (!System.IO.File.Exists(mxdPath))
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Title = "选择扑火跟踪实验的 MXD 文件";
                    dialog.Filter = "ArcMap 文档 (*.mxd)|*.mxd";
                    if (dialog.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                    mxdPath = dialog.FileName;
                }
            }

            try
            {
                // 2. 加载地图文档
                axMapControl1.LoadMxFile(mxdPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("MXD 加载失败，可能是版本不兼容。\n错误：" + ex.Message);
                return;
            }

            // ========================================================
            // 3. 自动在地图里寻找“那条路”
            // ========================================================
            IFeatureLayer targetLayer = null;

            // 遍历所有图层
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                ILayer layer = axMapControl1.get_Layer(i);

                if (layer is IFeatureLayer)
                {
                    IFeatureLayer fl = layer as IFeatureLayer;

                    // 检查数据源是否有效 (是否是红色感叹号)
                    if (fl.FeatureClass != null)
                    {
                        // 检查是不是线
                        if (fl.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                        {
                            targetLayer = fl; // 找到了！
                            break; // 停止寻找，我们要的就是第一个线图层
                        }
                    }
                }
            }

            // 如果 MXD 中的数据源断开，尝试加载随项目提供的 FireLine.shp。
            if (targetLayer == null)
            {
                string dataFolder = System.IO.Path.Combine(Application.StartupPath, "Data");
                string sampleShapefile = System.IO.Path.Combine(dataFolder, "FireLine.shp");
                if (System.IO.File.Exists(sampleShapefile))
                {
                    axMapControl1.AddShapeFile(dataFolder, "FireLine.shp");
                    targetLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
                }
            }

            // ========================================================
            // 4. 提取数据
            // ========================================================
            if (targetLayer != null)
            {
                IFeatureCursor cursor = targetLayer.FeatureClass.Search(null, false);
                IFeature feature = cursor.NextFeature();

                if (feature != null)
                {
                    // 1. 先获取线的几何形状
                    IPolyline pPolyline = feature.Shape as IPolyline;

                    // 2. 【关键步骤】加密点
                    // 我们强制把这条线切分成 100 段，这样就会有 101 个点
                    // 不管原来的线有多短，这样都能保证有动画效果
                    IPolycurve pPolycurve = pPolyline as IPolycurve;
                    pPolycurve.Densify(pPolyline.Length / 100, 0);

                    // 3. 把加密后的形状赋给点集
                    m_ptCollection = pPolycurve as IPointCollection;

                    // 4. 设置一下 Timer 的速度，防止跑太快
                    // 建议设置为 50~100 毫秒
                    timer1.Interval = 50;

                    // 提示信息
                    MessageBox.Show("地图加载成功！\n" +
                                    "原始数据可能只有2个点，但我已经在内存里把它加密成了 " + m_ptCollection.PointCount + " 个点。\n" +
                                    "现在点击【扑火跟踪】应该会很流畅了！");
                }
                else
                {
                    MessageBox.Show("图层 " + targetLayer.Name + " 里是空的，没有画线！");
                }

                if (System.Runtime.InteropServices.Marshal.IsComObject(cursor))
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(cursor);
                }
            }
            else
            {
                MessageBox.Show("MXD 加载了，但在里面没找到任何有效的【线图层】。\n请确认数据源没有断开（红色感叹号）。");
            }
        }

        private void btnTrack_Click(object sender, EventArgs e)
        {
            // 1. 安全检查：确保路径数据（m_ptCollection）已经加载成功了
            if (m_ptCollection == null)
            {
                MessageBox.Show("未获取到路径数据，请检查地图加载是否正确！");
                return;
            }

            // 2. 重置计数器，每次点击都从第0个点开始跑
            m_step = 0;

            // 3. 启动计时器！
            // 计时器一旦启动，就会每隔100毫秒自动去触发 timer1_Tick 事件
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 如果没有路径数据，直接停止
            if (m_ptCollection == null) return;

            // --- 判断是否走到了终点 ---
            if (m_step < m_ptCollection.PointCount)
            {
                // 1. 获取当前这一步的目标点坐标
                IPoint currentPoint = m_ptCollection.get_Point(m_step);

                // 2. 处理地图上的图标（MarkerElement）
                if (m_markerElement == null)
                {
                    // === 情况A：第一次运行，图标还不存在，需要创建 ===

                    // A1. 创建一个标记元素
                    m_markerElement = new MarkerElementClass();

                    // A2. 创建一个符号（这里我们用一个红色的大圆点代表消防车）
                    // 如果你想用图片，需要用 IPictureMarkerSymbol，这里先用简单点防止报错
                    ISimpleMarkerSymbol simpleMarker = new SimpleMarkerSymbolClass();
                    simpleMarker.Style = esriSimpleMarkerStyle.esriSMSCircle; // 圆形
                    simpleMarker.Color = GetRgbColor(255, 0, 0); // 红色 (下面需要补一个辅助函数)
                    simpleMarker.Size = 10; // 大小

                    // A3. 把符号赋给元素
                    m_markerElement.Symbol = simpleMarker;

                    // A4. 把当前点的位置赋给元素
                    ((IElement)m_markerElement).Geometry = currentPoint;

                    // A5. 将元素添加到地图的“图形容器”中显示出来
                    // MapControl.Map 也是一个 GraphicsContainer
                    IGraphicsContainer graphicsContainer = axMapControl1.Map as IGraphicsContainer;
                    graphicsContainer.AddElement((IElement)m_markerElement, 0);
                }
                else
                {
                    // === 情况B：图标已经有了，只需要移动它的位置 ===
                    ((IElement)m_markerElement).Geometry = currentPoint;
                }

                // 3. 刷新地图，让位置变化显示出来
                // 使用 PartialRefresh 只刷新图形层，比全屏刷新不闪烁，性能更好
                axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, m_markerElement, null);

                // 4. 步数加1，准备下一次Tick走下一个点
                m_step++;
            }
            else
            {
                // --- 已经走完所有点了 ---
                timer1.Enabled = false; // 关掉计时器
                MessageBox.Show("抵达终点！");
            }
        }

        // === 这是一个辅助函数，用来生成颜色对象，放在 Form1 类里面即可 ===
        private IRgbColor GetRgbColor(int r, int g, int b)
        {
            IRgbColor rgbColor = new RgbColorClass();
            rgbColor.Red = r;
            rgbColor.Green = g;
            rgbColor.Blue = b;
            return rgbColor;
        }

        private void btnLocate_Click(object sender, EventArgs e)
        {
            // 1. 安全检查：必须先有路径数据，因为我们要用起点和终点作为瞭望台
            if (m_ptCollection == null || m_ptCollection.PointCount < 2)
            {
                MessageBox.Show("请先加载地图数据！\n我们需要路径的起点和终点作为已知点。");
                return;
            }

            // 2. 获取用户输入的角度
            double angle1, angle2;
            try
            {
                angle1 = double.Parse(txtAngle1.Text); // 瞭望台1的角度
                angle2 = double.Parse(txtAngle2.Text); // 瞭望台2的角度
            }
            catch
            {
                MessageBox.Show("输入错误！\n请输入纯数字角度（例如 45 或 60）。");
                return;
            }

            // 3. 确定已知点（起点和终点）
            // 起点作为瞭望台 A
            IPoint pPointA = m_ptCollection.get_Point(0);
            // 终点作为瞭望台 B (注意：是加密后的最后一个点)
            IPoint pPointB = m_ptCollection.get_Point(m_ptCollection.PointCount - 1);

            // 4. 【核心算法】前方交会
            // ArcEngine 的接口需要用“弧度”计算，所以要把“度”转为“弧度”
            double rad1 = angle1 * Math.PI / 180.0;
            double rad2 = angle2 * Math.PI / 180.0;

            // 创建一个空点，用来存计算结果
            IPoint pFirePoint = new PointClass();
            IConstructPoint pConstructPoint = pFirePoint as IConstructPoint;

            // 调用高级几何接口进行计算
            // 参数：点A，角度A，点B，角度B
            try
            {
                pConstructPoint.ConstructAngleIntersection(pPointA, rad1, pPointB, rad2);
            }
            catch
            {
                MessageBox.Show("计算失败！\n这两个角度的射线可能没有交点（或者是平行的）。\n请尝试调整角度数值。");
                return;
            }

            // 5. 将计算出的火点显示在地图上
            // 为了和红色的小圆点区分，我们用一个“橙色的大星形”
            ISimpleMarkerSymbol pSymbol = new SimpleMarkerSymbolClass();
            // 修改为菱形 (Diamond)
            pSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond;
            pSymbol.Color = GetRgbColor(255, 165, 0); // 橙色
            pSymbol.Size = 20; // 很大，很显眼
            pSymbol.Outline = true; // 加个边框

            // 创建元素
            IMarkerElement pElement = new MarkerElementClass();
            pElement.Symbol = pSymbol;
            ((IElement)pElement).Geometry = pFirePoint; // 放入计算出的坐标

            // 添加到地图
            IGraphicsContainer pGC = axMapControl1.Map as IGraphicsContainer;
            pGC.AddElement((IElement)pElement, 0);

            // 刷新视图
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            // 6. 弹窗显示具体坐标（实验报告里可能要填）
            MessageBox.Show("🔥 火点定位成功！\n\n" +
                            "X 坐标: " + pFirePoint.X.ToString("0.000") + "\n" +
                            "Y 坐标: " + pFirePoint.Y.ToString("0.000"));
        }

        // === 这是一个辅助函数，专门用来在地图上画一个五角星（代表火点） ===
        private void ShowFirePoint(IPoint pPoint)
        {
            // 创建一个简单的标记符号
            ISimpleMarkerSymbol pSymbol = new SimpleMarkerSymbolClass();
            pSymbol.Style = esriSimpleMarkerStyle.esriSMSDiamond; // 菱形/星形
            pSymbol.Color = GetRgbColor(255, 165, 0); // 橙色
            pSymbol.Size = 15; // 大一点，显眼

            // 创建元素
            IMarkerElement pElement = new MarkerElementClass();
            pElement.Symbol = pSymbol;
            ((IElement)pElement).Geometry = pPoint; // 别忘了强转接口

            // 添加到地图
            IGraphicsContainer pGC = axMapControl1.Map as IGraphicsContainer;

            // 清除之前的火点（可选，看你需求）
            // pGC.DeleteAllElements(); 

            pGC.AddElement((IElement)pElement, 0);

            // 刷新
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

            // 弹窗告知坐标
            MessageBox.Show("火点定位成功！\n坐标 X: " + pPoint.X.ToString("0.00") + "\n坐标 Y: " + pPoint.Y.ToString("0.00"));
        }
    }
}
