namespace RasterAnalysisSystem
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
            this.axToolbarControl1 = new ESRI.ArcGIS.Controls.AxToolbarControl();
            this.axLicenseControl1 = new ESRI.ArcGIS.Controls.AxLicenseControl();
            this.axTOCControl1 = new ESRI.ArcGIS.Controls.AxTOCControl();
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnQueryGray = new System.Windows.Forms.Button();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnSlope = new System.Windows.Forms.Button();
            this.btnAspect = new System.Windows.Forms.Button();
            this.btnContour = new System.Windows.Forms.Button();
            this.txtCol = new System.Windows.Forms.TextBox();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBand = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtContourInterval = new System.Windows.Forms.TextBox();
            this.txtExtractVal = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // axToolbarControl1
            // 
            this.axToolbarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.axToolbarControl1.Location = new System.Drawing.Point(0, 0);
            this.axToolbarControl1.Name = "axToolbarControl1";
            this.axToolbarControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axToolbarControl1.OcxState")));
            this.axToolbarControl1.Size = new System.Drawing.Size(1774, 28);
            this.axToolbarControl1.TabIndex = 0;
            // 
            // axLicenseControl1
            // 
            this.axLicenseControl1.Enabled = true;
            this.axLicenseControl1.Location = new System.Drawing.Point(1660, 684);
            this.axLicenseControl1.Name = "axLicenseControl1";
            this.axLicenseControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axLicenseControl1.OcxState")));
            this.axLicenseControl1.Size = new System.Drawing.Size(32, 32);
            this.axLicenseControl1.TabIndex = 1;
            // 
            // axTOCControl1
            // 
            this.axTOCControl1.Location = new System.Drawing.Point(0, 28);
            this.axTOCControl1.Name = "axTOCControl1";
            this.axTOCControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axTOCControl1.OcxState")));
            this.axTOCControl1.Size = new System.Drawing.Size(236, 683);
            this.axTOCControl1.TabIndex = 2;
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(236, 28);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(649, 683);
            this.axMapControl1.TabIndex = 3;
            // 
            // btnLoad
            // 
            this.btnLoad.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoad.Location = new System.Drawing.Point(1459, 28);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(175, 44);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "加载栅格数据";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnQueryGray
            // 
            this.btnQueryGray.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnQueryGray.Location = new System.Drawing.Point(51, 143);
            this.btnQueryGray.Name = "btnQueryGray";
            this.btnQueryGray.Size = new System.Drawing.Size(161, 44);
            this.btnQueryGray.TabIndex = 5;
            this.btnQueryGray.Text = "灰度值查询";
            this.btnQueryGray.UseVisualStyleBackColor = true;
            this.btnQueryGray.Click += new System.EventHandler(this.btnQueryGray_Click);
            // 
            // btnExtract
            // 
            this.btnExtract.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExtract.Location = new System.Drawing.Point(71, 75);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(141, 44);
            this.btnExtract.TabIndex = 7;
            this.btnExtract.Text = "提取栅格";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // btnSlope
            // 
            this.btnSlope.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSlope.Location = new System.Drawing.Point(55, 27);
            this.btnSlope.Name = "btnSlope";
            this.btnSlope.Size = new System.Drawing.Size(141, 44);
            this.btnSlope.TabIndex = 8;
            this.btnSlope.Text = "坡度计算";
            this.btnSlope.UseVisualStyleBackColor = true;
            this.btnSlope.Click += new System.EventHandler(this.btnSlope_Click);
            // 
            // btnAspect
            // 
            this.btnAspect.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAspect.Location = new System.Drawing.Point(55, 27);
            this.btnAspect.Name = "btnAspect";
            this.btnAspect.Size = new System.Drawing.Size(141, 44);
            this.btnAspect.TabIndex = 9;
            this.btnAspect.Text = "坡向计算";
            this.btnAspect.UseVisualStyleBackColor = true;
            this.btnAspect.Click += new System.EventHandler(this.btnAspect_Click);
            // 
            // btnContour
            // 
            this.btnContour.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnContour.Location = new System.Drawing.Point(51, 65);
            this.btnContour.Name = "btnContour";
            this.btnContour.Size = new System.Drawing.Size(161, 44);
            this.btnContour.TabIndex = 10;
            this.btnContour.Text = "生成等值线";
            this.btnContour.UseVisualStyleBackColor = true;
            this.btnContour.Click += new System.EventHandler(this.btnContour_Click);
            // 
            // txtCol
            // 
            this.txtCol.Location = new System.Drawing.Point(96, 109);
            this.txtCol.Name = "txtCol";
            this.txtCol.Size = new System.Drawing.Size(100, 28);
            this.txtCol.TabIndex = 12;
            // 
            // txtRow
            // 
            this.txtRow.Location = new System.Drawing.Point(96, 75);
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(100, 28);
            this.txtRow.TabIndex = 13;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBand);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCol);
            this.groupBox1.Controls.Add(this.txtRow);
            this.groupBox1.Controls.Add(this.btnQueryGray);
            this.groupBox1.Location = new System.Drawing.Point(1408, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(258, 195);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "栅格数值查询";
            // 
            // txtBand
            // 
            this.txtBand.Location = new System.Drawing.Point(96, 38);
            this.txtBand.Name = "txtBand";
            this.txtBand.Size = new System.Drawing.Size(100, 28);
            this.txtBand.TabIndex = 17;
            this.txtBand.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 18);
            this.label3.TabIndex = 16;
            this.label3.Text = "列";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 18);
            this.label2.TabIndex = 15;
            this.label2.Text = "行";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 14;
            this.label1.Text = "波段";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSlope);
            this.groupBox2.Location = new System.Drawing.Point(1408, 279);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 80);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "坡度";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnAspect);
            this.groupBox3.Location = new System.Drawing.Point(1408, 365);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(258, 82);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "坡向";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtContourInterval);
            this.groupBox4.Controls.Add(this.btnContour);
            this.groupBox4.Location = new System.Drawing.Point(1408, 453);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(258, 115);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "等值线";
            // 
            // txtContourInterval
            // 
            this.txtContourInterval.Location = new System.Drawing.Point(55, 27);
            this.txtContourInterval.Name = "txtContourInterval";
            this.txtContourInterval.Size = new System.Drawing.Size(137, 28);
            this.txtContourInterval.TabIndex = 11;
            this.txtContourInterval.Text = "100";
            // 
            // txtExtractVal
            // 
            this.txtExtractVal.Location = new System.Drawing.Point(87, 34);
            this.txtExtractVal.Name = "txtExtractVal";
            this.txtExtractVal.Size = new System.Drawing.Size(100, 28);
            this.txtExtractVal.TabIndex = 18;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.txtExtractVal);
            this.groupBox5.Controls.Add(this.btnExtract);
            this.groupBox5.Location = new System.Drawing.Point(1408, 574);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(258, 131);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "栅格提取";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 19;
            this.label4.Text = "高程 >";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1774, 1155);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.axMapControl1);
            this.Controls.Add(this.axTOCControl1);
            this.Controls.Add(this.axLicenseControl1);
            this.Controls.Add(this.axToolbarControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.axToolbarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axLicenseControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axTOCControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxToolbarControl axToolbarControl1;
        private ESRI.ArcGIS.Controls.AxLicenseControl axLicenseControl1;
        private ESRI.ArcGIS.Controls.AxTOCControl axTOCControl1;
        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnQueryGray;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnSlope;
        private System.Windows.Forms.Button btnAspect;
        private System.Windows.Forms.Button btnContour;
        private System.Windows.Forms.TextBox txtCol;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtContourInterval;
        private System.Windows.Forms.TextBox txtExtractVal;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
    }
}

