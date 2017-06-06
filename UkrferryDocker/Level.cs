using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Timers;

namespace UkrferryDocker
{
    public enum MoveDirections
    {
        Rigth,
        Left,
        Up,
        Down
    }

    public class Level
    {
        public const int indentLeft = 15;
        public const int indentUp = 15;

        #region Private

        private int currManPosX = -1;
        private int currManPosY = -1;
        private bool finished = false;
        Timer secondsCounterTimer;

        #endregion

        #region Fields

        private int width;
        private int heigth;
        private List<Item>[,] levelItems;
        private int movesCnt;
        private int secondsCnt = 0;

        public int Width { get { return width; } }
        public int Heigth { get { return heigth; } }
        public List<Item>[,] LevelItems { get { return levelItems; } }
        public int MovesCnt { get { return movesCnt; } }
        public int SecondsCnt { get { return secondsCnt; } }

        #endregion

        #region Constructors

        public Level(string fName, bool constructorMode) 
        {
            if (!constructorMode)
            {
                secondsCounterTimer = new Timer(1000);
                secondsCounterTimer.Elapsed += new ElapsedEventHandler(secondsCounterTimer_Elapsed);
            }
            ReadFromFile(fName); 
        }

        void secondsCounterTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            secondsCnt++;
        }

        public Level(int _width, int _heigth)
        {
            width = _width;
            heigth = _heigth;
            levelItems = new List<Item>[_width, _heigth];
        }

        #endregion

        public void SetLevelItem(int xCoord, int yCoord, Item itm)
        {
            if (levelItems[xCoord, yCoord] == null)
            {
                levelItems[xCoord, yCoord] = new List<Item>();
                levelItems[xCoord, yCoord].Add(itm);
            }
            else
                levelItems[xCoord, yCoord].Add(itm);
        }

        public List<Item> GetItem(int xCoord, int yCoord)
        {
            return levelItems[xCoord, yCoord];
        }

        public bool Finished
        {
            get { return finished; }
        }

        public void DrawLevel(Graphics g, bool ConstructorMode)
        {
            Brush b = Brushes.White;
            g.FillRegion(b, g.Clip);
            Pen p = new Pen(Brushes.Black);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                {
                    List<Item> itm = GetItem(i, j);
                    if (itm == null)
                    {
                        g.FillRectangle(b, new Rectangle(new Point(i * 35 + indentLeft, j * 35 + indentUp), new Size(35, 35)));
                        if (ConstructorMode)
                            g.DrawRectangle(p, new Rectangle(new Point(i * 35 + indentLeft, j * 35 + indentUp), new Size(35, 35)));
                    }
                    else
                        foreach (Item item in itm)
                            if (item != null)
                            {
                                if (item.ItemImage is Bitmap)
                                    ((Bitmap)item.ItemImage).MakeTransparent(Color.FromArgb(255, 0, 0, 0));
                                g.DrawImage(item.ItemImage, new Point(i * 35 + indentLeft, j * 35 + indentUp));
                                if (ConstructorMode)
                                    g.DrawRectangle(p, new Rectangle(new Point(i * 35 + indentLeft, j * 35 + indentUp), new Size(35, 35)));
                            }
                }
        }

        public Point HighligthItem(Point p, Graphics g, Point prevPoint)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    if (prevPoint.X >= i * 35 + indentLeft && prevPoint.X <= i * 35 + indentLeft + 35)
                        if (prevPoint.Y >= j * 35 + indentUp && prevPoint.Y <= j * 35 + indentUp + 35)
                            if (i <= width)
                                if (j <= heigth)
                                    g.DrawRectangle(new Pen(Brushes.Black), new Rectangle(new Point(i * 35 + indentLeft, j * 35 + indentUp), new Size(35, 35)));

            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    if (p.X >= i * 35 + indentLeft && p.X <= i * 35 + indentLeft + 35)
                        if (p.Y >= j * 35 + indentUp && p.Y <= j * 35 + indentUp + 35)
                            if (i <= width)
                                if (j <= heigth)
                                {
                                    g.DrawRectangle(new Pen(Brushes.Red), new Rectangle(new Point(i * 35 + indentLeft, j * 35 + indentUp), new Size(35, 35)));
                                    return new Point(i * 35 + indentLeft, j * 35 + indentUp);
                                }
            return new Point(-1, -1);
        }

        public void AddItem(Point p, Item itm)
        {
            if (itm != null)
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < heigth; j++)
                        if (p.X >= i * 35 + indentLeft && p.X <= i * 35 + indentLeft + 35)
                            if (p.Y >= j * 35 + indentUp && p.Y <= j * 35 + indentUp + 35)
                                if (i <= width)
                                    if (j <= heigth)
                                    {
                                        Item newItem = new Item(itm.ItemType, itm.ItemImage, itm.Code, itm.Name);
                                        newItem.XPos = i;
                                        newItem.YPos = j;
                                        if (levelItems[i, j] == null)
                                            levelItems[i, j] = new List<Item>();
                                        else // проверяем совместимость
                                            switch (newItem.ItemType)
                                            {
                                                case ItemType.Wall:
                                                    if (!(System.Windows.Forms.MessageBox.Show("В данной клетке уже есть элемент. Добавить еще один элемент?", "Внимание!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning)== System.Windows.Forms.DialogResult.Yes))
                                                        return;
                                                    break;
                                                case ItemType.Floor:
                                                    if (!(System.Windows.Forms.MessageBox.Show("В данной клетке уже есть элемент. Добавить еще один элемент?", "Внимание!", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning)== System.Windows.Forms.DialogResult.Yes))
                                                        return;
                                                    break;
                                                case ItemType.Man:
                                                    currManPosX = newItem.XPos;
                                                    currManPosY = newItem.YPos;
                                                    break;
                                            }
                                        levelItems[i, j].Add(newItem);

                                    }
        }

        public void DeleteItem(Point p)
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    if (p.X >= i * 35 + indentLeft && p.X <= i * 35 + indentLeft + 35)
                        if (p.Y >= j * 35 + indentUp && p.Y <= j * 35 + indentUp + 35)
                            if (i <= width)
                                if (j <= heigth)
                                    levelItems[i, j] = null;
        }

        public bool SaveToFile(string fName)
        {
            if (fName != "")
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("    ");
                using (XmlWriter writer = XmlWriter.Create(fName, settings))
                {
                    writer.WriteStartElement("UkrferryDockerLevel");
                    writer.WriteStartElement("mainInfo");
                    writer.WriteElementString("width", width.ToString());
                    writer.WriteElementString("heigth", heigth.ToString());
                    writer.WriteEndElement();
                    for (int i = 0; i < width; i++)
                        for (int j = 0; j < heigth; j++)
                            if (levelItems[i, j] != null)
                                foreach (Item item in levelItems[i, j])
                                {
                                    writer.WriteStartElement("Item");
                                    writer.WriteElementString("x", item.XPos.ToString());
                                    writer.WriteElementString("y", item.YPos.ToString());
                                    writer.WriteElementString("code", item.Code.ToString());
                                    writer.WriteEndElement();
                                }
                    writer.WriteEndElement();
                    writer.Flush();
                    return true;
                }
            }
            return false;
        }

        public bool ReadFromFile(string fName)
        {
            if (fName != "")
                if (System.IO.File.Exists(fName))
                {
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.ConformanceLevel = ConformanceLevel.Fragment;
                    settings.IgnoreWhitespace = true;
                    settings.IgnoreComments = true;
                    XmlReader reader = XmlReader.Create(fName, settings);

                    while (reader.Read())
                        if (reader.IsStartElement("mainInfo")) 
                        {
                            reader.ReadStartElement("mainInfo");

                            reader.ReadStartElement("width");
                            width = reader.ReadContentAsInt();
                            reader.ReadEndElement();

                            reader.ReadStartElement("heigth");
                            heigth = reader.ReadContentAsInt();
                            reader.ReadEndElement();

                            levelItems = new List<Item>[width, heigth];
                        }
                        else
                            if (reader.IsStartElement("Item")) 
                            {
                                reader.ReadStartElement("Item");

                                reader.ReadStartElement("x");
                                int x = reader.ReadContentAsInt();
                                reader.ReadEndElement();

                                reader.ReadStartElement("y");
                                int y = reader.ReadContentAsInt();
                                reader.ReadEndElement();

                                reader.ReadStartElement("code");
                                int code = reader.ReadContentAsInt();
                                reader.ReadEndElement();
                                if (levelItems[x, y] == null)
                                    levelItems[x, y] = new List<Item>();
                                Item newItem = ItemsCollection.GetItemByCode(code);
                                newItem.XPos = x;
                                newItem.YPos = y;
                                levelItems[x, y].Add(newItem);
                            }
                    reader.Close();
                    return true;
                }
            return false;
        }

        public void MoveAction(Graphics g, MoveDirections moveDirections)
        {
            if (finished)
                return;

            if (secondsCounterTimer.Enabled == false)
                secondsCounterTimer.Enabled = true;

            // проверяем есть ли кого двигать
            if (currManPosX == -1 || currManPosY == -1)
            {
                bool founded = false;
                // выполняем проход по элементам и ищем человечка
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < heigth; j++)
                        if (levelItems[i, j] != null)
                            foreach (Item itm in levelItems[i, j])
                                if (itm.ItemType == ItemType.Man)
                                {
                                    founded = true;
                                    currManPosX = itm.XPos;
                                    currManPosY = itm.YPos;
                                }
                if (!founded)
                    throw new Exception("Некого двигать!");
            }

            int offsetX = 0, offsetY = 0;
            switch (moveDirections)
            {
                case MoveDirections.Rigth:
                    offsetX = 1;
                    if (currManPosX + offsetX >= width)
                        return;
                    break;
                case MoveDirections.Left:
                    offsetX = -1;
                    if (currManPosX + offsetX < 0)
                        return;
                    break;
                case MoveDirections.Up:
                    offsetY = -1;
                    if (currManPosY + offsetY < 0)
                        return;
                    break;
                case MoveDirections.Down:
                    offsetY = 1;
                    if (currManPosY + offsetY >= heigth)
                        return;
                    break;
            }

            if (levelItems[currManPosX + offsetX, currManPosY + offsetY] != null)
            {
                foreach (Item item in levelItems[currManPosX + offsetX, currManPosY + offsetY])
                {
                    if (item.ItemType == ItemType.Wall) // стенка?
                        return;
                    if (item.ItemType == ItemType.Package) // коробка и ее необходимо сдвинуть...
                    {
                        bool move = false;
                        switch (moveDirections)
                        {
                            case MoveDirections.Rigth:
                                offsetX = 1;
                                if (currManPosX + (offsetX * 2) >= width)
                                    return;
                                break;
                            case MoveDirections.Left:
                                offsetX = -1;
                                if (currManPosX + (offsetX * 2) < 0)
                                    return;
                                break;
                            case MoveDirections.Up:
                                offsetY = -1;
                                if (currManPosY + (offsetY * 2) < 0)
                                    return;
                                break;
                            case MoveDirections.Down:
                                offsetY = 1;
                                if (currManPosY + (offsetY * 2) >= heigth)
                                    return;
                                break;
                        }
                        //проверяем возможность движения коробки вправо
                        // если коробку не куда двигать, отменяем действие
                        if (levelItems[currManPosX + (offsetX * 2), currManPosY + (offsetY * 2)] != null)
                        {
                            foreach (Item itm in levelItems[currManPosX + (offsetX * 2), currManPosY + (offsetY * 2)])
                            {
                                if (itm.ItemType == ItemType.Wall) // стенка?
                                    return;
                                if (itm.ItemType == ItemType.Package) // другая коробка?
                                    return;
                            }
                            move = true;
                        }
                        if (move) // двигаем!
                        {
                            Item PckgToDel = null;
                            // у старой позиции удаляем коробку
                            foreach (Item itm in levelItems[currManPosX + offsetX, currManPosY + offsetY])
                                if (item.ItemType == ItemType.Package)
                                    PckgToDel = item;
                            if (PckgToDel != null)
                                levelItems[currManPosX + offsetX, currManPosY + offsetY].Remove(PckgToDel);
                            // добавляем коробку в новое положение
                            // проверяем есть ли во всех элементах goal, если есть то вид коробки нужно изменить
                            // создаем новый вид человечка в соответствии с направлением движения
                            Item newBoxItem;
                            switch (item.Code)
                            {
                                case 4:
                                case 6:
                                    newBoxItem = new Item(ItemsCollection.GetItemByCode(4));
                                    foreach (Item itm in levelItems[currManPosX + (offsetX * 2), currManPosY + (offsetY * 2)])
                                        if (itm.Code == 5)
                                            newBoxItem = new Item(ItemsCollection.GetItemByCode(6));
                                    break;
                                case 27:
                                case 29: newBoxItem = new Item(ItemsCollection.GetItemByCode(27));
                                    foreach (Item itm in levelItems[currManPosX + (offsetX * 2), currManPosY + (offsetY * 2)])
                                        if (itm.Code == 28)
                                            newBoxItem = new Item(ItemsCollection.GetItemByCode(29));
                                    break;
                                case 30:
                                case 32: newBoxItem = new Item(ItemsCollection.GetItemByCode(30));
                                    foreach (Item itm in levelItems[currManPosX + (offsetX * 2), currManPosY + (offsetY * 2)])
                                        if (itm.Code == 31)
                                            newBoxItem = new Item(ItemsCollection.GetItemByCode(32));
                                    break;
                                default:
                                    return;

                            }
                            newBoxItem.XPos = currManPosX + (offsetX * 2);
                            newBoxItem.YPos = currManPosY + (offsetY * 2);

                            levelItems[currManPosX + (offsetX * 2), currManPosY + (offsetY * 2)].Add(newBoxItem);
                            RedrawItemAtCoords(g, currManPosX + (offsetX * 2), currManPosY + (offsetY * 2));
                            break;
                        }
                        else return;
                    }
                }
                Item ManToDel = null;

                // у старой позиции удаляем человечка
                foreach (Item item in levelItems[currManPosX, currManPosY])
                    if (item.ItemType == ItemType.Man)
                        ManToDel = item;
                if (ManToDel != null)
                    levelItems[currManPosX, currManPosY].Remove(ManToDel);
                RedrawItemAtCoords(g, currManPosX, currManPosY); // перерисовывает старую позицию уже без человечка

                // создаем новый вид человечка в соответствии с направлением движения
                Item newManItem = null;
                switch (moveDirections)
                {
                    case MoveDirections.Rigth:
                        newManItem = new Item(ItemsCollection.GetItemByCode(23));
                        break;
                    case MoveDirections.Left:
                        newManItem = new Item(ItemsCollection.GetItemByCode(24));
                        break;
                    case MoveDirections.Up:
                        newManItem = new Item(ItemsCollection.GetItemByCode(22));
                        break;
                    case MoveDirections.Down:
                        newManItem = new Item(ItemsCollection.GetItemByCode(25));
                        break;
                }
                currManPosX = currManPosX + offsetX;
                currManPosY = currManPosY + offsetY;
                newManItem.XPos = currManPosX;
                newManItem.YPos = currManPosY;

                // добавляем человечка на новой позиции
                levelItems[currManPosX, currManPosY].Add(newManItem);
                RedrawItemAtCoords(g, currManPosX, currManPosY);
                movesCnt++;
                isFinished();
            }
        }

        private void isFinished()
        {
            // выполняем проход по всем элементам, если все элементы на метках - конец
            for (int i = 0; i < width; i++)
                for (int j = 0; j < heigth; j++)
                    if (levelItems[i, j] != null)
                        foreach (Item item in levelItems[i, j])
                            if (item.Code == 4 || item.Code == 27 || item.Code == 30)
                            {
                                finished = false;
                                return;
                            }
            finished = true;
            secondsCounterTimer.Enabled = false;
        }

        public void RedrawItemAtCoords(Graphics g, int x, int y)
        {
            Brush b = Brushes.White;
            List<Item> itm = GetItem(x, y);
            if (itm == null)
                g.FillRectangle(b, new Rectangle(new Point(x * 35 + indentLeft, y * 35 + indentUp), new Size(35, 35)));
            else
                foreach (Item item in itm)
                    if (item != null)
                    {
                        if (item.ItemImage is Bitmap)
                            ((Bitmap)item.ItemImage).MakeTransparent(Color.FromArgb(255, 0, 0, 0));
                        g.DrawImage(item.ItemImage, new Point(x * 35 + indentLeft, y * 35 + indentUp));
                    }

        }

    }
}
