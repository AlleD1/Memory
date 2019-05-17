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
    //UPPGIFT: Mata tillbaka spelarens namn till MemoryBoard
    public partial class UserInput : Form
    {
        public string Name { get; set; }

        public UserInput()
        {
            InitializeComponent();
        }

        //När "Continue"-knappen klickas
        private void Button1_Click(object sender, EventArgs e)
        {
            string input = textBox1.Text;

            //Om namn angets
            if(!(input == ""))
            {
                this.Name = input;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            //Om namn INTE angets
            else
            {
                MessageBox.Show("No name specified");
            }
        }
    }
}
