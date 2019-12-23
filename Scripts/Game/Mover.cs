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
    public class Mover
    {
        public Vector2f Position { get; set; }
        public Vector2f Velocity { get; set; }

        Shape shape;
        Vector2f acceleration;

        public Mover(Vector2f pos = new Vector2f(), Vector2f vel = new Vector2f())
        {
            Position = pos;
            Velocity = vel;

            var vertexes = new Vector2f[]{
                new Vector2f(0, -15),
                new Vector2f(-10, 15),
                new Vector2f(10, 15)
            };
            shape = new Shape(vertexes);
            acceleration = new Vector2f(0, 0);
        }

        public void ApplyForce(Vector2f force)
        {
            acceleration += force;
        }

        public void Update()
        {
            Edge();
            Velocity *= 0.99f;

            Velocity += acceleration;
            Position += Velocity;
            acceleration *= 0;

            shape.Position = Position;

            var rot = GetAngle(Velocity) + MathF.PI / 2;
            if (GetMagnitude(Velocity) <= 2f) rot = 0;
            shape.Rotation = rot;

        }

        public void Display()
        {
            shape.Display();
        }

        private void Edge()
        {
            if (Position.X > winSizeX - 200)
            {
                Velocity = new Vector2f(-MathF.Abs(Velocity.X), Velocity.Y);
                acceleration *= 0;
            }
            if (Position.X < 200)
            {
                Velocity = new Vector2f(MathF.Abs(Velocity.X), Velocity.Y);
                acceleration *= 0;
            }
            if (Position.Y > winSizeY - 200)
            {
                Velocity = new Vector2f(Velocity.X, -MathF.Abs(Velocity.Y));
                acceleration *= 0;
            }
            if (Position.Y < 200)
            {
                Velocity = new Vector2f(Velocity.X, MathF.Abs(Velocity.Y));
                acceleration *= 0;
            }
        }
    }
}