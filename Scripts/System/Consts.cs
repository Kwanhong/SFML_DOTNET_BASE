using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace Base
{
    static class Consts
    {
        public const int winFrameLimit = 60;
        public const uint winSizeX = 2560;
        public const uint winSizeY = 1600;
        public static Color winBackColor = new Color(32, 32, 32);
        public static VideoMode winMode = new VideoMode(winSizeX, winSizeY);
        public static string winTitle = "SFML DOTNET BASE";
        public static Styles winStyle = Styles.None;
        public static ContextSettings winSettings = new ContextSettings(1, 1, 16);
    }
}