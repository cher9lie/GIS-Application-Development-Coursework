using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GISDev8
{
    public partial class frmAttribute : Form
    {
        public frmAttribute()
        {
            InitializeComponent();
        }
        public void SetDataSource(System.Data.DataTable table)
        {
            dataGridView1.DataSource = table;
        }

        private void frmAttribute_Load(object sender, EventArgs e)
        {

        }

    }
}
