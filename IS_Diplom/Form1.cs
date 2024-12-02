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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String loginT = textBox1.Text;
            { 
            switch (loginT) { 

                case "Cli":
                        FormC fc = new FormC();
                        fc.Show();                  
                        this.Close();
                        break;
                case "Adm":
                        FormA fa = new FormA();
                        fa.Show();
                        this.Close();
                        break;
                case "Gruz1":
                        FormGruz1 fg1= new FormGruz1();
                        fg1.Show();
                        this.Close();
                        break;
                case "Deliv":
                        FormDeliv fd = new FormDeliv();
                        fd.Show();
                        this.Close();
                        break;
                case "Gruz2":
                        FormGruz2 fg2 = new FormGruz2();
                        fg2.Show();
                        this.Close();
                        break;
                case "Pol":
                        FormPoluch fp = new FormPoluch();
                        fp.Show();
                        this.Close();
                        break;

                }

            }
              
        }
    }
}
