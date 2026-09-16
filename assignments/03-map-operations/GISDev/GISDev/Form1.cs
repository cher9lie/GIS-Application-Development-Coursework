using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;

namespace GISDev
{
    public partial class Form1 : Form
    {
        string m_BasicOperationTool = "";
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "open map document";
            openFileDialog.Filter = "map document (*.mxd)|*.mxd";
            openFileDialog.ShowDialog();
            string strFilePath = openFileDialog.FileName;
            if (axMapControl1.CheckMxFile(strFilePath))
            {
                axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerHourglass;
                axMapControl1.LoadMxFile(strFilePath, 0, Type.Missing);
                axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerDefault;
            }
            else
            {
                MessageBox.Show(strFilePath + " is not a valid map document");
            }
        }

        // 全屏
        private void tlbGloble_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "isZoomFull";
            axMapControl1.Extent = axMapControl1.FullExtent; // 直接生效
        }
        // 放大
        private void tlbZoomIn_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "ZoomIn";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerZoomIn;
        }
        // 缩小
        private void tlbZoomOut_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "ZoomOut";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerZoomOut;
        }
        // 漫游
        private void tlbPan_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "ZoomPan";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerPan;
        }

        private void axMapControl1_OnMouseDown(object sender, IMapControlEvents2_OnMouseDownEvent e)
        {
            // 本方法用于在axMapControl上进行点击后才生效的功能,如放大缩小和漫游
            switch (m_BasicOperationTool)
            {
                case "ZoomIn":
                    axMapControl1.Extent = axMapControl1.TrackRectangle(); // 拉框放大
                    break;
                case "ZoomOut":
                    IEnvelope objEnvelope = axMapControl1.TrackRectangle();
                    // 扩大包络线范围以缩小显示
                    objEnvelope.Expand(2, 2, true);
                    axMapControl1.Extent = objEnvelope;
                    break;
                case "ZoomPan":
                    axMapControl1.Pan(); // 鼠标拖动平移
                    break;

                // vvvv 使用 IMap.Identify(IGeometry) 替代的“属性查询”代码 vvvv
                case "Identify":

                    // 1. 检查地图是否加载
                    if (axMapControl1.Map == null || axMapControl1.LayerCount == 0)
                    {
                        MessageBox.Show("调试：地图未加载或地图中没有图层。请先加载 MXD 文件。");
                        return;
                    }

                    // 2. 获取点击点，并创建一个带容差的包络矩形 (IEnvelope)
                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(e.mapX, e.mapY);
                    IEnvelope pEnvelope = pPoint.Envelope;
                    double tolerance = (axMapControl1.Extent.Width / axMapControl1.Width) * 4;
                    pEnvelope.Expand(tolerance, tolerance, false);

                    // 3. [核心] 遍历所有图层，执行空间查询 (不使用 IIdentify 接口)
                    string sResult = "";
                    IMap map = axMapControl1.Map;

                    for (int i = 0; i < map.LayerCount; i++)
                    {
                        ILayer layer = map.get_Layer(i);

                        // 4. 检查图层是否是可见的、可查询的要素图层
                        if (layer is IFeatureLayer && layer.Visible == true)
                        {
                            IFeatureLayer featureLayer = (IFeatureLayer)layer;
                            IFeatureClass featureClass = featureLayer.FeatureClass;

                            if (featureClass == null) continue; // 跳过没有要素类的图层

                            // 5. 创建空间过滤器 (SpatialFilter)
                            ISpatialFilter spatialFilter = new SpatialFilterClass();
                            spatialFilter.Geometry = pEnvelope; // 使用我们创建的矩形框
                            spatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; // 查询方式：相交

                            // 6. 执行查询 (IFeatureClass.Search)
                            IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);
                            IFeature feature = featureCursor.NextFeature();

                            // 7. 遍历查询到的要素
                            while (feature != null)
                            {
                                sResult += "==============================\n";
                                sResult += "图层: " + layer.Name + "\n";
                                sResult += "FID: " + feature.OID + "\n";

                                for (int j = 0; j < feature.Fields.FieldCount; j++)
                                {
                                    IField pField = feature.Fields.get_Field(j);
                                    if (pField.Type != esriFieldType.esriFieldTypeGeometry)
                                    {
                                        sResult += pField.Name + ": " + feature.get_Value(j) + "\n";
                                    }
                                }
                                feature = featureCursor.NextFeature();
                            }

                            // 释放 COM 对象 (非常重要)
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(featureCursor);
                        }
                    }

                    // 8. 显示最终结果
                    if (!string.IsNullOrEmpty(sResult))
                    {
                        MessageBox.Show(sResult, "属性查询结果");
                    }
                    else
                    {
                        MessageBox.Show("调试：在点击位置未查询到任何结果。");
                    }
                    break;

                    default:

            if (m_BasicOperationTool != "")
            {
                // (如果工具不是空的，我们才提示，免得每次点击都弹窗)
                MessageBox.Show("调试：OnMouseDown 触发，但工具状态未知: " + m_BasicOperationTool);
            }
                    break;
            }
        }

        private void tlbIdentify_Click(object sender, EventArgs e)
        {
            m_BasicOperationTool = "Identify";
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerIdentify;
        }
    }
}
