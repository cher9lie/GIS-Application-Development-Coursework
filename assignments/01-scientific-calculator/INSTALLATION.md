# 安装和使用指南

## 系统要求

### 运行要求
- **操作系统**: Windows XP SP3 或更高版本
- **运行时**: .NET Framework 4.0 Client Profile 或完整版本
- **内存**: 至少128MB可用内存
- **硬盘空间**: 约10MB

### 开发要求
- **IDE**: Visual Studio 2010 或更高版本
- **SDK**: .NET Framework 4.0 SDK
- **操作系统**: Windows XP SP3 或更高版本

## 安装步骤

### 对于最终用户

1. **检查系统要求**
   - 确保安装了 .NET Framework 4.0
   - 如果没有，请从Microsoft官网下载并安装

2. **获取程序文件**
   - 下载编译好的可执行文件
   - 或者从源代码编译（见开发者安装步骤）

3. **运行程序**
   - 双击 `ScientificCalculator.exe` 启动计算器
   - 首次运行可能需要几秒钟加载时间

### 对于开发者

1. **克隆或下载源代码**
   ```bash
   git clone [repository-url]
   ```

2. **打开项目**
   - 启动 Visual Studio 2010
   - 打开 `ScientificCalculator.sln` 解决方案文件

3. **检查项目设置**
   - 确认目标框架为 .NET Framework 4.0
   - 确认平台目标为 x86

4. **编译项目**
   - 按 `Ctrl+Shift+B` 或选择"生成">"生成解决方案"
   - 检查输出窗口确认无错误

5. **运行程序**
   - 按 `F5` 调试运行
   - 或按 `Ctrl+F5` 直接运行

## 文件说明

### 核心文件
- `ScientificCalculator.exe` - 主程序可执行文件
- `ScientificCalculator.exe.config` - 配置文件（如果存在）

### 源代码文件
- `Program.cs` - 程序入口点
- `CalculatorForm.cs` - 主窗体逻辑代码
- `CalculatorForm.Designer.cs` - 窗体设计器生成的代码
- `CalculatorForm.resx` - 窗体资源文件

### 项目文件
- `ScientificCalculator.sln` - Visual Studio解决方案文件
- `ScientificCalculator.csproj` - C#项目文件
- `AssemblyInfo.cs` - 程序集信息

## 故障排除

### 常见问题

**问题1: 程序无法启动**
- **症状**: 双击程序文件无反应或出现错误
- **解决**: 确保安装了.NET Framework 4.0
- **检查方法**: 
  - 控制面板 > 程序和功能 > 查看已安装的更新
  - 寻找 "Microsoft .NET Framework 4"

**问题2: 缺少.NET Framework**
- **症状**: 提示"需要.NET Framework"
- **解决**: 下载并安装.NET Framework 4.0
- **下载地址**: Microsoft官方网站

**问题3: 在Visual Studio中编译失败**
- **症状**: 编译时出现MSBuild错误
- **解决**: 
  - 确认VS2010已正确安装
  - 检查项目文件路径中是否包含特殊字符
  - 尝试"清理解决方案"然后重新生成

**问题4: 界面显示异常**
- **症状**: 按钮位置错乱或文字显示不正常
- **解决**: 
  - 检查系统DPI设置（推荐100%）
  - 确保使用标准Windows主题

### 性能优化

**启动优化**
- 将程序添加到Windows启动优化列表
- 确保有足够的系统内存

**运行优化**
- 避免同时运行大量其他程序
- 定期清理系统临时文件

## 卸载说明

### 完全卸载
1. 删除程序文件夹
2. 清理注册表项（如果有）
3. 删除用户设置文件（通常在用户配置文件目录）

程序不会在注册表中创建大量条目，主要文件都在程序安装目录中。

## 升级说明

### 升级到新版本
1. 备份当前版本（如果需要）
2. 下载新版本文件
3. 替换旧版本文件
4. 运行新版本进行测试

### 兼容性说明
- 向后兼容：新版本通常兼容旧版本的设置
- 向前兼容：旧版本可能无法读取新版本的设置文件

## 技术支持

如遇到问题，请检查：
1. 系统是否满足最低要求
2. .NET Framework是否正确安装
3. 是否有足够的系统权限
4. 防病毒软件是否阻止程序运行

## 许可证信息

本软件采用开源许可证，详细信息请查看LICENSE文件。