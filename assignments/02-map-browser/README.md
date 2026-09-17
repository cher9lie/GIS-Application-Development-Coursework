# 作业 2：ArcGIS Engine 地图浏览器

## 作业目标

本作业用于认识 ArcGIS Engine 在 WinForms 中的基础控件，以及控件之间的绑定关系。它是后续地图操作、查询、制图和空间分析作业的框架起点。

## 环境与入口

- Visual Studio 2010
- .NET Framework 4.0
- ArcGIS Engine / ArcObjects 10.2
- 目标平台：x86
- 解决方案：`GISDev/GISDev.sln`

## 界面组成

窗体主要由四类 ArcGIS Engine ActiveX 控件组成：

- `AxMapControl`：显示地图和图层
- `AxTOCControl`：显示图层目录、符号和可见性
- `AxToolbarControl`：承载 ArcGIS 内置地图工具
- `AxLicenseControl`：协助初始化运行许可

该工程主要通过 Visual Studio 设计器配置控件和工具条，没有额外的业务逻辑代码。这一点与后续作业不同：它展示的是“先把 Engine 控件正确放到窗体中并连接起来”。

## 运行步骤

1. 安装 ArcGIS Engine 10.2 Runtime 和 Developer Kit。
2. 打开 `GISDev/GISDev.sln`。
3. 确认解决方案平台为 x86。
4. 如果设计器提示 ActiveX 控件无法创建，先检查 Engine 控件是否已注册。
5. 生成并运行程序。
6. 通过工具条中的内置命令加载地图或数据。
7. 在 MapControl 中浏览地图，在 TOCControl 中查看和切换图层。

仓库不附带本作业的默认 MXD，因此运行后需要自行准备可用的地图文档或数据。

## 学习重点

- MapControl 与 TOCControl 的伙伴控件关系
- ToolbarControl 如何复用 ArcGIS 内置命令
- ActiveX 控件在 `.Designer.cs` 和 `.resx` 中的序列化方式
- ArcGIS Engine 应用为什么必须使用正确的运行时和位数

## 常见问题

### 窗体设计器显示白屏或 ActiveX 错误

通常是 ArcGIS Engine Developer Kit 未安装、控件未注册，或者工程用 x64/Any CPU 打开。先改回 x86，再检查安装。

### 工具条存在但按钮无效

确认 ToolbarControl 的 BuddyControl 指向 MapControl，并确认运行时许可已初始化。

### 引用出现黄色感叹号

重新添加 ArcGIS 10.2 的程序集引用，并把“嵌入互操作类型”设为 `False`。不要引用其他 ArcGIS 版本的 DLL。
