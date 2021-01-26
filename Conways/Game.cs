using System;

namespace Conways
{
    public class Game
    {
        private ConwaysGameOfLife life;
        private int boardSize;
        public Game(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("Size must be greater than zero");
                
            boardSize = size;  
            life = new ConwaysGameOfLife(size);
        }

        public void PlayGame(int generationCount)
        {
            RandomlyInitializeCells(boardSize);

            BeginInitialGeneration();

            ProcessGenerations(generationCount);
        }

        private void RandomlyInitializeCells(int boardSize)
        {
            //randomly initialize cells
            //random num upto size*size

            Random rnd = new Random();
            int numberOfCells = rnd.Next(1, boardSize * boardSize);

            for (int i = 0; i < numberOfCells; i++)
            {
                int x = rnd.Next(0, boardSize);
                int y = rnd.Next(0, boardSize);

                life.ToggleCell(x, y);
            }
        }

        private void BeginInitialGeneration()
        {
            life.BeginGeneration();
            life.Wait();
            PrintLifeBoard();
        }

        private void ProcessGenerations(int generationCount)
        {
            for (int k = 0; k < generationCount; k++)
            {
                life.ProcessNextGeneration();
                life.Wait();
                PrintLifeBoard();
            }
        }

        private void PrintLifeBoard()
        {
            string line = new string('-', life.BoardSize);
            Console.WriteLine(line);
            Console.WriteLine();

            for (int y = 0; y < life.BoardSize; y++)
            {
                for (int x = 0; x < life.BoardSize; x++)
                    Console.Write(life[x, y] ? "1" : "0");

                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
