using System;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public class Point : GameObject
    {
        public Point(Map map, int x, int y) : base(map)
        {
            this.X = x;
            this.Y = y;
        }

        public override int X { get; protected set; }

        public override int Y { get; protected set; }

        public override int Width { get; protected set; } = 1;

        public override int Height { get; protected set; } = 1;
        private int disposeProcess = 0;
        private DateTime? disposeStartedOn = null;

        public override Sprite[] Sprites
        {
            get
            {
                char[,] sprite;

                if (disposeProcess == 3)
                {
                    sprite = new char[,]
                    {
                        { '\\', '\0', '/' },
                        { '-', '\0', '-' },
                        { '/', '\0', '\\' }
                    };
                    return new Sprite[] { new Sprite(sprite, this.X - 2, this.Y) };
                }
                else if (disposeProcess <= 2 && disposeProcess > 0)
                {
                    sprite = new char[,]
                    {
                        { '\\', '\0', '\0', '\0', '/' },
                        { '\0', '\\', '\0', '/', '\0' },
                        { '-', '-', '\0', '-', '-' },
                        { '\0', '/', '\0', '\\', '\0' },
                        { '/', '\0', '\0', '\0', '\\' },
                    };
                    return new Sprite[] { new Sprite(sprite, this.X - 3, this.Y) };
                }

                sprite = new char[,]
                {
                    { DateTime.UtcNow.Second % 2 == 0 ? '|' : 'O' }
                };

                return new Sprite[] { new Sprite(sprite, this.X, this.Y) };

            }
        }

        public override void OnUpdated()
        {
            if (disposeStartedOn != null)
            {
                disposeProcess = Math.Max(1, 3 - (int)((DateTime.UtcNow - disposeStartedOn.Value).TotalSeconds * 4));
            }

            if (disposeProcess == 1)
            {
                this.Map.DisposeGameObject(this);
            }
        }

        public override bool OnTriggered(IGameObject gameObject, bool check, int x, int y)
        {
            if (check)
            {
                return true;
            }

            if (disposeProcess == 0)
            {
                if (gameObject is Player)
                {
                    this.Map.Player.Point++;
                }
                disposeStartedOn = DateTime.UtcNow;
                disposeProcess = 4;
            }

            return true;
        }
    }
}