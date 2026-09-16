using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display; // source: 2
using ESRI.ArcGIS.Output;  // source: 2
using Microsoft.VisualBasic;


namespace GISDev7
{
    public partial class Form1 : Form
    {
        private int currentOperation = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnAddLegend_Click(object sender, EventArgs e)
        {
            // 1. 强制切换到排版视图（因为只有在排版视图才能加整饰要素）
            tabControl1.SelectedIndex = 1;

            // 2. 设置状态为 1 (代表图例)
            currentOperation = 1;

            // 3. 改变鼠标指针，提示用户现在可以点击了
            axPageLayoutControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        private void btnAddNorthArrow_Click(object sender, EventArgs e)
        {
            // 1. 强制切换到排版视图（因为只有在排版视图才能加整饰要素）
            tabControl1.SelectedIndex = 1;

            // 2. 设置状态为 1 (代表图例)
            currentOperation = 2;

            // 3. 改变鼠标指针，提示用户现在可以点击了
            axPageLayoutControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        private void btnAddScaleBar_Click(object sender, EventArgs e)
        {
            // 1. 强制切换到排版视图（因为只有在排版视图才能加整饰要素）
            tabControl1.SelectedIndex = 1;

            // 2. 设置状态为 1 (代表图例)
            currentOperation = 3;

            // 3. 改变鼠标指针，提示用户现在可以点击了
            axPageLayoutControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        private void btnAddTitle_Click(object sender, EventArgs e)
        {
            // 1. 强制切换到排版视图（因为只有在排版视图才能加整饰要素）
            tabControl1.SelectedIndex = 1;

            // 2. 设置状态为 1 (代表图例)
            currentOperation = 4;

            // 3. 改变鼠标指针，提示用户现在可以点击了
            axPageLayoutControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
        }

        private void axPageLayoutControl1_OnMouseDown(object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            // 如果没有点击任何功能按钮，直接退出
    if (currentOperation == 0) return;

    // 获取点击位置的坐标
    IPoint pPoint = new PointClass();
    pPoint.PutCoords(e.pageX, e.pageY);

    // 获取 PageLayout 和 ActiveView
    IPageLayout pPageLayout = axPageLayoutControl1.PageLayout;
    IActiveView pActiveView = pPageLayout as IActiveView;

    // 构建一个包络线 Envelope (用于确定元素的大小和位置)
// 文档  代码片段中使用了 Envelope
    IEnvelope pEnv = new EnvelopeClass();
    // 以点击点为中心，创建一个小矩形范围 (宽高可以根据实际效果调整，这里先设个固定值)
    double width = 5.0; // 假设单位
    double height = 5.0;
    pEnv.PutCoords(e.pageX, e.pageY, e.pageX + width, e.pageY + height);

    try
    {
        switch (currentOperation)
        {
            case 1: // 添加图例
                AddLegend(pPageLayout, pEnv); // 这个函数我们要自己写
                break;
            case 2: // 添加指北针
                AddNorthArrow(pPageLayout, pEnv); // 这个函数我们要自己写
                break;
            case 3: // 添加比例尺
                AddScaleBar(pPageLayout, pEnv); // 这个函数我们要自己写
                break;
            case 4: // 添加图名
                AddMapTitle(pPageLayout, pPoint); // 图名通常只需要一个点
                break;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("添加失败: " + ex.Message);
    }
    finally
    {
        // 操作完成后，重置状态
        currentOperation = 0;
        axPageLayoutControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
    }
        }

        // 添加图例的核心方法
        private void AddLegend(IPageLayout pPageLayout, IEnvelope pEnv)
{
    // 1. 获取图形容器和ActiveView
    IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
    IActiveView pActiveView = pPageLayout as IActiveView;
    IMap pMap = pActiveView.FocusMap;

    // 2. 只有地图里有图层时才加图例，否则没意义
    if (pMap.LayerCount == 0) return;

    // 3. 创建 UID 标识符，告诉 ArcGIS 我们要创建的是 "Legend"
    UID pID = new UIDClass();
    pID.Value = "esriCarto.Legend"; // [cite: 870]

    // 4. 获取 MapFrame（图例必须关联到一个 MapFrame）
    IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame; // [cite: 872]
    if (pMapFrame == null) return;

    // 5. 创建 MapSurroundFrame（图例的外框）
    // 第二个参数为 null，表示使用默认样式，不从 StyleGallery 获取
    IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, null); // [cite: 873]
    if (pMapSurroundFrame == null) return;

    // 6. 设置图例的具体属性
    IMapSurround pMapSurround = pMapSurroundFrame.MapSurround;
    ILegend pLegend = pMapSurround as ILegend;
    pLegend.Title = "图例"; // 设置图例标题 [cite: 896]
    pLegend.ClearItems();
    
// 7. 遍历地图中的图层，添加到图例中 [cite: 897]
    for (int i = 0; i < pMap.LayerCount; i++)
    {
        ILegendItem pLegendItem = new HorizontalLegendItemClass();
        pLegendItem.Layer = pMap.get_Layer(i); // 关联图层 [cite: 901]
        pLegendItem.ShowDescriptions = false;
        pLegendItem.Columns = 1;
        pLegendItem.ShowHeading = true;
        pLegendItem.ShowLabels = true;
        pLegend.AddItem(pLegendItem); // 添加到图例 [cite: 906]
    }

    // 8. 将 MapSurroundFrame 转为 Element 并设置位置
    IElement pElement = pMapSurroundFrame as IElement;
    pElement.Geometry = pEnv; // 设置为你鼠标画出的矩形框 [cite: 892]

    // 9. 添加到版面并刷新
    pGraphicsContainer.AddElement(pElement, 0); // [cite: 907]
    pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null); // [cite: 908]
}

        // 添加指北针的核心方法
        private void AddNorthArrow(IPageLayout pPageLayout, IEnvelope pEnv)
        {
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IActiveView pActiveView = pPageLayout as IActiveView;
            IMap pMap = pActiveView.FocusMap;

            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            if (pMapFrame == null) return;

            // 1. 创建 UID
            UID pID = new UIDClass();
            pID.Value = "esriCarto.MarkerNorthArrow"; // 

            // 2. 创建 MapSurroundFrame
            IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, null);
            if (pMapSurroundFrame == null) return;

            // 3. 强制设置一个默认样式（防止 create 出来是空的）
            // 文档中使用了 pStyleGalleryItem，但为了简化我们直接实例化一个默认的
            IMarkerNorthArrow pMarkerNorthArrow = new MarkerNorthArrowClass(); // [cite: 963]
            // 可以设置颜色等属性，这里暂时用默认黑色

            // 将刚才创建的默认指北针样式赋给 Frame
            // 注意：这里是一个简化的处理，标准做法是从 StyleGallery 选择
            INorthArrow pNorthArrow = pMarkerNorthArrow as INorthArrow;
            // pNorthArrow.Size = 20; // 可以固定大小，也可以随 pEnv 变化

            // 这里的逻辑有点绕：CreateSurroundFrame 已经创建了一个内部的 Surround 对象
            // 我们通常直接用生成的 Frame 调整位置即可

            // 4. 设置位置
            IElement pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnv; // [cite: 978]

            // 5. 添加并刷新
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        // 添加比例尺的核心方法
        private void AddScaleBar(IPageLayout pPageLayout, IEnvelope pEnv)
        {
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IActiveView pActiveView = pPageLayout as IActiveView;
            IMap pMap = pActiveView.FocusMap;

            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            if (pMapFrame == null) return;

            // 1. 创建 UID
            UID pID = new UIDClass();
            pID.Value = "esriCarto.AlternatingScaleBar"; // 

            // 2. 创建 MapSurroundFrame
            IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, null);
            if (pMapSurroundFrame == null) return;

            // 3. 获取比例尺对象并设置单位
            IScaleBar pScaleBar = pMapSurroundFrame.MapSurround as IScaleBar;
            pScaleBar.Units = esriUnits.esriKilometers; // 设置单位为千米 [cite: 1021]
            pScaleBar.UnitLabel = "km";

            // 4. 设置位置
            IElement pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnv; // [cite: 1049]

            // 5. 添加并刷新
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        // 添加图名的核心方法
        private void AddMapTitle(IPageLayout pPageLayout, IPoint pPoint)
        {
            // 1. 交互式获取标题文本 (需要引用 Microsoft.VisualBasic)
            // 如果你没加引用，就把下面这行改成 string title = "默认地图标题";
            string title = Microsoft.VisualBasic.Interaction.InputBox("请输入地图标题：", "添加标题", "某某地区专题图");

            if (string.IsNullOrEmpty(title)) return;

            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IActiveView pActiveView = pPageLayout as IActiveView;

            // 2. 创建文本元素
            ITextElement pTextElement = new TextElementClass();
            pTextElement.Text = title; // [cite: 1081]

            // 3. 设置文本样式 (可选，但为了美观最好设置)
            ITextSymbol pTextSymbol = new TextSymbolClass();
            pTextSymbol.Size = 20; // 字号 [cite: 1081]
            pTextElement.Symbol = pTextSymbol;

            // 4. 设置位置 (注意图名是一个点，不是矩形框)
            IElement pElement = pTextElement as IElement;
            pElement.Geometry = pPoint; // [cite: 1082]

            // 5. 添加并刷新
            pGraphicsContainer.AddElement(pElement, 0);
            pActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 如果没有选中任何标签页，直接返回
  if (tabControl1.TabPages.Count == 0) return;

     try
     {
         // 假设索引1是“排版”视图
         if (tabControl1.SelectedIndex == 1)
         {
           // 检查 MapControl 是否有地图对象
             if (axMapControl1.Map == null)
             {
                 MessageBox.Show("MapControl 中没有地图对象！");
                 return;
             }

           // === 核心：将 MapControl 的内容同步到 PageLayoutControl ===
             IObjectCopy pObjectCopy = new ObjectCopyClass();
             object pSourceMap = axMapControl1.Map;
             object pCopiedMap = pObjectCopy.Copy(pSourceMap);
             object pOverwritedMap = axPageLayoutControl1.ActiveView.FocusMap;

             pObjectCopy.Overwrite(pCopiedMap, ref pOverwritedMap);

           // 刷新视图
             axPageLayoutControl1.ActiveView.Refresh();

           // 联动工具条
             axToolbarControl1.SetBuddyControl(axPageLayoutControl1);
         }
         else
         {
             // 切换回地图视图
             axToolbarControl1.SetBuddyControl(axMapControl1);
         }
     }
     catch (Exception ex)
     {
         // 如果这里报错，说明是 ObjectCopy 的问题
         MessageBox.Show("视图同步失败（严重错误）：\n" + ex.Message);
     }
        }

private void btnOpenMap_Click(object sender, EventArgs e)
{
    // 1. 设置文件过滤，支持 mxd 和 shp
    openFileDialog1.Filter = "地图文档 (*.mxd)|*.mxd|Shapefile (*.shp)|*.shp";
    openFileDialog1.Title = "打开地图";
    openFileDialog1.Multiselect = false; // 禁止多选
    openFileDialog1.FileName = ""; // 清空上次的文件名

    // 2. 弹出对话框，如果用户点了“确定”
    if (openFileDialog1.ShowDialog() == DialogResult.OK)
    {
        string fileName = openFileDialog1.FileName;
        
        // 获取文件扩展名（转小写，防止大小写不匹配）
        string ext = System.IO.Path.GetExtension(fileName).ToLower();

        try
        {
            // 3. 判断文件类型并加载
            if (ext == ".mxd")
            {
                // 如果是 mxd，直接用 LoadMxFile
// 判断文件是否有效 [cite: 100]
                if (axMapControl1.CheckMxFile(fileName)) 
                {
                    axMapControl1.LoadMxFile(fileName);
                }
                else
                {
                    MessageBox.Show("无效的地图文档！");
                    return;
                }
            }
            else if (ext == ".shp")
            {
// 如果是 shp，用 AddShapeFile [cite: 97]
                // AddShapeFile 需要两个参数：路径 和 文件名（不带后缀）
                string path = System.IO.Path.GetDirectoryName(fileName);
                string file = System.IO.Path.GetFileNameWithoutExtension(fileName);
                
                axMapControl1.AddShapeFile(path, file);
            }

            // 4. 加载完后，为了防止视图不同步，建议切回“地图”标签页看一下
            // 这样能确保你看到加载进来的数据
            if (tabControl1.SelectedIndex != 0)
            {
                tabControl1.SelectedIndex = 0;
            }
            
            // 刷新一下活动视图
            axMapControl1.ActiveView.Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show("加载地图失败: " + ex.Message);
        }
    }
}

private void btnExport_Click(object sender, EventArgs e)
{
    try
    {
        // 1. 初始化保存对话框
        SaveFileDialog pSaveDialog = new SaveFileDialog();
        pSaveDialog.FileName = "MyMap";
// 设置支持的格式，对应文档中的 ExportImage 和 ExportVector [cite: 1189, 1190]
        pSaveDialog.Filter = "JPEG 图片 (*.jpg)|*.jpg|PDF 文档 (*.pdf)|*.pdf|TIFF 图片 (*.tif)|*.tif";
        
        if (pSaveDialog.ShowDialog() == DialogResult.OK)
        {
            // 2. 获取当前的分辨率 (DPI)
            IActiveView pActiveView = axPageLayoutControl1.ActiveView;
            double iScreenDispalyResolution = pActiveView.ScreenDisplay.DisplayTransformation.Resolution; // [cite: 1235]
            
            // 3. 根据选择的格式创建对应的 Exporter 对象
            IExport pExporter = null;
            switch (pSaveDialog.FilterIndex)
            {
                case 1:
                    pExporter = new ExportJPEGClass(); // [cite: 1240]
                    break;
                case 2:
                    pExporter = new ExportPDFClass(); // [cite: 1248]
                    break;
                case 3:
                    pExporter = new ExportTIFFClass(); // [cite: 1245]
                    break;
            }

            if (pExporter == null) return;

            // 4. 设置输出参数
            pExporter.ExportFileName = pSaveDialog.FileName; // [cite: 1249]
            pExporter.Resolution = iScreenDispalyResolution; // [cite: 1250]
            
            // 获取输出范围 (整张纸)
            tagRECT deviceRect = pActiveView.ScreenDisplay.DisplayTransformation.get_DeviceFrame(); // [cite: 1251]
            IEnvelope pDeviceEnvelope = new EnvelopeClass();
            pDeviceEnvelope.PutCoords(deviceRect.left, deviceRect.bottom, deviceRect.right, deviceRect.top); // [cite: 1253]
            pExporter.PixelBounds = pDeviceEnvelope; // [cite: 1254]

            // 5. 执行输出
            ITrackCancel pCancel = new CancelTrackerClass(); // [cite: 1255]
// 开始导出 [cite: 1256]
            pActiveView.Output(pExporter.StartExporting(), (int)pExporter.Resolution, ref deviceRect, pActiveView.Extent, pCancel);
            
            pExporter.FinishExporting(); // [cite: 1259]
            pExporter.Cleanup(); // 释放资源

            MessageBox.Show("地图导出成功！");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("导出失败: " + ex.Message); // [cite: 1265]
    }
}

private void btnPrint_Click(object sender, EventArgs e)
{
    try
    {
        // 确保我们在排版视图
        if (tabControl1.SelectedIndex != 1)
        {
            MessageBox.Show("请先切换到版面视图！");
            return;
        }

        // 1. 设置打印机 (使用默认打印机)
// PageLayoutControl 已经封装了打印功能，我们可以直接调用 [cite: 163]
        IPrinter pPrinter = axPageLayoutControl1.Printer;
        
        // 如果需要弹出打印设置对话框，可以使用 PrintDialog (System.Windows.Forms)
        // 这里为了演示简单，直接使用默认设置进行打印预览或打印
        
        // 2. 执行打印
        // 参数: 起始页, 结束页, 重叠范围
        // 通常只打印当前页，所以是 1, 1, 0
        axPageLayoutControl1.PrintPageLayout(1, 1, 0); // [cite: 275]

        // 注意：这通常会直接发送到默认打印机。
        // 如果你想看效果，可以把默认打印机设为 "Microsoft Print to PDF"
    }
    catch (Exception ex)
    {
        MessageBox.Show("打印失败: " + ex.Message);
    }
}
    }
}
