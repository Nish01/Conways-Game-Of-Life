namespace Conways
{
    public interface IGameOfLife
    {
        void BeginGeneration();
        void ProcessNextGeneration();
        void Wait();

        void ToggleCell(int x, int y);

         int BoardSize { get; }
         int Generation { get; }

    

    }
}
