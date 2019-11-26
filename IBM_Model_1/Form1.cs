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
        Model model;
        public Form1()
        {
            InitializeComponent();
            richTextBox1.Enabled = false;
            richTextBox2.Enabled = false;
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
            model = new Model(textBox1.Text, textBox2.Text, (int)numericUpDown1.Value);
            model.Train();
            richTextBox1.Enabled = true;
            richTextBox2.Enabled = true;
            richTextBox2.ReadOnly = true;
            textBox3.Enabled = true;
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

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Text = model.getStatisticTranslation(richTextBox1.Text);
            richTextBox2.Text += "\n" + model.getBrutalTranslation(richTextBox1.Text);
            Refresh();
        }

        private void textBox3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox3.Text = openFileDialog.FileName;
                model.getDictionary(textBox3.Text);
            }
        }
    }
}
