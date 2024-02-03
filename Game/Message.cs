using System;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public class Message : GameObject
    {
        public Message(Map map, int x, int y) : base(map)
        {
            this.X = x;
            this.Y = y;
        }

        private int xValue;
        public override int X
        {
            get
            {
                return this.xValue + this.Map.X;
            }
            protected set
            {
                this.xValue = value;
            }
        }

        public override int Y { get; protected set; }
        public Func<string> Text { get; set; } = () => "";

        public override Sprite[] Sprites
        {
            get
            {
                string[] lines = this.Text().Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
                char[,] sprite = new char[lines.Length, lines.Max(o => o.Length)];
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    for (int x = 0; x < line.Length; x++)
                    {
                        sprite[i, x] = line[x];
                    }
                }

                return new Sprite[] { new Sprite(sprite, this.X, this.Y) };
            }
        }
    }
}