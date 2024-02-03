using System;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public class Block : GameObject
    {
        public Block(Map map, int x, int y, int width, int height) : base(map)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public override int X { get; protected set; }

        public override int Y { get; protected set; }

        public override int Width { get; protected set; }

        public override int Height { get; protected set; }

        public override Sprite[] Sprites
        {
            get
            {
                char[,] sprite = new char[this.Height, this.Width];
                for (int y = 0; y < this.Height; y++)
                {
                    sprite[y, 0] = '|';
                    sprite[y, this.Width - 1] = '|';
                }
                for (int x = 0; x < this.Width; x++)
                {
                    sprite[0, x] = '-';
                    sprite[this.Height - 1, x] = '-';
                }

                sprite[0, 0] = '/';
                sprite[this.Height - 1, 0] = '\\';
                sprite[0, this.Width - 1] = '\\';
                sprite[this.Height - 1, this.Width - 1] = '/';

                return new Sprite[] { new Sprite(sprite, this.X, this.Y) };
            }
        }

        public override bool OnTriggered(IGameObject gameObject, bool check, int x, int y)
        {
            return false;
        }
    }
}