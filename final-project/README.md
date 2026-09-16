# 综合实习：福田 GIS 系统

这是课程综合项目，使用 ArcGIS Engine 10.2 和 WinForms 实现一个桌面 GIS 原型。

## 功能

- 加载 Shapefile、Personal Geodatabase（MDB）和 File Geodatabase（GDB）中的要素类
- 按字段进行唯一值渲染
- 点击识别、属性查询、拉框空间选择、属性统计
- 根据选中要素绘制缓冲区，并查询缓冲区内要素
- 新增点要素及基础编辑操作
- 在地图已有 Network Analyst 路径图层时设置起终点并求解路线
- 将当前地图导出为 PDF

登录界面使用课程演示账号 `admin` / `123`，只是本地硬编码入口，不具备真实身份认证能力。

## 运行条件与限制

打开 `FuTianGIS/FuTianGIS.sln`，使用 Visual Studio 2010、.NET Framework 4.0、x86 和 ArcGIS Engine 10.2 编译。普通功能需要 Engine 许可；路径分析还需要 Network Analyst 扩展，并要求当前 MXD 已配置可用的路径分析图层、Stops 和 Routes 类。

加载 GDB 时请选择数据库文件夹；程序读取其根目录中的要素类，不遍历要素数据集。MDB 和 SHP 通过文件选择器加载。编辑功能会直接改写所选数据，请先使用副本练习。
