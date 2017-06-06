using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace UkrferryDocker
{
    public partial class MainForm : Form
    {
        private const bool chngUserNameAllowed = false;
        private int blinkCnt = 0;


        public MainForm()
        {
            InitializeComponent();
            //if (Environment.UserName == "avo")
                btnConstructor.Visible = true;
            //else
              //  btnConstructor.Visible = false;
            linkLabelUserName.Text = Environment.UserName;
            if (!chngUserNameAllowed)
                linkLabelUserName.LinkBehavior = LinkBehavior.NeverUnderline;
        }

        private void btnConstructor_Click(object sender, EventArgs e)
        {
            ConstructorForm cFrm = new ConstructorForm();
            cFrm.ShowDialog();
            RefreshLevelList();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshLevelList();
        }

        private void RefreshLevelList()
        {
            listView1.Items.Clear();
            if (Directory.Exists(Application.StartupPath + "\\levels"))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Application.StartupPath + "\\levels");
                foreach (FileInfo fInfo in dirInfo.GetFiles())
                    if (fInfo.Extension == ".ukd" || fInfo.Extension == ".UKD")
                    {
                        ListViewItem lstItem = new ListViewItem(fInfo.Name.Substring(0, fInfo.Name.IndexOf(".ukd", 0, StringComparison.InvariantCultureIgnoreCase)));
                        lstItem.Tag = fInfo.Name;
                        lstItem.SubItems.Add("0");
                        lstItem.SubItems.Add("");
                        lstItem.SubItems.Add("0");
                        listView1.Items.Add(lstItem);
                    }
                Records recMan = new Records();
                if (listView1.Items != null)
                    if (listView1.Items.Count > 0)
                        foreach (RecordItem recItem in recMan.RecordsLst)
                            foreach (ListViewItem itm in listView1.Items)
                                if (itm.SubItems[0].Text == recItem.LvlName)
                                {
                                    itm.SubItems[1].Text = recItem.Record.ToString();
                                    itm.SubItems[2].Text = recItem.RecordsMan;
                                    itm.SubItems[3].Text = recItem.Seconds.ToString();
                                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRecords_Click(object sender, EventArgs e)
        {

        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems != null)
                if (listView1.SelectedItems.Count == 1)
                    if (listView1.SelectedItems[0].Tag is string)
                        if (!String.IsNullOrEmpty((string)listView1.SelectedItems[0].Tag))
                        {
                            this.Visible = false;
                            GameForm gFrm = new GameForm(Application.StartupPath + "\\levels\\" + (string)listView1.SelectedItems[0].Tag, listView1.SelectedItems[0].SubItems[1].Text, listView1.SelectedItems[0].SubItems[2].Text, listView1.SelectedItems[0].SubItems[3].Text);
                            gFrm.ShowDialog();
                            this.Visible = true;
                            RefreshLevelList();
                            return;
                        }
            timer1.Enabled = true;
        }

        private void linkLabelUserName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (chngUserNameAllowed)
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (blinkCnt == 0)
            {
                groupBox1.ForeColor = Color.Black;
                toolTip1.Show("Сделайте выбор уровня!", this, 50, 0,3000);
            }

            blinkCnt++;

            if (groupBox1.ForeColor == Color.Black)
                groupBox1.ForeColor = Color.Red;
            else
                groupBox1.ForeColor = Color.Black;

            if (blinkCnt == 5)
            {
                timer1.Enabled = false;
                blinkCnt = 0;
                groupBox1.ForeColor = SystemColors.ActiveCaption;
            }
        }


    }
}
