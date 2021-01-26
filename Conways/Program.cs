using System;

namespace Conways
{
    class Program
    {
        static void Main(string[] args)
        {
            bool restart = true;
            while (restart)
            {
                Console.WriteLine("Please enter your life's board size:");
                string size = Console.ReadLine();
                int boardSize;

                while (!Int32.TryParse(size, out boardSize) || size.Trim() == "0")
                {
                    Console.WriteLine("Please enter a valid number above 0");
                    size = Console.ReadLine();
                }            

                Console.WriteLine("Please enter the number of generations:");
                string genCount = Console.ReadLine();
                int generationCount;

                while (!Int32.TryParse(genCount, out generationCount) || genCount.Trim() == "0")
                {
                    Console.WriteLine("Please enter a valid number above 0");
                    genCount = Console.ReadLine();
                }
         
                Game newGame = new Game(boardSize);

                newGame.PlayGame(generationCount);

                Console.ReadKey();
            }
        }        
    }
}
