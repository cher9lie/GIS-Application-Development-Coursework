namespace GISDev5
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axMapControl1 = new ESRI.ArcGIS.Controls.AxMapControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnTrack = new System.Windows.Forms.Button();
            this.btnLocate = new System.Windows.Forms.Button();
            this.lblAngle1 = new System.Windows.Forms.Label();
            this.lblAngle2 = new System.Windows.Forms.Label();
            this.txtAngle1 = new System.Windows.Forms.TextBox();
            this.txtAngle2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // axMapControl1
            // 
            this.axMapControl1.Location = new System.Drawing.Point(12, 12);
            this.axMapControl1.Name = "axMapControl1";
            this.axMapControl1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMapControl1.OcxState")));
            this.axMapControl1.Size = new System.Drawing.Size(425, 427);
            this.axMapControl1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 700;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnTrack
            // 
            this.btnTrack.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTrack.Location = new System.Drawing.Point(719, 98);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(123, 40);
            this.btnTrack.TabIndex = 1;
            this.btnTrack.Text = "扑火跟踪";
            this.btnTrack.UseVisualStyleBackColor = true;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // btnLocate
            // 
            this.btnLocate.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLocate.Location = new System.Drawing.Point(719, 170);
            this.btnLocate.Name = "btnLocate";
            this.btnLocate.Size = new System.Drawing.Size(123, 40);
            this.btnLocate.TabIndex = 4;
            this.btnLocate.Text = "火点定位";
            this.btnLocate.UseVisualStyleBackColor = true;
            this.btnLocate.Click += new System.EventHandler(this.btnLocate_Click);
            // 
            // lblAngle1
            // 
            this.lblAngle1.AutoSize = true;
            this.lblAngle1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAngle1.Location = new System.Drawing.Point(713, 261);
            this.lblAngle1.Name = "lblAngle1";
            this.lblAngle1.Size = new System.Drawing.Size(172, 31);
            this.lblAngle1.TabIndex = 5;
            this.lblAngle1.Text = "瞭望台1角度：";
            // 
            // lblAngle2
            // 
            this.lblAngle2.AutoSize = true;
            this.lblAngle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAngle2.Location = new System.Drawing.Point(713, 381);
            this.lblAngle2.Name = "lblAngle2";
            this.lblAngle2.Size = new System.Drawing.Size(172, 31);
            this.lblAngle2.TabIndex = 6;
            this.lblAngle2.Text = "瞭望台2角度：";
            // 
            // txtAngle1
            // 
            this.txtAngle1.Location = new System.Drawing.Point(719, 319);
            this.txtAngle1.Name = "txtAngle1";
            this.txtAngle1.Size = new System.Drawing.Size(123, 28);
            this.txtAngle1.TabIndex = 7;
            // 
            // txtAngle2
            // 
            this.txtAngle2.Location = new System.Drawing.Point(719, 445);
            this.txtAngle2.Name = "txtAngle2";
            this.txtAngle2.Size = new System.Drawing.Size(123, 28);
            this.txtAngle2.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 741);
            this.Controls.Add(this.txtAngle2);
            this.Controls.Add(this.txtAngle1);
            this.Controls.Add(this.lblAngle2);
            this.Controls.Add(this.lblAngle1);
            this.Controls.Add(this.btnLocate);
            this.Controls.Add(this.btnTrack);
            this.Controls.Add(this.axMapControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axMapControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ESRI.ArcGIS.Controls.AxMapControl axMapControl1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.Button btnLocate;
        private System.Windows.Forms.Label lblAngle1;
        private System.Windows.Forms.Label lblAngle2;
        private System.Windows.Forms.TextBox txtAngle1;
        private System.Windows.Forms.TextBox txtAngle2;
    }
}

