using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using static Base.Utility;
using static Base.Consts;
using static Base.Data;

namespace Base
{
    public class ConsoleBox
    {
        private string ENTER = ((char)13).ToString();
        public const uint MaxCharSize = 75;
        public const uint MinCharSize = 5;
        public enum ViewMode { Left, Right, Top, Bott }
        public Vector2f Position { get; set; }
        public Vector2f Origin { get; set; }
        public Vector2f Size { get; set; }
        public Color BackColor { get; set; }
        public Color TextColor { get; set; }
        public string String
        {
            get => str; set
            {
                str = value;
                if (text != null)
                {
                    text.DisplayedString = str;
                    ResetTextBox();
                }
            }
        }

        private Button extBtn;
        private Button minBtn;
        private Button maxBtn;
        private Text text;
        private string str;
        private Font font;
        private RenderTexture texture;
        private View view;
        private bool isDragging;
        private bool isTbDragging;
        private Vector2f delta;
        private Vector2f tbDelta;
        private Vector2f textBox;
        private int cursor;
        private const string textBar = "_";

        public ConsoleBox(Vector2f position, Vector2f size)
        {
            this.cursor = 0;
            this.Position = position;
            this.Size = size;
            this.Origin = size / 2;

            this.font = new Font("Resources\\Fonts\\NANUMGOTHIC.TTF");
            this.text = new Text(String, font);
            this.text.CharacterSize = 15;
            this.BackColor = new Color(50, 50, 50);
            this.TextColor = Color.White;
            this.texture = new RenderTexture((uint)Size.X, (uint)Size.Y);
            this.view = new View(new FloatRect(0, 0, Size.X, Size.Y));
            this.String = "";

            this.isDragging = false;
            this.extBtn = new Button();
            this.minBtn = new Button();
            this.maxBtn = new Button();

            events.MouseMovedEvents.Add(OnMouseMoved);
            events.MouseReleasedEvents.Add(OnMouseReleased);
            events.MousePressedEvents.Add(OnMousePressed);
            events.MouseScrolledEvents.Add(OnMouseScrolled);
            events.TextEnteredEvents.Add(OnTextEntered);
            events.KeyPressedEvents.Add(OnKeyPressed);
        }

        public void Update()
        {
            extBtn.Update();
            minBtn.Update();
            maxBtn.Update();

            var newView = view.Center;

            if (textBox.X < this.Size.X)
            {
                if (newView.X - textBox.X < -(Origin.X - 10))
                    newView.X = textBox.X - (Origin.X - 10);

                if (newView.X > Origin.X + 25)
                    newView.X = Origin.X + 25;
            }
            else
            {
                if (newView.X - textBox.X > -(Origin.X - 10))
                    newView.X = textBox.X - (Origin.X - 10);

                if (newView.X < Origin.X + 25)
                    newView.X = Origin.X + 25;
            }

            if (textBox.Y < this.Size.Y)
            {
                var offset = Map(text.CharacterSize, MinCharSize, MaxCharSize, 10, -53) - 10;
                if (newView.Y - textBox.Y < -(Origin.Y + offset))
                    newView.Y = textBox.Y - (Origin.Y + offset);

                offset = Map(text.CharacterSize, MinCharSize, MaxCharSize, 10, -53) - 10;
                if (newView.Y > Origin.Y + offset)
                    newView.Y = Origin.Y + offset;
            }
            else
            {
                if (newView.Y < Origin.Y)
                    newView.Y = Origin.Y;

                if (newView.Y - textBox.Y > -Origin.Y)
                    newView.Y = textBox.Y - Origin.Y;
            }

            view.Center = newView;
        }

        public void Display()
        {
            DisplayCursor();
            DisplayConsole();
            DisplayToolbar();
            DeleteCursor();
        }

        #region Public Methods
        public void Write(object str)
        {
            this.String += str;
        }

        public void WriteLine(object str)
        {
            this.String += str + "\n";
        }

        public void Endline(int times = 1)
        {
            for (int i = 0; i < times; i++)
                this.String += "\n";
        }

        public void Clear()
        {
            SetView(ViewMode.Left);
            SetView(ViewMode.Top);
            this.String = "";
        }

        public void SetView(ViewMode viewMode)
        {
            if (textBox.X > this.Size.X)
            {
                if (viewMode == ViewMode.Left)
                    view.Center = new Vector2f(Origin.X, view.Center.Y);
                if (viewMode == ViewMode.Right)
                    view.Center = new Vector2f(textBox.X - Origin.X, view.Center.Y);
            }
            if (textBox.Y > this.Size.Y)
            {
                if (viewMode == ViewMode.Top)
                    view.Center = new Vector2f(view.Center.X, Origin.Y);
                if (viewMode == ViewMode.Bott)
                    view.Center = new Vector2f(view.Center.X, textBox.Y - Origin.Y);
            }
        }
        #endregion

        #region Private Methods
        private void DisplayCursor()
        {
            if (cursor <= 0) cursor = 0;
            if (cursor > String.Length) return;

            this.String =
            String.Substring(0, cursor) + textBar +
            String.Substring(cursor, String.Length - cursor);
            cursor++;
        }

        private void DeleteCursor()
        {
            if (cursor <= 0) return;
            if (cursor > String.Length) return;

            this.String =
            String.Substring(0, cursor - 1) +
            String.Substring(cursor, String.Length - cursor);
            cursor--;
        }

        private void DisplayConsole()
        {
            texture.SetView(view);
            texture.Clear(BackColor);
            text.Position = new Vector2f(30, 30);
            text.FillColor = this.TextColor;
            texture.Draw(text);
            texture.Display();

            Sprite rect = new Sprite(texture.Texture);
            rect.Origin = this.Origin;
            rect.Position = this.Position;
            window.Draw(rect);
        }

        private void DisplayToolbar()
        {
            Vector2f tbSize = new Vector2f(this.Size.X, 30);
            Vector2f tbOrigin = tbSize / 2;
            Vector2f tbPosition = this.Position - this.Origin + new Vector2f(this.Origin.X, tbOrigin.Y);
            Color tbColor = new Color(43, 43, 43);
            RectangleShape toolBar = new RectangleShape(tbSize);
            toolBar.Origin = tbOrigin;
            toolBar.Position = tbPosition;
            toolBar.FillColor = tbColor;
            window.Draw(toolBar);

            Text tbText = new Text("CONSOLE", font);
            tbText.CharacterSize = 14;
            tbText.Position = toolBar.Position + new Vector2f(5, 5);
            tbText.Origin = toolBar.Origin;
            tbText.FillColor = new Color(145, 145, 145);
            window.Draw(tbText);

            extBtn.Position = toolBar.Position + new Vector2f(toolBar.Size.X / 2 - 15, 0);
            extBtn.Size = new Vector2f(7, 7);
            extBtn.Style.buttonShape = Button.ButtonShape.Circle;
            extBtn.Style.fillColor = new Color(255, 76, 71);
            extBtn.Display();
            
            maxBtn.Position = toolBar.Position + new Vector2f(toolBar.Size.X / 2 - 35, 0);
            maxBtn.Size = new Vector2f(7, 7);
            maxBtn.Style.buttonShape = Button.ButtonShape.Circle;
            maxBtn.Style.fillColor = new Color(255, 184, 42);
            maxBtn.Display();

            minBtn.Position = toolBar.Position + new Vector2f(toolBar.Size.X / 2 - 55, 0);
            minBtn.Size = new Vector2f(7, 7);
            minBtn.Style.buttonShape = Button.ButtonShape.Circle;
            minBtn.Style.fillColor = new Color(36, 195, 57);
            minBtn.Display();
        }

        private bool OnTheConsole(Vector2f mousePos)
        {
            if (mousePos.X > Position.X - Origin.X &&
                mousePos.X < Position.X + Origin.X &&
                mousePos.Y > Position.Y - Origin.Y + 30 &&
                mousePos.Y < Position.Y + Origin.Y)
                return true;
            else
                return false;
        }

        private bool OnTheToolbar(Vector2f mousePos)
        {
            Vector2f tbSize = new Vector2f(this.Size.X, 30);
            Vector2f tbOrigin = tbSize / 2;
            Vector2f tbPosition = this.Position - this.Origin +
            new Vector2f(this.Origin.X, tbOrigin.Y);

            if (mousePos.X > tbPosition.X - tbOrigin.X &&
                mousePos.X < tbPosition.X + tbOrigin.X &&
                mousePos.Y > tbPosition.Y - tbOrigin.Y &&
                mousePos.Y < tbPosition.Y + tbOrigin.Y)
                return true;
            else
                return false;
        }

        private void ResetTextBox()
        {
            this.textBox = new Vector2f
            (
               text.GetGlobalBounds().Width + 30,
               text.GetGlobalBounds().Height + 60 - text.CharacterSize * 1.6f
            );
        }

        private void GetUpperLineLoc(out int first, out int start, out int end)
        {
            first = 0;
            start = 0;
            end = 0;

            var cnt = 0;
            for (int i = cursor - 1; i >= 0; i--)
            {
                if (str.Substring(i, 1) == "\n"
                || i == 0)
                {
                    if (cnt == 0)
                        first = i;
                    else if (cnt == 1)
                        end = i;
                    else if (cnt == 2)
                    {
                        start = i;
                        break;
                    }
                    cnt++;
                }
            }

            if (end < start)
                end = start;

        }

        private void GetLowerLineLoc(out int first, out int start, out int end)
        {
            first = 0;
            start = 0;
            end = 0;

            var cnt = 0;
            for (int i = cursor + 1; i < str.Length; i++)
            {
                if (str.Substring(i, 1) == "\n"
                || i == str.Length - 1)
                {
                    if (cnt == 0)
                        first = i;
                    else if (cnt == 1)
                        start = i;
                    else if (cnt == 2)
                    {
                        end = i;
                        break;
                    }
                    cnt++;
                }
            }

            if (end < start)
                end = start;

        }
        #endregion

        #region Events
        private void OnMousePressed(Vector2f mousePos, Mouse.Button button)
        {
            if (button != Mouse.Button.Left && button != Mouse.Button.Right) return;

            if (!isTbDragging && OnTheToolbar(mousePos))
            {
                isTbDragging = true;
                tbDelta = mousePos - Position;
            }
            else if (!isDragging && OnTheConsole(mousePos))
            {
                isDragging = true;
                delta = -mousePos - view.Center;
            }
        }

        private void OnMouseMoved(Vector2f mousePos)
        {
            if (isTbDragging)
            {
                var newPos = mousePos - tbDelta;
                Position = newPos;
            }
            else if (isDragging)
            {
                var newView = -mousePos - delta;
                view.Center = newView;
            }
        }

        private void OnMouseReleased(Vector2f mousePos, Mouse.Button button)
        {
            if (isDragging)
                isDragging = false;
            else if (isTbDragging)
                isTbDragging = false;
        }

        private void OnMouseScrolled(Vector2f mousePos, float delta)
        {
            if (!OnTheConsole(mousePos)) return;

            text.CharacterSize += (uint)(delta);
            if (delta > 0 && text.CharacterSize > MaxCharSize)
                text.CharacterSize = MaxCharSize;
            if (delta < 0 && text.CharacterSize < MinCharSize)
                text.CharacterSize = MinCharSize;

            OnMouseMoved(mousePos);
            ResetTextBox();
        }

        private void OnTextEntered(TextEventArgs e)
        {
            if (e.Unicode == ENTER)
            {
                str = str.Substring(0, cursor) + "\n" + str.Substring(cursor);
                cursor++;
                return;
            }
            else if (e.Unicode == "")
            {
                if (cursor <= 0) return;
                str = str.Remove(cursor - 1, 1);
                cursor--;
                return;
            }
            else if (e.Unicode == "")
            {
                str = str.Substring(0, cursor) + Clipboard.Contents + str.Substring(cursor);
                cursor += Clipboard.Contents.Length;
                return;
            }

            var chr = e.Unicode;
            str = str.Substring(0, cursor) + chr + str.Substring(cursor);
            cursor++;
        }

        private void OnKeyPressed(Keyboard.Key key)
        {
            if (key == Keyboard.Key.Left)
            {
                if (cursor > 0)
                    cursor--;
            }
            else if (key == Keyboard.Key.Right)
            {
                if (cursor < str.Length)
                    cursor++;
            }
            else if (key == Keyboard.Key.Up)
            {
                int first;
                int start;
                int end;
                GetUpperLineLoc(out first, out start, out end);
                cursor = end + (cursor - first);
            }
            else if (key == Keyboard.Key.Down)
            {
                int first;
                int start;
                int end;
                GetLowerLineLoc(out first, out start, out end);
                cursor = start - (first - cursor);
            }
        }
        #endregion
    }
}