using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
// 1. 确保引用了这个命名空间
using ESRI.ArcGIS;

namespace RasterAnalysisSystem
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ==========================================================
            // 【关键修改】必须放在所有代码的第一行！
            // 告诉程序使用 Engine 许可。如果你装的是 Desktop，也可以写 ProductCode.EngineOrDesktop
            if (!RuntimeManager.Bind(ProductCode.Engine))
            {
                if (!RuntimeManager.Bind(ProductCode.Desktop))
                {
                    MessageBox.Show("无法绑定 ArcGIS 许可，请确认已安装 ArcGIS Engine 或 Desktop。");
                    return;
                }
            }
            // ==========================================================

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}