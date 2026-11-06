using System;
using System.Windows.Forms;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;

namespace GISDev7
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            IAoInitialize license = null;
            bool initialized = false;
            try
            {
                bool engine = RuntimeManager.Bind(ProductCode.Engine);
                if (!engine && !RuntimeManager.Bind(ProductCode.Desktop))
                {
                    MessageBox.Show("无法绑定 ArcGIS 运行时。请确认已安装 ArcGIS Engine 或 Desktop 10.2。", "ArcGIS 许可");
                    return;
                }

                license = new AoInitializeClass();
                esriLicenseProductCode[] products = engine
                    ? new[] { esriLicenseProductCode.esriLicenseProductCodeEngine,
                              esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB }
                    : new[] { esriLicenseProductCode.esriLicenseProductCodeBasic,
                              esriLicenseProductCode.esriLicenseProductCodeStandard,
                              esriLicenseProductCode.esriLicenseProductCodeAdvanced };

                foreach (esriLicenseProductCode product in products)
                {
                    if (license.IsProductCodeAvailable(product) != esriLicenseStatus.esriLicenseAvailable)
                        continue;

                    initialized = license.Initialize(product) == esriLicenseStatus.esriLicenseCheckedOut;
                    if (initialized)
                        break;
                }

                if (!initialized)
                {
                    MessageBox.Show("无法初始化 ArcGIS 产品许可。请在 ArcGIS Administrator 中检查授权。", "ArcGIS 许可");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (Form1 form = new Form1())
                {
                    Application.Run(form);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序启动或运行失败：" + ex.Message, "ArcGIS");
            }
            finally
            {
                if (initialized)
                    license.Shutdown();
            }
        }
    }
}
