using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace UkrferryDocker
{
    public partial class GameForm : Form
    {
        private Level lvl;
        private string lvlFileName;

        public GameForm(string _lvlFileName, string record, string recordsmen, string recSeconds)
        {
            lvlFileName = _lvlFileName;
            InitializeComponent();
            lvl = new Level(lvlFileName, false);
            if (lvl != null) // подгоняем размеры формы под уровень
            {
                this.Width = lvl.Width * 35 + 100 + Level.indentLeft + 25;
                this.Height = lvl.Heigth * 35 + Level.indentUp + 40;
            }
            lblRecord.Text = record;
            lblRecordsmen.Text = recordsmen;
            lblRecordSeconds.Text = recSeconds + "сек.";
        }

        private void groupBox2_Paint(object sender, PaintEventArgs e)
        {
            if (lvl != null)
                lvl.DrawLevel(groupBoxLvl.CreateGraphics(), false);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (lvl != null)
            {
                if (e.KeyCode == Keys.Left)
                    lvl.MoveAction(groupBoxLvl.CreateGraphics(), MoveDirections.Left);
                if (e.KeyCode == Keys.Right)
                    lvl.MoveAction(groupBoxLvl.CreateGraphics(), MoveDirections.Rigth);
                if (e.KeyCode == Keys.Up)
                    lvl.MoveAction(groupBoxLvl.CreateGraphics(), MoveDirections.Up);
                if (e.KeyCode == Keys.Down)
                    lvl.MoveAction(groupBoxLvl.CreateGraphics(), MoveDirections.Down);
                lblMovesCnt.Text = lvl.MovesCnt.ToString();
                if (lvl.Finished)
                {
                    if (((Int32.Parse(lblRecord.Text) > lvl.MovesCnt) || Int32.Parse(lblRecord.Text) == 0) || ((Int32.Parse(lblRecord.Text) == lvl.MovesCnt && (Int32.Parse(lblSeconds.Text.Replace("сек.", "")) < Int32.Parse(lblRecordSeconds.Text.Replace("сек.", ""))))))
                    {
                        MessageBox.Show("Игра завершена! Поздравляем, Вы установили новый рекорд!", "Поздравляем!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Records records = new Records();
                        records.NewRecord(new RecordItem(Path.GetFileNameWithoutExtension(lvlFileName), Environment.UserName, lvl.SecondsCnt, lvl.MovesCnt));
                        Close();
                    }
                    else
                        MessageBox.Show("Игра завершена!", "Поздравляем!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                }
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (lvl != null)
                lblSeconds.Text = lvl.SecondsCnt.ToString() + "сек.";
        }
    }
}
