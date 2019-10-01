using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.IO;
using static Base.Consts;
using static Base.Utility;
using static Base.Data;

namespace Base
{
    class Game
    {
        ConsoleBox console;

        public void Run()
        {
            while (window.IsOpen)
            {
                events.HandleEvents();
                Update();
                Display();

                window.Display();
                window.Clear(winBackColor);

                LateUpdate();
            }
        }

        public void InitializeOnce()
        {
            window.SetFramerateLimit(winFrameLimit);
            Initialize();
        }

        public void Initialize()
        {
            var pos = new Vector2f(winSizeX, winSizeY) * 0.5f;
            var siz = new Vector2f(500, 500);
            console = new ConsoleBox(pos, siz);
        }

        private void Update()
        {
            console.Update();
        }

        private void Display()
        {
            console.Display();
        }

        private void LateUpdate()
        {
        }
    }
}