using System;
using System.Windows.Forms;

namespace ImportMO
{
    public partial class MENU : Form
    {
        private static CheckImportMOXML xsd_check;

        public MENU()
        {
            InitializeComponent();
            xsd_check = new CheckImportMOXML(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xsd_check.ImportMO_START();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label2.Text = openFileDialog1.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                label3.Text = openFileDialog2.FileName;
            }
        }

        private void button_check_Click(object sender, EventArgs e)
        {
            CheckMyXML.CheckMyFile(label2.Text, label3.Text);
        }
    }
}