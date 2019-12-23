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
        int moverCnt = 0;
        int consoleCnt = 1;
        Mover[] movers;
        ConsoleBox[] consoles;

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
            consoles = new ConsoleBox[consoleCnt];
            for (var i = 0; i < consoleCnt; i++)
                consoles[i] = new ConsoleBox(new Vector2f(winSizeX, winSizeY) / 2, new Vector2f(600, 450));

            movers = new Mover[moverCnt];
            for (var i = 0; i < moverCnt; i++)
                movers[i] = new Mover(new Vector2f(Random(200, winSizeX - 200), Random(200, winSizeY - 200)));
        }

        private void Update()
        {
            foreach (var console in consoles)
                console.Update();

            foreach (var mover in movers)
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    var wind = (Vector2f)Mouse.GetPosition(window) - mover.Position;
                    wind = SetMagnitude(wind, 1f);
                    mover.ApplyForce(-wind);
                }
                var gravity = new Vector2f(0, 0.5f);
                mover.ApplyForce(gravity);

                mover.Update();
            }

        }

        private void Display()
        {
            foreach (var mover in movers)
                mover.Display();

            foreach (var console in consoles)
                console.Display();
            
            //if (Mouse.IsButtonPressed(Mouse.Button.Left))
            //{
            //    CircleShape circle = new CircleShape(40);
            //    circle.Origin = new Vector2f(40, 40);
            //    circle.Position = (Vector2f)Mouse.GetPosition(window);
            //    circle.FillColor = new Color(255, 100, 100, 100);
            //    window.Draw(circle);
            //}
        }

        private void LateUpdate()
        {
        }

    }
}