using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Saker när programmet öppnar
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void PictureBoxClick(object sender, EventArgs e)
        {
            MessageBox.Show("Test");
        }
    }
}
