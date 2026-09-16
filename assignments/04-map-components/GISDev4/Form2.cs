using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace GISDev4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        // 【新增方法】用于接收数据并显示
        public void LoadDataTable(DataTable dt)
        {
            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }
    }
}