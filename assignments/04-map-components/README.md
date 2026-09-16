# 作业 4：地图组件与属性操作

## 已实现功能

- 加载 Shapefile、删除首个图层、全图显示、放大、缩小和漫游
- 主地图与鹰眼图联动，鹰眼中显示当前视图范围
- 按 `NAME` 字段精确查询并定位要素
- 拉框选择要素
- 从图层右键菜单打开属性表

原说明曾写到可加载 TIF、IMG 等栅格，但源码的“添加图层”功能只使用 Shapefile Workspace，因此 README 按实际实现更正为仅加载 SHP。查询数据若没有 `NAME` 字段，程序会给出提示。

打开 `GISDev4.sln`，在 ArcGIS Engine 10.2、.NET Framework 4.0、x86 环境下编译。
