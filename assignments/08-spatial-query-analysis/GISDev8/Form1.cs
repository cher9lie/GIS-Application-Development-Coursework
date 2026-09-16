using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geoprocessing; // GP工具必须
using ESRI.ArcGIS.Geoprocessor;  // GP工具必须
using System.Runtime.InteropServices; // 用于释放COM对象

namespace GISDev8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 将工具栏与地图控件绑定
            axToolbarControl1.SetBuddyControl(axMapControl1);
        }

        private void btnAttributeSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string searchName = txtSearchName.Text.Trim();
                if (string.IsNullOrEmpty(searchName))
                {
                    MessageBox.Show("请输入村名！");
                    return;
                }

                // 1. 获取第一个图层（假设是村图层）
                IFeatureLayer pFeatureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
                if (pFeatureLayer == null) return;
                IFeatureClass pFeatureClass = pFeatureLayer.FeatureClass;

                // 2. 定义查询过滤器 
                IQueryFilter pQueryFilter = new QueryFilterClass();
                // 注意：Shapefile中字符串字段通常需要单引号，如 NAME = '张村'
                pQueryFilter.WhereClause = "NAME = '" + searchName + "'";

                // 3. 执行选择 
                IFeatureSelection pFeatureSelection = pFeatureLayer as IFeatureSelection;
                pFeatureSelection.SelectFeatures(pQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, false);

                // 4. 【提高要求】异常控制与居中显示 
                ISelectionSet pSelectionSet = pFeatureSelection.SelectionSet;
                if (pSelectionSet.Count == 0)
                {
                    MessageBox.Show("没有查到数据，请检查村名是否正确。");
                }
                else
                {
                    // 居中显示逻辑：获取选择集的范围
                    IEnumIDs pEnumIDs = pSelectionSet.IDs;
                    int id = pEnumIDs.Next();
                    IFeature pFeature = pFeatureClass.GetFeature(id);

                    // 将地图中心移动到要素的中心，并稍微放大
                    IEnvelope pEnvelope = pFeature.Shape.Envelope;
                    pEnvelope.Expand(1.5, 1.5, true); // 扩大显示范围，不然填满屏幕不好看
                    axMapControl1.ActiveView.Extent = pEnvelope;

                    axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeoSelection, null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出错：" + ex.Message);
            }
        }

        private void btnSpatialSearch_Click(object sender, EventArgs e)
        {
            // 1. 在地图上画一个矩形框（TrackRectangle）
            // 或者使用 TrackPolygon() 如实验书所示
            IEnvelope pEnvelope = axMapControl1.TrackRectangle();

            if (pEnvelope == null || pEnvelope.IsEmpty) return;

            // 2. 空间过滤器 
            ISpatialFilter pSpatialFilter = new SpatialFilterClass();
            pSpatialFilter.Geometry = pEnvelope;
            pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; // 相交关系

            // 3. 获取图层并搜索
            IFeatureLayer pFeatureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            if (pFeatureLayer == null) return;

            IFeatureCursor pFeatureCursor = pFeatureLayer.Search(pSpatialFilter, false);
            IFeature pFeature = pFeatureCursor.NextFeature();

            // 4. 准备 DataTable 存储数据
            System.Data.DataTable pDataTable = new System.Data.DataTable();
            // 添加列名 (以NAME字段为例，实际可以遍历Fields添加所有列)
            pDataTable.Columns.Add("OID");
            pDataTable.Columns.Add("Name");

            // 5. 遍历结果
            while (pFeature != null)
            {
                System.Data.DataRow dr = pDataTable.NewRow();
                dr["OID"] = pFeature.OID;
                // 假设shp里有一个叫 "NAME" 的字段，需要根据实际数据调整
                int index = pFeature.Fields.FindField("NAME");
                if (index != -1)
                    dr["Name"] = pFeature.get_Value(index).ToString();

                pDataTable.Rows.Add(dr);

                // 高亮显示该要素（可选）
                axMapControl1.Map.SelectFeature(pFeatureLayer, pFeature);

                pFeature = pFeatureCursor.NextFeature();
            }

            axMapControl1.ActiveView.Refresh(); // 刷新高亮

            // 6. 【提高要求】在“新窗体”中显示结果 
            if (pDataTable.Rows.Count > 0)
            {
                frmAttribute frm = new frmAttribute();
                frm.SetDataSource(pDataTable);
                frm.Show(); // 弹出新窗体
            }
            else
            {
                MessageBox.Show("框选范围内没有目标要素。");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Shapefile (*.shp)|*.shp";
            openFileDialog.Title = "打开地图数据";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string folderPath = System.IO.Path.GetDirectoryName(filePath);
                string fileName = System.IO.Path.GetFileName(filePath);

                axMapControl1.AddShapeFile(folderPath, fileName);
                axMapControl1.Extent = axMapControl1.FullExtent;
            }
        }

        private void btnBuffer_Click(object sender, EventArgs e)
        {
            try
            {
                IFeatureLayer pLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
                if (pLayer == null) return;

                // 初始化GP 
                Geoprocessor gp = new Geoprocessor();
                gp.OverwriteOutput = true; // 允许覆盖同名文件

                // 定义Buffer工具 
                ESRI.ArcGIS.AnalysisTools.Buffer bufferTool = new ESRI.ArcGIS.AnalysisTools.Buffer();

                // 设置输入：必须是完整路径或图层对象
                // 如果数据是Shapefile，建议传入完整路径字符串，比较稳定
                IDataset pDataset = pLayer.FeatureClass as IDataset;
                bufferTool.in_features = pDataset.Workspace.PathName + "\\" + pDataset.Name + ".shp";

                // 设置输出路径
                string outPath = @"c:\temp\buffer_result.shp"; // 请确保c:\temp存在，或者用SaveFileDialog获取路径
                bufferTool.out_feature_class = outPath;

                // 设置缓冲距离 
                bufferTool.buffer_distance_or_field = "500 Meters"; // 500米缓冲

                // 执行
                gp.Execute(bufferTool, null);

                MessageBox.Show("缓冲区分析成功！");

                // 将结果加载回地图
                axMapControl1.AddShapeFile(@"c:\temp", "buffer_result.shp");
            }
            catch (Exception ex)
            {
                MessageBox.Show("分析失败: " + ex.Message);
                // 查看详细GP错误信息
                // 实际开发中可以通过 gp.GetMessages(ref object) 查看
            }
        }
    }
}
