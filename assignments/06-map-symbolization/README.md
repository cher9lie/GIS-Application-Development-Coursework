# 作业 6：地图符号化与专题图

## 作业目标

本作业练习 ArcObjects 的 Renderer 与 Symbol 体系，在同一个要素图层上应用单一符号、唯一值、图表和分级符号化。

## 环境与入口

- Visual Studio 2010
- .NET Framework 4.0
- ArcGIS Engine 10.2
- 目标平台：x86
- 解决方案：`MapSymbolizationLab.sln`
- 主要代码：`MapSymbolizationLab/Form1.cs`

程序使用 `RuntimeManager.Bind(ProductCode.EngineOrDesktop)` 绑定可用运行时。

## 已实现功能

### 打开地图

“打开地图”选择并加载 MXD。专题渲染按钮默认从当前地图中取得要素图层。

### 单值专题图

按钮名称为“单值专题图”，源码实际使用 `UniqueValueRenderer`，按 `CONTINENT` 字段为不同大洲设置不同填充颜色，并设置默认符号。

### 柱状专题图

使用 `ChartRenderer` 和 `BarChartSymbol`，字段固定为：

- `SQMI`
- `SQKM`

最大值在源码中设为 `40000000`，适合课程示例世界数据，不会根据任意数据自动计算。

### 饼状专题图

同样使用 `SQMI` 和 `SQKM`，以 `PieChartSymbol` 显示两个字段的相对构成。

### 分级专题图

按 `SQKM` 字段使用 `ClassBreaksRenderer`，固定分为 5 级。分级断点与颜色在源码中预设，不是动态自然断点分类。

### 通用符号化

遍历地图中的要素图层，根据几何类型选择基础符号：

- 点：Marker Symbol
- 线：Line Symbol
- 面：Fill Symbol

“通用”指按点、线、面适配符号类型，并不表示自动识别任意专题字段。

## 推荐操作步骤

1. 准备包含 `CONTINENT`、`SQMI`、`SQKM` 字段的课程示例面数据，并制作 MXD。
2. 启动程序，点击“打开地图”加载该 MXD。
3. 依次测试单值、柱状、饼状和分级专题图。
4. 每次渲染后观察 TOC 符号和地图变化。
5. 再加载点或线数据，测试“通用符号化”。

## 更换数据时必须修改的内容

如果自己的数据没有上述字段，应在 `Form1.cs` 中同步修改：

- `UniqueValueRenderer.set_Field`
- `IRendererFields.AddField`
- `ClassBreaksRenderer.Field`
- 唯一值列表
- 图表最大值
- 分级断点

只替换 MXD 而不改字段名，通常会报错或得到空白专题图。

## 学习重点

- `IGeoFeatureLayer.Renderer` 是渲染器应用到图层的入口
- Renderer 决定如何按属性分类
- Symbol 决定点、线、面的实际视觉样式
- 渲染后需要刷新地图和 TOC
- 字段类型、取值范围和几何类型必须与渲染器匹配

## 已知限制

这是针对固定实验数据的教学实现，没有字段选择界面、自动统计最大值、动态生成分类断点或保存渲染方案。原报告若把专题图描述为“任意数据自动适配”，并不符合源码。
