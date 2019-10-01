using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
using static Base.Data;

namespace Base
{
    class Event
    {
        public List<Action<Vector2f, Mouse.Button>> MousePressedEvents { get; set; }
        public List<Action<Vector2f, Mouse.Button>> MouseReleasedEvents { get; set; }
        public List<Action<Vector2f>> MouseMovedEvents { get; set; }
        public List<Action<Vector2f, float>> MouseScrolledEvents { get; set; }
        public List<Action<Keyboard.Key>> KeyPressedEvents { get; set; }
        public List<Action<Keyboard.Key>> KeyReleasedEvents { get; set; }
        public List<Action<TextEventArgs>> TextEnteredEvents { get; set; }

        public void InitializeOnce()
        {
            window.Closed += OnClosed;
            window.MouseButtonPressed += OnMousePressed;
            window.MouseButtonReleased += OnMouseReleased;
            window.MouseMoved += OnMouseMoved;
            window.MouseWheelScrolled += OnMouseScrolled;
            window.KeyPressed += OnKeyPressed;
            window.KeyReleased += OnKeyReleased;
            window.TextEntered += OnTextEntered;
            Initialize();
        }

        public void Initialize()
        {
            MousePressedEvents = new List<Action<Vector2f, Mouse.Button>>();
            MouseReleasedEvents = new List<Action<Vector2f, Mouse.Button>>();
            MouseMovedEvents = new List<Action<Vector2f>>();
            MouseScrolledEvents = new List<Action<Vector2f, float>>();
            KeyPressedEvents = new List<Action<Keyboard.Key>>();
            KeyReleasedEvents = new List<Action<Keyboard.Key>>();
            TextEnteredEvents = new List<Action<TextEventArgs>>();
        }

        public void HandleEvents()
        {
            window.DispatchEvents();
        }

        public void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }

        #region  Texting
        public void OnTextEntered(object sender, TextEventArgs e)
        {
            if (TextEnteredEvents == null) return;

            foreach (var evnt in TextEnteredEvents)
                evnt(e);
        }
        #endregion

        #region  Keyboard
        public void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
                window.Close();
            else if (e.Code == Keyboard.Key.F5)
                game.Initialize();

            if (KeyPressedEvents == null) return;

            foreach (var evnt in KeyPressedEvents)
                evnt(e.Code);
        }
        public void OnKeyReleased(object sender, KeyEventArgs e)
        {
            if (KeyReleasedEvents == null) return;

            foreach (var evnt in KeyReleasedEvents)
                evnt(e.Code);
        }
        #endregion

        #region  Mouse
        public void OnMousePressed(object sender, MouseButtonEventArgs e)
        {
            if (MousePressedEvents == null) return;

            foreach (var evnt in MousePressedEvents)
                evnt(new Vector2f(e.X, e.Y), e.Button);
        }
        public void OnMouseReleased(object sender, MouseButtonEventArgs e)
        {
            if (MouseReleasedEvents == null) return;

            foreach (var evnt in MouseReleasedEvents)
                evnt(new Vector2f(e.X, e.Y), e.Button);
        }
        public void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (MouseMovedEvents == null) return;

            foreach (var evnt in MouseMovedEvents)
                evnt(new Vector2f(e.X, e.Y));
        }
        public void OnMouseScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if (MouseScrolledEvents == null) return;

            foreach (var evnt in MouseScrolledEvents)
                evnt(new Vector2f(e.X, e.Y), e.Delta);
        }
        #endregion
    }
}