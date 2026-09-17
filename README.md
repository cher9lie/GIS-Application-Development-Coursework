# GIS 应用开发课程作业（Visual Studio 2010 / ArcGIS Engine 10.2）

本仓库整理了 GIS 应用开发课程的 9 次作业和 1 个综合实习项目。项目保留课程原有技术栈，没有升级到新版 Visual Studio、.NET 或 ArcGIS SDK，目的是让下一届在相同实验环境中能够直接打开、编译和学习。

所有说明均根据当前源码重新核对。原 Word/PDF 报告中的姓名、学号、教师等个人信息未保留；与源码不一致的描述已更正。原提交 ZIP、报告文件和编译输出不上传 GitHub。

## 内容导航

| 目录 | 主题 | 主要技术点 | 是否附示例数据 |
| --- | --- | --- | --- |
| [01-scientific-calculator](assignments/01-scientific-calculator/) | 科学计算器 | WinForms 事件、状态管理、数值校验 | 不需要 |
| [02-map-browser](assignments/02-map-browser/) | 地图浏览器 | MapControl、TOCControl、ToolbarControl | 不附带 |
| [03-map-operations](assignments/03-map-operations/) | 地图基本操作 | MXD 加载、范围操作、空间识别 | 不附带 |
| [04-map-components](assignments/04-map-components/) | 地图组件与属性操作 | SHP、鹰眼、查询、框选、属性表 | 不附带 |
| [05-geometry-spatial-analysis](assignments/05-geometry-spatial-analysis/) | 几何与空间分析 | Densify、动态显示、前方交会 | 已附 |
| [06-map-symbolization](assignments/06-map-symbolization/) | 地图符号化 | 唯一值、图表、分级、通用符号 | 不附带 |
| [07-map-layout-export](assignments/07-map-layout-export/) | 地图整饰与输出 | PageLayout、图例、比例尺、导出、打印 | 不附带 |
| [08-spatial-query-analysis](assignments/08-spatial-query-analysis/) | 空间查询与分析 | 属性查询、空间过滤、GP Buffer | 不附带 |
| [09-raster-analysis](assignments/09-raster-analysis/) | 栅格分析 | 坡度、坡向、像元、等值线、提取 | 已附 DEM |
| [final-project](final-project/) | 福田 GIS 系统 | 数据加载、查询统计、编辑、缓冲区、路径分析 | 工程内原有数据 |

## 统一运行环境

建议使用与课程一致的环境：

- Windows 7/10 时代的 x86 桌面环境；当前也可在安装了旧组件的兼容 Windows 环境中运行
- Microsoft Visual Studio 2010
- .NET Framework 4.0
- ArcGIS Engine 10.2 Runtime
- ArcObjects / ArcGIS Engine 10.2 Developer Kit
- 目标平台：x86

作业 1 是普通 WinForms 程序，不需要 ArcGIS。作业 9 需要 Spatial Analyst 扩展。综合实习的路径分析需要 Network Analyst 扩展，并且地图中必须事先存在配置正确的路径分析图层。

## 获取与编译

1. 克隆或下载本仓库。
2. 进入对应作业目录，用 Visual Studio 2010 打开 README 中指定的 `.sln`。
3. 在“配置管理器”中确认平台为 `x86`，不要改成 `Any CPU` 或 `x64`。
4. 检查 ArcObjects 引用是否正常加载。
5. 先执行“生成解决方案”，再运行程序。
6. 如果项目需要数据，按对应 README 的“操作步骤”加载数据。

也可以在安装了 .NET Framework 4.0 开发工具的命令行中编译：

```powershell
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe 项目.sln /t:Rebuild /p:Configuration=Release /p:Platform=x86
```

本次整理已经用上述 MSBuild 对 10 个解决方案逐一进行 `Release | x86` 重建，均编译通过。作业 2、3、8 可能出现 CS1762 互操作引用警告，这是 ArcGIS 10.2 与旧式 COM 程序集组合时的常见现象，本次构建不受影响。

## ArcObjects 引用异常时怎么处理

如果 Visual Studio 中出现黄色感叹号、找不到 `ESRI.ArcGIS.*` 或窗体设计器打不开，可依次检查：

1. ArcGIS Engine 10.2 Runtime 与 Developer Kit 是否完整安装。
2. 项目平台是否为 x86。
3. 引用属性中的“嵌入互操作类型”是否为 `False`。
4. ActiveX 控件是否已注册，工具箱中能否看到 MapControl、TOCControl、ToolbarControl、PageLayoutControl 和 LicenseControl。
5. 本机是否有可用的 Engine 或 Desktop 许可。
6. 若引用路径失效，删除失效引用后，从 ArcGIS 10.2 安装目录或 GAC 重新添加同名 10.2 程序集，不要混用其他版本。

不要让 Visual Studio 自动迁移项目到更高版本后再提交给使用 VS2010 的同学；升级后的项目文件、语言特性或 NuGet 依赖可能无法在原环境打开。

## 数据使用约定

- Shapefile 必须整套复制，至少应同时保留 `.shp`、`.shx`、`.dbf`，有投影时还应保留 `.prj`。
- MXD 只保存图层引用，不一定包含数据。若打开 MXD 后图层出现红色感叹号，需要重新指定数据源。
- 涉及距离和面积的分析应使用合适的投影坐标系。经纬度坐标直接做“500 米缓冲”等操作可能得到错误结果。
- 示例数据仅用于教学演示。编辑类功能会直接修改数据，建议先复制一份再练习。
- 作业 6、8 对字段名有明确要求，详见各自 README。

## 仓库整理记录

整理过程遵循“先保留原版，再修复”的顺序：

- `98cc65a`：原始源码基线
- `3820916`：目录整理、文档校正、示例数据补充及兼容性修复
- 原始压缩包另行保存在本地，不上传 GitHub

本次修复没有更改 VS2010、.NET 4.0、x86 和 ArcGIS 10.2 的版本约束。具体来源和快照说明见 [ORIGINAL_SNAPSHOT.md](ORIGINAL_SNAPSHOT.md)。

## 常见问题

### 程序启动后立即提示许可失败

先打开 ArcGIS Administrator，确认许可可用；再确认程序与 Runtime 位数一致。作业 9 和综合实习还要检查对应扩展许可。

### 编译成功，但运行时找不到数据

优先检查 MXD 中的数据源是否失效。对于未附示例数据的作业，可加载自己准备的 SHP 或 MXD，但字段结构必须符合 README 的要求。

### 更换数据后查询或专题图没有反应

这是最常见的问题，通常不是控件失效，而是字段名不一致。例如作业 8 使用 `NAME`，作业 6 使用 `CONTINENT`、`SQMI`、`SQKM`。应先在 ArcMap 属性表中核对字段名。

### 为什么不上传原 ZIP 和报告

ZIP 与源码重复，Word/PDF 报告主要用于当时提交和教师查阅，并含个人信息。仓库改用与源码对应的 README，便于维护和公开分享。
