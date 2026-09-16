using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS; // 1. 添加这行引用

namespace MapSymbolizationLab
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 2. 必须在 Application.EnableVisualStyles() 之前添加下面这行！
            // 这句话的意思是：绑定到 Engine 或者 Desktop 产品，谁有许可就用谁
            if (!RuntimeManager.Bind(ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("无法绑定到 ArcGIS 运行时，请确保安装了 ArcGIS 并且许可可用。");
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}