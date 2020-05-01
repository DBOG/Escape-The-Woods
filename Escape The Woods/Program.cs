using System;
using System.Collections.Generic;
using System.Drawing;
namespace Escape_The_Woods
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBaseManager.SetupDataBase();
            CreateRandomNamesAndColorsList();
            List<Forest> forests = new List<Forest>();
            Forest f = new Forest(0, 1000, 0, 1000);
            f.ID = 0;
            f.GenerateForest();
            DataBaseManager.WriteForestToDataBase(f);
            
            for (int i = 0; i < 8; i++)
            {
                f.GenerateMonkey(namen[i], colors[i]);
            }
            f.MakeMonkeysJump();
            forests.Add(f);
            foreach (Forest forest in forests)
            {
                BitmapGenerator bmGen = new BitmapGenerator(forest.xmax - forest.xmin, forest.ymax - forest.ymin, forest);
                bmGen.createBitmap();
            }
            

        }
        public static List<String> namen = new List<string>();
        public static List<Color> colors = new List<Color>();
        public static void CreateRandomNamesAndColorsList()
        {
            namen.Add("Jef");
            namen.Add("Marie");
            namen.Add("Tom");
            namen.Add("Luc");
            namen.Add("Frederiek");
            namen.Add("Lies");
            namen.Add("Sofie");
            namen.Add("Anne");

            colors.Add(Color.Red);
            colors.Add(Color.Blue);
            colors.Add(Color.Yellow);
            colors.Add(Color.Brown);
            colors.Add(Color.Pink);
            colors.Add(Color.Magenta);
            colors.Add(Color.AntiqueWhite);
            colors.Add(Color.DarkOrange);
        }
    }
}
