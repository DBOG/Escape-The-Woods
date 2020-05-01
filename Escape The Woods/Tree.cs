using System;
using System.Collections.Generic;
using System.Text;

namespace Escape_The_Woods
{
    class Tree
    {
        public int ID { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool IsOccupied { get; set; }
        public Tree(int id, int x, int y)
        {
            this.ID = id;
            this.PositionX = x;
            this.PositionY = y;
            IsOccupied = false;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
