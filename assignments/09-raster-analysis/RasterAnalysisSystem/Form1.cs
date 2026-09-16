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
using ESRI.ArcGIS.DataSourcesRaster; // 栅格数据源
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SpatialAnalyst;   // 空间分析核心
using ESRI.ArcGIS.GeoAnalyst;       // 地理分析核心

namespace RasterAnalysisSystem
{
    public partial class Form1 : Form
    {
        // 全局变量
        private ISurfaceOp m_SurfaceOp;
        private IExtractionOp m_ExtractionOp;

        private void Form1_Load(object sender, EventArgs e)
        {


            // 初始化对象
    //        m_SurfaceOp = new RasterSurfaceOpClass();
    //        m_ExtractionOp = new RasterExtractionOpClass();
        }

        // 辅助函数：获取当前栅格
        private IGeoDataset GetCurrentRaster()
        {
            // 1. 尝试获取用户鼠标选中的图层
            ESRI.ArcGIS.Controls.esriTOCControlItem itemType = ESRI.ArcGIS.Controls.esriTOCControlItem.esriTOCControlItemNone;
            IBasicMap basicMap = null;
            ILayer layer = null;
            object unk = null;
            object data = null;

            try
            {
                axTOCControl1.GetSelectedItem(ref itemType, ref basicMap, ref layer, ref unk, ref data);
            }
            catch { }

            // 如果选中的是栅格，直接返回
            if (layer is IRasterLayer)
            {
                return (layer as IRasterLayer).Raster as IGeoDataset;
            }

            // 2. 【智能搜寻】如果没选中，或者选中的不是栅格，就自动遍历所有图层找栅格
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                ILayer loopLayer = axMapControl1.get_Layer(i);
                if (loopLayer is IRasterLayer)
                {
                    // 找到了！直接返回这个栅格 (通常是 dem.tif)
                    return (loopLayer as IRasterLayer).Raster as IGeoDataset;
                }
            }

            // 3. 实在找不到了
            MessageBox.Show("未找到任何栅格图层！\n请先加载 dem.tif 数据。");
            return null;
        }

        // 辅助函数：添加结果到地图
        private void AddResultToMap(IGeoDataset dataset, string name)
        {
            if (dataset == null) return;

            ILayer newLayer = null;

            try
            {
                // ========================================================
                // 【核心修复】增加对内存临时栅格的兼容性
                // ========================================================
                if (dataset is IRaster || dataset is IRasterDataset || dataset is IRasterBandCollection)
                {
                    IRasterLayer rLayer = new RasterLayerClass();

                    // 策略A：尝试作为 Dataset 创建
                    if (dataset is IRasterDataset)
                    {
                        rLayer.CreateFromDataset(dataset as IRasterDataset);
                    }
                    // 策略B：如果 A 失败（或者结果是内存 Raster），尝试作为 Raster 创建
                    else if (dataset is IRaster)
                    {
                        rLayer.CreateFromRaster(dataset as IRaster);
                    }
                    // 策略C：保底尝试
                    else
                    {
                        // 强转为 IRaster 再创建
                        rLayer.CreateFromRaster(dataset as IRaster);
                    }

                    rLayer.Name = name;
                    newLayer = rLayer;
                }
                else if (dataset is IFeatureClass)
                {
                    // 矢量数据 (等值线)
                    IFeatureLayer fLayer = new FeatureLayerClass();
                    fLayer.FeatureClass = dataset as IFeatureClass;
                    fLayer.Name = name;
                    newLayer = fLayer;
                }

                // ========================================================
                // 【强制刷新】确保图层树立即更新
                // ========================================================
                if (newLayer != null)
                {
                    axMapControl1.AddLayer(newLayer, 0); // 添加到最上面

                    axMapControl1.ActiveView.Refresh();  // 刷新地图
                    axTOCControl1.SetBuddyControl(axMapControl1); // 重新绑定一次，防脱落
                    axTOCControl1.Update();              // 强制刷新左侧列表
                }
                else
                {
                    MessageBox.Show("警告：图层创建失败，无法将结果转换为图层显示。\n结果类型：" + dataset.GetType().ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加到地图时出错：" + ex.Message);
            }
        }

public Form1()
{
    InitializeComponent();

    // =============================================================
    // 【强行开启许可】防止界面勾选无效
    // =============================================================
    try
    {
        ESRI.ArcGIS.esriSystem.IAoInitialize aoInit = new ESRI.ArcGIS.esriSystem.AoInitializeClass();
        ESRI.ArcGIS.esriSystem.esriLicenseStatus status = aoInit.CheckOutExtension(ESRI.ArcGIS.esriSystem.esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst);

        if (status != ESRI.ArcGIS.esriSystem.esriLicenseStatus.esriLicenseCheckedOut)
        {
            MessageBox.Show("警告：Spatial Analyst 许可开启失败！状态：" + status.ToString());
        }

        // 初始化分析工具
        m_SurfaceOp = new RasterSurfaceOpClass();
        m_ExtractionOp = new RasterExtractionOpClass();
    }
    catch (Exception ex)
    {
        MessageBox.Show("初始化失败：" + ex.Message);
    }
}

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Raster Files (*.tif;*.img;*.jpg)|*.tif;*.img;*.jpg";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IRasterLayer rasterLayer = new RasterLayerClass();
                    rasterLayer.CreateFromFilePath(dlg.FileName);
                    axMapControl1.AddLayer(rasterLayer, 0);
                }
                catch (Exception ex) { MessageBox.Show("加载失败: " + ex.Message); }
            }
        }

        private void btnSlope_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 智能获取输入数据 (会自动跳过等值线找到DEM)
                IGeoDataset input = GetCurrentRaster();
                if (input == null) return;

                // ========================================================
                // 【强制唤醒】不管之前有没有初始化，这里强行再初始化一次
                // ========================================================
                try
                {
                    // 强行开启许可
                    ESRI.ArcGIS.esriSystem.IAoInitialize aoInit = new ESRI.ArcGIS.esriSystem.AoInitializeClass();
                    aoInit.CheckOutExtension(ESRI.ArcGIS.esriSystem.esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst);

                    // 强行创建工具 (防止 m_SurfaceOp 为空)
                    if (m_SurfaceOp == null)
                    {
                        m_SurfaceOp = new RasterSurfaceOpClass();
                    }
                }
                catch (Exception initEx)
                {
                    MessageBox.Show("严重警告：无法初始化分析工具。\n请检查您的 ArcGIS 许可是否正常。\n" + initEx.Message);
                    return;
                }

                // 2. 设置环境 (关键步骤)
                IRasterAnalysisEnvironment env = m_SurfaceOp as IRasterAnalysisEnvironment;
                env.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, input);
                env.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, input.Extent);

                // 3. 执行计算 (3参数版本)
                object zFactor = 1.0;

                // 这里的 Ref 必须保留
                IGeoDataset output = m_SurfaceOp.Slope(input, esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopeDegrees, ref zFactor);

                if (output == null)
                {
                    MessageBox.Show("计算结果为空，请检查 DEM 数据是否正常。");
                    return;
                }

                // 4. 显示结果
                AddResultToMap(output, "坡度图");

                // 提示用户成功
                MessageBox.Show("坡度计算成功！已添加到图层。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("运行报错：\n" + ex.Message);
            }
        }

        private void btnAspect_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 智能搜寻输入数据 (自动跳过等值线找DEM)
                IGeoDataset input = GetCurrentRaster();
                if (input == null) return;

                // 2. 强制唤醒分析工具 (防止为空)
                if (m_SurfaceOp == null)
                {
                    try
                    {
                        ESRI.ArcGIS.esriSystem.IAoInitialize aoInit = new ESRI.ArcGIS.esriSystem.AoInitializeClass();
                        aoInit.CheckOutExtension(ESRI.ArcGIS.esriSystem.esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst);
                        m_SurfaceOp = new RasterSurfaceOpClass();
                    }
                    catch { MessageBox.Show("无法初始化坡向分析工具。"); return; }
                }

                // 3. 环境设置 (防止 010050 错误)
                IRasterAnalysisEnvironment env = m_SurfaceOp as IRasterAnalysisEnvironment;
                env.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, input);
                env.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, input.Extent);

                // 4. 执行坡向计算
                IGeoDataset output = m_SurfaceOp.Aspect(input);

                // 5. 显示结果
                AddResultToMap(output, "坡向图");
            }
            catch (Exception ex)
            {
                MessageBox.Show("坡向计算失败：\n" + ex.Message);
            }
        }

        private void btnQueryGray_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. 获取输入参数
                int bandIndex = int.Parse(txtBand.Text); // 波段索引
                int row = int.Parse(txtRow.Text);        // 行
                int col = int.Parse(txtCol.Text);        // 列

                // 2. 获取数据
                IGeoDataset geoDataset = GetCurrentRaster();
                if (geoDataset == null) return;
                IRaster raster = geoDataset as IRaster;
                IRaster2 raster2 = raster as IRaster2;

                // 3. 查询 (GetPixelValue 参数顺序是: 波段, 列, 行)
                // 注意：ArcGIS SDK 中通常是 (Band, Col, Row)
                object val = raster2.GetPixelValue(bandIndex, col, row);

                MessageBox.Show(string.Format("查询结果：\n波段: {0}\n行: {1}, 列: {2}\n像素值: {3}",
                    bandIndex, row, col, val.ToString()));
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出错，请检查输入是否为整数，且未超出范围。\n" + ex.Message);
            }
        }

        private void btnContour_Click(object sender, EventArgs e)
        {
            try
            {
                IGeoDataset input = GetCurrentRaster();
                if (input == null) return;

                double interval;
                if (!double.TryParse(txtContourInterval.Text, out interval)) return;

                // =========================================================
                // 【核心修复】虽然等值线是矢量，但设置环境能避免很多内部错误
                // =========================================================
                IRasterAnalysisEnvironment env = m_SurfaceOp as IRasterAnalysisEnvironment;
                env.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, input);
                env.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, input.Extent);

                object baseContour = 0.0;

                // 使用我们之前确定可用的 3 参数版本
                ESRI.ArcGIS.GeoAnalyst.ISurfaceOp surfaceOp = m_SurfaceOp as ESRI.ArcGIS.GeoAnalyst.ISurfaceOp;
                IGeoDataset output = surfaceOp.Contour(input, interval, ref baseContour);

                AddResultToMap(output, "等值线_间距" + interval);
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成等值线失败：" + ex.Message);
            }
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            try
            {
                string val = txtExtractVal.Text;

                IGeoDataset input = GetCurrentRaster();
                if (input == null) return;

                if (m_ExtractionOp == null)
                {
                    try
                    {
                        ESRI.ArcGIS.esriSystem.IAoInitialize aoInit = new ESRI.ArcGIS.esriSystem.AoInitializeClass();
                        aoInit.CheckOutExtension(ESRI.ArcGIS.esriSystem.esriLicenseExtensionCode.esriLicenseExtensionCodeSpatialAnalyst);
                        m_ExtractionOp = new RasterExtractionOpClass();
                    }
                    catch { return; }
                }

                IRasterAnalysisEnvironment env = m_ExtractionOp as IRasterAnalysisEnvironment;
                env.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, input);
                env.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, input.Extent);

                IRasterDescriptor descriptor = new RasterDescriptorClass();
                IQueryFilter filter = new QueryFilterClass();
                filter.WhereClause = "Value > " + val;
                descriptor.Create(input as IRaster, filter, "Value");

                IGeoDataset output = m_ExtractionOp.Attribute(descriptor);

                // ========================================================
                // 【核心修复】计算统计值 (修复版)
                // ========================================================
                if (output != null)
                {
                    try
                    {
                        ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection bandColl = output as ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection;

                        if (bandColl == null && output is ESRI.ArcGIS.Geodatabase.IRaster)
                        {
                            bandColl = (output as ESRI.ArcGIS.Geodatabase.IRaster) as ESRI.ArcGIS.DataSourcesRaster.IRasterBandCollection;
                        }

                        if (bandColl != null && bandColl.Count > 0)
                        {
                            ESRI.ArcGIS.DataSourcesRaster.IRasterBand band = bandColl.Item(0);
                            if (band != null)
                            {
                                // 修正点：通过 band 获取 RasterDataset
                                ESRI.ArcGIS.Geodatabase.IRasterDataset rasterDs = band.RasterDataset;

                                if (rasterDs != null)
                                {
                                    // 修正点：IRasterDatasetEdit 在 Geodatabase 命名空间下
                                    ESRI.ArcGIS.Geodatabase.IRasterDatasetEdit rasterEdit =
                                        rasterDs as ESRI.ArcGIS.Geodatabase.IRasterDatasetEdit;

                                    if (rasterEdit != null)
                                    {
                                        rasterEdit.ComputeStats(0);
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }

                AddResultToMap(output, "提取_大于" + val);
            }
            catch (Exception ex)
            {
                MessageBox.Show("提取失败：\n" + ex.Message);
            }
        }
    }
}
