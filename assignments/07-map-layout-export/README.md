# 作业 7：地图整饰、输出与打印

## 作业目标

本作业从数据视图进入版面视图，练习 `PageLayoutControl`、地图整饰元素、图像/矢量输出和打印。界面包含“数据视图”和“版面视图”两个选项卡。

## 环境与入口

- Visual Studio 2010
- .NET Framework 4.0
- ArcGIS Engine 10.2
- 目标平台：x86
- 解决方案：`GISDev7.sln`
- 主要代码：`GISDev7/Form1.cs`

程序优先绑定 ArcGIS Engine，依次尝试 Engine 和 Engine GeoDB 许可。未安装 Engine 时绑定 Desktop，依次尝试 Basic、Standard 和 Advanced。产品许可不可用时，程序提示原因并退出。

## 已实现功能

- 加载 MXD 或 Shapefile
- MapControl 与 PageLayoutControl 地图内容同步
- 在版面中添加图例
- 添加指北针
- 添加千米单位比例尺
- 输入并添加图名
- 导出 JPEG、PDF、TIFF
- 使用默认打印机打印页面布局

## 操作步骤

1. 启动程序，点击“打开地图”。
2. 选择 `.mxd` 或 `.shp`。
3. 在“数据视图”中检查地图范围和图层。
4. 切换到“版面视图”，确认地图已经同步。
5. 点击“添加图例”，在页面上拖出矩形确定位置和大小。
6. 用相同方式添加指北针和比例尺。
7. 点击“添加图名”，输入标题后在页面上点击放置位置。
8. 点击“地图输出”，选择 JPG、PDF 或 TIF 并指定保存位置。
9. 如需打印，先确认 Windows 默认打印机，再点击“打印地图”。

添加整饰元素后鼠标会变为十字形，下一次在 PageLayoutControl 上的点击或拖框负责放置元素。

## 各元素的源码行为

- 图例：基于当前地图框，为地图中的图层创建图例项，标题为“图例”。
- 指北针：使用 `MarkerNorthArrow`。
- 比例尺：使用 `AlternatingScaleBar`，单位固定为千米，标签为 `km`。
- 图名：创建 `TextElement` 放到点击位置。
- 导出：根据保存对话框的格式创建 `ExportJPEGClass`、`ExportPDFClass` 或 `ExportTIFFClass`。
- 打印：调用 `PrintPageLayout`，输出到当前默认打印机。

## 版面编辑与导出的对象链

`MapControl` 管地图内容和地图坐标范围，`PageLayoutControl` 管页面以及放在页面上的元素。切换标签时先看 `tabControl1_SelectedIndexChanged` 如何同步地图。图例、指北针、比例尺依赖地图框，标题走文本元素分支。可在 `axPageLayoutControl1_OnMouseDown` 处观察页面点击或拖框如何提供位置，再跟进 `AddLegend`、`AddNorthArrow`、`AddScaleBar` 和 `AddMapTitle`。

导出从 `btnExport_Click` 进入，顺序为设置导出范围和分辨率、`StartExporting` 取得输出设备、调用 `IActiveView.Output`、完成导出并清理。排查导出缺少整饰时，先看传给 `Output` 的活动视图属于地图还是页面，再核对当前页签和导出范围。

## 数据与版面注意事项

- MXD 的图层数据源必须有效。
- 单独加载 SHP 时，程序会把数据添加到 MapControl，再同步到布局。
- 图例、比例尺和指北针依赖有效的地图框。
- 比例尺采用千米单位，数据空间参考不合理时显示可能不符合预期。
- 输出内容以当前版面视图为准，不是只导出数据视图。
- 打印按钮不会提供完整的打印机选择流程，建议预先设置默认打印机或 PDF 打印机。

## 已知限制

- 没有元素选择、移动、删除和样式编辑界面。
- 多次点击添加按钮会创建多个相同元素。
- 导出分辨率使用当前实现中的设置，不提供 DPI 输入框。
- 复杂 MXD、特殊字体或打印机驱动可能造成不同机器上的排版差异。

## 学习重点

重点阅读地图框同步、`IMapSurroundFrame`、`IGraphicsContainer`、`IExport` 和设备矩形换算代码。这些是 ArcObjects 制图输出的核心接口。
