using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
        public Forest(int id, int xmin , int xmax, int ymin, int ymax)
        {
            ID = id;
            this.xmin = xmin;
            this.xmax = xmax;
            this.ymin = ymin;
            this.ymax = ymax;
        }
        public void GenerateForest()
        {
            Random r = new Random();
            for (int i = 0; i < 500; i++)
            {
                Tree newTree = new Tree(Trees.Count, r.Next(xmin, xmax), r.Next(ymin, ymax));
                Trees.Add(newTree);
            }
        }
        public void GenerateMonkey(string naam, Color color)
        {
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
        }
        public async Task MakeMonkeysJump()
        {
            List<Task> tasks = new List<Task>();
            foreach(Monkey monkey in MonkeysInTheForest)
            {
                tasks.Add(Task.Run(() => CalculateEscapeRoute(monkey)));
            }
            Task.WaitAll(tasks.ToArray());
            await DataBaseManager.WriteLogsToDataBase(this);

        }
        public async Task CalculateEscapeRoute(Monkey monkey)
        {
            Console.WriteLine($"Start calculating excape route for wood : {this.ID}, Monkey : {monkey.Naam}");
            while (!monkey.Escaped)
            {
                await monkey.CalculateClosestTree(this);
            }
            Console.WriteLine($"End calculating escape route for wood : {this.ID}, Monkey : {monkey.Naam}");
        }
    }
}
