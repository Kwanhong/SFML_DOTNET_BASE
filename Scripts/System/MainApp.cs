using static Base.Data;

namespace Base
{
    static class MainApp
    {
        static void Main(string[] args)
        {
            events = new Event();
            events.InitializeOnce();

            game = new Game();
            game.InitializeOnce();

            game.Run();
        }
    }
}
