using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            var contract = new ContractForFuncs { MainForm = this };
            var action = new ActionBuilder();
            action.Func = textBox1.Text;
            try
            {
                action.Validate();
                await action._sourceCodeModelForLogRequest.Item(contract);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
    

        }
    }
}
