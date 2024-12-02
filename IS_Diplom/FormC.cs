using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IS_Diplom
{
    public partial class FormC : Form
    {
        public FormC()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormOrder fc = new FormOrder();
            fc.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormCView fcv = new FormCView();
            fcv.Show();
            this.Close();
        }
    }
}
