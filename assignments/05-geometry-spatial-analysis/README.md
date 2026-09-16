# 作业 5：几何与空间分析

本作业包含三组实验：

- 动态车辆沿消防路线移动，并在到达终点后重新开始
- 对折线执行 Densify 加密并绘制结果
- 以折线首、尾点为观测点，输入距离和方位角进行前方交会

`GISDev5/Data` 中附有原实验的 `FireResponse.mxd`、消防路线和瞭望塔 Shapefile。程序优先读取输出目录下的示例 MXD；找不到时会让使用者选择 MXD，若 MXD 的数据源失效则尝试加载随工程复制的 `FireLine.shp`。这替代了原来写死的 `D:\Data\FireResponse.mxd` 路径。

打开 `GISDev5.sln`，使用 ArcGIS Engine 10.2、.NET Framework 4.0、x86 编译。前方交会中的角度单位为度，距离单位与地图坐标系一致。
