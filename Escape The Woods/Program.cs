using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
namespace Escape_The_Woods
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBaseManager.SetupDataBase();
            CreateRandomNamesAndColorsList();
            Console.Write("How many monkeys do you want in each forest? : ");
            int amount = int.Parse(Console.ReadLine());
            Run(amount);
        }
        public static void Run(int amount)
        {
            List<Forest> forests = new List<Forest>();
            forests.Add(new Forest(0, 0, 1000, 0, 1000));
            forests.Add(new Forest(1, 0, 1000, 0, 1000));
            forests.Add(new Forest(2, 0, 1000, 0, 1000));
            forests.Add(new Forest(3, 0, 1000, 0, 1000));

            foreach(Forest forest in forests)
            {
                forest.GenerateForest();
            }

            List<Task> tasks = new List<Task>();
            foreach(Forest forest in forests)
            {
                for (int i = 0; i < amount; i++)
                {
                    forest.GenerateMonkey(namen[i], colors[i]);
                }
                tasks.Add(DataBaseManager.WriteForestToDataBase(forest));
            }
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            foreach(Forest forest in forests)
            {
                tasks.Add(forest.MakeMonkeysJump());
            }
            Task.WhenAll(tasks);
            foreach(Forest forest in forests)
            {
                BitmapGenerator bmGen = new BitmapGenerator(forest.xmax - forest.xmin, forest.ymax - forest.ymin, forest);
                Task.Run(() => bmGen.createBitmap());
            }

            //foreach (Forest f in forests)
            //{
            //    f.GenerateForest();
            //    Task.Run(() => DataBaseManager.WriteForestToDataBase(f));
            //    for (int i = 0; i < 8; i++)
            //    {
            //        f.GenerateMonkey(namen[i], colors[i]);
            //    }

            //    Task.Run(() => f.MakeMonkeysJump());
            //    BitmapGenerator bmGen = new BitmapGenerator(f.xmax - f.xmin, f.ymax - f.ymin, f);
            //    Task.Run(() => bmGen.createBitmap());
            //}

        }
        public static List<string> namen = new List<string>();
        public static List<Color> colors = new List<Color>();
        public static string NewNaam()
        {
            int lastIndex = 10;
            string naam = "";
            if (lastIndex == 10)
            {
                lastIndex = 0;
                naam = namen[lastIndex];
            }
            if (lastIndex != 10)
            {
                lastIndex = lastIndex + 1;
                naam = namen[lastIndex];
            }
            return naam;
        }
        public static Color NewColor()
        {
            int lastIndex = 10;
            Color color = Color.Black;
            if (lastIndex == 10)
            {
                lastIndex = 0;
                color = colors[lastIndex];
            }
            if (lastIndex != 10)
            {
                lastIndex = lastIndex + 1;
                color = colors[lastIndex];
            }
            return color;
        }
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
