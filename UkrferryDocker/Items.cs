using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace UkrferryDocker
{
    public enum ItemType
    {
        Wall,
        Floor,
        Package,
        Man,
        Goal
    }

    /// <summary>
    /// Класс для описания строительных кирпичиков
    /// </summary>
    public class Item
    {
        private ItemType itemType;  // Type: wall, floor, etc..
        private int xPos;           // X position in the level
        private int yPos;           // Y position in the level
        private Image itemImage;
        private int code;
        private string name;

        public Item(ItemType _ItemType, Image _itemImage, int _code, string _name)
        {
            itemType = _ItemType;
            itemImage = _itemImage;
            code = _code;
            name = _name;
        }

        public Item(Item itm)
        {
            itemType = itm.ItemType;
            itemImage = itm.ItemImage;
            code = itm.Code;
            name = itm.Name;
        }


        // Properties
        public ItemType ItemType
        {
            get { return itemType; }
        }

        public int XPos
        {
            get { return xPos; }
            set { xPos = value; }
        }

        public int YPos
        {
            get { return yPos; }
            set { yPos = value; }
        }

        public Image ItemImage
        {
            get { return itemImage; }
        }

        public int Code
        {
            get { return code; }
        }

        public string Name
        {
            get { return name;}
        }
    }

    public static class ItemsCollection
    {
        public static List<Item> Items = new List<Item>() { 
                                                            new Item(ItemType.Wall, Properties.Resources.vesselBorderLeft, 1, "VesselBorderLeft"), 
                                                            new Item(ItemType.Wall, Properties.Resources.vesselBorderRigth, 2, "VesselBorderRigth"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselFloor, 3, "VesselFloor"),
                                                            new Item(ItemType.Package, Properties.Resources.box, 4, "Box"), 
                                                            new Item(ItemType.Goal, Properties.Resources.boxGoal, 5, "BoxGoal"), 
                                                            new Item(ItemType.Package, Properties.Resources.boxOnGoal, 6, "BoxOnGoal"), 
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseLeft1, 7, "vesselNoseLeft1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseLeft2, 8, "vesselNoseLeft2"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseMiddle, 9, "vesselNoseMiddle"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseRigth1, 10, "vesselNoseRigth1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseRigth2, 11, "vesselNoseRigth2"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselRearLeft1, 12, "vesselReareLeft1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselRearLeft2, 13, "vesselRearLeft2"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselRearMiddle, 14, "vesselRearMiddle"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselRearRigth1, 15, "vesselRearRigth1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselRearRigth2, 16, "vesselRearRigth2"),
                                                            new Item(ItemType.Wall, Properties.Resources.sea, 17, "sea"),
                                                            new Item(ItemType.Wall, Properties.Resources.seaBorderLeft, 18, "seaBorderLeft"),
                                                            new Item(ItemType.Wall, Properties.Resources.seaBorderUp, 19, "seaBorderUp"),
                                                            new Item(ItemType.Wall, Properties.Resources.seaBorderDown, 20, "seaBorderDown"),
                                                            new Item(ItemType.Wall, Properties.Resources.seaBorderCorner, 21, "seaBorderCorner"),
                                                            new Item(ItemType.Man, Properties.Resources.manDU, 22, "manDU"),
                                                            new Item(ItemType.Man, Properties.Resources.manLR, 23, "manLR"),
                                                            new Item(ItemType.Man, Properties.Resources.manRL, 24, "manRL"),
                                                            new Item(ItemType.Man, Properties.Resources.manUD, 25, "manUD"),
                                                            new Item(ItemType.Floor, Properties.Resources.floor, 26, "floor"),
                                                            new Item(ItemType.Package, Properties.Resources.barrel, 27, "barrel"), 
                                                            new Item(ItemType.Goal, Properties.Resources.barrelGoal, 28, "barrelGoal"), 
                                                            new Item(ItemType.Package, Properties.Resources.barrelOnGoal, 29, "barrelOnGoal"), 
                                                            new Item(ItemType.Package, Properties.Resources.hank, 30, "hank"), 
                                                            new Item(ItemType.Goal, Properties.Resources.hankGoal, 31, "hankGoal"), 
                                                            new Item(ItemType.Package, Properties.Resources.hankOnGoal, 32, "hankOnGoal"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorWithTree1, 33, "floorWithTree1"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorWithTree2, 34, "floorWithTree2"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorWithTree3, 35, "floorWithTree3"),
                                                            new Item(ItemType.Wall, Properties.Resources.wallWithTree, 36, "wallWithTree"),
                                                            new Item(ItemType.Wall, Properties.Resources.fontan, 37, "fontan"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizDown, 38, "vesselHorizDown"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizLeftDown, 39, "vesselHorizLeftDown"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizLeftEndDown, 40, "vesselHorizLeftEndDown"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizLeftEndDown1, 41, "vesselHorizLeftEndDown1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizLeftEndUp, 42, "vesselHorizLeftEndUp"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselHorizLeftUp, 43, "vesselHorizLeftUp"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizRigthDown, 44, "vesselHorizRigthDown"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizRigthEndDown, 45, "vesselHorizRigthEndDown"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizRigthEndDown1, 46, "vesselHorizRigthEndDown1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizRigthEndUp, 47, "vesselHorizRigthEndUp"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselHorizRigthUp, 48, "vesselHorizRigthUp"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselHorizUp, 49, "vesselHorizUp"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselHorizFloor, 50, "vesselHorizFloor"),
                                                            new Item(ItemType.Floor, Properties.Resources.trapLR, 51, "trapLR"),
                                                            new Item(ItemType.Floor, Properties.Resources.trapToVesselHoriz, 52, "trapToVesselHoriz"),
                                                            new Item(ItemType.Floor, Properties.Resources.trapUD, 53, "trapUD"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselHorizFloor1, 54, "vesselHorizFloor1"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizLeftEndMiddle, 55, "vesselHorizLeftEndMiddle"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizRigthEndMiddle, 56, "vesselHorizRigthEndMiddle"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizDownMiddle, 57, "vesselHorizDownMiddle"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizLeftEndDown2, 58, "vesselHorizLeftEndDown2"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselHorizRigthEndDown2, 59, "vesselHorizRigthEndDown2"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorInRoom, 60, "floorInRoom"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall1, 61, "wall1"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall2, 62, "wall2"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall3, 63, "wall3"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall4, 64, "wall4"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall5, 65, "wall5"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall6, 66, "wall6"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall7, 67, "wall7"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall8, 68, "wall8"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall9, 69, "wall9"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorInRoomToFloor, 70, "floorInRoomToFloor"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorInRoomToFloorWithWall, 71, "floorInRoomToFloorWithWall"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorWithWall, 72, "floorWithWall"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselRearMiddleMult, 73, "vesselRearMiddleMult"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselRearMiddleMultFloor, 74, "vesselRearMiddleMultFloor"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseMiddleMult, 75, "vesselNoseMiddleMult"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselNoseMiddleMultFloor, 76, "vesselNoseMiddleMultFloor"),
                                                            new Item(ItemType.Floor, Properties.Resources.vesselNoseMiddleMultFloorWithTrap, 77, "vesselNoseMiddleMultFloorWithTrap"),
                                                            new Item(ItemType.Wall, Properties.Resources.vesselNoseRigth3, 78, "vesselNoseRigth3"),
                                                            new Item(ItemType.Floor, Properties.Resources.floorsimple, 79, "floorsimple"),
                                                            new Item(ItemType.Wall, Properties.Resources.wall, 80, "wall")
                                                          };


        public static Item GetItemByCode(int code)
        {
            foreach (Item itm in Items)
                if (itm.Code == code)
                    return new Item(itm.ItemType, itm.ItemImage, itm.Code, itm.Name);
            return null;
        }
    }
}
