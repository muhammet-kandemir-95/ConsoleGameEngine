using System;
using System.Drawing;
using LessonConsoleGame.Engine;

namespace LessonConsoleGame.Game
{
    public abstract class GameObject : IGameObject
    {
        public Map Map { get; private set; }
        public GameEngine GameEngine => this.Map.GameEngine;

        public int ScreenX => this.X - this.Map.X;
        public int ScreenY => this.Map.Height - this.Y;

        public virtual int X { get; protected set; }
        public virtual int Y { get; protected set; }
        public virtual int Width { get; protected set; }
        public virtual int Height { get; protected set; }

        public bool IsRemoved { get; set; }

        public bool IsFalling => this.CanMove(0, -1, out bool boundaryLimitExceed, out _) && !boundaryLimitExceed;

        public Rectangle Rectangle => new Rectangle(this.X, this.Y, this.Width, this.Height);

        public virtual Sprite[] Sprites { get; set; }

        public GameObject(Map map)
        {
            this.Map = map;
        }

        public virtual void OnDisposed() { }

        public virtual void OnInitialized() { }

        public virtual void OnKeyPressed(ConsoleKey key) { }

        public virtual bool OnTriggered(IGameObject gameObject, bool check, int moveX, int moveY)
        {
            return true;
        }

        public bool CanMove(int x, int y, out bool boundaryLimitExceed, out IGameObject intersectWith)
        {
            int newX = Math.Max(0, (this.X + x));
            int newY = Math.Max(0, (this.Y + y));
            Rectangle rectangle = new Rectangle(newX, newY, this.Width, this.Height);

            intersectWith = this.Map.GameObjects
                                                    .Where(o => o != this)
                                                    .FirstOrDefault(o => o.Rectangle.IntersectsWith(rectangle));

            bool allowed = true;
            if (intersectWith != null)
            {
                bool allowed1 = this.OnTriggered(intersectWith, true, x, y);
                bool allowed2 = intersectWith.OnTriggered(this, true, x, y);

                allowed = allowed1 && allowed2;
            }

            boundaryLimitExceed = this.X + x < 0 || this.Y + y < 0;

            return allowed;
        }

        public bool Move(int x, int y)
        {
            bool boundaryLimitExceed;
            IGameObject intersectWith;
            bool allowed = this.CanMove(x, y, out boundaryLimitExceed, out intersectWith);

            if (intersectWith != null)
            {
                this.OnTriggered(intersectWith, false, x, y);
                intersectWith.OnTriggered(this, false, x, y);
            }

            int newX = Math.Max(0, (this.X + x));
            int newY = Math.Max(0, (this.Y + y));
            if (allowed)
            {
                this.X = newX;
                this.Y = newY;
            }

            return allowed && !boundaryLimitExceed;
        }

        public virtual void OnUpdated()
        {

        }
    }
}