using System;
using System.Windows.Forms;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;

namespace FuTianGIS
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        internal static bool NetworkAnalysisAvailable { get; private set; }

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
                    MessageBox.Show("无法绑定 ArcGIS Engine 或 Desktop 10.2 运行时。", "许可错误");
                    return;
                }
                license = new AoInitializeClass();
                esriLicenseProductCode[] products = engine
                    ? new[] { esriLicenseProductCode.esriLicenseProductCodeEngineGeoDB,
                              esriLicenseProductCode.esriLicenseProductCodeEngine }
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
                    MessageBox.Show("无法初始化 ArcGIS 产品许可。", "许可错误");
                    return;
                }
                NetworkAnalysisAvailable = license.CheckOutExtension(esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork) ==
                    esriLicenseStatus.esriLicenseCheckedOut;
                if (!NetworkAnalysisAvailable)
                    MessageBox.Show("Network Analyst 扩展许可不可用，路径分析功能将不可用。", "许可提示");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                using (MainForm form = new MainForm())
                {
                    Application.Run(form);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("启动或运行失败：" + ex.Message, "ArcGIS");
            }
            finally
            {
                try
                {
                    if (NetworkAnalysisAvailable)
                        license.CheckInExtension(esriLicenseExtensionCode.esriLicenseExtensionCodeNetwork);
                    NetworkAnalysisAvailable = false;
                }
                finally
                {
                    if (initialized)
                        license.Shutdown();
                }
            }
        }

    }
}
