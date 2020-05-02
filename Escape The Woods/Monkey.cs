using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;


namespace Escape_The_Woods
{
    class Monkey
    {
        public int ID{ get; set; }
        public string Naam { get; set; }
        public Tree CurrentTree { get; set; }
        public List<Tree> VisitedTrees { get; set; } = new List<Tree>();
        public Tree ClosestTree { get; set; }
        public bool Escaped { get; set; }
        public System.Drawing.Color Color { get; set; }
        public Monkey(int id, string naam, System.Drawing.Color color)
        {
            ID = id;
            Naam = naam;
            Escaped = false;
            Color = color;
        }
        public async Task Jump(Forest forest)
        {
            VisitedTrees.Add(ClosestTree);
            CurrentTree.IsOccupied = false;
            forest.Trees[ClosestTree.ID].IsOccupied = true;
            CurrentTree = ClosestTree;
            ClosestTree = null;
        }
        public async Task CalculateClosestTree(Forest forest)
        {
            double distanceToBorder = (new List<double>() { forest.ymax - CurrentTree.PositionY,
                                                            forest.xmax - CurrentTree.PositionX,
                                                            CurrentTree.PositionY - forest.ymin,
                                                            CurrentTree.PositionX - forest.xmin}).Min();
            double distanceToClosestTree = 100;
            foreach (Tree tree in forest.Trees)
            {
                if (!VisitedTrees.Contains(tree) && tree.ID != CurrentTree.ID && !tree.IsOccupied)
                {
                    if(ClosestTree == null && tree != CurrentTree) ClosestTree = tree;
                    else
                    {
                        int x1 = CurrentTree.PositionX;
                        int y1 = CurrentTree.PositionY;
                        int x2 = tree.PositionX;
                        int y2 = tree.PositionY;

                        double distanceToTree = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
                        distanceToClosestTree = Math.Sqrt(Math.Pow(x1 - ClosestTree.PositionX, 2) + Math.Pow(y1 - ClosestTree.PositionY, 2));

                        if (distanceToTree < distanceToClosestTree) ClosestTree = tree;
                    }
                }
            }
            if(distanceToBorder < distanceToClosestTree)
            {
                Escaped = true;
                await DataBaseManager.WriteMonkeyRecordsToDataBase(this, forest.ID);
            }
            else
            {
                await DataBaseManager.WriteToTextFile(forest);
                await Jump(forest);
            }
        }
    }
}
