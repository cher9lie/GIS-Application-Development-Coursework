# 作业 9：栅格分析

程序可加载 TIF、IMG、JPG 栅格，执行坡度、坡向分析，点击查询像元值，生成等值线，并按阈值提取栅格。程序在可用时读取栅格统计信息，但没有实现原说明中所称的独立“统计面板”或自定义拉伸渲染器。

`RasterAnalysisSystem/Data` 附有课程实验 DEM。打开 `RasterAnalysisSystem.sln`，使用 ArcGIS Engine 10.2、.NET Framework 4.0、x86 编译；运行坡度、坡向、等值线和提取功能需要 Spatial Analyst 扩展许可。

等值距必须为正数，阈值必须为有效数字。分析结果由 ArcObjects 生成，运行时请把输出保存到有写权限的位置。仓库不保留 `bin/Debug` 中旧的派生栅格和编译产物。
