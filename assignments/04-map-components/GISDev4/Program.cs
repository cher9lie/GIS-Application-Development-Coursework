using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;


namespace GISDev4
{
    static class Program
    {
        private static IAoInitialize m_AoInitialize = null;

        [STAThread]
        static void Main()
        {
            // 1. 绑定产品 (你已经做过的)
            if (!ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop))
            {
                MessageBox.Show("无法绑定 ArcGIS 运行时，请确认已安装 ArcGIS。");
                return;
            }

            // 2. 【关键新增步骤】 初始化具体许可
            // 没有这一步，打开 Shapefile 有时会失败
            try
            {
                m_AoInitialize = new AoInitializeClass();
                esriLicenseStatus licenseStatus = esriLicenseStatus.esriLicenseUnavailable;

                // 尝试初始化不同级别的许可 (高级 -> 标准 -> 基础 -> Engine)
                // 只要有一个成功即可
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
                    licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic); // 对应 ArcView
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

            // 4. 程序退出时释放许可
            if (m_AoInitialize != null)
            {
                m_AoInitialize.Shutdown();
            }
        }
    }
}
