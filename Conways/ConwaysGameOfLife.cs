using System.Threading.Tasks;

namespace Conways
{
    public class ConwaysGameOfLife : IGameOfLife
    {
        
        public int BoardSize { get; private set; }
        public int Generation { get; private set; }
        private bool[,] worldBoard;
        private bool[,] nextGenerationBoard;
        private Task processTask;

        public ConwaysGameOfLife(int size)
        {
            this.BoardSize = size;
            this.worldBoard = new bool[size, size];
            this.nextGenerationBoard = new bool[size, size];
        }
        public bool this[int x, int y]
        {
            get { return this.worldBoard[x, y]; }
            set { this.worldBoard[x, y] = value; }
        }

        public void ToggleCell(int x, int y)
        {
            bool currentValue = this.worldBoard[x, y];
            this.worldBoard[x, y] = !currentValue;
        }

        public void BeginGeneration()
        {
            if (this.processTask == null || (this.processTask != null && this.processTask.IsCompleted))              
                this.processTask = this.ProcessGeneration();
        }

        public void ProcessNextGeneration()
        {
            if (this.processTask != null && this.processTask.IsCompleted)
            {
                var flip = this.nextGenerationBoard;
                this.nextGenerationBoard = this.worldBoard;
                this.worldBoard = flip;
                Generation++;

                //Begin the next generation's processing asynchronously
                this.processTask = this.ProcessGeneration();
            }
        }

        public void Wait()
        {
            if (this.processTask != null)
                this.processTask.Wait(); //Let it complete
        }

        private Task ProcessGeneration()
        {
            return Task.Factory.StartNew(() =>
            {
                Parallel.For(0, BoardSize, x =>
                {
                    Parallel.For(0, BoardSize, y =>
                    {
                        int numberOfNeighbors = IsNeighborAlive(worldBoard, BoardSize, x, y, -1, 0)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, -1, 1)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, 0, 1)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, 1, 1)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, 1, 0)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, 1, -1)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, 0, -1)
                            + IsNeighborAlive(worldBoard, BoardSize, x, y, -1, -1);

                        bool shouldLive = false;
                        bool isAlive = worldBoard[x, y];

                        if (isAlive && (numberOfNeighbors == 2 || numberOfNeighbors == 3))
                            shouldLive = true;
                        else if (!isAlive && numberOfNeighbors == 3) 
                            shouldLive = true;

                        nextGenerationBoard[x, y] = shouldLive;

                    });
                });
            });
        }

        private int IsNeighborAlive(bool[,] world, int size, int x, int y, int offsetx, int offsety)
        {
            int result = 0;

            int proposedOffsetX = x + offsetx;
            int proposedOffsetY = y + offsety;
            bool outOfBounds = proposedOffsetX < 0 || proposedOffsetX >= size | proposedOffsetY < 0 || proposedOffsetY >= size;
            
            if (!outOfBounds)
                result = world[x + offsetx, y + offsety] ? 1 : 0;
            
            return result;
        }
    }
}

