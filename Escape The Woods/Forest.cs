using System;
using System.Collections.Generic;
using System.Text;

namespace Escape_The_Woods
{
    class Forest
    {
        public int ID{ get; set; }
        public List<Tree> Trees { get; set; } = new List<Tree>();
        public List<Monkey> MonkeysInTheForest { get; set; } = new List<Monkey>();
        public int xmin { get; set; }
        public int xmax { get; set; }
        public int ymin { get; set; }
        public int ymax { get; set; }
        public Forest(int xmin , int xmax, int ymin, int ymax)
        {
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
        }
        public void GenerateForest()
        {
            Console.WriteLine("Start generating forest");
            Random r = new Random();
            for (int i = 0; i < 500; i++)
            {
                Tree newTree = new Tree(Trees.Count, r.Next(xmin, xmax), r.Next(ymin, ymax));
                Trees.Add(newTree);
            }
            Console.WriteLine("Finished generating forest");
        }
        public void GenerateMonkey(string naam, System.Drawing.Color color)
        {
            Console.WriteLine("Start generating monkeys");
            Random r = new Random();
            Monkey monkey = new Monkey(MonkeysInTheForest.Count + 1, naam, color);
            bool monkeyIsSuccesfullyPlaced = false;
            while (!monkeyIsSuccesfullyPlaced)
            {
                int nextRandom = r.Next(0,100);
                if (!Trees[nextRandom].IsOccupied)
                {
                    Trees[nextRandom].IsOccupied = true;
                    monkey.CurrentTree = Trees[nextRandom];
                    monkey.VisitedTrees.Add(Trees[nextRandom]);
                    monkeyIsSuccesfullyPlaced = true;
                }
            }
            MonkeysInTheForest.Add(monkey);
            Console.WriteLine("Finished generating monkeys");
        }
        public void MakeMonkeysJump()
        {
            foreach(Monkey monkey in MonkeysInTheForest)
            {
                Console.WriteLine($"Start calculating excape route for wood : {this.ID}, Monkey : {monkey.Naam}");
                while (!monkey.Escaped)
                {
                    monkey.CalculateClosestTree(this);
                }
                Console.WriteLine($"End calculating escape route for wood : {this.ID}, Monkey : {monkey.Naam}");
            }
            DataBaseManager.WriteLogsToDataBase(this);
        }
    }
}
