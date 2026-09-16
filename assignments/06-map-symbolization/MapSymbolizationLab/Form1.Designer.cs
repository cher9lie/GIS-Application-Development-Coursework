namespace MapSymbolizationLab
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
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.btnUniqueValue = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenMxd = new System.Windows.Forms.Button();
            this.btnBarChart = new System.Windows.Forms.Button();
            this.btnPieChart = new System.Windows.Forms.Button();
            this.btnGraduated = new System.Windows.Forms.Button();
            this.btnAutoSymbol = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(12, 65);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(705, 632);
            this.axMapControl1.TabIndex = 0;
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1434, 28);
            this.axToolbarControl1.TabIndex = 1;
            // 
            // btnUniqueValue
            // 
            this.btnUniqueValue.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUniqueValue.Location = new System.Drawing.Point(1116, 386);
            this.btnUniqueValue.Name = "btnUniqueValue";
            this.btnUniqueValue.Size = new System.Drawing.Size(279, 56);
            this.btnUniqueValue.TabIndex = 3;
            this.btnUniqueValue.Text = "单值专题图";
            this.btnUniqueValue.UseVisualStyleBackColor = true;
            this.btnUniqueValue.Click += new System.EventHandler(this.btnUniqueValue_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnOpenMxd
            // 
            this.btnOpenMxd.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenMxd.Location = new System.Drawing.Point(1116, 303);
            this.btnOpenMxd.Name = "btnOpenMxd";
            this.btnOpenMxd.Size = new System.Drawing.Size(279, 56);
            this.btnOpenMxd.TabIndex = 7;
            this.btnOpenMxd.Text = "打开地图";
            this.btnOpenMxd.UseVisualStyleBackColor = true;
            this.btnOpenMxd.Click += new System.EventHandler(this.btnOpenMxd_Click);
            // 
            // btnBarChart
            // 
            this.btnBarChart.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBarChart.Location = new System.Drawing.Point(1116, 719);
            this.btnBarChart.Name = "btnBarChart";
            this.btnBarChart.Size = new System.Drawing.Size(279, 56);
            this.btnBarChart.TabIndex = 8;
            this.btnBarChart.Text = "柱状专题图";
            this.btnBarChart.UseVisualStyleBackColor = true;
            this.btnBarChart.Click += new System.EventHandler(this.btnBarChart_Click);
            // 
            // btnPieChart
            // 
            this.btnPieChart.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPieChart.Location = new System.Drawing.Point(1116, 801);
            this.btnPieChart.Name = "btnPieChart";
            this.btnPieChart.Size = new System.Drawing.Size(279, 56);
            this.btnPieChart.TabIndex = 9;
            this.btnPieChart.Text = "饼状专题图";
            this.btnPieChart.UseVisualStyleBackColor = true;
            this.btnPieChart.Click += new System.EventHandler(this.btnPieChart_Click);
            // 
            // btnGraduated
            // 
            this.btnGraduated.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGraduated.Location = new System.Drawing.Point(1116, 879);
            this.btnGraduated.Name = "btnGraduated";
            this.btnGraduated.Size = new System.Drawing.Size(279, 56);
            this.btnGraduated.TabIndex = 10;
            this.btnGraduated.Text = "分级专题图";
            this.btnGraduated.UseVisualStyleBackColor = true;
            this.btnGraduated.Click += new System.EventHandler(this.btnGraduated_Click);
            // 
            // btnAutoSymbol
            // 
            this.btnAutoSymbol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAutoSymbol.Location = new System.Drawing.Point(1172, 555);
            this.btnAutoSymbol.Name = "btnAutoSymbol";
            this.btnAutoSymbol.Size = new System.Drawing.Size(162, 54);
            this.btnAutoSymbol.TabIndex = 11;
            this.btnAutoSymbol.Text = "通用符号化";
            this.btnAutoSymbol.UseVisualStyleBackColor = true;
            this.btnAutoSymbol.Click += new System.EventHandler(this.btnAutoSymbol_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1434, 1117);
            this.Controls.Add(this.btnAutoSymbol);
            this.Controls.Add(this.btnGraduated);
            this.Controls.Add(this.btnPieChart);
            this.Controls.Add(this.btnBarChart);
            this.Controls.Add(this.btnOpenMxd);
            this.Controls.Add(this.btnUniqueValue);
            this.Controls.Add(this.axToolbarControl1);
            this.Controls.Add(this.axMapControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private System.Windows.Forms.Button btnUniqueValue;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnOpenMxd;
        private System.Windows.Forms.Button btnBarChart;
        private System.Windows.Forms.Button btnPieChart;
        private System.Windows.Forms.Button btnGraduated;
        private System.Windows.Forms.Button btnAutoSymbol;
    }
}

