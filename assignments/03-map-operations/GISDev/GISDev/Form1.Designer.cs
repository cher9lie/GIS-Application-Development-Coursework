namespace GISDev
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlbOpen = new System.Windows.Forms.ToolStripButton();
            this.tlbGlobal = new System.Windows.Forms.ToolStripButton();
            this.tlbZoomIn = new System.Windows.Forms.ToolStripButton();
            this.tlbZoomOut = new System.Windows.Forms.ToolStripButton();
            this.tlbPan = new System.Windows.Forms.ToolStripButton();
            this.tlbIdentify = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(13, 28);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 0;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(13, 75);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(226, 654);
            this.axTOCControl1.TabIndex = 2;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(245, 75);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(615, 654);
            this.axMapControl1.TabIndex = 3;
            this.axMapControl1.OnMouseDown += new ESRI.ArcGIS.Controls.IMapControlEvents2_Ax_OnMouseDownEventHandler(this.axMapControl1_OnMouseDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbOpen,
            this.tlbGlobal,
            this.tlbZoomIn,
            this.tlbZoomOut,
            this.tlbPan,
            this.tlbIdentify});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(894, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlbOpen
            // 
            this.tlbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tlbOpen.Image")));
            this.tlbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbOpen.Name = "tlbOpen";
            this.tlbOpen.Size = new System.Drawing.Size(23, 22);
            this.tlbOpen.Text = "tlbOpen";
            this.tlbOpen.ToolTipText = "打开图层";
            this.tlbOpen.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tlbGlobal
            // 
            this.tlbGlobal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbGlobal.Image = ((System.Drawing.Image)(resources.GetObject("tlbGlobal.Image")));
            this.tlbGlobal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbGlobal.Name = "tlbGlobal";
            this.tlbGlobal.Size = new System.Drawing.Size(23, 22);
            this.tlbGlobal.Text = "tlbGlobal";
            this.tlbGlobal.ToolTipText = "全屏";
            this.tlbGlobal.Click += new System.EventHandler(this.tlbGloble_Click);
            // 
            // tlbZoomIn
            // 
            this.tlbZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("tlbZoomIn.Image")));
            this.tlbZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbZoomIn.Name = "tlbZoomIn";
            this.tlbZoomIn.Size = new System.Drawing.Size(23, 22);
            this.tlbZoomIn.Text = "tlbZoomIn";
            this.tlbZoomIn.ToolTipText = "放大";
            this.tlbZoomIn.Click += new System.EventHandler(this.tlbZoomIn_Click);
            // 
            // tlbZoomOut
            // 
            this.tlbZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("tlbZoomOut.Image")));
            this.tlbZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbZoomOut.Name = "tlbZoomOut";
            this.tlbZoomOut.Size = new System.Drawing.Size(23, 22);
            this.tlbZoomOut.Text = "tlbZoomOut";
            this.tlbZoomOut.ToolTipText = "缩小";
            this.tlbZoomOut.Click += new System.EventHandler(this.tlbZoomOut_Click);
            // 
            // tlbPan
            // 
            this.tlbPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbPan.Image = ((System.Drawing.Image)(resources.GetObject("tlbPan.Image")));
            this.tlbPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbPan.Name = "tlbPan";
            this.tlbPan.Size = new System.Drawing.Size(23, 22);
            this.tlbPan.Text = "tlbPan";
            this.tlbPan.ToolTipText = "漫游";
            this.tlbPan.Click += new System.EventHandler(this.tlbPan_Click);
            // 
            // tlbIdentify
            // 
            this.tlbIdentify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbIdentify.Image = ((System.Drawing.Image)(resources.GetObject("tlbIdentify.Image")));
            this.tlbIdentify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbIdentify.Name = "tlbIdentify";
            this.tlbIdentify.Size = new System.Drawing.Size(23, 22);
            this.tlbIdentify.Text = "toolStripButton1";
            this.tlbIdentify.ToolTipText = "属性查询";
            this.tlbIdentify.Click += new System.EventHandler(this.tlbIdentify_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 761);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Name = "Form1";
            this.Text = "MapViewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlbOpen;
        private System.Windows.Forms.ToolStripButton tlbGlobal;
        private System.Windows.Forms.ToolStripButton tlbZoomIn;
        private System.Windows.Forms.ToolStripButton tlbZoomOut;
        private System.Windows.Forms.ToolStripButton tlbPan;
        private System.Windows.Forms.ToolStripButton tlbIdentify;
    }
}

