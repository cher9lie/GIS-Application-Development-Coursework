using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem; // 引用这是必须的

// 注意：这里要改成你当前项目的名字，根据报错看应该是 GISDev7
namespace GISDev7
{
    static class Program
    {
        private static IAoInitialize m_AoInitialize = null;

        [STAThread]
        static void Main()
        {
            // ====================================================================
            // 修复核心：在创建窗体之前，先绑定 ArcGIS 运行时
            // ====================================================================
            if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("无法绑定 ArcGIS 运行时，请确认已安装 ArcGIS。");
                return;
            }

            // ====================================================================
            // 许可初始化 (双保险：防止 AxLicenseControl 有时失效)
            // ====================================================================
            try
            {
                m_AoInitialize = new AoInitializeClass();

                // === 修改测试：直接初始化 Engine 许可，不试探 Advanced 了 ===
                // 很多实验环境只配置了 Engine 的许可
                esriLicenseStatus licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngine);

                // 如果 Engine 不行，再试试 EngineGeoDB (有的破解版是这个)
                if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                {
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB);
                }

                // 如果还是不行，才试 Advanced (桌面版许可)
                if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                {
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
                }

                if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                {
                    MessageBox.Show("许可初始化失败，状态: " + licenseStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("许可初始化异常: " + ex.Message);
            }

            // ====================================================================
            // 启动程序
            // ====================================================================
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1()); // 确保这里是 Form1

            // 程序退出时释放许可
            if (m_AoInitialize != null)
            {
                m_AoInitialize.Shutdown();
            }
        }
    }
}