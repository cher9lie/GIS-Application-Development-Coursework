using System.IO;
using System;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile; 
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System.Windows.Forms;
using System.Data; // 用于创建 DataTable

namespace GISDev4
{
    public partial class Form1 : Form
    {
        string m_BasicOperationTool = "";
        public Form1()
        {
            InitializeComponent();
        }

        private ILayer _selectedLayer = null;

        private void Form1_Load(object sender, System.EventArgs e)
        {
            axTOCControl1.SetBuddyControl(axMapControl1);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "打开图层文件";
            openFileDialog1.Filter = "map documents (*.shp)|*.shp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string pFullPath = openFileDialog1.FileName;
                // 确保路径不以 \ 结尾，虽然通常不会
                string pFolder = System.IO.Path.GetDirectoryName(pFullPath);
                string pFileName = System.IO.Path.GetFileNameWithoutExtension(pFullPath);

                try
                {
                    // 【修改点】使用 ShapefileWorkspaceFactoryClass
                    // 确保 ESRI.ArcGIS.DataSourcesFile 引用的“嵌入互操作类型”为 False
                    IWorkspaceFactory pWorkspaceFactory = new ShapefileWorkspaceFactoryClass();

                    // 检查文件夹是否存在
                    if (!System.IO.Directory.Exists(pFolder))
                    {
                        MessageBox.Show("文件夹路径不存在: " + pFolder);
                        return;
                    }

                    IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(pFolder, 0);
                    IFeatureWorkspace pFeatureWorkspace = pWorkspace as IFeatureWorkspace;

                    // 打开要素类
                    IFeatureClass pFC = pFeatureWorkspace.OpenFeatureClass(pFileName);

                    IFeatureLayer pFLayer = new FeatureLayer();
                    pFLayer.FeatureClass = pFC;
                    pFLayer.Name = pFC.AliasName;

                    ILayer pLayer = pFLayer as ILayer;
                    axMapControl1.Map.AddLayer(pLayer);
                    axMapControl1.ActiveView.Refresh();
                    // 同步鹰眼地图
                    SynchronizeEagleEye();
                }
                catch (System.Runtime.InteropServices.COMException comEx)
                {
                    // 专门捕获 COM 错误，显示详细代码
                    MessageBox.Show("COM 组件错误 (0x" + comEx.ErrorCode.ToString("X") + "): " + comEx.Message +
                                    "\n\n请确认：\n1. 任务管理器中程序是 32 位。\n2. 路径中没有乱码。\n3.DataSourcesFile 引用属性 Embed Interop Types = False。");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("普通错误: " + ex.Message);
                }
            }
        }

        private void toolStripButton2_Click(object sender, System.EventArgs e)
        {
            if (axMapControl1.Map != null && axMapControl1.Map.LayerCount > 0)
            {
                //删除主地图中的第一图层
                IMap pMap = axMapControl1.Map;
                ILayer player = pMap.get_Layer(0);
                pMap.DeleteLayer(player);
                //鹰眼地图同步
                SynchronizeEagleEye();
            }
        }

        private void toolStripButton3_Click(object sender, System.EventArgs e)
        {
            m_BasicOperationTool = "isZoomFull";
            axMapControl1.Extent = axMapControl1.FullExtent;
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerZoom;
        }

        private void toolStripButton4_Click(object sender, System.EventArgs e)
        {
            m_BasicOperationTool = "isZoomIn";
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerZoomIn;
        }

        private void toolStripButton5_Click(object sender, System.EventArgs e)
        {
            m_BasicOperationTool = "isZoomOut";
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerZoomOut;
        }

        private void toolStripButton6_Click(object sender, System.EventArgs e)
        {
            m_BasicOperationTool = "isZoomPan";
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerPan;
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            IEnvelope objEnvelope = null;
IPoint pPoint = null;
IActiveView pActiveView = axMapControl1.ActiveView.FocusMap as IActiveView;
pPoint = pActiveView.ScreenDisplay.DisplayTransformation.ToMapPoint(e.x, e.y);
switch (m_BasicOperationTool)
{
    case "isZoomIn":
        objEnvelope = axMapControl1.TrackRectangle();
        axMapControl1.Extent = objEnvelope;
        break;
    case "isZoomOut":
        objEnvelope = axMapControl1.TrackRectangle();
        double mapWidth = objEnvelope.Width;
        double mapHeight = objEnvelope.Height;
        double x1 = pPoint.X - mapWidth;
        double x2 = pPoint.X + mapWidth;
        double y1 = pPoint.Y - mapHeight;
        double y2 = pPoint.Y + mapHeight;
        objEnvelope.XMax = x2 + mapWidth * 2;
        objEnvelope.XMin = x1 - mapWidth * 2;
        objEnvelope.YMax = y2 + mapHeight * 2;
        objEnvelope.YMin = y1 - mapHeight * 2;
        axMapControl1.Extent = objEnvelope;
        break;
    case "isZoomPan":
        axMapControl1.Pan();
        break;
}
        }

        private void toolStripButton7_Click(object sender, System.EventArgs e)
        {
            // 1. 检查地图是否为空
            if (axMapControl1.Map.LayerCount == 0) return;

            // 2. 获取第一个图层 (假设我们要查的是北京区县图层，它通常在最上面)
            IMap pMap = axMapControl1.Map;
            ILayer pLayer = pMap.get_Layer(0);

            // 3. 转换为要素选择接口
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            if (pFeatureLayer == null) return; // 如果不是矢量图层就退出

            IFeatureSelection pFeatureSelection = pFeatureLayer as IFeatureSelection;

            // 4. 构建查询过滤器
            IQueryFilter pQuery = new QueryFilterClass();
            // 【注意】这里假设你的shp文件里有一个叫 "NAME" 的字段
            // 如果查询没反应，可能是字段名不对（比如叫 "Name" 或 "名称"）
            pQuery.WhereClause = "NAME = '" + toolStripTextBox1.Text + "'";

            // 5. 执行选择 (高亮显示)
            // esriSelectionResultNew 表示创建一个新的选择集
            pFeatureSelection.SelectFeatures(pQuery, esriSelectionResultEnum.esriSelectionResultNew, false);

            // 6. 刷新视图 (如果不刷新，高亮不会立即显示)
            axMapControl1.ActiveView.Refresh();
        }

        private void toolStripButton8_Click(object sender, System.EventArgs e)
        {
            axMapControl1.CurrentTool = null;
            ControlsSelectFeaturesTool pTool = new ControlsSelectFeaturesToolClass();
            pTool.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pTool as ITool;
        }
        // 辅助函数1：获取RGB颜色
        private IRgbColor GetRgbColor(int intR, int intG, int intB)
        {
            IRgbColor pRgbColor = null;
            if (intR < 0 || intR > 255 || intG < 0 || intG > 255 || intB < 0 || intB > 255)
            {
                return pRgbColor;
            }
            pRgbColor = new RgbColorClass();
            pRgbColor.Red = intR;
            pRgbColor.Green = intG;
            pRgbColor.Blue = intB;
            return pRgbColor;
        }

        // 辅助函数2：在鹰眼地图上画矩形框
        private void DrawRectangle(IEnvelope pEnvelope)
        {
            //在绘制前,清除鹰眼中之前绘制的矩形框
            IGraphicsContainer pGraphicsContainer = axMapControl2.Map as IGraphicsContainer;
            IActiveView pActiveView = pGraphicsContainer as IActiveView;
            pGraphicsContainer.DeleteAllElements();
            //得到当前视图范围
            IRectangleElement pRectangleElement = new RectangleElementClass();
            IElement pElement = pRectangleElement as IElement;
            pElement.Geometry = pEnvelope;
            //设置矩形框(实质为中间透明度面)
            IRgbColor pColor = new RgbColorClass();
            pColor = GetRgbColor(255, 0, 0);
            pColor.Transparency = 255;
            ILineSymbol pOutLine = new SimpleLineSymbolClass();
            pOutLine.Width = 2;
            pOutLine.Color = pColor;
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pColor = new RgbColorClass();
            pColor.Transparency = 0;
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutLine;
            //向鹰眼中添加矩形框
            IFillShapeElement pFillShapeElement = pElement as IFillShapeElement;
            pFillShapeElement.Symbol = pFillSymbol;
            pGraphicsContainer.AddElement((IElement)pFillShapeElement, 0);
            //刷新
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        // 辅助函数3：鹰眼窗口同步函数
        private void SynchronizeEagleEye()
        {
            if (axMapControl2.LayerCount > 0)
            {
                axMapControl2.ClearLayers();
            }
            //设置鹰眼和主地图的坐标系统一致
            axMapControl2.SpatialReference = axMapControl1.SpatialReference;
            for (int i = axMapControl1.LayerCount - 1; i >= 0; i--)
            {
                //使鹰眼视图与数据视图的图层上下顺序保持一致
                ILayer player = axMapControl1.get_Layer(i);
                axMapControl2.AddLayer(player);
            }
            //设置鹰眼地图全图显示
            axMapControl2.Extent = axMapControl1.FullExtent;
            IEnvelope pEnv = axMapControl1.Extent as IEnvelope;
            DrawRectangle(pEnv);
            axMapControl2.ActiveView.Refresh();
        }

        private void axMapControl1_OnExtentUpdated(object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            IEnvelope pEnvelope = e.newEnvelope as IEnvelope;
            DrawRectangle(pEnvelope);
        }

        private void axMapControl2_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            if (e.button == 2) // 右键
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;

                // 判断点到了什么
                axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

                // 如果点到的是图层
                if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    _selectedLayer = layer; // 记录下来！
                    contextMenuStrip1.Show(axTOCControl1, e.x, e.y); // 弹出菜单
                }
            }
        }

        // 【核心功能】将 ArcGIS 的要素图层转换为 C# 的 DataTable
        public static DataTable CreateDataTable(ILayer pLayer)
        {
            DataTable pDataTable = new DataTable();

            // 1. 检查是否为要素图层
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            if (pFeatureLayer == null) return null;

            IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;
            if (pFeatureClass == null) return null;

            // 2. 创建字段 (列)
            // 遍历图层的所有字段，在 DataTable 中创建对应的列
            for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
            {
                IField pField = pFeatureClass.Fields.get_Field(i);
                // 排除掉几何字段 (Shape)，我们只看属性
                if (pField.Type != esriFieldType.esriFieldTypeGeometry)
                {
                    DataColumn pDataColumn = new DataColumn(pField.Name);
                    // 这里简单处理，统一转为 String 显示，防止类型冲突
                    pDataColumn.DataType = typeof(string);
                    pDataTable.Columns.Add(pDataColumn);
                }
            }

            // 3. 填充数据 (行)
            // 使用游标 (Cursor) 遍历所有要素
            IFeatureCursor pFeatureCursor = pFeatureClass.Search(null, false);
            IFeature pFeature = pFeatureCursor.NextFeature();

            while (pFeature != null)
            {
                DataRow pDataRow = pDataTable.NewRow();

                // 遍历该要素的每一个字段
                for (int i = 0; i < pFeatureClass.Fields.FieldCount; i++)
                {
                    IField pField = pFeatureClass.Fields.get_Field(i);
                    // 同样排除几何字段
                    if (pField.Type != esriFieldType.esriFieldTypeGeometry)
                    {
                        // 获取字段值并转为字符串
                        object value = pFeature.get_Value(i);
                        pDataRow[pField.Name] = (value == null) ? "" : value.ToString();
                    }
                }

                pDataTable.Rows.Add(pDataRow);
                pFeature = pFeatureCursor.NextFeature(); // 移动到下一条
            }

            // 4. 释放游标 (非常重要，否则会锁死文件)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(pFeatureCursor);

            return pDataTable;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // _selectedLayer 是你在做“删除图层”时定义的全局变量
            // 用来记录右键到底点到了哪个图层
            if (_selectedLayer == null)
            {
                MessageBox.Show("请先右键选择一个图层！");
                return;
            }

            try
            {
                // 1. 调用刚才写的转换函数
                DataTable dt = CreateDataTable(_selectedLayer);

                if (dt != null)
                {
                    // 2. 创建并显示属性表窗口
                    Form2 frmAttribute = new Form2();
                    frmAttribute.Text = "属性表: " + _selectedLayer.Name; // 设置窗口标题
                    frmAttribute.LoadDataTable(dt); // 加载数据
                    frmAttribute.Show(); // 显示窗口
                }
                else
                {
                    MessageBox.Show("无法读取该图层的属性表（可能不是矢量图层）。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开属性表出错: " + ex.Message);
            }
        }

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            // 1. 判断是不是鼠标右键 (Button 2 代表右键)
            if (e.button == 2)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;
                IBasicMap map = null;
                ILayer layer = null;
                object other = null;
                object index = null;

                // 2. 核心：HitTest 碰撞检测，看看鼠标点到了什么东西
                axTOCControl1.HitTest(e.x, e.y, ref item, ref map, ref layer, ref other, ref index);

                // 3. 如果点到的是一个“图层”类型
                if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    _selectedLayer = layer; // 关键：把点中的图层记录下来，给后面“打开属性表”用

                    // 4. 在鼠标点击的位置弹出右键菜单
                    contextMenuStrip1.Show(axTOCControl1, e.x, e.y);
                }
            }
        }


    }
}
