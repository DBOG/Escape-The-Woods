using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using System.IO;

namespace Escape_The_Woods
{
    class BitmapGenerator
    {
        private int _width { get; set; }
        private int _height { get; set; }
        private Forest _forest { get; set; }
        public BitmapGenerator(int width, int height, Forest forest)
        {
            _width = width;
            _height = height;
            _forest = forest;
        }
        public void createBitmap()
        {
            Bitmap bm = new Bitmap(_width, _height);
            string path = @"D:\Hogent\Programmeren\Programmeren 4\Escape The Woods";
            
            Graphics g = Graphics.FromImage(bm);
            Brush BgBrush = new SolidBrush(Color.Black);
            g.FillRectangle(BgBrush, _width - _width, _height - _height, _width, _height); // makes background black
            foreach(Tree tree in _forest.Trees)
            {
                Pen TreePen = new Pen(Color.Green, 1);
                g.DrawEllipse(TreePen, tree.PositionX, tree.PositionY, 10, 10);
            }
            foreach(Monkey monkey in _forest.MonkeysInTheForest)
            {
                Brush b = new SolidBrush(monkey.Color);
                Pen pen = new Pen(monkey.Color, 1);
                Tree firstTree = monkey.VisitedTrees.First();
                g.FillEllipse(b, firstTree.PositionX, firstTree.PositionY, 9, 9);

                for (int i = 1; i < monkey.VisitedTrees.Count; i++)
                {
                    g.DrawLine(pen, monkey.VisitedTrees[i - 1].PositionX, monkey.VisitedTrees[i - 1].PositionY, monkey.VisitedTrees[i].PositionX, monkey.VisitedTrees[i].PositionY);
                }
                Tree lastTree = monkey.VisitedTrees.Last();
                List<double> positionsToBorder = new List<double>() { _forest.ymax - lastTree.PositionY,// 0 == bottom
                                                                     _forest.xmax - lastTree.PositionX,// 1 == right
                                                                     lastTree.PositionY - _forest.ymin,// 2 == top
                                                                     lastTree.PositionX - _forest.xmin};// 3 == left

                int x = 0;
                int y = 0;
                switch (positionsToBorder.IndexOf(positionsToBorder.Min()))
                {
                    case 0:
                        x = lastTree.PositionX;
                        y = _forest.ymax;
                        break;
                    case 1:
                        x = _forest.xmax;
                        y = lastTree.PositionY;
                        break;
                    case 2:
                        x = lastTree.PositionX;
                        y = _forest.ymin;
                        break;
                    case 3:
                        x = _forest.xmin;
                        y = lastTree.PositionY;
                        break;
                }
                g.DrawLine(pen, lastTree.PositionX, lastTree.PositionY, x, y);
            }
            bm.Save(Path.Combine(path, _forest.ID.ToString() + "_escapeRoutes.jpg"));
        }
    }
}
