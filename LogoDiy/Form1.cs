using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogoDiy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogoDiyViewModel.Instance.CreateViewData();
            if (!LogoDiyViewModel.Instance.UIIsEnable)
            {
                MessageBox.Show("Unsupported BIOS!\nThis application only supports Lenovo computers with newer BIOS versions.");
                Application.Exit();
                return;
            }
            if (LogoDiyViewModel.Instance.VidibalyLodingIco)
            {
                checkBox1.Checked = LogoDiyViewModel.Instance.IsShowLodingIco;
                checkBox1.Enabled = true;
            }
            label2.Text = "Format: " + LogoDiyViewModel.Instance.Filter + " / Max: " + LogoDiyViewModel.Instance.DefaultWidth.ToString() + "x" + LogoDiyViewModel.Instance.DefaultHeight.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LogoDiyViewModel.Instance.SelectedImageClick();
            showTip();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LogoDiyViewModel.Instance.SaveLogoClick();
            showTip();
        }

        private void showTip()
        {
            if (LogoDiyViewModel.Instance.ShowWarning)
            {
                MessageBox.Show(LogoDiyViewModel.Instance.ShowWarnInfo);
                LogoDiyViewModel.Instance.ShowWarning = false;
            }
            if (LogoDiyViewModel.Instance.ShowSuccessTip)
            {
                MessageBox.Show(LogoDiyViewModel.Instance.ShowSuccessText);
                LogoDiyViewModel.Instance.ShowSuccessTip = false;
            }
            Apply.Enabled = LogoDiyViewModel.Instance.FunEnable;
            Recovery.Enabled = LogoDiyViewModel.Instance.CanRecovery;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LogoDiyViewModel.Instance.ToRecovery();
            showTip();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Enabled)
            {
                return;
            }
            if (LogoDiyViewModel.Instance.ChangeLodingIco(checkBox1.Checked))
            {
                MessageBox.Show("Done!");
            } else
            {
                MessageBox.Show("Failed!");
            }
        }
    }
}
