using System;
using System.Drawing;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public interface IGameObject
    {
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
        Rectangle Rectangle { get; }
        Sprite[] Sprites { get; }
        public bool IsRemoved { get; set; }

        void OnInitialized();
        void OnDisposed();
        void OnUpdated();
        void OnKeyPressed(ConsoleKey key);
        bool OnTriggered(IGameObject gameObject, bool check, int moveX, int moveY);
    }
}