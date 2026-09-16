# GIS 应用开发课程作业（VS2010 / ArcGIS Engine 10.2）

这是一套按课程顺序整理的旧版 GIS 桌面开发示例。源码仍以 Visual Studio 2010、.NET Framework 4.0、x86 和 ArcGIS Engine / ArcObjects 10.2 为目标，没有升级项目格式或依赖版本，便于下一届在相同实验环境中直接打开。

| 内容 | 主要功能 |
| --- | --- |
| [作业 1：科学计算器](assignments/01-scientific-calculator/) | 四则运算、百分比、三角与常用科学函数 |
| [作业 2：地图浏览器](assignments/02-map-browser/) | MapControl、TOCControl、ToolbarControl、LicenseControl 的基础组合 |
| [作业 3：地图操作](assignments/03-map-operations/) | 打开 MXD、缩放、漫游、要素识别 |
| [作业 4：地图组件](assignments/04-map-components/) | 图层管理、鹰眼、属性查询、框选、属性表 |
| [作业 5：几何与空间分析](assignments/05-geometry-spatial-analysis/) | 动态车辆、折线加密、前方交会 |
| [作业 6：地图符号化](assignments/06-map-symbolization/) | 单一、唯一值、柱状图、饼图、分级符号 |
| [作业 7：地图整饰与输出](assignments/07-map-layout-export/) | 图例、指北针、比例尺、标题、导出与打印 |
| [作业 8：空间查询与分析](assignments/08-spatial-query-analysis/) | 属性查询、空间选择、缓冲区 |
| [作业 9：栅格分析](assignments/09-raster-analysis/) | 坡度、坡向、像元查询、等值线、阈值提取 |
| [综合实习：福田 GIS 系统](final-project/) | 数据加载、查询统计、编辑、缓冲区、路径分析、PDF 输出 |

## 运行环境

1. 安装 Visual Studio 2010、.NET Framework 4.0 和 ArcGIS Engine 10.2 Developer Kit。
2. 确认 ArcGIS Engine 许可可用；作业 9 还需要 Spatial Analyst，综合实习的路径分析需要 Network Analyst。
3. 用各目录中的 `.sln` 打开工程，选择 `Release | x86` 或 `Debug | x86` 后编译。
4. 若 ArcObjects 引用显示异常，重新从 ArcGIS 10.2 安装目录或 GAC 添加同名程序集，并保持“嵌入互操作类型”为 `False`。

本次整理已在安装 ArcGIS 10.2 的 Windows 环境中用 .NET 4.0 MSBuild 逐个编译。部分项目会出现 ArcObjects 嵌入互操作相关的 CS1762 警告，这是旧版程序集的常见警告，不影响本次编译结果。

## 整理原则

原始源码先保存为 Git 基线提交 `98cc65a`，之后才进行修复和文档改写。原提交压缩包、Word/PDF 报告和生成目录不上传；README 已去除姓名、学号、教师等个人信息，并以当前源码为准校正文档。详情见 [原始材料说明](ORIGINAL_SNAPSHOT.md)。
