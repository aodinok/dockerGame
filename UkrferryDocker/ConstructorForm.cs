using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UkrferryDocker
{
    public partial class ConstructorForm : Form
    {
        private Level lvl;
        private Point prevPnt = new Point(-1, -1); 

        public ConstructorForm()
        {
            InitializeComponent();
            groupBoxLvl.AllowDrop = true;
            groupBoxLvl.MouseDown += new MouseEventHandler(groupBoxLvl_MouseDown);
            groupBoxLvl.MouseMove += new MouseEventHandler(groupBoxLvl_MouseMove);
        }

        void groupBoxLvl_MouseMove(object sender, MouseEventArgs e)
        {
            if (lvl != null)
                prevPnt = lvl.HighligthItem(new Point(e.X, e.Y), groupBoxLvl.CreateGraphics(), prevPnt);
        }

        void groupBoxLvl_MouseDown(object sender, MouseEventArgs e)
        {
            if (groupBoxLvl.Cursor == Cursors.Hand)
            {
                groupBoxLvl.Cursor = Cursors.Default;
                if (lvl != null)
                {
                    lvl.DeleteItem(new Point(e.X, e.Y));
                    groupBoxLvl.Refresh();
                }
            }
            else
                if (lvl != null)
                    if (treeView1.SelectedNode != null)
                    {
                        lvl.AddItem(new Point(e.X, e.Y), (Item)treeView1.SelectedNode.Tag);
                        groupBoxLvl.Refresh();
                    }
        }

        private void ConstructorForm_Load(object sender, EventArgs e)
        {
            TreeNode tnFloor = treeView1.Nodes.Add("Элементы пола (разрешено двигаться)");
            TreeNode tnGoal = treeView1.Nodes.Add("Элементы цели");
            TreeNode tnMan = treeView1.Nodes.Add("Элементы машинки");
            TreeNode tnPackage = treeView1.Nodes.Add("Элементы для передвиганий");
            TreeNode tnWall = treeView1.Nodes.Add("Элементы стен (нельзя двигаться)");
            foreach (Item itm in ItemsCollection.Items)
            {
                imageList1.Images.Add(itm.Code.ToString(), itm.ItemImage);
                TreeNode node;
                switch (itm.ItemType)
                {
                    case ItemType.Wall:
                        node = tnWall.Nodes.Add(itm.Code.ToString(), itm.Name, itm.Code.ToString(), itm.Code.ToString());
                        node.Tag = itm;
                        break;
                    case ItemType.Floor:
                        node = tnFloor.Nodes.Add(itm.Code.ToString(), itm.Name, itm.Code.ToString(), itm.Code.ToString());
                        node.Tag = itm;
                        break;
                    case ItemType.Package:
                        node = tnPackage.Nodes.Add(itm.Code.ToString(), itm.Name, itm.Code.ToString(), itm.Code.ToString());
                        node.Tag = itm;
                        break;
                    case ItemType.Man:
                        node = tnMan.Nodes.Add(itm.Code.ToString(), itm.Name, itm.Code.ToString(), itm.Code.ToString());
                        node.Tag = itm;
                        break;
                    case ItemType.Goal:
                        node = tnGoal.Nodes.Add(itm.Code.ToString(), itm.Name, itm.Code.ToString(), itm.Code.ToString());
                        node.Tag = itm;
                        break;
                }

            }
        }

        private void btnCreateLevel_Click(object sender, EventArgs e)
        {
            lvl = new Level(Int32.Parse(textBoxWidth.Text), Int32.Parse(textBoxHeigth.Text));
            groupBoxLvl.Refresh();
        }

        private void groupBoxLvl_Paint(object sender, PaintEventArgs e)
        {
            if (lvl != null)
                lvl.DrawLevel(groupBoxLvl.CreateGraphics(), true);
        }

        private void groupBoxLvl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                Item itm = (Item)e.Data.GetData(typeof(Item));
                if (itm != null)
                    if (lvl != null)
                    {
                        prevPnt = lvl.HighligthItem(groupBoxLvl.PointToClient(new Point(e.X, e.Y)), groupBoxLvl.CreateGraphics(), prevPnt);
                        e.Effect = DragDropEffects.Copy;
                    }
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            TreeView tree = (TreeView)sender;
            TreeNode node = tree.GetNodeAt(e.X, e.Y);
            tree.SelectedNode = node;

            if (node != null && node.Tag != null)
                tree.DoDragDrop(node.Tag, DragDropEffects.Copy);
        }

        private void groupBoxLvl_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                Item itm = (Item)e.Data.GetData(typeof(Item));
                if (itm != null)
                    if (lvl != null)
                    {
                        lvl.AddItem(groupBoxLvl.PointToClient(new Point(e.X, e.Y)), itm);
                        groupBoxLvl.Refresh();
                    }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            groupBoxLvl.Cursor = Cursors.Hand;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lvl != null)
            {
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "Файл уровня UkrferryDocker (*.ukd)|*.ukd";
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    if (lvl.SaveToFile(saveFileDialog1.FileName))
                        MessageBox.Show(this, "Успешно сохранено в файл!", "Сохранено!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lvl = new Level(openFileDialog1.FileName, true);
                groupBoxLvl.Refresh();
            }
        }


    }
}
