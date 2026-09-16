using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;        // 解决 IFeatureLayer, ILayer 报错
using ESRI.ArcGIS.Display;      // 解决 IRgbColor, IColorRamp 报错
using ESRI.ArcGIS.Geometry;     // 解决几何图形报错
using ESRI.ArcGIS.Geodatabase;  // 解决数据库相关报错
using ESRI.ArcGIS.esriSystem;   // 解决系统基础报错
using ESRI.ArcGIS.Controls;     // 解决控件报错


namespace MapSymbolizationLab
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // ★★★ 必须放在这里！★★★
            // 把工具条绑定给地图控件
            this.axToolbarControl1.SetBuddyControl(this.axMapControl1);
        }

        // 1. 设置颜色方法 (辅助工具)
        private IRgbColor GetRgbColor(int r, int g, int b)
        {
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = r;
            pColor.Green = g;
            pColor.Blue = b;
            return pColor;
        }

        // 2. 根据图层名字获取图层 (辅助工具)
        private IFeatureLayer GetFeatureLayer(string layerName)
        {
            // 防止地图还未加载就点击导致报错
            if (axMapControl1.LayerCount == 0) return null;

            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                ILayer pLayer = axMapControl1.get_Layer(i);
                // 如果图层名字包含我们找的字（防止大小写或后缀问题），就返回
                if (pLayer.Name.ToUpper().Contains(layerName.ToUpper()) && pLayer is IFeatureLayer)
                {
                    return pLayer as IFeatureLayer;
                }
            }
   //         MessageBox.Show("未找到图层: " + layerName + "\n请检查TOC中图层名字是否正确");
            return null;
        }

        // 3. 生成随机颜色带 (辅助工具)
        private IColorRamp CreateAlgorithmicColorRamp(int count)
        {
            // 1. 创建算法色带对象
            IAlgorithmicColorRamp pAlgoRamp = new AlgorithmicColorRampClass();
            pAlgoRamp.Algorithm = esriColorRampAlgorithm.esriCIELabAlgorithm; // 使用 CIELab 算法，渐变更自然

            // 2. 设置起始颜色 (比如浅红色)
            IRgbColor pFromColor = GetRgbColor(255, 235, 235);
            pAlgoRamp.FromColor = pFromColor;

            // 3. 设置终止颜色 (比如深红色)
            IRgbColor pToColor = GetRgbColor(139, 0, 0);
            pAlgoRamp.ToColor = pToColor;

            // 4. 设置生成的颜色数量
            pAlgoRamp.Size = count;

            // 5. 生成
            bool ok = true;
            pAlgoRamp.CreateRamp(out ok);
            return pAlgoRamp;
        }

        // ★★★ 新增：这是生成随机颜色的方法，专门给“单值专题图”用 ★★★
        private IColorRamp CreateRandomColorRamp(int count)
        {
            IRandomColorRamp pColorRamp = new RandomColorRampClass();
            pColorRamp.StartHue = 0;
            pColorRamp.EndHue = 360;
            pColorRamp.MinSaturation = 15;
            pColorRamp.MaxSaturation = 100;
            pColorRamp.MinValue = 0;
            pColorRamp.MaxValue = 100;
            pColorRamp.Size = count;

            bool ok = true;
            pColorRamp.CreateRamp(out ok);
            return pColorRamp;
        }

        // ★★★ 新增：通用符号化逻辑（自动识别点/线/面） ★★★
        private void SymbolizeByLayerType(IFeatureLayer pLayer)
        {
            IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;
            ISimpleRenderer pSimpleRenderer = new SimpleRendererClass();
            ISymbol pSymbol = null;

            // 判断图层几何类型
            switch (pLayer.FeatureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    ISimpleMarkerSymbol pMarkerSym = new SimpleMarkerSymbolClass();
                    pMarkerSym.Style = esriSimpleMarkerStyle.esriSMSCircle;
                    pMarkerSym.Color = GetRgbColor(255, 0, 0); // 红点
                    pMarkerSym.Size = 8;
                    pSymbol = pMarkerSym as ISymbol;
                    break;

                case esriGeometryType.esriGeometryPolyline:
                    ISimpleLineSymbol pLineSym = new SimpleLineSymbolClass();
                    pLineSym.Color = GetRgbColor(0, 0, 255); // 蓝线
                    pLineSym.Width = 2;
                    pSymbol = pLineSym as ISymbol;
                    break;

                case esriGeometryType.esriGeometryPolygon:
                    ISimpleFillSymbol pFillSym = new SimpleFillSymbolClass();
                    pFillSym.Color = GetRgbColor(0, 255, 0); // 绿面
                    pSymbol = pFillSym as ISymbol;
                    break;
            }

            if (pSymbol != null)
            {
                pSimpleRenderer.Symbol = pSymbol;
                pGeoLayer.Renderer = pSimpleRenderer as IFeatureRenderer;
                axMapControl1.Refresh();
                axMapControl1.Update();
            }
        }

        private void btnOpenMxd_Click(object sender, EventArgs e)
        {
            // 过滤文件类型，只显示 mxd
            openFileDialog1.Filter = "Map Documents (*.mxd)|*.mxd";
            openFileDialog1.Title = "打开地图文档";

            // 如果用户点击了“确定”
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // 检查文件名是否有效
                if (axMapControl1.CheckMxFile(openFileDialog1.FileName))
                {
                    axMapControl1.LoadMxFile(openFileDialog1.FileName);
                    axMapControl1.ActiveView.Refresh(); // 刷新视图
                }
                else
                {
                    MessageBox.Show("无效的地图文档！");
                }
            }
        }

        private void btnUniqueValue_Click(object sender, EventArgs e)
        {
            // 1. 获取图层 (根据你的文件名，我推测图层名包含 "continent")
            IFeatureLayer pLayer = GetFeatureLayer("continent");
            if (pLayer == null) return;

            IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;

            // 2. 创建唯一值渲染器
            IUniqueValueRenderer pUVRenderer = new UniqueValueRendererClass();
            pUVRenderer.FieldCount = 1;
            pUVRenderer.set_Field(0, "CONTINENT"); // 使用大洲名称字段

            // 3. 准备符号
            // 这里我们简单处理：硬编码添加几个大洲，防止复杂的遍历代码出错
            string[] continents = { "Asia", "Europe", "Africa", "North America", "South America", "Oceania", "Antarctica" };

            // 生成随机色带
            IColorRamp pColorRamp = CreateRandomColorRamp(12);
            IEnumColors pEnumColors = pColorRamp.Colors;
            pEnumColors.Reset();

            // 4. 循环添加
            foreach (string area in continents)
            {
                ISimpleFillSymbol pSym = new SimpleFillSymbolClass();
                pSym.Style = esriSimpleFillStyle.esriSFSSolid;
                pSym.Color = pEnumColors.Next();
                pSym.Outline = null; // 去掉边框

                pUVRenderer.AddValue(area, "Continent", pSym as ISymbol);
            }

            // 添加一个默认符号处理其他未知区域
            ISimpleFillSymbol pDefaultSym = new SimpleFillSymbolClass();
            pDefaultSym.Color = GetRgbColor(200, 200, 200); // 灰色
            pUVRenderer.DefaultSymbol = pDefaultSym as ISymbol;
            pUVRenderer.UseDefaultSymbol = true;

            // 5. 应用渲染
            pGeoLayer.Renderer = pUVRenderer as IFeatureRenderer;
            axMapControl1.Refresh();
            axMapControl1.Update(); // 刷新TOC列表
        }

        private void btnBarChart_Click(object sender, EventArgs e)
        {
            IFeatureLayer pLayer = GetFeatureLayer("continent");
            if (pLayer == null) return;
            IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;

            // 1. 定义柱状图符号
            IBarChartSymbol pBarSymbol = new BarChartSymbolClass();
            pBarSymbol.Width = 10; // 柱子宽度

            // 2. 添加符号到数组 (红柱子 和 绿柱子)
            ISymbolArray pSymbolArray = pBarSymbol as ISymbolArray;

            ISimpleFillSymbol pFill1 = new SimpleFillSymbolClass();
            pFill1.Color = GetRgbColor(255, 100, 100); // 红
            pSymbolArray.AddSymbol(pFill1 as ISymbol);

            ISimpleFillSymbol pFill2 = new SimpleFillSymbolClass();
            pFill2.Color = GetRgbColor(100, 255, 100); // 绿
            pSymbolArray.AddSymbol(pFill2 as ISymbol);

            // 3. 创建渲染器
            IChartRenderer pChartRenderer = new ChartRendererClass();
            IRendererFields pRendererFields = pChartRenderer as IRendererFields;

            // 添加要显示的字段 (确保属性表里有这些字段)
            pRendererFields.AddField("SQMI", "SQMI");
            pRendererFields.AddField("SQKM", "SQKM");

            // 4. 设置最大值 (控制柱子高度)
            pChartRenderer.ChartSymbol = pBarSymbol as IChartSymbol;
            // 这里设置一个合适的最大值，防止柱子太高遮住地图
            pChartRenderer.ChartSymbol.MaxValue = 40000000;

            // 背景透明
            ISimpleFillSymbol pBackground = new SimpleFillSymbolClass();
            pBackground.Style = esriSimpleFillStyle.esriSFSNull;
            pChartRenderer.BaseSymbol = pBackground as ISymbol;
            pChartRenderer.UseOverposter = false;

            // 5. 应用
            pGeoLayer.Renderer = pChartRenderer as IFeatureRenderer;
            axMapControl1.Refresh();
            axMapControl1.Update();
        }

        private void btnPieChart_Click(object sender, EventArgs e)
        {
            IFeatureLayer pLayer = GetFeatureLayer("continent");
            if (pLayer == null) return;
            IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;

            // 1. 定义饼图符号
            IPieChartSymbol pPieSymbol = new PieChartSymbolClass();
            ISymbolArray pSymbolArray = pPieSymbol as ISymbolArray;

            // 2. 添加两瓣符号 (蓝 和 黄)
            ISimpleFillSymbol pFill1 = new SimpleFillSymbolClass();
            pFill1.Color = GetRgbColor(0, 100, 255); // 蓝
            pSymbolArray.AddSymbol(pFill1 as ISymbol);

            ISimpleFillSymbol pFill2 = new SimpleFillSymbolClass();
            pFill2.Color = GetRgbColor(255, 255, 0); // 黄
            pSymbolArray.AddSymbol(pFill2 as ISymbol);

            // 3. 创建渲染器
            IChartRenderer pChartRenderer = new ChartRendererClass();
            IRendererFields pRendererFields = pChartRenderer as IRendererFields;

            pRendererFields.AddField("SQMI", "SQMI");
            pRendererFields.AddField("SQKM", "SQKM");

            pChartRenderer.ChartSymbol = pPieSymbol as IChartSymbol;
            pChartRenderer.ChartSymbol.MaxValue = 40000000;

            ISimpleFillSymbol pBackground = new SimpleFillSymbolClass();
            pBackground.Style = esriSimpleFillStyle.esriSFSNull;
            pChartRenderer.BaseSymbol = pBackground as ISymbol;

            pGeoLayer.Renderer = pChartRenderer as IFeatureRenderer;
            axMapControl1.Refresh();
            axMapControl1.Update();
        }

        private void btnGraduated_Click(object sender, EventArgs e)
        {
            IFeatureLayer pLayer = GetFeatureLayer("continent");
            if (pLayer == null) return;
            IGeoFeatureLayer pGeoLayer = pLayer as IGeoFeatureLayer;

            // 1. 创建分级渲染器
            IClassBreaksRenderer pClassRenderer = new ClassBreaksRendererClass();
            pClassRenderer.Field = "SQKM"; // 分级字段
            pClassRenderer.BreakCount = 5; // 分成5级
            pClassRenderer.SortClassesAscending = true; // 升序

            // 2. 生成色带
            IColorRamp pColorRamp = CreateAlgorithmicColorRamp(5);
            IEnumColors pEnumColors = pColorRamp.Colors;
            pEnumColors.Reset();

            // 3. 手动设置断点 (模拟自然断点法，写死数值以便运行成功)
            // 这里的数值是大概估算的，单位是平方公里
            double[] breaks = { 0, 5000000, 10000000, 20000000, 30000000, 100000000 };

            for (int i = 0; i < 5; i++)
            {
                ISimpleFillSymbol pSym = new SimpleFillSymbolClass();
                pSym.Style = esriSimpleFillStyle.esriSFSSolid;
                pSym.Color = pEnumColors.Next();

                pClassRenderer.set_Symbol(i, pSym as ISymbol);
                pClassRenderer.set_Break(i, breaks[i + 1]); // 设置每一级的上限值
            }

            pGeoLayer.Renderer = pClassRenderer as IFeatureRenderer;
            axMapControl1.Refresh();
            axMapControl1.Update();
        }

        private void btnAutoSymbol_Click(object sender, EventArgs e)
        {
            // 1. 检查地图有没有图层
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("请先加载地图！");
                return;
            }

            // 2. 遍历地图上的每一个图层
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                ILayer pLayer = axMapControl1.get_Layer(i);

                // 如果这个图层是“要素图层”(FeatureLayer)
                if (pLayer is IFeatureLayer)
                {
                    // 调用我们写好的智能符号化方法
                    SymbolizeByLayerType(pLayer as IFeatureLayer);
                }
            }

            // 3. 刷新视图
            axMapControl1.ActiveView.Refresh();
            MessageBox.Show("所有图层已按几何类型自动符号化完成！");
        }



    }
}
