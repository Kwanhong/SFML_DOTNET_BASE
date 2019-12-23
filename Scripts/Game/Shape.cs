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
    public class Shape
    {
        public Vector2f Position { get; set; }
        public float Rotation
        {
            get => angle; set
            {
                var preAngle = angle;
                angle = value;
                this.Rotate(angle - preAngle);
            }
        }
        float angle;

        public Vector2f[] Vertexes { get; set; }
        public Shape(Vector2f[] vtxs, Vector2f pos = new Vector2f())
        {
            Position = pos;
            Vertexes = vtxs;
        }

        public void Rotate(float angle)
        {
            for (var i = 0; i < Vertexes.Length; i++)
            {
                Vertexes[i] = RotateVector(Vertexes[i], angle);
            }
        }

        public void Display()
        {
            VertexArray shape = new VertexArray(PrimitiveType.Triangles, (uint)Vertexes.Length);
            foreach (var vtx in Vertexes)
            {
                shape.Append(new Vertex(vtx + Position));
            }
            window.Draw(shape);
        }
    }
}