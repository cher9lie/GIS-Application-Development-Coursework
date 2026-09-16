using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS.esriSystem; // 核心引用

namespace GISDev5 // <--- 确认这里是你项目的名字
{
    static class Program
    {
        private static IAoInitialize m_AoInitialize = null;

        [STAThread]
        static void Main()
        {
            // 1. 绑定产品 (这一步仍然需要)
            if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("无法绑定 ArcGIS 运行时，请确认已安装 ArcGIS。");
                return;
            }

            // 2. 【关键修正】 显式初始化许可
            // 这就像是进门前先要把门票（许可）检票，进去后才能玩项目（读数据）
            try
            {
                m_AoInitialize = new AoInitializeClass();
                esriLicenseStatus licenseStatus = esriLicenseStatus.esriLicenseUnavailable;

                // 就像你代码里写的，从高级到低级依次尝试获取许可
                if (m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeAdvanced) == esriLicenseStatus.esriLicenseAvailable)
                {
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeAdvanced);
                }
                else if (m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeStandard) == esriLicenseStatus.esriLicenseAvailable)
                {
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeStandard);
                }
                else if (m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeBasic) == esriLicenseStatus.esriLicenseAvailable)
                {
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);
                }
                else if (m_AoInitialize.IsProductCodeAvailable(esriLicenseProductCode.esriLicenseProductCodeEngine) == esriLicenseStatus.esriLicenseAvailable)
                {
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngine);
                }

                if (licenseStatus != esriLicenseStatus.esriLicenseCheckedOut)
                {
                    MessageBox.Show("许可初始化失败，状态: " + licenseStatus);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("许可初始化异常: " + ex.Message);
                return;
            }

            // 3. 启动窗体
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // 4. 程序退出时释放许可 (好习惯)
            if (m_AoInitialize != null)
            {
                m_AoInitialize.Shutdown();
            }
        }
    }
}