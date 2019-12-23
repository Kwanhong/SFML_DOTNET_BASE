using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using static Base.Consts;
using static Base.Utility;
using static Base.Data;

namespace Base
{
    class Button
    {
        public enum ButtonShape
        {
            Rect, Circle, Ellipse
        }
        public class ButtonStyle
        {
            public ButtonStyle()
            {
                outlineThickness = 0;
                fillColor = Color.White;
                outlineColor = Color.White;
                textColor = Color.Black;
                fontName = "NANUMGOTHICLIGHT";
                font = new Font("Resources\\Fonts\\" + FontName + ".TTF");
                buttonShape = ButtonShape.Rect;
            }
            public string FontName
            {
                get => fontName; set
                {
                    fontName = value;
                    font = new Font("Resources\\Fonts\\" + fontName + ".TTF");
                }
            }
            public float outlineThickness;
            public Color outlineColor;
            public Color fillColor;
            public Color textColor;
            public ButtonShape buttonShape;
            public Font font;
            public string fontName;
        }

        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public string Caption { get; set; }
        public bool Enabled { get; set; }
        public bool IsPressed { get; set; }

        public List<Action> ButtonPressedEvents { get; set; }
        public List<Action> ButtonReleasedEvents { get; set; }
        public ButtonStyle Style { get; set; }

        private int brightness;

        public Button(Vector2f position = new Vector2f(), Vector2f size = new Vector2f(), string caption = "")
        {
            this.Position = position;
            this.Size = size;
            this.Caption = caption;

            Enabled = true;
            IsPressed = false;
            brightness = 0;

            ButtonPressedEvents = new List<Action>();
            ButtonReleasedEvents = new List<Action>();

            events.MouseMovedEvents.Add(OnMouseMoved);
            events.MouseReleasedEvents.Add(OnMouseReleased);
            events.MousePressedEvents.Add(OnMousePressed);

            this.Style = new ButtonStyle();
        }

        public void Update()
        {
        }

        public void Display()
        {
            var btnPos = new Vector2f();
            var btnColor = new Color
            (
                (byte)Limit(Style.fillColor.R + brightness, 0, 255),
                (byte)Limit(Style.fillColor.G + brightness, 0, 255),
                (byte)Limit(Style.fillColor.B + brightness, 0, 255)
            );

            switch (this.Style.buttonShape)
            {
                case ButtonShape.Rect:
                    RectangleShape rect = new RectangleShape(Size);
                    rect.Origin = this.Size;
                    rect.Position = this.Position;
                    rect.FillColor = btnColor;
                    rect.OutlineColor = Style.outlineColor;
                    rect.OutlineThickness = Style.outlineThickness;
                    window.Draw(rect);
                    btnPos = rect.Position;
                    break;
                case ButtonShape.Circle:
                    CircleShape circle = new CircleShape(Size.X, 50);
                    circle.Origin = this.Size;
                    circle.Position = this.Position;
                    circle.FillColor = btnColor;
                    circle.OutlineColor = Style.outlineColor;
                    circle.OutlineThickness = Style.outlineThickness;
                    window.Draw(circle);
                    btnPos = circle.Position;
                    break;
                case ButtonShape.Ellipse:
                    var quality = 20u;
                    ConvexShape ellipse = new ConvexShape(quality);
                    for (var i = 0u; i < quality; ++i)
                    {
                        float rad = (360 / quality) / (360 / MathF.PI / 2);
                        float x = MathF.Cos(rad) * 100 + 100;//Size.X;
                        float y = MathF.Sin(rad) * 100 + 100;//Size.Y;
                        ellipse.SetPoint(i, new Vector2f(x, y));
                    }
                    ellipse.Origin = this.Size / 2;
                    ellipse.Position = this.Position;
                    ellipse.FillColor = btnColor;
                    window.Draw(ellipse);
                    btnPos = ellipse.Position;
                    break;
            }

            Text text = new Text(Caption, Style.font);
            text.Origin = new Vector2f(text.GetGlobalBounds().Width * 0.52f, text.GetGlobalBounds().Height * 0.9f);
            text.Position = btnPos;
            text.FillColor = Style.textColor;
            window.Draw(text);
        }

        private void OnMousePressed(Vector2f mousePos, Mouse.Button button)
        {
            if (button != Mouse.Button.Left) return;

            if (OnTheButton(mousePos))
            {
                IsPressed = true;
                brightness = -10;

                foreach (var evnt in ButtonPressedEvents)
                    evnt();

            }
        }

        private void OnMouseReleased(Vector2f mousePos, Mouse.Button button)
        {
            if (!IsPressed) return;
            IsPressed = false;

            if (OnTheButton(mousePos))
            {
                brightness = 10;

                foreach (var evnt in ButtonReleasedEvents)
                    evnt();
            }
            else
            {
                brightness = 0;
            }
        }

        private void OnMouseMoved(Vector2f mousePos)
        {
            if (IsPressed) return;

            if (OnTheButton(mousePos))
            {
                brightness = 20;
            }
            else
            {
                brightness = 0;
            }
        }

        private bool OnTheButton(Vector2f mousePos)
        {
            switch (this.Style.buttonShape)
            {
                case ButtonShape.Rect:
                    if (mousePos.X > Position.X - Size.X / 2 &&
                        mousePos.X < Position.X + Size.X / 2 &&
                        mousePos.Y > Position.Y - Size.Y / 2 &&
                        mousePos.Y < Position.Y + Size.Y / 2)
                        return true;
                    else
                        return false;
                case ButtonShape.Circle:
                    if (Distnace(mousePos, Position) < Size.X)
                        return true;
                    else
                        return false;
            }
            return false;
        }
    }
}