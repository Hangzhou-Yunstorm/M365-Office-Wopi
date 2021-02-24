using System;
using System.Windows.Forms;
using UnifyEditor_Setup.CloudAPI;

namespace UnifyEditor_Interface
{
    public partial class UnifyEditor_NLB : Form
    {
        public UnifyEditor_NLB()
        {
            InitializeComponent();
            this.NLBSelect.SelectedIndex = 0;
        }

        private void CreateNLBButton_Click(object sender, EventArgs e)
        {
            var localIP = this.LocalIPBox.Text;
            var nlbIP = this.NLBIPBox.Text;
            if (string.IsNullOrEmpty(localIP))
            {
                MessageBox.Show("请输入本机IP！", "输入参数", MessageBoxButtons.OK);
                return;
            }
            if (string.IsNullOrEmpty(nlbIP))
            {
                MessageBox.Show("请输入群集IP！", "输入参数", MessageBoxButtons.OK);
                return;
            }



            if (this.NLBSelect.SelectedIndex == 0)
            {
                this.CreateNLBButton.Enabled = false;
                this.NLBBox.AppendText("这不会花费太久，请稍等...\r\n");
                this.NLBBox.AppendText("创建负载均衡开始...\r\n");
                var result3 = PowerShellHelper.RunMasterPS(localIP, nlbIP);
                if (result3.STATE != "OK")
                {
                    this.NLBBox.AppendText(result3.DATA + "\r\n");
                }
            }
            else
            {
                var masterName = this.MasterNameBox.Text;
                if (string.IsNullOrEmpty(masterName))
                {
                    MessageBox.Show("请输入主节点机器名！", "输入参数", MessageBoxButtons.OK);
                    return;
                }

                this.CreateNLBButton.Enabled = false;
                this.NLBBox.AppendText("这不会花费太久，请稍等...\r\n");
                this.NLBBox.AppendText("创建负载均衡开始...\r\n");
                string machineName = Environment.MachineName;
                var result4 = PowerShellHelper.RunBackupPS(localIP, nlbIP, masterName, machineName);
                if (result4.STATE != "OK")
                {
                    this.NLBBox.AppendText(result4.DATA + "\r\n");
                }
            }

            this.NLBBox.AppendText("创建负载均衡结束。\r\n");
            this.CreateNLBButton.Enabled = true;
        }

        private static int LastSelect = 1;

        private void NLBSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (this.NLBSelect.SelectedIndex == 0)
            {
                this.MasterLabel.Hide();
                this.MasterNameBox.Hide();
                this.MasterX.Hide();

                if (LastSelect == 1)
                {
                    LastSelect = 0;
                    var location = this.CreateNLBButton.Location;
                    this.CreateNLBButton.Location = new System.Drawing.Point(location.X, location.Y - 28);
                }
            }
            else
            {
                this.MasterLabel.Show();
                this.MasterNameBox.Show();
                this.MasterX.Show();

                if (LastSelect == 0)
                {
                    LastSelect = 1;
                    var location = this.CreateNLBButton.Location;
                    this.CreateNLBButton.Location = new System.Drawing.Point(location.X, location.Y + 28);
                }
            }

        }
    }
}
