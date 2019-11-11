using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IBM_Model_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == null)
            {
                MessageBox.Show("Fill in polish file path");
                return;
            }
            if (textBox2.Text == null)
            {
                MessageBox.Show("Fill in english file path");
                return;
            }
            var model = new Model(textBox1.Text, textBox2.Text, (int)numericUpDown1.Value);
            model.Train();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if(result == DialogResult.OK)
            {
                textBox2.Text = openFileDialog.FileName;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName;
            }
        }
    }
}
